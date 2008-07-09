//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
        private ArrayList m_AcceptedConnections = new ArrayList();
        private bool m_PrefixFiltering;
        private Queue m_PendingRequests = new Queue();
        private ManualResetEvent m_RequestReady = new ManualResetEvent(false);
        private IPEndPoint m_IPEndPoint;
        private Socket m_AccepterSocket;

        private const string responseToBadRequest = "HTTP/1.1 400 Bad Request\r\nServer: HttpWebListener\r\n\r\n<HTML><BODY>Bad Request<BODY></HTML>";
        private static readonly byte[] responseToBadRequestBytes = Encoding.ASCII.GetBytes(responseToBadRequest);

        private const string responseTo100Continue = "HTTP/1.1 100 Continue\r\nServer: HttpWebListener\r\n\r\n";
        public static readonly byte[] responseTo100ContinueBytes = Encoding.ASCII.GetBytes(responseTo100Continue);

        public static AsyncCallback staticReceiveCallback = new AsyncCallback(ReceiveCallback);
        private static AsyncCallback staticAcceptCallback = new AsyncCallback(AcceptCallback);
        private int m_Timeout;
        private bool m_Auto100Continue = true;

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
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::Start() ipEndPoint:" + ipEndPoint.ToString() + " prefixFiltering:" + prefixFiltering.ToString() );
            }
#endif
            if (m_PrefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::Start() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
            m_IPEndPoint = ipEndPoint;
            m_Timeout = System.Threading.Timeout.Infinite;
            m_RequestReady.Reset();
            m_PrefixFiltering = prefixFiltering;

            m_AccepterSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // this might throw if the address is already in use
            m_AccepterSocket.Bind(m_IPEndPoint);
            m_AccepterSocket.Listen((int)SocketOptionName.MaxConnections);
            m_AccepterSocket.BeginAccept(staticAcceptCallback, this);
        }

        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            HttpConnectionState connectionState = asyncResult.AsyncState as HttpConnectionState;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback()");
            }
#endif

            int read = 0;
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            Socket checkSocket = connectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Socket.EndReceive()");
                    }
#endif
                    read = checkSocket.EndReceive(asyncResult);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() caught exception in Socket.EndReceive():" + exception.ToString());
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
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() EndReceive() returns: " + read.ToString() + " m_EofOffset: " + connectionState.EndOfOffset.ToString());
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
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() caught exception in ConnectionState.Parse(): " + exception.ToString());
                }
#endif
                goto cleanup;
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() Parse() returns: " + parseState.ToString());
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
                else
                {
                }

                lock (connectionState.Listener.m_PendingRequests)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Enqueue()");
                    }
#endif
                    connectionState.Listener.m_PendingRequests.Enqueue(connectionState.Request);
                }

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Set()");
                }
#endif
                connectionState.Listener.m_RequestReady.Set();
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
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Send(responseToBadRequestBytes)");
                    }
#endif

                    try
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Socket.Send()");
                        }
#endif
                        checkSocket.Send(responseToBadRequestBytes);
                    }
                    catch (Exception exception)
                    {
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() caught exception in Socket.Send(): " + exception.ToString());
                        }
#endif
                    }
                }

            lock (connectionState.Listener.m_AcceptedConnections)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Remove()");
                }
#endif

                try
                {
                    connectionState.Listener.m_AcceptedConnections.Remove(connectionState);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() caught exception in ArrayList.Remove(): " + exception.ToString());
                    }
#endif
                }

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(connectionState) + "::ReceiveCallback() calling Close()");
                }
#endif
                connectionState.Close();
            }
        }

        public HttpListenerWebRequest GetNextRequest()
        {
            HttpListenerWebRequest httpListenerRequest = null;
            lock (this.m_PendingRequests)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetNextRequest() m_PendingRequests.Count: " + m_PendingRequests.Count.ToString());
                }
#endif

                if (m_PendingRequests.Count > 0)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetNextRequest() calling Dequeue()");
                    }
#endif

                    httpListenerRequest = (HttpListenerWebRequest)m_PendingRequests.Dequeue();

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetNextRequest() calling Reset()");
                    }
#endif

                    m_RequestReady.Reset();
                }
            }
            return httpListenerRequest;
        }

        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            HttpWebListener httpWebListener = asyncResult.AsyncState as HttpWebListener;
            Socket acceptedSocket = null;
            try
            {
                acceptedSocket = httpWebListener.m_AccepterSocket.EndAccept(asyncResult);
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) + "::Accepter() caught exception in Socket.EndAccept(): " + exception.ToString());
                }
#endif
                httpWebListener.Stop();
                return;
            }

            try
            {
                httpWebListener.m_AccepterSocket.BeginAccept(staticAcceptCallback, httpWebListener);
            }
            catch (Exception exception)
            {
#if DEBUG
                if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) + "::Accepter() caught exception in Socket.BeginAccept(): " + exception.ToString());
                }
#endif
                httpWebListener.Stop();
                return;
            }

            HttpConnectionState connectionState = new HttpConnectionState(acceptedSocket, 4096, httpWebListener);
            lock (httpWebListener.m_AcceptedConnections)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) + "::Accepter() calling Add()");
                }
#endif
                try
                {
                    httpWebListener.m_AcceptedConnections.Add(connectionState);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) + "::Accepter() caught exception in ArrayList.Add(): " + exception.ToString());
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
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(httpWebListener) + "::Accepter() calling BeginReceive()");
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
            if (m_AccepterSocket != null)
            {
                m_AccepterSocket.Close();
                m_AccepterSocket = null;
            }

            lock (m_AcceptedConnections)
            {
                foreach (HttpConnectionState connectionState in m_AcceptedConnections)
                {
                    connectionState.Close();
                }
                m_AcceptedConnections.Clear();
            }

            lock (m_PendingRequests)
            {
                foreach (HttpListenerWebRequest httpListenerRequest in  m_PendingRequests) 
                {
                    httpListenerRequest.Close();
                }                
                m_PendingRequests.Clear();
            }

            m_RequestReady.Reset();
            m_IPEndPoint = null;
        }

        public void AddUriPrefix(Uri uriPrefix)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::AddUriPrefix() uriPrefix:" + uriPrefix.ToString());
            }
#endif
            if (m_PrefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::AddUriPrefix() throwing: " + exception.ToString());
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
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::RemoveUriPrefix() uriPrefix:" + uriPrefix.ToString());
            }
#endif
            if (m_PrefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::RemoveUriPrefix() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
        }

        //
        // if a request comes in for a Uri that doesn't match
        // the current set of Registered prefixes we return
        // an error and close the connection on the client
        //
        private bool CanServe(Uri uri)
        {
            if (m_PrefixFiltering)
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::CanServe() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
            return true;
        }

        public IAsyncResult BeginGetRequest(AsyncCallback callback, object stateObject)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::BeginGetRequest()");
            }
#endif
            HttpListenerAsyncResult asyncResult = new HttpListenerAsyncResult(callback, stateObject, this);

            //
            // check to see if there are requests in the queue
            //
            asyncResult.Request = GetNextRequest();

            if (asyncResult.Request == null)
            {
                //
                // if not go async
                //
                ThreadPool.RegisterWaitForSingleObject(
                    m_RequestReady,
                    HttpListenerAsyncResult.StaticCallback,
                    asyncResult,
                    -1,
                    true );
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
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::EndGetRequest() asyncResult#" + HttpTraceHelper.HashString(asyncResult));
            }
#endif
            HttpListenerAsyncResult castedAsyncResult = asyncResult as HttpListenerAsyncResult;
            if (!castedAsyncResult.IsCompleted)
            {
                castedAsyncResult.AsyncWaitHandle.WaitOne(System.Threading.Timeout.Infinite, false);
            }
            return castedAsyncResult.Request;
        }

        public IPEndPoint LocalEndPoint
        {
            get
            {
                return m_IPEndPoint;
            }
        }

        public int Timeout
        {
            get
            {
                return m_Timeout;
            }
            
            set
            {
                m_Timeout = value;
            }
        }

        public ManualResetEvent RequestReady
        {
            get
            {
                return m_RequestReady;
            }
        }

        public int ActiveConnections
        {
            get
            {
                return HttpConnectionState.ActiveConnections;
            }
        }


        public bool Auto100Continue
        {
            get
            {
                return m_Auto100Continue;
            }
            
            set
            {
                m_Auto100Continue = value;
            }
        }

        public HttpListenerWebRequest GetRequest()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetRequest()");
            }
#endif
            HttpListenerAsyncResult asyncResult = BeginGetRequest(null, null) as HttpListenerAsyncResult;
            if (m_Timeout != System.Threading.Timeout.Infinite && !asyncResult.IsCompleted)
            {
                asyncResult.AsyncWaitHandle.WaitOne(m_Timeout, false);
                if (!asyncResult.IsCompleted)
                {
                    Exception exception = new Exception("Timeout");
#if DEBUG
                    if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpWebListener#" + HttpTraceHelper.HashString(this) + "::GetRequest() throwing: " + exception.ToString());
                    }
#endif
                    throw exception;
                }
            }
            return asyncResult.Request;
        }
    }
}