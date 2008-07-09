//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.IO;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Game;
using Terrarium.Tools;
using Terrarium.Forms;

namespace Terrarium.PeerToPeer 
{
    // This is the class that does all the work for asynchronously teleporting an organism
    // from this Terrarium to another.
    // WARNING: Everything on this class is accessed on a worker thread.  Make sure every method it calls is
    // threadsafe
    internal class TeleportWorkItem
    {
        string address;
        object state;
        int httpPort;
        int networkTimeoutMsec;
        TerrariumLed peerConnectionLed;
        Version peerVersion = Assembly.GetExecutingAssembly().GetName().Version;
        NetworkEngine engine;

        internal TeleportWorkItem(NetworkEngine engine, string address, object state, int httpPort, int networkTimeoutMsec, TerrariumLed led)
        {
            this.engine = engine;
            this.address = address;
            this.state = state;
            this.httpPort = httpPort;
            this.peerConnectionLed = led;
            this.networkTimeoutMsec = networkTimeoutMsec;
        }

        // Called by the worker thread to do the actual work of the Teleport
        // returns the teleported object if it couldn't teleport or null if successful
        internal object DoTeleport()
        {
            // Check to see if the versions match
            Version remoteVersion = null;
            string remoteChannel = null;
            try
            {
                if (peerConnectionLed != null)
                    peerConnectionLed.LedState = LedStates.Waiting;

                NameValueCollection peerInfo = GetPeerTerrariumInfo(address);
                remoteVersion = new Version(Convert.ToInt32(peerInfo["major"]),Convert.ToInt32(peerInfo["minor"]), Convert.ToInt32(peerInfo["build"]));             
                remoteChannel = peerInfo["channel"];        
                        
                // Check to see if there is a channel and if they don't match
                if (remoteChannel.ToLower(CultureInfo.InvariantCulture) != GameEngine.Current.PeerChannel.ToLower(CultureInfo.InvariantCulture))
                    throw new Exception("An attempt was made to teleport an organism to another peer using a different channel.");              
            }
            catch (Exception e)
            {
                if (peerConnectionLed != null)
                    peerConnectionLed.LedState = LedStates.Failed;

                ErrorLog.LogHandledException(e);
                GameEngine.Current.NetworkEngine.LastTeleportationException = e.ToString();            

                // Remove the bad IP from the list
                engine.PeerManager.RemovePeer(address);          
                GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();

                return state;
            }

            if (peerConnectionLed != null)
                peerConnectionLed.LedState = LedStates.Idle;

            bool versionsMatch = false;
            if (remoteVersion != null)
            {
                if (remoteVersion.Build == peerVersion.Build && remoteVersion.Major == peerVersion.Major &&
                    remoteVersion.Minor == peerVersion.Minor)
                    versionsMatch = true;
            }

            if (versionsMatch)
            {
                // Send the peer assembly - Assuming this succeeds
                engine.WriteProtocolInfo("DoTeleport: Versions Match: Send the peer assembly");
                try
                {
                    if (peerConnectionLed != null)
                        peerConnectionLed.LedState = LedStates.Waiting;

                    SendAssemblyToPeer(address, state);
                }
                catch (Exception exception)
                {
                    GameEngine.Current.NetworkEngine.LastTeleportationException = exception.ToString();
                    GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();

                    if (exception is AbortPeerDiscussionException)
                    {
                        // Don't remove them from the peer list, just teleport locally until they get up to date
                        if (peerConnectionLed != null)
                            peerConnectionLed.LedState = LedStates.Idle;
                                        
                        return state;
                    }

                    ErrorLog.LogHandledException(exception);
                    if (peerConnectionLed != null)
                        peerConnectionLed.LedState = LedStates.Failed;
                
                    // Remove the bad IP from the list
                    engine.PeerManager.RemovePeer(address);              
                    return state;
                }

                if (peerConnectionLed != null)
                    peerConnectionLed.LedState = LedStates.Idle;
            }
            else
            {
                // Versions don't match so return the object and remove peer from list
                // Remove the bad IP from the list
                engine.WriteProtocolInfo("DoTeleport: Versions don't match: remove peer from list.");

                engine.PeerManager.RemovePeer(address);
                GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();          
                return state;
            }

            return null;
        }

        // Gets the Terrarium version of a peer
        void SendAssemblyToPeer(string address, object state)
        {
            string peerAddress = null;
            int buffSize = 4096;
            byte[] uploadBuffer = new byte[buffSize];
            int bytesRead = 0;
            TeleportState theOrganism = (TeleportState)state;
            string assemblyName = ((Species) theOrganism.OrganismState.Species).AssemblyInfo.FullName;
            HttpWebRequest req = null;
            WebResponse resp = null;
            string assemblyResponse = null;
            string assemblyVersionResponse = null;

            //1. Check to see if the peer has this assembly
            peerAddress = "http://" + address + ":" + httpPort.ToString() + "/organisms/assemblycheck";
            req = (HttpWebRequest)WebRequest.Create(peerAddress);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Method = "POST";
            req.Timeout = networkTimeoutMsec;
            req.ContentType = "application/text";
            byte[] assemblyBuf = Encoding.ASCII.GetBytes(assemblyName);

            // Write the assembly name to the request stream
            req.ContentLength = assemblyBuf.Length;
            using (Stream stream = req.GetRequestStream())
                stream.Write(assemblyBuf, 0, (int) assemblyBuf.Length);

            // Send the request and see what the response is
            using (resp = req.GetResponse())
            {
                using (StreamReader assemblyVersionReader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    assemblyVersionResponse = assemblyVersionReader.ReadToEnd();
                    assemblyVersionReader.Close();
                }
                resp.Close();
            }

            // 2. Send the assembly if necessary
            if (assemblyVersionResponse == "<assemblyexists>false</assemblyexists>")
            {
                // They don't have the assembly so we need to read it
                // and send it over to the peer
                this.engine.WriteProtocolInfo("SendAssemblyToPeer: They don't have the assembly.");

                peerAddress = "http://" + address + ":" + httpPort.ToString() + "/organisms/assemblies";
                HttpWebRequest assemblyReq = (HttpWebRequest)WebRequest.Create(peerAddress);
                assemblyReq.UserAgent = "Microsoft .NET Terrarium";
                assemblyReq.Method = "POST";
                assemblyReq.Timeout = networkTimeoutMsec;
                assemblyReq.ContentType = "application/octet-stream";

                TeleportState teleportOrganism = (TeleportState)state;
                GameEngine engine = GameEngine.Current;
                if (engine == null)
                    throw new GameEngineException("Engine no longer exists");

                string file = engine.Pac.GetFileName(((Species) teleportOrganism.OrganismState.Species).AssemblyInfo.FullName);
                assemblyReq.Headers["Assembly"] = ((Species) teleportOrganism.OrganismState.Species).AssemblyInfo.FullName;

                // Write the file to the request stream
                using (Stream fileStream = File.OpenRead(file))
                {
                    MemoryStream originalAssemblyStream = new MemoryStream(buffSize);

                    bytesRead = fileStream.Read(uploadBuffer, 0, buffSize);
                    while (bytesRead > 0)
                    {
                        originalAssemblyStream.Write(uploadBuffer, 0, bytesRead);
                        bytesRead = fileStream.Read(uploadBuffer, 0, buffSize);
                    }

                    originalAssemblyStream.Position = 0;
                    assemblyReq.ContentLength = originalAssemblyStream.Length;
                    
                    using (Stream uploadAssemblyStream = assemblyReq.GetRequestStream())
                    {
                        uploadAssemblyStream.Write(originalAssemblyStream.GetBuffer(), 0, (int) originalAssemblyStream.Length);
                    }    
                }

                // Send the assembly over and read the response
                using (WebResponse assemblyResp = assemblyReq.GetResponse())
                {
                    using (StreamReader assemblyReader = new StreamReader(assemblyResp.GetResponseStream(), Encoding.ASCII))
                    {
                        assemblyResponse = assemblyReader.ReadToEnd();
                        assemblyReader.Close();
                    }

                    assemblyResp.Close();
                }

                this.engine.WriteProtocolInfo("SendAssemblyToPeer: Assembly sent.");
            }
            else
            {
                engine.WriteProtocolInfo("SendAssemblyToPeer: They have the assembly.");
            }

            //3. Send the object state
            peerAddress = "http://" + address + ":" + httpPort.ToString() + "/organisms/state";
            req = (HttpWebRequest)WebRequest.Create(peerAddress);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Method = "POST";
            req.Timeout = networkTimeoutMsec;
            req.ContentType = "application/octet-stream";
            req.Headers["peerChannel"] = GameEngine.Current.PeerChannel;

            // Serialize the state object into the request stream
            BinaryFormatter channel = new BinaryFormatter();
            MemoryStream stateStream = new MemoryStream();
            channel.Serialize(stateStream, state);
            
            req.ContentLength = stateStream.Length;
            using (Stream stream = req.GetRequestStream())
                stream.Write(stateStream.GetBuffer(), 0, (int) stateStream.Length);

            // Send the request and look to see if the state was accepted
            string response;
            using (resp = req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    response = reader.ReadToEnd();
                    reader.Close();
                }
                resp.Close();
            }

            engine.WriteProtocolInfo("SendAssemblyToPeer: Organism state sent.");

            // Look for <organismArrived>false</organismArrived>, because that means the organism
            // didn't make it and should be teleported locally
            if (NetworkEngine.GetValueFromContent("<organismArrived>", "</organismArrived>", response) == "false")
            {
                string errorMessage = "The organism didn't arrive on their side for some reason.";
                if (NetworkEngine.GetValueFromContent("<reason>", "</reason>", response) != null)
                {
                    errorMessage = "The organism didn't arrive on the remote peer. " + NetworkEngine.GetValueFromContent("<reason>", "</reason>", response);
                }
                throw new AbortPeerDiscussionException(errorMessage);        
            }

            return;
        }

        // Gets the Terrarium version of a peer
        NameValueCollection GetPeerTerrariumInfo(string address)
        {
            address = "http://" + address + ":" + httpPort.ToString() + "/version";
            string content = null;
            NameValueCollection peerInfo = new NameValueCollection();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(address);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Timeout = networkTimeoutMsec;

            using (WebResponse resp = req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    content = reader.ReadToEnd();
                    reader.Close();
                }
                resp.Close();
            }

            string tempString = NetworkEngine.GetValueFromContent("<major>", "</major>", content);
            int major = Convert.ToInt32(tempString);

            tempString = NetworkEngine.GetValueFromContent("<minor>", "</minor>", content);
            int minor = Convert.ToInt32(tempString);

            tempString = NetworkEngine.GetValueFromContent("<build>", "</build>", content);
            int build = Convert.ToInt32(tempString);

            string currentChannel = NetworkEngine.GetValueFromContent("<channel>", "</channel>", content);

            peerInfo["major"] = major.ToString();
            peerInfo["minor"] = minor.ToString();
            peerInfo["build"] = build.ToString();

            if (currentChannel != null)
                peerInfo["channel"] = currentChannel;

            return peerInfo;
        }
    }
}