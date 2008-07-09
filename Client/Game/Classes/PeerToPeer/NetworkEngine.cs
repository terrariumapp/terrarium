//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Reflection;
using System.Data;
using System.Runtime.Remoting.Messaging;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Game;
using Terrarium.Net;
using Terrarium.Tools;
using Terrarium.Forms;
using Terrarium.Services.Discovery;
using System.Globalization;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    ///  The NetworkEngine which encapsulates all of the peer to peer functionality
    ///  of the Terrarium application.
    /// </summary>
    public class NetworkEngine
    {
        /// <summary>
        ///  The thread used by the network engine in order to announce this peers presence asynchronously.
        /// </summary>
        internal Thread          announceThread = null;
        PeerManager peerManager = new PeerManager();
        private HttpNamespaceManager namespaceManager = null;

        IAsyncResult lastAsyncCall;
        private const bool oneTeleportAtATime = false;  // used for debugging

        private IPAddress        hostIP = IPAddress.Parse("127.0.0.1");
        private string           peerChannel = "EcoSystem";
        private string           lastTeleportationException = "none";
        private int              totalPeersOnChannel = 0;
        private Version          peerVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private const int        networkTimeoutMsec = 60000; // 5 minute timeout
        private const int        portNumber = 50000;
        private const int        announceAndRegisterPeerWaitMsec = 300000;
        private string networkStatusMessage = "";

        /// <summary>
        ///  Message to show if the user is detected to be behind a NAT
        /// </summary>
        public static string NetworkBehindNatMessage = "Terrarium has detected that you are behind a NAT or Firewall. Check the FAQ at http://www.windowsforms.net/terrarium for how to set up Terrarium to work in this configuration.";

        private TerrariumLed discoveryLed;
        private TerrariumLed peerConnectionLed;
        private TerrariumLed receivedPeerConnectionLed;

        // Don't access these directly, use the Count... methods instead
        // Multiple threads are incrementing them
        int numberOfTeleportations = 0;
        int numberOfLocalTeleportations = 0;
        int numberOfRemoteTeleportations = 0;
        int numberOfFailedTeleportationReceives = 0;
        int numberOfFailedTeleportationSends = 0; 
 
        // Switch that is useful for debugging
        BooleanSwitch protocolDebuggingSwitch;

        internal void InitializeNetwork(string peerChannel, TerrariumLed [] leds)
        {
            // Set up the LEDs
            discoveryLed = leds[(int)LedIndicators.DiscoveryWebService];
            peerConnectionLed = leds[(int)LedIndicators.PeerConnection];
            receivedPeerConnectionLed = leds[(int)LedIndicators.PeerReceivedConnection];

            // Start listening on HTTP
            this.peerChannel = peerChannel;        
            //GlobalProxySelection.Select = GlobalProxySelection.GetEmptyWebProxy();
            WebRequest.DefaultWebProxy = null;
            SetHostIPInformation();
            StartHttpNamespaceManager();

            // Start our announcement thread
            announceThread = new Thread(new ThreadStart(this.AnnounceAndRegisterPeer));
            announceThread.Name = "Peer Discovery Thread"; // Useful for debugging
            announceThread.Start();
        }

        internal void ShutdownNetwork()
        {
            if (announceThread != null)
            {
                announceThread.Abort();
            }
            
            // Stop Listening
            StopHttpNamespaceManager();

            discoveryLed = null;
            peerConnectionLed = null; 
        }

        /// <summary>
        ///  Start the HTTP Listener used for Peer to Peer interaction.
        /// </summary>
        public void StartHttpNamespaceManager()
        {
            namespaceManager = new HttpNamespaceManager();
            namespaceManager.BeforeProcessRequest += new EventHandler(BeforeProcessRequest);
            namespaceManager.AfterProcessRequest += new EventHandler(AfterProcessRequest);

            // Starting the manager starts the http listener
            namespaceManager.Start(HostIP, portNumber);

            // Register the namespaces we intend to service
            VersionNamespaceHandler versionHandler = new VersionNamespaceHandler();
            OrganismsNamespaceHandler organismsHandler = new OrganismsNamespaceHandler(this);
            namespaceManager.RegisterNamespace("version", versionHandler);
            namespaceManager.RegisterNamespace("organisms", organismsHandler);
            namespaceManager.RegisterNamespace("organisms/", organismsHandler);
        }

        /// <summary>
        ///  Stop the HTTP Listener used for Peer to Peer interaction.
        /// </summary>
        public void StopHttpNamespaceManager()
        {
            namespaceManager.Stop();
            namespaceManager = null;
        }

        // Used to find other peers on the network
        // This routine is run on its own thread and just goes through the
        // while loop forever
        internal void AnnounceAndRegisterPeer()
        {
            try
            {
                PeerDiscoveryService peerService = new PeerDiscoveryService();
                peerService.Url = GameConfig.WebRoot + "/discovery/discoverydb.asmx";
                peerService.Timeout = networkTimeoutMsec;

                while(true)
                {
                    // Make sure our bad peer list remains manageable
                    peerManager.TruncateBadPeerList();

                    // Register with the central server and get peers
                    try
                    {
                        if (GameConfig.UseConfigForDiscovery || ValidatePeer(peerService.ValidatePeer()))
                        {
                            networkStatusMessage = "";

                            if (discoveryLed != null)
                                discoveryLed.LedState = LedStates.Waiting;

                            DataSet data = null;

                            if (GameConfig.UseConfigForDiscovery)
                            {
                                // Get it from the config file
                                totalPeersOnChannel = GetNumPeersFromConfig(peerChannel);
                                if (discoveryLed != null)
                                    discoveryLed.LedState = LedStates.Idle;

                                if (discoveryLed != null)
                                    discoveryLed.LedState = LedStates.Waiting;
                            
                                data = GetAllPeersFromConfig(peerChannel);
                                if (discoveryLed != null)
                                    discoveryLed.LedState = LedStates.Idle;
                            }
                            else
                            {
                                // Talk to the webservice
                                RegisterPeerResult rpe = peerService.RegisterMyPeerGetCountAndPeerList(Assembly.GetExecutingAssembly().GetName().Version.ToString(), peerChannel, GameEngine.Current.CurrentVector.State.StateGuid, out data, out totalPeersOnChannel);
                            }
                        
                            if (discoveryLed != null)
                                discoveryLed.LedState = LedStates.Idle;

                            if (data != null)
                            {
                                DataTable peerTable = data.Tables["Peers"];
                                Hashtable newPeersHash = Hashtable.Synchronized(new Hashtable());
                                foreach (DataRow row in peerTable.Rows)
                                {
                                    string ipAddress = (string) row["IPAddress"];
                                    DateTime peerLease = (DateTime) row["Lease"];
                                    if (ipAddress == hostIP.ToString())
                                    {
                                        continue;
                                    }
                                    
                                    // If the bad peer has updated their lease, then they
                                    // are online again, count them as good
                                    if(!peerManager.ClearBadPeer(ipAddress, peerLease))
                                        continue;

                                    newPeersHash.Add(ipAddress, new Peer(ipAddress, peerLease));
                                }

                                peerManager.KnownPeers = newPeersHash;
                            }
                        }
                        else
                        {
                            networkStatusMessage = NetworkBehindNatMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (discoveryLed != null)
                            discoveryLed.LedState = LedStates.Failed;

                        ErrorLog.LogHandledException(ex);
                    }

                    // Sleep for a while so we don't flood the network
                    Thread.Sleep(announceAndRegisterPeerWaitMsec);
                }
            }
            catch (ThreadAbortException)
            {
                // percolate out and exit
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                if (!(e.InnerException is ThreadAbortException))
                {
                    ErrorLog.LogHandledException(e);
                }
            }
        }

        internal void Teleport(TeleportState state)
        {
            IPAddress sendAddress = null;
            Debug.Assert(state != null);

            // Check to ensure the network is available.  If it isn't, teleport
            // to the local peer
//            if (oneTeleportAtATime == true && lastAsyncCall != null)
//            {
//                GameEngine.Current.ReceiveTeleportation(state, true);
//                CountLocalTeleportation();
//                return;
//            }

            // If there is more than one peer world, randomly pick
            // one to send the organism to
            if (PeerManager.KnownPeers.Count != 0)
            {
                // Pick a random peer
                sendAddress = peerManager.GetRandomPeer();

                // If we pick ourselves, short circuit for perf (and so things don't get announced 
                // on the screen as a valid teleport)
                if (sendAddress.Equals(hostIP))
                    sendAddress = null;
            }

            if(sendAddress == null)
            {                
                // Otherwise, teleport back to self
//                GameEngine.Current.ReceiveTeleportation(state, true);
//                CountLocalTeleportation();
                sendAddress = hostIP;
//                return;
            }

            TeleportWorkItem workItem = new TeleportWorkItem(this, sendAddress.ToString(), state, portNumber, networkTimeoutMsec, peerConnectionLed);

            // Actually do the teleportation asynchronously so we don't block the UI
            TeleportDelegate teleport = new TeleportDelegate(workItem.DoTeleport);
            lastAsyncCall = teleport.BeginInvoke(new AsyncCallback(TeleportCallback), null);
        }

        internal void TeleportCallback(IAsyncResult ar)
        {
            lastAsyncCall = null;

            // Extract the delegate from the AsyncResult
            TeleportDelegate teleportDelegate = (TeleportDelegate)((AsyncResult)ar).AsyncDelegate;

            // Obtain the result
            object unteleportedObject = teleportDelegate.EndInvoke(ar);
            if (unteleportedObject != null)
            {
                // Something failed, so we need to teleport locally
                GameEngine engine = GameEngine.Current;

                // The engine may be gone now since we're on a different thread.
                // If so, the organism is lost
                if (engine != null)
                {
                    engine.ReceiveTeleportation(unteleportedObject, true);
                    CountLocalTeleportation();
                }
            }
            else
            {
                CountRemoteTeleportation();
            }
        }

        /// <summary>
        ///  Called when the HTTP listener starts to process a request
        /// </summary>
        internal void BeforeProcessRequest(object sender, EventArgs e)
        {
            if (receivedPeerConnectionLed != null)
            {
                receivedPeerConnectionLed.AddActivityCount();
            }
        }

        /// <summary>
        ///  Called when the HTTP listener is done processing a request
        /// </summary>
        internal void AfterProcessRequest(object sender, EventArgs e)
        {
            if (receivedPeerConnectionLed != null)
            {
                receivedPeerConnectionLed.RemoveActivityCount();
            }
        }

        /// <summary>
        ///  Returns the PeerManager that tracks which peers are valid
        /// </summary>
        public PeerManager PeerManager
        {
            get 
            {
                return peerManager;
            }
        }
        
        internal int GetNumPeersFromConfig(string channel)
        {
            DataSet data = GetAllPeersFromConfig(channel);
            return data.Tables["Peers"].Rows.Count;
        }

        internal DataSet GetAllPeersFromConfig(string channel)
        {
            string peerList = GameConfig.PeerList;

            DataSet data = new DataSet();
            data.Locale = CultureInfo.InvariantCulture;

            DataTable peerTable = data.Tables.Add("Peers");
            peerTable.Columns.Add("IPAddress", typeof(System.String));  
            peerTable.Columns.Add("Lease", typeof(System.DateTime));    

            // The string needs to be Channel,IP,Channel,IP, etc.
            string [] peers = peerList.Split(',');
            for (int i = 0; i < peers.Length; i += 2)
            {
                if (peers[i].Trim().ToLower(CultureInfo.InvariantCulture) == channel.ToLower(CultureInfo.InvariantCulture) || 
                    peers[i].Trim().ToLower(CultureInfo.InvariantCulture) == "all")
                {
                    DataRow row = peerTable.NewRow();
                
                    // If we get a bad format in the config file, we should throw an exception so that the 
                    // discovery engine shows red.  having a 0.0.0.0 should count as a problem since it
                    // will teleport locally                                                                              
                    if (IPAddress.Parse(peers[i+1].Trim()).ToString() == "0.0.0.0")
                    {
                        throw new ApplicationException("An IP Address in the config file resolved to 0.0.0.0");
                    }

                    row["IPAddress"] = peers[i+1].Trim();
                    row["Lease"] = DateTime.Now.AddDays(5);   // make it expire in 5 days from now, so it is always different, and always far away  
                    peerTable.Rows.Add(row);    
                }
            }

            return data;
        }
        
        // This forms an XML document that reports lots of statistics that are useful for debugging
        internal string GetNetworkStatistics()
        {
            StringBuilder stats = new StringBuilder();
            stats.Append("<?xml version=\"1.0\"?>");
            stats.Append("<stats>");
            stats.Append("<guid>" + GameEngine.Current.CurrentVector.State.StateGuid.ToString() + "</guid>");
            stats.Append("<worldHeight>" + GameEngine.Current.WorldHeight.ToString() + "</worldHeight>");
            stats.Append("<teleportations>");
            stats.Append("<totalteleportations>" + numberOfTeleportations + "</totalteleportations>" );
            stats.Append("<localteleportations>" + numberOfLocalTeleportations + "</localteleportations>" );
            stats.Append("<remoteteleportations>" + numberOfRemoteTeleportations + "</remoteteleportations>" ); 
            stats.Append("<failedteleportationreceives>" + numberOfFailedTeleportationReceives + "</failedteleportationreceives>" );
            stats.Append("<failedteleportationsends>" + numberOfFailedTeleportationSends + "</failedteleportationsends>" );
            stats.Append("<lastteleportationexception>" + lastTeleportationException + "</lastteleportationexception>" );           
            stats.Append("</teleportations> \t");

            stats.Append(peerManager.GetReport());        

            int availableWorkerThreads, maxWorkerThreads;
            int availableIOThreads, maxIOThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableIOThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIOThreads);

            stats.Append("<threadPool>");
            stats.Append("<workerThreadsInUse>" + (maxWorkerThreads - availableWorkerThreads).ToString() + "</workerThreadsInUse>");
            stats.Append("<ioThreadsInUse>" + (maxIOThreads - availableIOThreads).ToString() + "</ioThreadsInUse>");
            stats.Append("</threadPool>");

            stats.Append("<gameVersion>");
            stats.Append(Assembly.GetExecutingAssembly().GetName().Version.ToString());
            stats.Append("</gameVersion>");

// -- REMOVING APP UPDATED STATUS --
//            stats.Append("<pendingVersion>");       
//            try
//            {
//                stats.Append(AppUpdater.AppUpdater.GetLatestInstalledVersion().ToString());
//            }
//            catch
//            {
//                stats.Append("Not Available");
//            }
//            stats.Append("</pendingVersion>");      

            Hashtable animals = new Hashtable();
            GameEngine engine = GameEngine.Current;
            if (engine != null)
            {
                WorldVector vector = engine.CurrentVector;
                if (vector != null)
                {
                    WorldState state = vector.State;
                    if (state != null)
                    {
                        foreach (OrganismState organism in state.Organisms)
                        {
                            if (!organism.IsAlive)
                            {
                                continue;
                            }
                        
                            string speciesName = ((Species)organism.Species).Name;
                            if (animals[speciesName] == null)
                            {
                                animals[speciesName] = 1;
                            }
                            else
                            {
                                animals[speciesName] = (int) animals[speciesName] + 1;
                            }
                        }
                    }
                }
            }

            stats.Append("<liveCreatures>");
            foreach (DictionaryEntry entry in animals)
            {
                stats.AppendFormat("<creature name=\"{0}\" value=\"{1}\" />",
                    entry.Key.ToString(),
                    entry.Value.ToString());
            }
            stats.Append("</liveCreatures>");

            if (engine != null)
            {
                PrivateAssemblyCache pac = engine.Pac;
                if (pac != null)
                {
                    stats.Append("<blacklistedCreatures>");
                    string [] blacklistedAssemblies = pac.GetBlacklistedAssemblies();
                    foreach (string creature in blacklistedAssemblies)
                    {
                        stats.AppendFormat("<creature name=\"{0}\" />",
                            creature);
                    }
                    stats.Append("</blacklistedCreatures>");
                }
            }
        
            stats.Append("</stats>");

            return stats.ToString();
        }

        /// <summary>
        /// Returns messages that should be propagated from deep in the networking classes to the UI
        /// </summary>
        public string NetworkStatusMessage
        {
            get
            {
                return networkStatusMessage;
            }
        }

        internal int PeerCount 
        {
            get 
            {
                return totalPeersOnChannel;
            }
        }

        internal string LastTeleportationException 
        {
            get 
            {
                return lastTeleportationException;
            }
            set
            {
                lastTeleportationException = value;
            }
        }

        internal string PeerChannel
        {
            get
            {
                return peerChannel;
            }
        }
        
        internal BooleanSwitch ProtocolDebuggingSwitch
        {
            get
            {
                if(protocolDebuggingSwitch == null)
                {
                    protocolDebuggingSwitch = new BooleanSwitch("Terrarium.PeerToPeer.Protocol", "Enable tracing for high level peer to peer protocol interactions.");
                }

                return protocolDebuggingSwitch;
            }
        }

        internal void WriteProtocolInfo(string output)
        {
            if(ProtocolDebuggingSwitch.Enabled)
            {
                Debug.WriteLine("P2P Protocol: " + output);
            }
        }

        // Figure out what IP Address to listen on
        internal void SetHostIPInformation()
        {   
            networkStatusMessage = "";

            if (GameConfig.LocalIPAddress.Length != 0)
            {
                hostIP = IPAddress.Parse(GameConfig.LocalIPAddress.Trim());     
                if (hostIP.ToString() == "0.0.0.0")
                {
                    Debug.Assert(networkStatusMessage.Length == 0);
                    networkStatusMessage = "You have not entered a valid ipAddress in the localIPAddress tag of the Userconfig.xml file.\nUse the format: <localIPAddress>X.X.X.X</localIPAddress>.";
                }
                return;
            }
        
            PeerDiscoveryService peerService = new PeerDiscoveryService();
            peerService.Url = GameConfig.WebRoot + "/discovery/discoverydb.asmx";
            peerService.Timeout = networkTimeoutMsec;

            string address = null;
            bool isVisibleNetworkAddress = false;
            bool contactedDiscoveryServer = false;

            try
            {               
                address = peerService.ValidatePeer();
                contactedDiscoveryServer = true;        
            }
            catch (Exception e)
            {
                Trace.WriteLine("HANDLED EXCEPTION: There was a problem accessing the Peer Discovery Web Service. " + e.ToString());
                Debug.Assert(networkStatusMessage.Length == 0);
                networkStatusMessage = "The .NET Terrarium is unable to connect to the Terrarium server. \nThis means you won't receive any creatures from other peers.\nYou'll need to restart the Terrarium when it is accessible again to receive creatures.";
                address = "127.0.0.1";
            }
        
            if (contactedDiscoveryServer)
            {
                isVisibleNetworkAddress = ValidatePeer(address);
            }
        
            if (isVisibleNetworkAddress)
            {
                hostIP = IPAddress.Parse(address);      
            }
            else
            {
                if (contactedDiscoveryServer)
                {
                    Debug.Assert(networkStatusMessage.Length == 0);
                    networkStatusMessage = NetworkBehindNatMessage;
                    IPHostEntry he = null;
                    try
                    {
                        he = Dns.GetHostEntry(Dns.GetHostName());
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("There was a problem resolving the hostname for this peer: " + address + " : " + ex.ToString());
                    }

                    if (he != null)
                    {
                        hostIP = he.AddressList[0];
                    }
                    else
                    {
                        hostIP = IPAddress.Parse("127.0.0.1");
                    }
                }
            }

            Trace.WriteLine("The IP address used for listening is " + hostIP);
        }

        internal string HostIP
        {
            get
            {
                if (hostIP != null)
                {
                    return hostIP.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        // Figure out if our local DNS thinks the passed in ipAddress is
        // valid for us.
        internal bool ValidatePeer(string ipAddress)
        {
            try
            {
                if (GameConfig.EnableNat)
                {
                    return true;
                }

                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.ToString() == ipAddress)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogHandledException(ex);
            }

            return false;
        }

        // This is threadsafe
        internal void CountLocalTeleportation()
        {
            Interlocked.Increment(ref numberOfTeleportations);
            Interlocked.Increment(ref numberOfLocalTeleportations);
        }

        // This is threadsafe
        internal void CountRemoteTeleportation()
        {
            Interlocked.Increment(ref numberOfTeleportations);
            Interlocked.Increment(ref numberOfRemoteTeleportations);
        }
    
        // This is threadsafe
        internal void CountFailedTeleportationReceives() 
        {
            Interlocked.Increment(ref numberOfFailedTeleportationReceives);     
        }

        // This is threadsafe
        internal void CountFailedTeleportationSends() 
        {
            Interlocked.Increment(ref numberOfFailedTeleportationSends);        
        }

        /// <summary>
        /// Simple way to parse an XML tag for content.  Doesn't confirm that the file is valid XML
        /// </summary>
        /// <param name="startTag">The start tag</param>
        /// <param name="endTag">The end tag</param>
        /// <param name="content">The content to be parsed.</param>
        /// <returns>The value between the start and end tag.</returns>
        public static string GetValueFromContent(string startTag, string endTag, string content)
        {
            int startIndex, endIndex, lastIndex;
            startIndex = content.IndexOf(startTag);
            string valueFromContent = null;

            if (startIndex>-1)
            {
                lastIndex = startIndex + startTag.Length;
                endIndex = content.IndexOf(endTag, lastIndex, content.Length - lastIndex);
                if (endIndex>-1)
                {
                    valueFromContent = content.Substring(lastIndex, endIndex-lastIndex);
                }
            }
            return valueFromContent;
        }

        /// <summary>
        ///  Retrieves a collection of values delimited by tags from content. Doesn't confirm that the file is valid XML.
        /// </summary>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <param name="content">The content to parse.</param>
        /// <returns>A collection of values parsed from the content.</returns>
        public static ArrayList GetValuesFromContent(string startTag, string endTag, string content)
        {
            int startIndex, endIndex, lastIndex;
            ArrayList items = new ArrayList();

            startIndex = content.IndexOf(startTag);
            while (startIndex>-1)
            {
                lastIndex = startIndex + startTag.Length;
                endIndex = content.IndexOf(endTag, lastIndex, content.Length - lastIndex);
                if (endIndex>-1)
                {
                    items.Add(content.Substring(lastIndex, endIndex-lastIndex));
                }

                startIndex = content.IndexOf(startTag, lastIndex, content.Length - lastIndex);
            }

            return items;
        }

		/// <summary>
		/// The total number of teleportations
		/// </summary>
		public int Teleportations
		{
			get
			{
				return this.numberOfTeleportations;
			}
		}

		/// <summary>
		/// Number of local teleportations
		/// </summary>
		public int LocalTeleportations
		{
			get
			{
				return this.numberOfLocalTeleportations;
			}
		}

		/// <summary>
		/// Number of remote teleportations
		/// </summary>
		public int RemoteTeleportations
		{
			get
			{
				return this.numberOfRemoteTeleportations;
			}
		}

		/// <summary>
		/// Number of failures receiving a creature
		/// </summary>
		public int FailedTeleportationReceives
		{
			get
			{
				return this.numberOfFailedTeleportationReceives;
			}
		}

		/// <summary>
		/// Number of failures sending a creature
		/// </summary>
		public int FailedTeleportationSends
		{
			get
			{
				return this.numberOfFailedTeleportationSends;
			}
		}
    }
}
