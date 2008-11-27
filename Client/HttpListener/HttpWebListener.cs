//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Terrarium.Net
{
    // This is the main Http Listener class.  Constructing an instance and calling one of the overloaded
    // Start methods will begin listening on a port for HTTP requests.
    // Call the Stop method to stop listening
    // Look at the NetworkEngine.StartHttpNamespaceManager() method to see how to 
    // set up an environment that accepts requests for specific URL's and responds to them.
    public class HttpWebListener
    {
        private const string ResponseTo100Continue =
            "HTTP/1.1 100 Continue\r\nServer: HttpWebListener\r\n\r\n";

        private const string ResponseToBadRequest =
            "HTTP/1.1 400 Bad Request\r\nServer: HttpWebListener\r\n\r\n<HTML><BODY>Bad Request<BODY></HTML>";

        private static readonly byte[] _responseToBadRequestBytes = Encoding.ASCII.GetBytes(ResponseToBadRequest);
        private static readonly AsyncCallback _staticAcceptCallback = AcceptCallback;
        public static readonly byte[] ResponseTo100ContinueBytes = Encoding.ASCII.GetBytes(ResponseTo100Continue);
        public static AsyncCallback staticReceiveCallback = ReceiveCallback;

        private readonly ArrayList _acceptedConnections = new ArrayList();
        private readonly Queue _pendingRequests = new Queue();
        private readonly ManualResetEvent _requestReady = new ManualResetEvent(false);
        private Socket _accepterSocket;

        private bool _auto100Continue = true;
        private IPEndPoint _IPEndPoint;
        private bool _prefixFiltering;
        private int _timeout;

        public IPEndPoint LocalEndPoint
        {
            get { return _IPEndPoint; }
        }

        public int Timeout
        {
            get { return _timeout; }

            set { _timeout = value; }
        }

        public ManualResetEvent RequestReady
        {
            get { return _requestReady; }
        }

        public int ActiveConnections
        {
            get { return HttpConnectionState.ActiveConnections; }
        }

        public bool Auto100Continue
        {
            get { return _auto100Continue; }

            set { _auto100Continue = value; }
        }

        public void Start(int port)
        {
            Start(port, false);
        }

        public void Start(int port, bool prefixFiltering)
        {
            Start(new IPEndPoint(IPAddress.Any, port), prefixFiltering);
        }

        public void Start(IPEndPoint ipEndPoint)
        {
            Start(ipEndPoint, false);
        }

        public void Start(IPEndPoint ipEndPoint, bool prefixFiltering)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                          "::Start() ipEndPoint:" + ipEndPoint + " prefixFiltering:" + prefixFiltering);
            }
#endif
            if (_prefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                              "::Start() throwing: " + exception);
                }
#endif
                throw exception;
            }
            _IPEndPoint = ipEndPoint;
            _timeout = System.Threading.Timeout.Infinite;
            _requestReady.Reset();
            _prefixFiltering = prefixFiltering;

            _accepterSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // this might throw if the address is already in use
            _accepterSocket.Bind(_IPEndPoint);
            _accepterSocket.Listen((int) SocketOptionName.MaxConnections);
            _accepterSocket.BeginAccept(_staticAcceptCallback, this);
        }

        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            var connectionState = asyncResult.AsyncState as HttpConnectionState;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                          "::ReceiveCallback()");
            }
#endif

            var read = 0;
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            var checkSocket = connectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() calling Socket.EndReceive()");
                    }
#endif
                    read = checkSocket.EndReceive(asyncResult);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() caught exception in Socket.EndReceive():" +
                                                  exception);
                    }
#endif
                    read = -1;
                }
            }

            if (read <= 0)
            {
                goto cleanup;
            }

            connectionState.EndOfOffset += read;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                          "::ReceiveCallback() EndReceive() returns: " + read + " m_EofOffset: " +
                                          connectionState.EndOfOffset);
            }
#endif
            ParseState parseState;
            try
            {
                parseState = connectionState.Parse();
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                              "::ReceiveCallback() caught exception in ConnectionState.Parse(): " +
                                              exception);
                }
#endif
                goto cleanup;
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                          "::ReceiveCallback() Parse() returns: " + parseState);
            }
#endif
            if (parseState == ParseState.Continue)
            {
                //
                // need more data, keep reading.
                //
                connectionState.StartReceiving();
                return;
            }

            if (parseState == ParseState.Done)
            {
                //
                // we're done, we have a request ready. add it to the queue. if the request
                // has no content length and it's keep alive, we should kick off reading another
                // one otherwise delegate to the request stream to kick it once it's done.
                // (this would add support for pipelining)
                //
                if (connectionState.Request.ContentLength == 0)
                {
                }

                lock (connectionState.Listener._pendingRequests)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() calling Enqueue()");
                    }
#endif
                    connectionState.Listener._pendingRequests.Enqueue(connectionState.Request);
                }

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                              "::ReceiveCallback() calling Set()");
                }
#endif
                connectionState.Listener._requestReady.Set();
                return;
            }

            cleanup:
            //
            // some error. clean up
            //
            if (read > 0)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                              "::ReceiveCallback() calling Send(_responseToBadRequestBytes)");
                }
#endif

                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() calling Socket.Send()");
                    }
#endif
                    checkSocket.Send(_responseToBadRequestBytes);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() caught exception in Socket.Send(): " + exception);
                    }
#endif
                }
            }

            lock (connectionState.Listener._acceptedConnections)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                              "::ReceiveCallback() calling Remove()");
                }
#endif

                try
                {
                    connectionState.Listener._acceptedConnections.Remove(connectionState);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                                  "::ReceiveCallback() caught exception in ArrayList.Remove(): " +
                                                  exception);
                    }
#endif
                }

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) +
                                              "::ReceiveCallback() calling Close()");
                }
#endif
                connectionState.Close();
            }
        }

        public HttpListenerWebRequest GetNextRequest()
        {
            HttpListenerWebRequest httpListenerRequest = null;
            lock (_pendingRequests)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                              "::GetNextRequest() _pendingRequests.Count: " + _pendingRequests.Count);
                }
#endif

                if (_pendingRequests.Count > 0)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                                  "::GetNextRequest() calling Dequeue()");
                    }
#endif

                    httpListenerRequest = (HttpListenerWebRequest) _pendingRequests.Dequeue();

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                                  "::GetNextRequest() calling Reset()");
                    }
#endif

                    _requestReady.Reset();
                }
            }
            return httpListenerRequest;
        }

        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            var httpWebListener = asyncResult.AsyncState as HttpWebListener;
            Socket acceptedSocket;
            try
            {
                acceptedSocket = httpWebListener._accepterSocket.EndAccept(asyncResult);
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) +
                                              "::Accepter() caught exception in Socket.EndAccept(): " + exception);
                }
#endif
                httpWebListener.Stop();
                return;
            }

            try
            {
                httpWebListener._accepterSocket.BeginAccept(_staticAcceptCallback, httpWebListener);
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) +
                                              "::Accepter() caught exception in Socket.BeginAccept(): " + exception);
                }
#endif
                httpWebListener.Stop();
                return;
            }

            var connectionState = new HttpConnectionState(acceptedSocket, 4096, httpWebListener);
            lock (httpWebListener._acceptedConnections)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) +
                                              "::Accepter() calling Add()");
                }
#endif
                try
                {
                    httpWebListener._acceptedConnections.Add(connectionState);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) +
                                                  "::Accepter() caught exception in ArrayList.Add(): " + exception);
                    }
#endif
                    return;
                }
            }
            //
            // now kick off async reading
            //
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) +
                                          "::Accepter() calling BeginReceive()");
            }
#endif
            connectionState.StartReceiving();
        }

        public void Stop()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::Stop()");
            }
#endif
            //
            // this Close() will cause any pending Accept()
            // to complete and throw, so the thread will cleanup and exit.
            //
            if (_accepterSocket != null)
            {
                _accepterSocket.Close();
                _accepterSocket = null;
            }

            lock (_acceptedConnections)
            {
                foreach (HttpConnectionState connectionState in _acceptedConnections)
                {
                    connectionState.Close();
                }
                _acceptedConnections.Clear();
            }

            lock (_pendingRequests)
            {
                foreach (HttpListenerWebRequest httpListenerRequest in  _pendingRequests)
                {
                    httpListenerRequest.Close();
                }
                _pendingRequests.Clear();
            }

            _requestReady.Reset();
            _IPEndPoint = null;
        }

        public void AddUriPrefix(Uri uriPrefix)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                          "::AddUriPrefix() uriPrefix:" + uriPrefix);
            }
#endif
            if (_prefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                              "::AddUriPrefix() throwing: " + exception);
                }
#endif
                throw exception;
            }
        }

        public void RemoveUriPrefix(Uri uriPrefix)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                          "::RemoveUriPrefix() uriPrefix:" + uriPrefix);
            }
#endif
            if (_prefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                              "::RemoveUriPrefix() throwing: " + exception);
                }
#endif
                throw exception;
            }
        }

        public IAsyncResult BeginGetRequest(AsyncCallback callback, object stateObject)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::BeginGetRequest()");
            }
#endif
            var asyncResult = new HttpListenerAsyncResult(callback, stateObject, this) {Request = GetNextRequest()};

            //
            // check to see if there are requests in the queue
            //

            if (asyncResult.Request == null)
            {
                //
                // if not go async
                //
                ThreadPool.RegisterWaitForSingleObject(
                    _requestReady,
                    HttpListenerAsyncResult.StaticCallback,
                    asyncResult,
                    -1,
                    true);
            }
            else
            {
                //
                // otherwise complete sync
                //
                asyncResult.Complete(true);
            }

            return asyncResult;
        }

        public HttpListenerWebRequest EndGetRequest(IAsyncResult asyncResult)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                          "::EndGetRequest() asyncResult#" + HttpTraceHelper.HashString(asyncResult));
            }
#endif
            var castedAsyncResult = asyncResult as HttpListenerAsyncResult;
            if (!castedAsyncResult.IsCompleted)
            {
                castedAsyncResult.AsyncWaitHandle.WaitOne(System.Threading.Timeout.Infinite, false);
            }
            return castedAsyncResult.Request;
        }

        public HttpListenerWebRequest GetRequest()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetRequest()");
            }
#endif
            var asyncResult = BeginGetRequest(null, null) as HttpListenerAsyncResult;
            if (_timeout != System.Threading.Timeout.Infinite && !asyncResult.IsCompleted)
            {
                asyncResult.AsyncWaitHandle.WaitOne(_timeout, false);
                if (!asyncResult.IsCompleted)
                {
                    var exception = new Exception("Timeout");
#if DEBUG
                    if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) +
                                                  "::GetRequest() throwing: " + exception);
                    }
#endif
                    throw exception;
                }
            }
            return asyncResult.Request;
        }
    }
}