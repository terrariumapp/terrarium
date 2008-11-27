//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Terrarium.Net
{
    /// <summary>
    ///  The HTTP Namespace manager provides the basic web server
    ///  type features required for the Terrarium application to
    ///  function when in peer to peer mode and enable easy transmission
    ///  of peer messages.
    /// </summary>
    public class HttpNamespaceManager
    {
        private readonly AsyncCallback _getRequestCB;
        private readonly Hashtable _nsModules = new Hashtable();
        private readonly AsyncCallback _readCB;
        private HttpWebListener _listener;

        /// <summary>
        ///  Don't create a new HttpNamespaceManager this way, use the
        ///  static methods instead.
        /// </summary>
        public HttpNamespaceManager()
        {
            _getRequestCB = GetRequestCallback;
            _readCB = ReadCallback;
        }

        public EventHandler BeforeProcessRequest { get; set; }

        public EventHandler AfterProcessRequest { get; set; }

        /// <summary>
        ///  Start listening on the given host IP Address, on the given
        ///  port.  
        /// </summary>
        /// <param name="hostIP">The host's IP address to bind to.</param>
        /// <param name="port">The port to bind on.</param>
        public void Start(string hostIP, int port)
        {
            HttpTraceHelper.ExceptionCaught.Level = TraceLevel.Verbose;

            _listener = new HttpWebListener();
            _listener.Start(port);

            // Put this in a loop if we want more listeners in the pool
            var stateObject = new HttpApplication();
            _listener.BeginGetRequest(_getRequestCB, stateObject);
        }

        /// <summary>
        ///  Shuts down the namespace manager by shutting down the
        ///  _listener.  Cleans up any references to the _listener.
        /// </summary>
        public void Stop()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
            }
        }

        /// <summary>
        ///  Registers a namespace handler for use with a specific namespace.
        ///  A namespace is the same as a web URI that you might append to
        ///  any web server to get at some resource.
        /// </summary>
        /// <param name="ns">The namespace or path of the resource to be handled.</param>
        /// <param name="nsHandler">The IHttpNamespaceHandler implementation that will handle the request.</param>
        public void RegisterNamespace(string ns, IHttpNamespaceHandler nsHandler)
        {
            if (_nsModules.Contains(ns))
            {
                throw new Exception("The " + ns + " namespace is already registered.");
            }
            _nsModules.Add(ns, nsHandler);
        }

        /// <summary>
        ///  Removes a registered namespace from the list so it can no longer be accessed.
        /// </summary>
        /// <param name="ns">The namespace or path of the resource to be removed.</param>
        public void UnregisterNamespace(string ns)
        {
            if (_nsModules.Contains(ns))
            {
                _nsModules.Remove(ns);
            }
        }

        /// <summary>
        ///  This method receives requests from other peers and maps the URI
        ///  namespace or resource name to an actual namespace handler.  If one
        ///  exists then the namespace handler is invoked to process the request.
        ///  If not some default processing is used.
        /// </summary>
        /// <param name="asyncResult">The Async object for the request.</param>
        public void GetRequestCallback(IAsyncResult asyncResult)
        {
            try
            {
                OnBeforeProcessRequest();

                var stateObject = asyncResult.AsyncState as HttpApplication;
                stateObject.HttpRequest = _listener.EndGetRequest(asyncResult);

                if (stateObject.HttpRequest.Method == "GET")
                {
                    var ns = stateObject.HttpRequest.RequestUri.Segments[1];
                    if (_nsModules.Contains(ns))
                    {
                        var nsHandler = (IHttpNamespaceHandler) _nsModules[ns];
                        nsHandler.ProcessRequest(stateObject);
                    }
                    else
                    {
                        HandleUnsupportedNamespace(stateObject);
                    }
                }
                else if (stateObject.HttpRequest.Method == "POST")
                {
                    if (stateObject.HttpRequest.ContentLength <= 0)
                    {
                        var message = "<html><body>Bad Request - POST with no data";
                        message += "</body></html>";
                        HandleBadRequest(stateObject, message);
                    }
                    else
                    {
                        stateObject.RequestStream = stateObject.HttpRequest.GetRequestStream();
                        try
                        {
                            stateObject.RequestStream.BeginRead(stateObject.ReadBuffer, 0, stateObject.ReadBuffer.Length,
                                                                _readCB, stateObject);
                        }
                        catch (Exception exception)
                        {
#if DEBUG
                            if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("Caught exception:" + exception);
                            }
#endif
                        }
                    }
                }
                else
                {
                    HandleUnsupportedNamespace(stateObject);
                }

                // Queue up another _listener
                var newStateObject = new HttpApplication();
                _listener.BeginGetRequest(_getRequestCB, newStateObject);
            }
            catch (Exception ex)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("Caught exception:" + ex);
                }
#endif
                var stateObject = asyncResult.AsyncState as HttpApplication;
                stateObject.Reset();
                _listener.BeginGetRequest(_getRequestCB, stateObject);
            }
            finally
            {
                OnAfterProcessRequest();
            }
        }

        /// <summary>
        ///  Callback used for asynchronously reading a request stream.
        ///  Used in order to process transfers will still accepting
        ///  new connections.
        /// </summary>
        /// <param name="asyncResult">The Async object representing the results of the read.</param>
        public void ReadCallback(IAsyncResult asyncResult)
        {
            try
            {
                var stateObject = asyncResult.AsyncState as HttpApplication;
                var readBytes = stateObject.RequestStream.EndRead(asyncResult);

                if (readBytes > 0)
                {
                    stateObject.ReadBytes += readBytes;

                    if (stateObject.ReadBytes > 262144 /* 256k */)
                        throw new Exception("Request exceeded limit");

                    // Write data received to the memory stream buffer
                    stateObject.Buffer.Write(stateObject.ReadBuffer, 0, readBytes);
                    stateObject.RequestStream.BeginRead(stateObject.ReadBuffer, 0, stateObject.ReadBuffer.Length,
                                                        _readCB,
                                                        stateObject);
                }
                else
                {
                    // Look to see which handler should be called
                    // Rewind the stream
                    stateObject.Buffer.Position = 0;
                    var ns = stateObject.HttpRequest.RequestUri.Segments[1];
                    if (_nsModules.Contains(ns))
                    {
                        var nsHandler = (IHttpNamespaceHandler) _nsModules[ns];
                        nsHandler.ProcessRequest(stateObject);
                    }
                    else
                    {
                        HandleUnsupportedNamespace(stateObject);
                    }
                }
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("Caught exception:" + exception);
                }
#endif
            }
        }

        /// <summary>
        ///  Handles a method that is not supported by the current
        ///  namespace implementation.  This happens whenever a users
        ///  requests a namespace using a method that isn't valid.  This
        ///  should only happen if Terrarium's of differing versions
        ///  connect to one another, or someone hits the peer with
        ///  a web browser and changes the Method to something other than
        ///  GET or POST.
        /// </summary>
        /// <param name="state">The web server state object for the connection.</param>
        public void HandleUnsupportedMethod(HttpApplication state)
        {
            state.HttpResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
            state.HttpResponse.StatusDescription = "Method Not Allowed";
            state.HttpResponse.Server = "Microsoft .Net Terrarium";
            state.HttpResponse.Date = DateTime.Now;
            state.HttpResponse.ContentType = "text/html";
            state.HttpResponse.KeepAlive = false;

            var body = "<HTML><BODY>" + "The method " + state.HttpRequest.Method;
            body += " is not allowed.</BODY></HTML>";

            var bodyBytes = Encoding.ASCII.GetBytes(body);
            state.HttpResponse.ContentLength = bodyBytes.Length;
            state.HttpResponse.Close(bodyBytes);
        }

        /// <summary>
        ///  Handles a namespace that isn't currently bound by this namespace
        ///  manager.  This could happen because of case sensitivity issues, trailing
        ///  path characters or any other string morph that causes the namespace
        ///  to not match one that is currently registered.  This should only happen
        ///  through a failed user connection using a web browser and a mistyped path.
        /// </summary>
        /// <param name="state">The web application state for the current connection.</param>
        public void HandleUnsupportedNamespace(HttpApplication state)
        {
            state.HttpResponse.StatusCode = HttpStatusCode.OK;
            state.HttpResponse.StatusDescription = "OK";
            state.HttpResponse.Server = "Microsoft .Net Terrarium";
            state.HttpResponse.Date = DateTime.Now;
            state.HttpResponse.ContentType = "text/html";
            state.HttpResponse.KeepAlive = false;

            var body = "<HTML><BODY>Unsupported URI.</BODY></HTML>";

            var bodyBytes = Encoding.ASCII.GetBytes(body);
            state.HttpResponse.ContentLength = bodyBytes.Length;
            state.HttpResponse.Close(bodyBytes);
        }

        /// <summary>
        ///  Used to handle a request where the method is supported
        ///  but insufficient data is available.  The primary culprit
        ///  here is a POST operation with no POST data.
        /// </summary>
        /// <param name="state">The web application state object for this request.</param>
        /// <param name="message">The message to be sent to the client to describe the failure.</param>
        public void HandleBadRequest(HttpApplication state, string message)
        {
            state.HttpResponse.StatusCode = HttpStatusCode.BadRequest;
            state.HttpResponse.StatusDescription = "Bad Request";
            state.HttpResponse.Server = "Microsoft .Net Terrarium";
            state.HttpResponse.Date = DateTime.Now;
            state.HttpResponse.ContentType = "text/html";
            state.HttpResponse.KeepAlive = false;
            var body = message;
            var bodyBytes = Encoding.ASCII.GetBytes(body);
            state.HttpResponse.ContentLength = bodyBytes.Length;
            state.HttpResponse.Close(bodyBytes);
        }

        private void OnBeforeProcessRequest()
        {
            if (BeforeProcessRequest != null)
            {
                BeforeProcessRequest(this, new EventArgs());
            }
        }

        private void OnAfterProcessRequest()
        {
            if (AfterProcessRequest != null)
            {
                AfterProcessRequest(this, new EventArgs());
            }
        }
    }
}