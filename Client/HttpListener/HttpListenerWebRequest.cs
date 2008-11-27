//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;

namespace Terrarium.Net
{
    public class HttpListenerWebRequest
    {
        private readonly HttpConnectionState _connectionState;
        private bool _handleConnectionCalled;
        private bool _interpretHeadersCalled;
        private HttpListenerWebResponse _response;
        private bool _sent100Continue;
        private Stream _stream;

        public HttpListenerWebRequest(HttpConnectionState connectionState)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::.ctor()");
            }
#endif
            _connectionState = connectionState;
#if DEBUG
            if (HttpTraceHelper.Socket.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::.ctor() calling Socket.get_RemoteEndPoint()");
            }
#endif
            RemoteEndPoint = (IPEndPoint) (connectionState.ConnectionSocket.RemoteEndPoint);
        }

        public HttpConnectionState ConnectionState
        {
            get { return _connectionState; }
        }

        public bool Uploading
        {
            get
            {
                //
                // make sure we interpreted headers
                //
                InterpretHeaders();

                return Chunked || ContentLength > 0;
            }
        }

        public int DelayResponse { get; set; }

        public int Delay100Continue { get; set; }

        public IPEndPoint RemoteEndPoint { get; private set; }

        public string RelativeUri { get; set; }

        public Uri RequestUri { get; set; }

        public string Method { get; set; }

        public Version ProtocolVersion { get; set; }

        public string Host { get; private set; }

        public bool Chunked { get; private set; }

        public long ContentLength { get; private set; }

        public bool KeepAlive { get; private set; }

        public string Connection { get; private set; }

        public WebHeaderCollection Headers { get; set; }

        public bool InterpretHeaders()
        {
            //
            // when all the headers are parsed, this method will
            // interpret the values of the "special" headers and set
            // the corresponding properties to the correct values.
            //
            if (Headers == null)
            {
                var exception = new Exception("Don't have headers yet");
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                              "::InterpretHeaders() throwing: " + exception);
                }
#endif
                throw exception;
            }

            if (_interpretHeadersCalled)
            {
                return false;
            }

            _interpretHeadersCalled = true;

            //
            // Connection && KeepAlive
            //
            Connection = Headers["Connection"];
            KeepAlive = Connection == null ||
                        Connection.ToLower(CultureInfo.InvariantCulture).IndexOf("close") == -1;

            //
            // RequestUri
            //
            Host = Headers["Host"];
            RequestUri = null;
            // we should try this only if this is a fully qualified Uri
            if (RelativeUri.StartsWith("http://") || RelativeUri.StartsWith("https://"))
            {
                try
                {
                    RequestUri = new Uri(RelativeUri);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::InterpretHeaders() caught exception in Uri..ctor(): " + exception);
                    }
#endif
                }
            }

            if (RequestUri == null || !(RequestUri.Scheme.Equals("http") || RequestUri.Scheme.Equals("https")))
            {
                try
                {
                    RequestUri = RelativeUri[0] != '/'
                                     ? new Uri("http://" + Host + "/" + RelativeUri)
                                     : new Uri("http://" + Host + RelativeUri);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::InterpretHeaders() caught exception in Uri..ctor(): " + exception);
                    }
#endif
                }
            }

            //
            // Chunked
            //
            var value = Headers["Transfer-Encoding"];
            Chunked = value != null && value.ToLower(CultureInfo.InvariantCulture).IndexOf("chunked") >= 0;

            //
            // ContentLength
            //
            if (Chunked)
            {
                //
                // if the client is chunking we ignore Content-Length
                //
                ContentLength = -1;
            }
            else
            {
                ContentLength = 0;
                value = Headers["Content-Length"];
                if (value != null)
                {
                    try
                    {
                        ContentLength = Int64.Parse(value);
                    }
                    catch (Exception exception)
                    {
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                      "::InterpretHeaders() caught exception in Int64.Parse(): " +
                                                      exception);
                        }
#endif
                    }
                }
            }

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::InterpretHeaders() ContentLength:" + ContentLength + " Chunked:" +
                                          Chunked);
            }
#endif

            // check validity of request delimiters
            // i.e.: will we be able to tell when the request is over?
            // if not, return a bad request and close the connection
            if ((string.Compare(Method, "post", true, CultureInfo.InvariantCulture) == 0 ||
                 string.Compare(Method, "put", true, CultureInfo.InvariantCulture) == 0) && !Uploading)
            {
                //
                // connection Keep Alive, neither Chunking nor Content Length
                // sorry, can't do it: will not be able to tell the end of the body
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                              "::InterpretHeaders() Client's not chunking, didn't set Content-Length and wants to do Keep-Alive... what a fool!");
                }
#endif
                return false;
            }

            if (!Uploading)
            {
                //
                // we can already start receiving the next request
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                              "::InterpretHeaders() Client's not uploading calling Close()");
                }
#endif
                Close();
            }

            return true;
        }

        public void Send100Continue()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::Send100Continue()");
            }
#endif
            //
            // don't send if we don't need to
            //
            if (Delay100Continue < 0 || _sent100Continue)
            {
                return;
            }
            //
            // make sure we've interpreted headers
            //
            InterpretHeaders();

            var expect = Headers["Expect"];

            if (expect != null && expect.ToLower(CultureInfo.InvariantCulture).IndexOf("100-continue") >= 0 && Uploading)
            {
                //
                // send 100-continue here
                //
                _sent100Continue = true;

                if (Delay100Continue > 0)
                {
                    //
                    // this feature is useful for testing
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::Send100Continue() delaying Send100Continue() for:" +
                                                  Delay100Continue);
                    }
#endif
                    Thread.Sleep(Delay100Continue);
                }

                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                var checkSocket = _connectionState.ConnectionSocket;
                if (checkSocket != null)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::Send100Continue() Send100Continue() sending 100 Continue on the socket");
                    }
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::Send100Continue() calling Socket.Send()");
                    }
#endif
                    checkSocket.Send(HttpWebListener.ResponseTo100ContinueBytes);
                }
            }
        }

        public Stream GetRequestStream()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::GetRequestStream()");
            }
#endif
            if (_stream == null)
            {
                if (!_connectionState.Listener.Auto100Continue)
                {
                    //
                    // if the client shut off this feature and hasn't sent
                    // the 100 Continue then we will send it here.
                    // (if Delay100Continue is -1 we still won't send it at all)
                    //
                    Send100Continue();
                }
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                              "::GetRequestStream() creating ListenerRequestStream()");
                }
#endif
                _stream = new HttpListenerRequestStream(_connectionState, ContentLength, Chunked);
            }
            return _stream;
        }

        public HttpListenerWebResponse GetResponse()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::GetResponse()");
            }
#endif
            if (_response == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                              "::GetResponse() creating HttpListenerWebResponse()");
                }
#endif
                _response = new HttpListenerWebResponse(this);
            }
            return _response;
        }

        public void Close()
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::Close() closing Stream");
            }
#endif
            if (Uploading)
            {
                GetRequestStream().Close();
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::Close() calling HandleConnection()");
            }
#endif
            HandleConnection();
        }

        public void HandleConnection()
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                          "::HandleConnection() _keepAlive:" + KeepAlive);
            }
#endif
            if (!_handleConnectionCalled)
            {
                _handleConnectionCalled = true;
                if (KeepAlive)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) +
                                                  "::HandleConnection() KeepAlive calling StartReceiving()");
                    }
#endif
                    //
                    // fix connection parsing state and buffer offsets
                    //
                    _connectionState.ParserState = ParseState.None;
                    _connectionState.StartReceiving();
                }
            }
        }
    }
}