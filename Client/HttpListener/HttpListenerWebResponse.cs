//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Terrarium.Net
{
    public class HttpListenerWebResponse 
    {
        private Version m_ProtocolVersion;
        private HttpStatusCode m_StatusCode;
        private string m_StatusDescription;

        private bool m_BufferHeaders;
        private bool m_Chunked;
        private bool m_KeepAlive;
        private string m_Server;
        private DateTime m_Date;

        private byte[] m_HeadersBuffer;
        private bool m_SentHeaders;

        private WebHeaderCollection m_Headers;

        private long m_ContentLength;
        private string m_ContentType;
        private Stream m_Stream;

        private HttpListenerWebRequest m_HttpListenerWebRequest;

        public HttpListenerWebResponse(HttpListenerWebRequest httpListenerRequest)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::.ctor() keepAlive:" + httpListenerRequest.KeepAlive.ToString());
            }
#endif
            m_HttpListenerWebRequest = httpListenerRequest;
            m_Date = DateTime.MinValue;
            m_Headers = new WebHeaderCollection();
            m_ProtocolVersion = new Version(1,1);
            m_SentHeaders = false;
            m_KeepAlive = httpListenerRequest.KeepAlive;
        }

        public byte[] HeadersBuffer
        {
            get 
            {
                return m_HeadersBuffer;
            }
        }

        public bool SentHeaders
        {
            get
            {
                return m_SentHeaders;
            }

            set
            {
                m_SentHeaders = value;
            }
        }

        public HttpListenerWebRequest Request 
        {
            get 
            {
                return m_HttpListenerWebRequest;
            }
        }

        public bool BufferHeaders 
        {
            get
            {
                return m_BufferHeaders;
            }
            
            set 
            {
                m_BufferHeaders = value;
            }
        }

        public bool KeepAlive 
        {
            get
            {
                return m_KeepAlive;
            }
            
            set 
            {
                m_KeepAlive = value;
            }
        }

        public bool Chunked 
        {
            get 
            {
                return m_Chunked;
            }
            
            set
            {
                m_Chunked = value;
            }
        }

        public string Server
        {
            get
            {
                return m_Server;
            }
            
            set
            {
                m_Server = value;
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

        public long ContentLength 
        {
            get
            {
                return m_ContentLength;
            }
            
            set 
            {
                m_ContentLength = value;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return m_StatusCode;
            }
            
            set 
            {
                m_StatusCode = value;
            }
        }

        public string StatusDescription 
        {
            get 
            {
                return m_StatusDescription;
            }
            
            set
            {
                m_StatusDescription = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return m_Date;
            }
            
            set
            {
                m_Date = value;
            }
        }

        public string ContentType 
        {
            get
            {
                return m_ContentType;
            }
            
            set 
            {
                m_ContentType = value;
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

        public void Close() 
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close()");
            }
#endif
            if (m_Stream == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close() m_SentHeaders:" + m_SentHeaders.ToString());
                }
#endif

                if (!m_SentHeaders)
                {
                    SerializeHeaders();
                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    Socket checkSocket = m_HttpListenerWebRequest.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close() calling Socket.Send() Length:" + m_HeadersBuffer.Length.ToString());
                        }
#endif
                        checkSocket.Send(m_HeadersBuffer);
                        m_SentHeaders = true;
                    }
                }
                HandleConnection();
            }
            else 
            {
                m_Stream.Close();
            }
        }

        public void Close(byte[] entityData) 
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close(byte[])");
            }
#endif
            if (entityData == null || entityData.Length <= 0 || m_Stream != null)
            {
                Close();
                return;
            }

            byte[] mergedBuffer;

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close() entityData.Length:" + entityData.Length.ToString() + " m_SentHeaders:" + m_SentHeaders.ToString());
            }
#endif

            if (!m_SentHeaders)
            {
                SerializeHeaders();
                mergedBuffer = new byte[m_HeadersBuffer.Length + entityData.Length];

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close() merging buffers mergedBuffer.Length:" + mergedBuffer.Length.ToString());
                }
#endif

                Buffer.BlockCopy(m_HeadersBuffer, 0, mergedBuffer, 0, m_HeadersBuffer.Length);
                Buffer.BlockCopy(entityData, 0, mergedBuffer, m_HeadersBuffer.Length, entityData.Length);
            }
            else
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::SerializeHeaders() no merge: mergedBuffer = entityData");
                }
#endif

                mergedBuffer = entityData;
            }
#if DEBUG
            if (HttpTraceHelper.Socket.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close() calling Socket.Send() Length:" + mergedBuffer.Length.ToString());
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            Socket checkSocket = m_HttpListenerWebRequest.ConnectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                checkSocket.Send(mergedBuffer);
                m_SentHeaders = true;
            }
            HandleConnection();
        }

        public void HandleConnection()
        {
            //
            // cleans up request side and decides on wether to close the connection or not.
            //
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::HandleConnection() calling HttpListenerWebRequest#" + HttpTraceHelper.HashString(this) + "::HandleConnection()");
            }
#endif

            m_HttpListenerWebRequest.HandleConnection();

            if (!m_KeepAlive)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::HandleConnection() !KeepAlive closing the Connection");
                }
#endif
                m_HttpListenerWebRequest.ConnectionState.Close();
            }
        }

        private void SerializeHeaders()
        {
            if (m_HeadersBuffer == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::SerializeHeaders()");
                }
#endif

                //
                // format headers and send them on the wire
                //
                StringBuilder stringBuilder = new StringBuilder(1024);

                stringBuilder.Append("HTTP/");
                stringBuilder.Append(m_ProtocolVersion.Major.ToString());
                stringBuilder.Append(".");
                stringBuilder.Append(m_ProtocolVersion.Minor.ToString());
                stringBuilder.Append(" ");
                stringBuilder.Append(((int)m_StatusCode).ToString());
                stringBuilder.Append(" ");
                if (m_StatusDescription == null || m_StatusDescription.Length==0)
                {
                    m_StatusDescription = m_StatusCode.ToString();
                }
                stringBuilder.Append(m_StatusDescription);
                stringBuilder.Append("\r\n");

                if (m_Server != null)
                {
                    m_Headers["Server"] = m_Server;
                }
                if (!m_Date.Equals(DateTime.MinValue))
                {
                    m_Headers["Date"] = m_Date.ToString("R");
                }
                if (!m_KeepAlive)
                {
                    m_Headers["Connection"] = "Close";
                }
                if (m_Chunked)
                {
                    m_Headers.Remove("Content-Length");
                    m_Headers["Transfer-Encoding"] = "Chunked";
                }
                else
                {
                    m_Headers["Content-Length"] = m_ContentLength.ToString("D");
                    m_Headers.Remove("Transfer-Encoding");
                }

                if (ContentType != null)
                {
                    m_Headers["Content-Type"] = ContentType;
                }

                if (m_Headers != null)
                {
                    for (int i = 0; i < m_Headers.Count ; i++)
                    {
                        stringBuilder.Append((string)m_Headers.GetKey(i));
                        stringBuilder.Append(": ");
                        stringBuilder.Append((string)m_Headers.Get(i));
                        stringBuilder.Append("\r\n");
                    }
                }
                stringBuilder.Append("\r\n");

                if (m_HttpListenerWebRequest.DelayResponse > 0)
                {
                    //
                    // this feature is useful for testing
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::SerializeHeaders() delaying Response() for:" + m_HttpListenerWebRequest.DelayResponse.ToString());
                    }
#endif
                    Thread.Sleep(m_HttpListenerWebRequest.DelayResponse);
                }

                m_HeadersBuffer = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            }
        }

        public Stream GetResponseStream()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::GetResponseStream()");
            }
#endif
            if (m_Stream == null)
            {
                //
                // if m_BufferHeaders is true we won't send the headers here, but we'll
                // do it later. at the latest when the user closes the response stream
                //
                if (!m_BufferHeaders)
                {
                    //
                    // this call can throw, the exception will be presented to the user
                    // calling GetResponseStream()
                    //
                    SerializeHeaders();

                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    Socket checkSocket = m_HttpListenerWebRequest.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::GetResponseStream() calling Socket.Send() Length:" + m_HeadersBuffer.Length.ToString());
                        }
#endif
                        checkSocket.Send(m_HeadersBuffer);
                        m_SentHeaders = true;
                    }
                }

                m_Stream = new HttpListenerResponseStream(this, m_ContentLength, m_Chunked);
            }
            return m_Stream;
        }
    }
}