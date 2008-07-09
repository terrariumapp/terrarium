//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

namespace Terrarium.Net
{
    public class HttpListenerWebRequest
    {
        private string m_Method;
        private Uri m_RequestUri;
        private string m_RelativeUri;
        private Version m_ProtocolVersion;
        private WebHeaderCollection m_Headers;
        private string m_Host;
        private long m_ContentLength;
        private string m_Connection;
        private bool m_Chunked;
        private bool m_Sent100Continue;
        private int m_Delay100Continue;
        private int m_DelayResponse;
        private Stream m_Stream;
        private HttpListenerWebResponse m_Response;
        private bool m_InterpretHeadersCalled;
        private bool m_HandleConnectionCalled;
        private IPEndPoint m_RemoteEndPoint;

        private bool m_KeepAlive;
        private HttpConnectionState m_ConnectionState;

        public HttpConnectionState ConnectionState
        {
            get
            {
                return m_ConnectionState;
            }
        }

        public HttpListenerWebRequest(HttpConnectionState connectionState)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::.ctor()");
            }
#endif
            m_ConnectionState = connectionState;
#if DEBUG
            if (HttpTraceHelper.Socket.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::.ctor() calling Socket.get_RemoteEndPoint()");
            }
#endif
            m_RemoteEndPoint = (IPEndPoint)(connectionState.ConnectionSocket.RemoteEndPoint);
        }

        public bool InterpretHeaders()
        {
            //
            // when all the headers are parsed, this method will
            // interpret the values of the "special" headers and set
            // the corresponding properties to the correct values.
            //
            if (m_Headers == null)
            {
                Exception exception = new Exception("Don't have headers yet");
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }

            if (m_InterpretHeadersCalled)
            {
                return false;
            }

            m_InterpretHeadersCalled = true;

            string value;

            //
            // Connection && KeepAlive
            //
            m_Connection = m_Headers["Connection"];
            m_KeepAlive = m_Connection==null || m_Connection.ToLower(CultureInfo.InvariantCulture).IndexOf("close")==-1;

            //
            // RequestUri
            //
            m_Host = m_Headers["Host"];
            m_RequestUri = null;
            // we should try this only if this is a fully qualified Uri
            if (m_RelativeUri.StartsWith("http://") || m_RelativeUri.StartsWith("https://"))
            {
                try
                {
                    m_RequestUri = new Uri(m_RelativeUri);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() caught exception in Uri..ctor(): " + exception.ToString());
                    }
#endif
                }
            }
            
            if (m_RequestUri == null || !(m_RequestUri.Scheme.Equals("http") || m_RequestUri.Scheme.Equals("https")))
            {
                try
                {
                    if (m_RelativeUri[0] != '/')
                    {
                        m_RequestUri = new Uri("http://" + m_Host + "/" + m_RelativeUri);
                    }
                    else
                    {
                        m_RequestUri = new Uri("http://" + m_Host + m_RelativeUri);
                    }
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() caught exception in Uri..ctor(): " + exception.ToString());
                    }
#endif
                }
            }

            //
            // Chunked
            //
            value = m_Headers["Transfer-Encoding"];
            m_Chunked = value!=null && value.ToLower(CultureInfo.InvariantCulture).IndexOf("chunked")>=0;

            //
            // ContentLength
            //
            if (m_Chunked)
            {
                //
                // if the client is chunking we ignore Content-Length
                //
                m_ContentLength = -1;
            }
            else
            {
                m_ContentLength = 0;
                value = m_Headers["Content-Length"];
                if (value != null)
                {
                    try
                    {
                        m_ContentLength = Int64.Parse(value);
                    }
                    catch (Exception exception)
                    {
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() caught exception in Int64.Parse(): " + exception.ToString());
                        }
#endif
                    }
                }
            }

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() ContentLength:" + m_ContentLength.ToString() + " Chunked:" + m_Chunked.ToString());
            }
#endif

            //
            // check validity of request delimiters
            // i.e.: will we be able to tell when the request is over?
            // if not, return a bad request and close the connection
            //
            if ((string.Compare(m_Method,"post",true, CultureInfo.InvariantCulture) == 0 || string.Compare(m_Method,"put",true,CultureInfo.InvariantCulture) == 0) && !Uploading)
            {
                //
                // connection Keep Alive, neither Chunking nor Content Length
                // sorry, can't do it: will not be able to tell the end of the body
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() Client's not chunking, didn't set Content-Length and wants to do Keep-Alive... what a fool!");
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
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::InterpretHeaders() Client's not uploading calling Close()");
                }
#endif
                Close();
            }

            return true;
        }

        public bool Uploading
        {
            get
            {
                //
                // make sure we interpreted headers
                //
                InterpretHeaders();

                return m_Chunked || m_ContentLength>0;
            }
        }

        public void Send100Continue()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Send100Continue()");
            }
#endif
            //
            // don't send if we don't need to
            //
            if (Delay100Continue < 0 || m_Sent100Continue)
            {
                return;
            }
            //
            // make sure we've interpreted headers
            //
            InterpretHeaders();

            string expect = m_Headers["Expect"];

            if (expect != null && expect.ToLower(CultureInfo.InvariantCulture).IndexOf("100-continue")>=0 && Uploading)
            {
                //
                // send 100-continue here
                //
                m_Sent100Continue = true;

                if (Delay100Continue > 0)
                {
                    //
                    // this feature is useful for testing
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Send100Continue() delaying Send100Continue() for:" + Delay100Continue.ToString());
                    }
#endif
                    Thread.Sleep(Delay100Continue);
                }

                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                Socket checkSocket = m_ConnectionState.ConnectionSocket;
                if (checkSocket != null)
                {

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Send100Continue() Send100Continue() sending 100 Continue on the socket");
                    }
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Send100Continue() calling Socket.Send()");
                    }
#endif
                    checkSocket.Send(HttpWebListener.responseTo100ContinueBytes);
                }
            }
        }

        public int DelayResponse
        {
            get
            {
                return m_DelayResponse;
            }
        
            set
            {
                m_DelayResponse = value;
            }
        }

        public int Delay100Continue
        {
            get
            {
                return m_Delay100Continue;
            }
            
            set
            {
                m_Delay100Continue = value;
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return m_RemoteEndPoint;
            }
        }

        public string RelativeUri
        {
            get
            {
                return m_RelativeUri;
            }
            set
            {
                m_RelativeUri = value;
            }
        }

        public Uri RequestUri 
        {
            get 
            {
                return m_RequestUri;
            }
            
            set 
            {
                m_RequestUri = value;
            }
        }

        public string Method 
        {
            get
            {
                return m_Method;
            }
            
            set 
            {
                m_Method = value;
            }
        }

        public Version ProtocolVersion
        {
            get
            {
                return m_ProtocolVersion;
            }
            
            set 
            {
                m_ProtocolVersion = value;
            }
        }

        public string Host
        {
            get
            {
                return m_Host;
            }
        }

        public bool Chunked 
        {
            get 
            {
                return m_Chunked;
            }
        }

        public long ContentLength 
        {
            get 
            {
                return m_ContentLength;
            }
        }

        public bool KeepAlive 
        {
            get
            {
                return m_KeepAlive;
            }
        }

        public string Connection 
        {
            get
            {
                return m_Connection;
            }
        }

        public WebHeaderCollection Headers 
        {
            get
            {
                return m_Headers;
            }
            
            set 
            {
                m_Headers = value;
            }
        }

        public Stream GetRequestStream() 
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::GetRequestStream()");
            }
#endif
            if (m_Stream == null)
            {
                if (!m_ConnectionState.Listener.Auto100Continue)
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
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::GetRequestStream() creating ListenerRequestStream()");
                }
#endif
                m_Stream = new HttpListenerRequestStream(m_ConnectionState, m_ContentLength, m_Chunked);
            }
            return m_Stream;
        }

        public HttpListenerWebResponse GetResponse()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::GetResponse()");
            }
#endif
            if (m_Response == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::GetResponse() creating HttpListenerWebResponse()");
                }
#endif
                m_Response = new HttpListenerWebResponse(this);
            }
            return m_Response;
        }

        public void Close()
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Close() closing Stream");
            }
#endif
            if (Uploading)
            {
                GetRequestStream().Close();
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::Close() calling HandleConnection()");
            }
#endif
            HandleConnection();
        }

        public void HandleConnection()
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::HandleConnection() m_KeepAlive:" + m_KeepAlive.ToString());
            }
#endif
            if (!m_HandleConnectionCalled)
            {
                m_HandleConnectionCalled = true;
                if (m_KeepAlive)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::HandleConnection() KeepAlive calling StartReceiving()");
                    }
#endif
                    //
                    // fix connection parsing state and buffer offsets
                    //
                    m_ConnectionState.ParserState = ParseState.None;
                    m_ConnectionState.StartReceiving();
                }
            }
        }
    }
}