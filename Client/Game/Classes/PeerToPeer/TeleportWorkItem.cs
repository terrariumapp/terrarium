//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using OrganismBase;
using Terrarium.Forms;
using Terrarium.Game;
using Terrarium.Tools;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    /// This is the class that does all the work for asynchronously teleporting an 
    /// organism from this Terrarium to another.
    /// 
    /// WARNING: Everything on this class is accessed on a worker thread.  Make sure 
    /// every method it calls is threadsafe!
    /// </summary>
    internal class TeleportWorkItem
    {
        private readonly string _address;
        private readonly NetworkEngine _engine;
        private readonly int _httpPort;
        private readonly int _networkTimeoutMsec;
        private readonly TerrariumLed _peerConnectionLed;
        private readonly Version _peerVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly object _state;

        internal TeleportWorkItem(NetworkEngine engine, string address, object state, int httpPort,
                                  int networkTimeoutMsec, TerrariumLed led)
        {
            _engine = engine;
            _address = address;
            _state = state;
            _httpPort = httpPort;
            _peerConnectionLed = led;
            _networkTimeoutMsec = networkTimeoutMsec;
        }

        // Called by the worker thread to do the actual work of the Teleport
        // returns the teleported object if it couldn't teleport or null if successful
        internal object DoTeleport()
        {
            // Check to see if the versions match
            Version remoteVersion;
            string remoteChannel;
            try
            {
                if (_peerConnectionLed != null)
                    _peerConnectionLed.LedState = LedStates.Waiting;

                var peerInfo = getPeerTerrariumInfo(_address);
                remoteVersion = new Version(Convert.ToInt32(peerInfo["major"]), Convert.ToInt32(peerInfo["minor"]),
                                            Convert.ToInt32(peerInfo["build"]));
                remoteChannel = peerInfo["channel"];

                // Check to see if there is a channel and if they don't match
                if (remoteChannel.ToLower(CultureInfo.InvariantCulture) !=
                    GameEngine.Current.PeerChannel.ToLower(CultureInfo.InvariantCulture))
                    throw new Exception(
                        "An attempt was made to teleport an organism to another peer using a different channel.");
            }
            catch (Exception e)
            {
                if (_peerConnectionLed != null)
                    _peerConnectionLed.LedState = LedStates.Failed;

                ErrorLog.LogHandledException(e);
                GameEngine.Current.NetworkEngine.LastTeleportationException = e.ToString();

                // Remove the bad IP from the list
                _engine.PeerManager.RemovePeer(_address);
                GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();

                return _state;
            }

            if (_peerConnectionLed != null)
                _peerConnectionLed.LedState = LedStates.Idle;

            var versionsMatch = false;
            if (remoteVersion.Build == _peerVersion.Build && remoteVersion.Major == _peerVersion.Major &&
                remoteVersion.Minor == _peerVersion.Minor)
                versionsMatch = true;

            if (versionsMatch)
            {
                // Send the peer assembly - Assuming this succeeds
                _engine.WriteProtocolInfo("DoTeleport: Versions Match: Send the peer assembly");
                try
                {
                    if (_peerConnectionLed != null)
                        _peerConnectionLed.LedState = LedStates.Waiting;

                    sendAssemblyToPeer(_address, _state);
                }
                catch (Exception exception)
                {
                    GameEngine.Current.NetworkEngine.LastTeleportationException = exception.ToString();
                    GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();

                    if (exception is AbortPeerDiscussionException)
                    {
                        // Don't remove them from the peer list, just teleport locally until they get up to date
                        if (_peerConnectionLed != null)
                            _peerConnectionLed.LedState = LedStates.Idle;

                        return _state;
                    }

                    ErrorLog.LogHandledException(exception);
                    if (_peerConnectionLed != null)
                        _peerConnectionLed.LedState = LedStates.Failed;

                    // Remove the bad IP from the list
                    _engine.PeerManager.RemovePeer(_address);
                    return _state;
                }

                if (_peerConnectionLed != null)
                    _peerConnectionLed.LedState = LedStates.Idle;
            }
            else
            {
                // Versions don't match so return the object and remove peer from list
                // Remove the bad IP from the list
                _engine.WriteProtocolInfo("DoTeleport: Versions don't match: remove peer from list.");

                _engine.PeerManager.RemovePeer(_address);
                GameEngine.Current.NetworkEngine.CountFailedTeleportationSends();
                return _state;
            }

            return null;
        }

        private void sendAssemblyToPeer(string address, object state)
        {
            const int buffSize = 4096;
            var uploadBuffer = new byte[buffSize];
            var theOrganism = (TeleportState) state;
            var assemblyName = ((Species) theOrganism.OrganismState.Species).AssemblyInfo.FullName;
            byte[] assemblyBuf;

            var req = checkToSeeIfPeerHasThisAssembly(address, assemblyName, out assemblyBuf);
            writeAssemblyNameToRequest(req, assemblyBuf);
            var assemblyVersionResponse = getAssemblyVersionResponse(req);
            sendAssemblyIfNecessary(buffSize, state, address, assemblyVersionResponse, uploadBuffer);
            req = sendObjectState(state, address);
            var response = sendRequest(req);
            _engine.WriteProtocolInfo("sendAssemblyToPeer: Organism state sent.");

            // Look for <organismArrived>false</organismArrived>, because that means the organism
            // didn't make it and should be teleported locally
            if (NetworkEngine.GetValueFromContent("<organismArrived>", "</organismArrived>", response) == "false")
            {
                var errorMessage = "The organism didn't arrive on their side for some reason.";
                if (NetworkEngine.GetValueFromContent("<reason>", "</reason>", response) != null)
                {
                    errorMessage = "The organism didn't arrive on the remote peer. " +
                                   NetworkEngine.GetValueFromContent("<reason>", "</reason>", response);
                }
                throw new AbortPeerDiscussionException(errorMessage);
            }

            return;
        }

        private static string sendRequest(WebRequest req)
        {
            WebResponse resp;
            string response;
            using (resp = req.GetResponse())
            {
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    response = reader.ReadToEnd();
                    reader.Close();
                }
                resp.Close();
            }
            return response;
        }

        private HttpWebRequest sendObjectState(object state, string address)
        {
            var peerAddress = "http://" + address + ":" + _httpPort + "/organisms/state";
            var req = (HttpWebRequest) WebRequest.Create(peerAddress);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Method = "POST";
            req.Timeout = _networkTimeoutMsec;
            req.ContentType = "application/octet-stream";
            req.Headers["peerChannel"] = GameEngine.Current.PeerChannel;

            var channel = new BinaryFormatter();
            var stateStream = new MemoryStream();
            channel.Serialize(stateStream, state);

            req.ContentLength = stateStream.Length;
            using (var stream = req.GetRequestStream())
                stream.Write(stateStream.GetBuffer(), 0, (int) stateStream.Length);
            return req;
        }

        private void sendAssemblyIfNecessary(int buffSize, object state, string address, string assemblyVersionResponse,
                                             byte[] uploadBuffer)
        {
            if (assemblyVersionResponse == "<assemblyexists>false</assemblyexists>")
            {
                // They don't have the assembly so we need to read it
                // and send it over to the peer
                _engine.WriteProtocolInfo("sendAssemblyToPeer: They don't have the assembly.");

                var peerAddress = "http://" + address + ":" + _httpPort + "/organisms/assemblies";
                var assemblyReq = (HttpWebRequest) WebRequest.Create(peerAddress);
                assemblyReq.UserAgent = "Microsoft .NET Terrarium";
                assemblyReq.Method = "POST";
                assemblyReq.Timeout = _networkTimeoutMsec;
                assemblyReq.ContentType = "application/octet-stream";

                var teleportOrganism = (TeleportState) state;
                var engine = GameEngine.Current;
                if (engine == null)
                    throw new GameEngineException("Engine no longer exists");

                var file =
                    engine.Pac.GetFileName(((Species) teleportOrganism.OrganismState.Species).AssemblyInfo.FullName);
                assemblyReq.Headers["Assembly"] =
                    ((Species) teleportOrganism.OrganismState.Species).AssemblyInfo.FullName;

                // Write the file to the request stream
                using (Stream fileStream = File.OpenRead(file))
                {
                    var originalAssemblyStream = new MemoryStream(buffSize);

                    var bytesRead = fileStream.Read(uploadBuffer, 0, buffSize);
                    while (bytesRead > 0)
                    {
                        originalAssemblyStream.Write(uploadBuffer, 0, bytesRead);
                        bytesRead = fileStream.Read(uploadBuffer, 0, buffSize);
                    }

                    originalAssemblyStream.Position = 0;
                    assemblyReq.ContentLength = originalAssemblyStream.Length;

                    using (var uploadAssemblyStream = assemblyReq.GetRequestStream())
                    {
                        uploadAssemblyStream.Write(originalAssemblyStream.GetBuffer(), 0,
                                                   (int) originalAssemblyStream.Length);
                    }
                }

                // Send the assembly over and read the response
                using (var assemblyResp = assemblyReq.GetResponse())
                {
                    using (var assemblyReader = new StreamReader(assemblyResp.GetResponseStream(), Encoding.ASCII))
                    {
                        assemblyReader.ReadToEnd();
                        assemblyReader.Close();
                    }

                    assemblyResp.Close();
                }

                _engine.WriteProtocolInfo("sendAssemblyToPeer: Assembly sent.");
            }
            else
            {
                _engine.WriteProtocolInfo("sendAssemblyToPeer: They have the assembly.");
            }
        }

        private static string getAssemblyVersionResponse(WebRequest req)
        {
            WebResponse resp;
            string assemblyVersionResponse;
            using (resp = req.GetResponse())
            {
                using (var assemblyVersionReader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    assemblyVersionResponse = assemblyVersionReader.ReadToEnd();
                    assemblyVersionReader.Close();
                }
                resp.Close();
            }
            return assemblyVersionResponse;
        }

        private static void writeAssemblyNameToRequest(WebRequest req, byte[] assemblyBuf)
        {
            req.ContentLength = assemblyBuf.Length;
            using (var stream = req.GetRequestStream())
                stream.Write(assemblyBuf, 0, assemblyBuf.Length);
        }

        private HttpWebRequest checkToSeeIfPeerHasThisAssembly(string address, string assemblyName,
                                                               out byte[] assemblyBuf)
        {
            var peerAddress = "http://" + address + ":" + _httpPort + "/organisms/assemblycheck";
            var req = (HttpWebRequest) WebRequest.Create(peerAddress);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Method = "POST";
            req.Timeout = _networkTimeoutMsec;
            req.ContentType = "application/text";
            assemblyBuf = Encoding.ASCII.GetBytes(assemblyName);
            return req;
        }

        private NameValueCollection getPeerTerrariumInfo(string address)
        {
            address = "http://" + address + ":" + _httpPort + "/version";
            string content;
            var peerInfo = new NameValueCollection();

            var req = (HttpWebRequest) WebRequest.Create(address);
            req.UserAgent = "Microsoft .NET Terrarium";
            req.Timeout = _networkTimeoutMsec;

            using (var resp = req.GetResponse())
            {
                using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.ASCII))
                {
                    content = reader.ReadToEnd();
                    reader.Close();
                }
                resp.Close();
            }

            var tempString = NetworkEngine.GetValueFromContent("<major>", "</major>", content);
            var major = Convert.ToInt32(tempString);

            tempString = NetworkEngine.GetValueFromContent("<minor>", "</minor>", content);
            var minor = Convert.ToInt32(tempString);

            tempString = NetworkEngine.GetValueFromContent("<build>", "</build>", content);
            var build = Convert.ToInt32(tempString);

            var currentChannel = NetworkEngine.GetValueFromContent("<channel>", "</channel>", content);

            peerInfo["major"] = major.ToString();
            peerInfo["minor"] = minor.ToString();
            peerInfo["build"] = build.ToString();

            if (currentChannel != null)
                peerInfo["channel"] = currentChannel;

            return peerInfo;
        }
    }
}