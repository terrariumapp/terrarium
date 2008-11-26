//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Forms;
using Terrarium.Game;
using Terrarium.Net;
using Terrarium.Services.Discovery;
using Terrarium.Tools;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    ///  The NetworkEngine which encapsulates all of the peer to peer functionality
    ///  of the Terrarium application.
    /// </summary>
    public class NetworkEngine
    {
        private const int AnnounceAndRegisterPeerWaitMsec = 300000;
        private const int NetworkTimeoutMsec = 60000; // 5 minute timeout
        private const int PortNumber = 50000;

        /// <summary>
        ///  Message to show if the user is detected to be behind a NAT
        /// </summary>
        public static string NetworkBehindNatMessage =
            "Terrarium has detected that you are behind a NAT or Firewall. Check the FAQ at http://www.windowsforms.net/terrarium for how to set up Terrarium to work in this configuration.";

        private readonly PeerManager _peerManager = new PeerManager();
        private TerrariumLed _discoveryLed;
        private IPAddress _hostIP = IPAddress.Parse("127.0.0.1");
        private string _lastTeleportationException = "none";
        private HttpNamespaceManager _namespaceManager;
        private string _networkStatusMessage = "";
        private int _numberOfFailedTeleportationReceives;
        private int _numberOfFailedTeleportationSends;
        private int _numberOfLocalTeleportations;
        private int _numberOfRemoteTeleportations;
        private int _numberOfTeleportations;
        private string _peerChannel = "EcoSystem";
        private TerrariumLed _peerConnectionLed;
        private BooleanSwitch _protocolDebuggingSwitch;
        private TerrariumLed _receivedPeerConnectionLed;
        private int _totalPeersOnChannel;

        /// <summary>
        ///  The thread used by the network engine in order to announce this peers presence asynchronously.
        /// </summary>
        internal Thread announceThread;

        /// <summary>
        ///  Returns the PeerManager that tracks which peers are valid
        /// </summary>
        public PeerManager PeerManager
        {
            get { return _peerManager; }
        }

        /// <summary>
        /// Returns messages that should be propagated from deep in the networking classes to the UI
        /// </summary>
        public string NetworkStatusMessage
        {
            get { return _networkStatusMessage; }
        }

        internal int PeerCount
        {
            get { return _totalPeersOnChannel; }
        }

        internal string LastTeleportationException
        {
            get { return _lastTeleportationException; }
            set { _lastTeleportationException = value; }
        }

        internal string PeerChannel
        {
            get { return _peerChannel; }
        }

        internal BooleanSwitch ProtocolDebuggingSwitch
        {
            get
            {
                if (_protocolDebuggingSwitch == null)
                {
                    _protocolDebuggingSwitch = new BooleanSwitch("Terrarium.PeerToPeer.Protocol",
                                                                 "Enable tracing for high level peer to peer protocol interactions.");
                }

                return _protocolDebuggingSwitch;
            }
        }

        internal string HostIP
        {
            get { return _hostIP != null ? _hostIP.ToString() : ""; }
        }

        /// <summary>
        /// The total number of teleportations
        /// </summary>
        public int Teleportations
        {
            get { return _numberOfTeleportations; }
        }

        /// <summary>
        /// Number of local teleportations
        /// </summary>
        public int LocalTeleportations
        {
            get { return _numberOfLocalTeleportations; }
        }

        /// <summary>
        /// Number of remote teleportations
        /// </summary>
        public int RemoteTeleportations
        {
            get { return _numberOfRemoteTeleportations; }
        }

        /// <summary>
        /// Number of failures receiving a creature
        /// </summary>
        public int FailedTeleportationReceives
        {
            get { return _numberOfFailedTeleportationReceives; }
        }

        /// <summary>
        /// Number of failures sending a creature
        /// </summary>
        public int FailedTeleportationSends
        {
            get { return _numberOfFailedTeleportationSends; }
        }

        internal void InitializeNetwork(string peerChannel, TerrariumLed[] leds)
        {
            // Set up the LEDs
            _discoveryLed = leds[(int) LedIndicators.DiscoveryWebService];
            _peerConnectionLed = leds[(int) LedIndicators.PeerConnection];
            _receivedPeerConnectionLed = leds[(int) LedIndicators.PeerReceivedConnection];

            // Start listening on HTTP
            _peerChannel = peerChannel;
            //GlobalProxySelection.Select = GlobalProxySelection.GetEmptyWebProxy();
            WebRequest.DefaultWebProxy = null;
            SetHostIPInformation();
            StartHttpNamespaceManager();

            // Start our announcement thread
            announceThread = new Thread(AnnounceAndRegisterPeer) {Name = "Peer Discovery Thread"};
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

            _discoveryLed = null;
            _peerConnectionLed = null;
        }

        /// <summary>
        ///  Start the HTTP Listener used for Peer to Peer interaction.
        /// </summary>
        public void StartHttpNamespaceManager()
        {
            _namespaceManager = new HttpNamespaceManager();
            _namespaceManager.BeforeProcessRequest += BeforeProcessRequest;
            _namespaceManager.AfterProcessRequest += AfterProcessRequest;

            // Starting the manager starts the http listener
            _namespaceManager.Start(HostIP, PortNumber);

            // Register the namespaces we intend to service
            var versionHandler = new VersionNamespaceHandler();
            var organismsHandler = new OrganismsNamespaceHandler(this);
            _namespaceManager.RegisterNamespace("version", versionHandler);
            _namespaceManager.RegisterNamespace("organisms", organismsHandler);
            _namespaceManager.RegisterNamespace("organisms/", organismsHandler);
        }

        /// <summary>
        ///  Stop the HTTP Listener used for Peer to Peer interaction.
        /// </summary>
        public void StopHttpNamespaceManager()
        {
            _namespaceManager.Stop();
            _namespaceManager = null;
        }

        // Used to find other peers on the network
        // This routine is run on its own thread and just goes through the
        // while loop forever
        internal void AnnounceAndRegisterPeer()
        {
            try
            {
                var peerService = new PeerDiscoveryService
                                      {
                                          Url = (GameConfig.WebRoot + "/discovery/discoverydb.asmx"),
                                          Timeout = NetworkTimeoutMsec
                                      };

                while (true)
                {
                    // Make sure our bad peer list remains manageable
                    _peerManager.TruncateBadPeerList();

                    // Register with the central server and get peers
                    try
                    {
                        if (GameConfig.UseConfigForDiscovery || ValidatePeer(peerService.ValidatePeer()))
                        {
                            _networkStatusMessage = "";

                            if (_discoveryLed != null)
                                _discoveryLed.LedState = LedStates.Waiting;

                            DataSet data;

                            if (GameConfig.UseConfigForDiscovery)
                            {
                                // Get it from the config file
                                _totalPeersOnChannel = GetNumPeersFromConfig(_peerChannel);
                                if (_discoveryLed != null)
                                    _discoveryLed.LedState = LedStates.Idle;

                                if (_discoveryLed != null)
                                    _discoveryLed.LedState = LedStates.Waiting;

                                data = GetAllPeersFromConfig(_peerChannel);
                                if (_discoveryLed != null)
                                    _discoveryLed.LedState = LedStates.Idle;
                            }
                            else
                            {
                                peerService.RegisterMyPeerGetCountAndPeerList(
                                    Assembly.GetExecutingAssembly().GetName().Version.ToString(), _peerChannel,
                                    GameEngine.Current.CurrentVector.State.StateGuid, out data,
                                    out _totalPeersOnChannel);
                            }

                            if (_discoveryLed != null)
                                _discoveryLed.LedState = LedStates.Idle;

                            if (data != null)
                            {
                                var peerTable = data.Tables["Peers"];
                                var newPeersHash = Hashtable.Synchronized(new Hashtable());
                                foreach (DataRow row in peerTable.Rows)
                                {
                                    var ipAddress = (string) row["IPAddress"];
                                    var peerLease = (DateTime) row["Lease"];
                                    if (ipAddress == _hostIP.ToString())
                                    {
                                        continue;
                                    }

                                    // If the bad peer has updated their lease, then they
                                    // are online again, count them as good
                                    if (!_peerManager.ClearBadPeer(ipAddress, peerLease))
                                        continue;

                                    newPeersHash.Add(ipAddress, new Peer(ipAddress, peerLease));
                                }

                                _peerManager.KnownPeers = newPeersHash;
                            }
                        }
                        else
                        {
                            _networkStatusMessage = NetworkBehindNatMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_discoveryLed != null)
                            _discoveryLed.LedState = LedStates.Failed;

                        ErrorLog.LogHandledException(ex);
                    }

                    // Sleep for a while so we don't flood the network
                    Thread.Sleep(AnnounceAndRegisterPeerWaitMsec);
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
                sendAddress = _peerManager.GetRandomPeer();

                // If we pick ourselves, short circuit for perf (and so things don't get announced 
                // on the screen as a valid teleport)
                if (sendAddress.Equals(_hostIP))
                    sendAddress = null;
            }

            if (sendAddress == null)
            {
                // Otherwise, teleport back to self
//                GameEngine.Current.ReceiveTeleportation(state, true);
//                CountLocalTeleportation();
                sendAddress = _hostIP;
//                return;
            }

            var workItem = new TeleportWorkItem(this, sendAddress.ToString(), state, PortNumber,
                                                NetworkTimeoutMsec, _peerConnectionLed);

            // Actually do the teleportation asynchronously so we don't block the UI
            TeleportDelegate teleport = workItem.DoTeleport;
            teleport.BeginInvoke(TeleportCallback, null);
        }

        internal void TeleportCallback(IAsyncResult ar)
        {
            // Extract the delegate from the AsyncResult
            var teleportDelegate = (TeleportDelegate) ((AsyncResult) ar).AsyncDelegate;

            // Obtain the result
            var unteleportedObject = teleportDelegate.EndInvoke(ar);
            if (unteleportedObject != null)
            {
                // Something failed, so we need to teleport locally
                var engine = GameEngine.Current;

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
            if (_receivedPeerConnectionLed != null)
            {
                _receivedPeerConnectionLed.AddActivityCount();
            }
        }

        /// <summary>
        ///  Called when the HTTP listener is done processing a request
        /// </summary>
        internal void AfterProcessRequest(object sender, EventArgs e)
        {
            if (_receivedPeerConnectionLed != null)
            {
                _receivedPeerConnectionLed.RemoveActivityCount();
            }
        }

        internal static int GetNumPeersFromConfig(string channel)
        {
            var data = GetAllPeersFromConfig(channel);
            return data.Tables["Peers"].Rows.Count;
        }

        internal static DataSet GetAllPeersFromConfig(string channel)
        {
            var peerList = GameConfig.PeerList;

            var data = new DataSet {Locale = CultureInfo.InvariantCulture};

            var peerTable = data.Tables.Add("Peers");
            peerTable.Columns.Add("IPAddress", typeof (String));
            peerTable.Columns.Add("Lease", typeof (DateTime));

            // The string needs to be Channel,IP,Channel,IP, etc.
            var peers = peerList.Split(',');
            for (var i = 0; i < peers.Length; i += 2)
            {
                if (peers[i].Trim().ToLower(CultureInfo.InvariantCulture) ==
                    channel.ToLower(CultureInfo.InvariantCulture) ||
                    peers[i].Trim().ToLower(CultureInfo.InvariantCulture) == "all")
                {
                    var row = peerTable.NewRow();

                    // If we get a bad format in the config file, we should throw an exception so that the 
                    // discovery engine shows red.  having a 0.0.0.0 should count as a problem since it
                    // will teleport locally                                                                              
                    if (IPAddress.Parse(peers[i + 1].Trim()).ToString() == "0.0.0.0")
                    {
                        throw new ApplicationException("An IP Address in the config file resolved to 0.0.0.0");
                    }

                    row["IPAddress"] = peers[i + 1].Trim();
                    row["Lease"] = DateTime.Now.AddDays(5);
                    // make it expire in 5 days from now, so it is always different, and always far away  
                    peerTable.Rows.Add(row);
                }
            }

            return data;
        }

        // This forms an XML document that reports lots of statistics that are useful for debugging
        internal string GetNetworkStatistics()
        {
            var stats = new StringBuilder();
            stats.Append("<?xml version=\"1.0\"?>");
            stats.Append("<stats>");
            stats.Append("<guid>" + GameEngine.Current.CurrentVector.State.StateGuid + "</guid>");
            stats.Append("<worldHeight>" + GameEngine.Current.WorldHeight + "</worldHeight>");
            stats.Append("<teleportations>");
            stats.Append("<totalteleportations>" + _numberOfTeleportations + "</totalteleportations>");
            stats.Append("<localteleportations>" + _numberOfLocalTeleportations + "</localteleportations>");
            stats.Append("<remoteteleportations>" + _numberOfRemoteTeleportations + "</remoteteleportations>");
            stats.Append("<failedteleportationreceives>" + _numberOfFailedTeleportationReceives +
                         "</failedteleportationreceives>");
            stats.Append("<failedteleportationsends>" + _numberOfFailedTeleportationSends +
                         "</failedteleportationsends>");
            stats.Append("<lastteleportationexception>" + _lastTeleportationException + "</lastteleportationexception>");
            stats.Append("</teleportations> \t");

            stats.Append(_peerManager.GetReport());

            int availableWorkerThreads, maxWorkerThreads;
            int availableIOThreads, maxIOThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableIOThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIOThreads);

            stats.Append("<threadPool>");
            stats.Append("<workerThreadsInUse>" + (maxWorkerThreads - availableWorkerThreads) + "</workerThreadsInUse>");
            stats.Append("<ioThreadsInUse>" + (maxIOThreads - availableIOThreads) + "</ioThreadsInUse>");
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

            var animals = new Hashtable();
            var engine = GameEngine.Current;
            if (engine != null)
            {
                var vector = engine.CurrentVector;
                if (vector != null)
                {
                    var state = vector.State;
                    if (state != null)
                    {
                        foreach (OrganismState organism in state.Organisms)
                        {
                            if (!organism.IsAlive)
                            {
                                continue;
                            }

                            var speciesName = ((Species) organism.Species).Name;
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
                                   entry.Key,
                                   entry.Value);
            }
            stats.Append("</liveCreatures>");

            if (engine != null)
            {
                var pac = engine.Pac;
                if (pac != null)
                {
                    stats.Append("<blacklistedCreatures>");
                    var blacklistedAssemblies = pac.GetBlacklistedAssemblies();
                    foreach (var creature in blacklistedAssemblies)
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

        internal void WriteProtocolInfo(string output)
        {
            if (ProtocolDebuggingSwitch.Enabled)
            {
                Debug.WriteLine("P2P Protocol: " + output);
            }
        }

        // Figure out what IP Address to listen on
        internal void SetHostIPInformation()
        {
            _networkStatusMessage = "";

            if (GameConfig.LocalIPAddress.Length != 0)
            {
                _hostIP = IPAddress.Parse(GameConfig.LocalIPAddress.Trim());
                if (_hostIP.ToString() == "0.0.0.0")
                {
                    Debug.Assert(_networkStatusMessage.Length == 0);
                    _networkStatusMessage =
                        "You have not entered a valid ipAddress in the localIPAddress tag of the Userconfig.xml file.\nUse the format: <localIPAddress>X.X.X.X</localIPAddress>.";
                }
                return;
            }

            var peerService = new PeerDiscoveryService
                                  {
                                      Url = (GameConfig.WebRoot + "/discovery/discoverydb.asmx"),
                                      Timeout = NetworkTimeoutMsec
                                  };

            string address;
            var isVisibleNetworkAddress = false;
            var contactedDiscoveryServer = false;

            try
            {
                address = peerService.ValidatePeer();
                contactedDiscoveryServer = true;
            }
            catch (Exception e)
            {
                Trace.WriteLine("HANDLED EXCEPTION: There was a problem accessing the Peer Discovery Web Service. " + e);
                Debug.Assert(_networkStatusMessage.Length == 0);
                _networkStatusMessage =
                    "The .NET Terrarium is unable to connect to the Terrarium server. \nThis means you won't receive any creatures from other peers.\nYou'll need to restart the Terrarium when it is accessible again to receive creatures.";
                address = "127.0.0.1";
            }

            if (contactedDiscoveryServer)
            {
                isVisibleNetworkAddress = ValidatePeer(address);
            }

            if (isVisibleNetworkAddress)
            {
                _hostIP = IPAddress.Parse(address);
            }
            else
            {
                if (contactedDiscoveryServer)
                {
                    Debug.Assert(_networkStatusMessage.Length == 0);
                    _networkStatusMessage = NetworkBehindNatMessage;
                    IPHostEntry he = null;
                    try
                    {
                        he = Dns.GetHostEntry(Dns.GetHostName());
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("There was a problem resolving the hostname for this peer: " + address + " : " +
                                        ex);
                    }

                    _hostIP = he != null ? he.AddressList[0] : IPAddress.Parse("127.0.0.1");
                }
            }

            Trace.WriteLine("The IP address used for listening is " + _hostIP);
        }

        /// <summary>
        /// Figure out if our local DNS thinks the passed in ipAddress is
        /// valid for us.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        internal static bool ValidatePeer(string ipAddress)
        {
            try
            {
                if (GameConfig.EnableNat)
                {
                    return true;
                }

                var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in hostEntry.AddressList)
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
            Interlocked.Increment(ref _numberOfTeleportations);
            Interlocked.Increment(ref _numberOfLocalTeleportations);
        }

        // This is threadsafe
        internal void CountRemoteTeleportation()
        {
            Interlocked.Increment(ref _numberOfTeleportations);
            Interlocked.Increment(ref _numberOfRemoteTeleportations);
        }

        // This is threadsafe
        internal void CountFailedTeleportationReceives()
        {
            Interlocked.Increment(ref _numberOfFailedTeleportationReceives);
        }

        // This is threadsafe
        internal void CountFailedTeleportationSends()
        {
            Interlocked.Increment(ref _numberOfFailedTeleportationSends);
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
            var startIndex = content.IndexOf(startTag);
            string valueFromContent = null;

            if (startIndex > -1)
            {
                var lastIndex = startIndex + startTag.Length;
                var endIndex = content.IndexOf(endTag, lastIndex, content.Length - lastIndex);
                if (endIndex > -1)
                {
                    valueFromContent = content.Substring(lastIndex, endIndex - lastIndex);
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
            var items = new ArrayList();

            var startIndex = content.IndexOf(startTag);
            while (startIndex > -1)
            {
                var lastIndex = startIndex + startTag.Length;
                var endIndex = content.IndexOf(endTag, lastIndex, content.Length - lastIndex);
                if (endIndex > -1)
                {
                    items.Add(content.Substring(lastIndex, endIndex - lastIndex));
                }

                startIndex = content.IndexOf(startTag, lastIndex, content.Length - lastIndex);
            }

            return items;
        }
    }
}