//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Terrarium.Game;
using Terrarium.Net;
using Terrarium.Tools;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    ///  Handles querying for organisms, transfering organism assemblies
    ///  and transferring of organism states.  Since the teleport process
    ///  is a multi-part conversation, this handler implements all of the
    ///  parts that interact with the organism data.
    /// </summary>
    internal class OrganismsNamespaceHandler : IHttpNamespaceHandler
    {
        private readonly NetworkEngine _engine;

        public OrganismsNamespaceHandler(NetworkEngine engine)
        {
            _engine = engine;
        }

        #region IHttpNamespaceHandler Members

        /// <summary>
        ///  Processes the HTTP Request.  This handler is capable of
        ///  processing several different messages and a series of
        ///  conditional logic is used to determine which message
        ///  is being invoked.
        /// </summary>
        /// <param name="webapp">The web application object for the request.</param>
        public void ProcessRequest(HttpApplication webapp)
        {
            // Initiailize any locals.  This sets up a basic successful
            // response and cahces the requested namespace for the conditional
            // logic.
            var requestedNamespace = webapp.HttpRequest.RequestUri.AbsolutePath;
            webapp.HttpResponse.Server = "Microsoft .Net Terrarium";
            webapp.HttpResponse.Date = DateTime.Now;
            webapp.HttpResponse.ContentType = "text/xml";
            webapp.HttpResponse.StatusCode = HttpStatusCode.OK;
            webapp.HttpResponse.StatusDescription = "OK";
            webapp.HttpResponse.KeepAlive = false;
            var failureReason = "none";
            string body;

            if (webapp.HttpRequest.Method == "GET")
            {
                // Code to implement the GET method.  The /organisms/stats namespace
                // is the important namespace here, and all other namespaces return
                // an error.

                if (requestedNamespace == "/organisms/stats")
                {
                    // Gets XML statistics information from the
                    // network engine.
                    body = _engine.GetNetworkStatistics();
                }
                else
                {
                    body = "<HTML><BODY>" + "Sorry, GET is not supported for ";
                    body += webapp.HttpRequest.RequestUri.ToString();
                    body += "</BODY></HTML>";
                    webapp.HttpResponse.ContentType = "text/html";
                    // Return an error stream.  The namespace being requested
                    // doesn't actually exist.  This is a BadRequest
                    webapp.HttpResponse.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            else if (webapp.HttpRequest.Method == "POST")
            {
                // Code to implement the POST method.  Most of the resources
                // provided by the OrganismNamespaceHandler have to be
                // retrieved using the POST method.

                // Prepares a binary formatter to serialize/deserialize
                // state information.
                var channel = new BinaryFormatter();

                // Add a special binder to the BinaryFormatter to ensure that
                // the stream hasn't been hacked to try to get us to instantiate an
                // object that the organism shouldn't be able to access
                channel.Binder = new TeleportStateBinder();
                TeleportState theOrganism = null;

                if (requestedNamespace == "/organisms/state")
                {
                    _engine.WriteProtocolInfo("/organisms/state: Start receiving TeleportState.");

                    // Provides an implementation for the /organisms/state
                    // resource.  This resource handles the retrieval of
                    // a state object that will be placed into the game
                    // engine (Step 4 of the conversation).

                    var exceptionOccurred = false;
                    var ipAddress = webapp.HttpRequest.RemoteEndPoint.Address.ToString();

                    // If they aren't on the same channel, set the exception bit
                    // this will cause the teleportation to fail
                    if (webapp.HttpRequest.Headers["peerChannel"].ToUpper(CultureInfo.InvariantCulture) !=
                        GameEngine.Current.PeerChannel.ToUpper(CultureInfo.InvariantCulture))
                    {
                        exceptionOccurred = true;
                        failureReason = "Peer channel mismatch";
                        _engine.WriteProtocolInfo("/organisms/state: Sender is wrong peer channel. Denied.");
                    }

                    if (_engine.PeerManager.BadPeer(ipAddress) || !_engine.PeerManager.ShouldReceive(ipAddress))
                    {
                        exceptionOccurred = true;
                        failureReason = "The peer " + ipAddress +
                                        " did not pass the check for badpeer/shouldreceive on the remote peer";
                        _engine.WriteProtocolInfo(
                            "/organisms/state: Sender is marked as a bad peer or is sending too often. Denied.");
                    }
                    else
                    {
                        try
                        {
                            theOrganism = (TeleportState) channel.Deserialize(webapp.Buffer);
                        }
                        catch (Exception e)
                        {
                            ErrorLog.LogHandledException(e);
                            GameEngine.Current.NetworkEngine.LastTeleportationException = e.ToString();
                            exceptionOccurred = true;
                            failureReason = "Exception occured during deserialization of the organism state";
                        }
                    }

                    if (!exceptionOccurred)
                    {
                        _engine.WriteProtocolInfo("/organisms/state: TeleportState successfully deserialized.");

                        // Check to see if the assembly is installed locally
                        Debug.Assert(GameEngine.Current != null);
                        Debug.Assert(theOrganism.OrganismState != null);
                        Debug.Assert(theOrganism.OrganismState.Species != null);
                        Debug.Assert(((Species) theOrganism.OrganismState.Species).AssemblyInfo != null);
                        if (
                            GameEngine.Current.Pac.Exists(
                                ((Species) theOrganism.OrganismState.Species).AssemblyInfo.FullName))
                        {
                            _engine.WriteProtocolInfo("/organisms/state: Assembly exists, add organism to game.");

                            // Add the teleported organism to the game
                            GameEngine.Current.ReceiveTeleportation(theOrganism, false);

                            // Let the peer know that we don't need the assembly
                            body = "<assemblyreceived>true</assemblyreceived>";
                        }
                        else
                        {
                            _engine.WriteProtocolInfo(
                                "/organisms/state: Assembly doesn't exist, don't add organism to game.");

                            // Let the peer know that we'll need the assembly
                            body = "<assemblyreceived>false</assemblyreceived>";
                        }

                        _engine.PeerManager.SetReceive(ipAddress);
                    }
                    else
                    {
                        _engine.WriteProtocolInfo("/organisms/state: Problem occurred:" + failureReason);

                        body = "<organismArrived>false</organismArrived><reason>" + failureReason + "</reason>";
                        GameEngine.Current.NetworkEngine.CountFailedTeleportationReceives();
                    }
                }
                else if (requestedNamespace == "/organisms/assemblies")
                {
                    _engine.WriteProtocolInfo("/organisms/assemblies: Start receiving organism assembly");

                    // Someone is sending us an assembly to save
                    try
                    {
                        var ipAddress = webapp.HttpRequest.RemoteEndPoint.Address.ToString();
                        if (_engine.PeerManager.BadPeer(ipAddress) || !_engine.PeerManager.ShouldReceive(ipAddress))
                        {
                            _engine.WriteProtocolInfo(
                                "/organisms/assemblies: Sender is marked as a bad peer or is sending too often. Denied.");
                            body = "<assemblysaved>false</assemblysaved>";
                        }
                        else
                        {
                            // Write the assembly to a location that is "safe" meaning that it is not 
                            // a location that is known or predictable in any way.  This prevents an attack
                            // where an assembly is sent to this machine with malicious code in it and saved
                            // in a known location that can be used at a later time.
                            var tempFile = PrivateAssemblyCache.GetSafeTempFileName();
                            using (Stream fileStream = File.OpenWrite(tempFile))
                            {
                                var bufferBytes = webapp.Buffer.GetBuffer();
                                fileStream.Write(bufferBytes, 0, bufferBytes.Length);
                            }

                            // Now save the assembly in the Private Assembly Cache
                            var assemblyFullName = webapp.HttpRequest.Headers["Assembly"];
                            try
                            {
                                GameEngine.Current.Pac.SaveOrganismAssembly(tempFile, assemblyFullName);
                                _engine.WriteProtocolInfo("/organisms/assemblies: Assembly saved in PAC.");
                            }
                            catch (Exception e)
                            {
                                // The assembly could fail validation
                                ErrorLog.LogHandledException(e);
                                GameEngine.Current.NetworkEngine.LastTeleportationException = e.ToString();
                                GameEngine.Current.NetworkEngine.CountFailedTeleportationReceives();
                            }

                            File.Delete(tempFile);

                            body = "<assemblysaved>true</assemblysaved>";
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.LogHandledException(ex);
                        body = "<assemblysaved>false</assemblysaved>";
                        GameEngine.Current.NetworkEngine.LastTeleportationException = ex.ToString();
                        GameEngine.Current.NetworkEngine.CountFailedTeleportationReceives();
                    }
                }
                else if (requestedNamespace == "/organisms/assemblycheck")
                {
                    _engine.WriteProtocolInfo("/organisms/assemblycheck: Checking to see if we have an assembly.");

                    // Check to see if the assembly is installed locally
                    var reader = new StreamReader(webapp.Buffer, Encoding.ASCII);
                    var assemblyName = reader.ReadToEnd();

                    Debug.Assert(GameEngine.Current != null);
                    if (GameEngine.Current.Pac.Exists(assemblyName))
                    {
                        // Let the peer know that we don't need the assembly
                        _engine.WriteProtocolInfo("/organisms/assemblycheck: We have it.");
                        body = "<assemblyexists>true</assemblyexists>";
                    }
                    else
                    {
                        // Let the peer know that we need the assembly
                        _engine.WriteProtocolInfo("/organisms/assemblycheck: Don't have it.");
                        body = "<assemblyexists>false</assemblyexists>";
                    }
                }
                else
                {
                    // Handles unsupported namespaces.  This could have been
                    // offloaded to a helper function in the HttpNamespaceManager.
                    body = "<HTML><BODY>" + "The namespace " + webapp.HttpRequest.RequestUri;
                    body += " is not supported.</BODY></HTML>";
                    webapp.HttpResponse.ContentType = "text/html";
                }
            }
            else
            {
                // Handles rendering of unsupport methods.  This could have been
                // offloaded to a helper function in the HttpNamespaceManager
                // or another class.
                webapp.HttpResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                webapp.HttpResponse.StatusDescription = "Method Not Allowed";
                body = "<HTML><BODY>" + "The method " + webapp.HttpRequest.Method;
                body += " is not allowed.</BODY></HTML>";
                webapp.HttpResponse.ContentType = "text/html";
            }

            // Encode the body response and output the response.
            var bodyBytes = Encoding.ASCII.GetBytes(body);
            webapp.HttpResponse.ContentLength = bodyBytes.Length;
            webapp.HttpResponse.Close(bodyBytes);
        }

        #endregion
    }
}