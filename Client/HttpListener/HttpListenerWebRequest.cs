//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Terrarium.Net
{
    public class HttpListenerWebRequest
    {
        private readonly HttpConnectionState _connectionState;
        private readonly IPEndPoint _remoteEndPoint;
        private bool _chunked;
        private string _connection;
        private long _contentLength;
        private int _delay100Continue;
        private int _delayResponse;
        private bool _handleConnectionCalled;
        private WebHeaderCollection _headers;
        private string _host;
        private bool _interpretHeadersCalled;
        private bool _keepAlive;
        private string _method;
        private Version _protocolVersion;
        private string _relativeUri;
        private Uri _requestUri;
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
            _remoteEndPoint = (IPEndPoint) (connectionState.ConnectionSocket.RemoteEndPoint);
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

                return _chunked || _contentLength > 0;
            }
        }

        public int DelayResponse
        {
            get { return _delayResponse; }
            set { _delayResponse = value; }
        }

        public int Delay100Continue
        {
            get { return _delay100Continue; }

            set { _delay100Continue = value; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return _remoteEndPoint; }
        }

        public string RelativeUri
        {
            get { return _relativeUri; }
            set { _relativeUri = value; }
        }

        public Uri RequestUri
        {
            get { return _requestUri; }
            set { _requestUri = value; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public Version ProtocolVersion
        {
            get { return _protocolVersion; }
            set { _protocolVersion = value; }
        }

        public string Host
        {
            get { return _host; }
        }

        public bool Chunked
        {
            get { return _chunked; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
        }

        public bool KeepAlive
        {
            get { return _keepAlive; }
        }

        public string Connection
        {
            get { return _connection; }
        }

        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

        public bool InterpretHeaders()
        {
            //
            // when all the headers are parsed, this method will
            // interpret the values of the "special" headers and set
            // the corresponding properties to the correct values.
            //
            if (_headers == null)
            {
                Exception exception = new Exception("Don't have headers yet");
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

            string value;

            //
            // Connection && KeepAlive
            //
            _connection = _headers["Connection"];
            _keepAlive = _connection == null ||
                          _connection.ToLower(CultureInfo.InvariantCulture).IndexOf("close") == -1;

            //
            // RequestUri
            //
            _host = _headers["Host"];
            _requestUri = null;
            // we should try this only if this is a fully qualified Uri
            if (_relativeUri.StartsWith("http://") || _relativeUri.StartsWith("https://"))
            {
                try
                {
                    _requestUri = new Uri(_relativeUri);
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

            if (_requestUri == null || !(_requestUri.Scheme.Equals("http") || _requestUri.Scheme.Equals("https")))
            {
                try
                {
                    if (_relativeUri[0] != '/')
                    {
                        _requestUri = new Uri("http://" + _host + "/" + _relativeUri);
                    }
                    else
                    {
                        _requestUri = new Uri("http://" + _host + _relativeUri);
                    }
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
            value = _headers["Transfer-Encoding"];
            _chunked = value != null && value.ToLower(CultureInfo.InvariantCulture).IndexOf("chunked") >= 0;

            //
            // ContentLength
            //
            if (_chunked)
            {
                //
                // if the client is chunking we ignore Content-Length
                //
                _contentLength = -1;
            }
            else
            {
                _contentLength = 0;
                value = _headers["Content-Length"];
                if (value != null)
                {
                    try
                    {
                        _contentLength = Int64.Parse(value);
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
                                          "::InterpretHeaders() ContentLength:" + _contentLength + " Chunked:" +
                                          _chunked);
            }
#endif

            // check validity of request delimiters
            // i.e.: will we be able to tell when the request is over?
            // if not, return a bad request and close the connection
            if ((string.Compare(_method, "post", true, CultureInfo.InvariantCulture) == 0 ||
                 string.Compare(_method, "put", true, CultureInfo.InvariantCulture) == 0) && !Uploading)
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

            string expect = _headers["Expect"];

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
                Socket checkSocket = _connectionState.ConnectionSocket;
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
                _stream = new HttpListenerRequestStream(_connectionState, _contentLength, _chunked);
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
                                          "::HandleConnection() _keepAlive:" + _keepAlive);
            }
#endif
            if (!_handleConnectionCalled)
            {
                _handleConnectionCalled = true;
                if (_keepAlive)
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