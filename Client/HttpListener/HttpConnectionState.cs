//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Terrarium.Net
{
    // Represents an active Http Connection.  When a request comes in, m_HttpListenerWebRequest
    // is filled in incrementally by the Parse method (which parses the Http request) until we have the full request.  
    // This class interacts with HttpWebListener.ReceiveCallback to fully read and parse the request asynchronously.
    public class HttpConnectionState
    {
        private Socket m_Socket;
        private HttpWebListener m_HttpWebListener;
        private HttpListenerWebRequest m_HttpListenerWebRequest;
        private byte[] m_Buffer;
        private int m_EofOffset;
        private int m_ParsedOffset;
        private ParseState m_ParserState;
        private static int m_ActiveConnections = 0;

        public HttpConnectionState(Socket socket, int bufferSize, HttpWebListener httpWebListener)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::ctor() New client connected from: " + socket.RemoteEndPoint.ToString());
            }
#endif
            Interlocked.Increment(ref m_ActiveConnections);
            m_HttpWebListener = httpWebListener;
            m_Socket = socket;
            m_Buffer = new byte[bufferSize];

            m_ParserState = ParseState.None;
            m_EofOffset = 0;
            m_ParsedOffset = 0;
        }

        ~HttpConnectionState()
        {
            Close();
        }

        public void Close()
        {
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            Socket checkSocket = ConnectionSocket;

            // null out the Socket as soon as possible, it's still possible
            // that two or more callers will execute the method below on the same
            // instance. for now that's ok.
            m_Socket = null;
            if (checkSocket != null)
            {
                // gracefully close (allow user to read the data)
                // ignore failures on the socket
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::ctor() calling Socket.Shutdown()");
                    }
#endif
                    checkSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Close() caught exception in Socket.Shutdown(): " + exception.ToString());
                    }
#endif
                }
#if DEBUG
                if (HttpTraceHelper.Socket.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Close() calling Socket.Close()");
                }
#endif
                checkSocket.Close();
            }

            Interlocked.Decrement(ref m_ActiveConnections);
        }

        public static int ActiveConnections
        {
            get 
            {
                return m_ActiveConnections;
            }

            set
            {
                m_ActiveConnections = value;
            }
        }

        public byte[] ConnectionBuffer 
        {
            get 
            {
                return m_Buffer;
            }

            set
            {
                m_Buffer = value;
            }
        }

        public int EndOfOffset
        {
            get 
            {
                return m_EofOffset;
            }

            set
            {
                 m_EofOffset = value;
            }
        }
   
        public ParseState ParserState
        {
            get 
            {
                return m_ParserState;
            }

            set
            {
                 m_ParserState= value;
            }
        }        

        public Socket ConnectionSocket
        {
            get
            {
                return m_Socket;
            }
        }

        public HttpWebListener Listener
        {
            get
            {
                return m_HttpWebListener;
            }
        }

        public HttpListenerWebRequest Request
        {
            get
            {
                return m_HttpListenerWebRequest;
            }
        }

        public int ParsedOffset
        {
            get
            {
                return m_ParsedOffset;
            }

            set
            {
                m_ParsedOffset = value;
            }
        }

        public void StartReceiving()
        {
            if (m_EofOffset <= m_ParsedOffset)
            {
                //
                // if we consumed all the data we'll move to the beginning of the block
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() moving to beginning of buffer");
                }
#endif
                m_ParsedOffset = 0;
                m_EofOffset = 0;
            }
            else if (m_EofOffset >= m_Buffer.Length)
            {
                //
                // if we're at the end of the buffer we have two options
                //
                if (m_ParsedOffset == 0)
                {
                    //
                    // the buffer is not big enough, double its size.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() growing the buffer");
                    }
#endif
                    byte[] newBuffer = new byte[m_Buffer.Length * 2];
                    Buffer.BlockCopy(m_Buffer, 0, newBuffer, 0, m_EofOffset);
                    m_Buffer = newBuffer;
                }
                else
                {
                    //
                    // there's space at the head of the buffer, move data.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() moving data at the top of the buffer"); 
                    }
#endif
                    Buffer.BlockCopy(m_Buffer, m_ParsedOffset, m_Buffer, 0, m_EofOffset-m_ParsedOffset);
                }
            }
            else
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose) 
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() reading in buffer with no changes"); 
                }
#endif
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() calling BeginReceive()");
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            Socket checkSocket = ConnectionSocket;
            if (checkSocket != null)
            {
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() calling Socket.BeginReceive()");
                    }
#endif
                    checkSocket.BeginReceive(
                        m_Buffer,
                        m_EofOffset,
                        m_Buffer.Length - m_EofOffset,
                        SocketFlags.None,
                        HttpWebListener.staticReceiveCallback,
                        this );
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::StartReceiving() caught exception in Socket.BeginReceive():" + exception.ToString());
                    }
#endif
                    Close();
                }
            }
        }

        private int SkipWhite(int offset)
        {
            while ((m_Buffer[offset] == (byte)' ' || m_Buffer[offset] == (byte)'\t') && offset < m_EofOffset)
            {
                offset++;
            }
            
            if (offset == m_EofOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindWhite(int offset)
        {
            while ((m_Buffer[offset] != (byte)' ' && m_Buffer[offset] != (byte)'\t') && offset < m_EofOffset)
            {
                offset++;
            }
            
            if (offset == m_EofOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindCrLf(int offset)
        {
            offset++;
            while (offset < m_EofOffset && (m_Buffer[offset-1] != (byte)'\r' || m_Buffer[offset] != (byte)'\n'))
            {
                offset++;
            }
            
            if (offset == m_EofOffset)
            {
                return -1;
            }
            return offset-1;
        }

        public ParseState Parse()
        {
            //
            // parse strictly, any exceptions will be treated as errors
            //
            int first, last, column;
            string header, headerName, headerValue;

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() entering m_ParsedOffset:" + m_ParsedOffset.ToString() + " m_EofOffset:" + m_EofOffset.ToString() + " m_ParserState:" + m_ParserState.ToString());
            }
#endif

            while (m_ParsedOffset < m_EofOffset)
            {
                switch (m_ParserState)
                {
                    case ParseState.None:
                        m_HttpListenerWebRequest = new HttpListenerWebRequest(this);
                        m_ParserState = ParseState.Method;
                        goto case ParseState.Method;
                    case ParseState.Method:
                        first = SkipWhite(m_ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        m_HttpListenerWebRequest.Method = Encoding.ASCII.GetString(m_Buffer, first, last-first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() Method:" + m_HttpListenerWebRequest.Method);
                        }
#endif
                        m_ParsedOffset = last;
                        m_ParserState = ParseState.Uri;
                        goto case ParseState.Uri;
                    case ParseState.Uri:
                        first = SkipWhite(m_ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        m_HttpListenerWebRequest.RelativeUri = Encoding.ASCII.GetString(m_Buffer, first, last-first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() RelativeUri:" + m_HttpListenerWebRequest.RelativeUri);
                        }
#endif
                        m_ParsedOffset = last;
                        m_ParserState = ParseState.Version;
                        goto case ParseState.Version;
                    case ParseState.Version:
                        first = SkipWhite(m_ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        if (last-first < 8 || m_Buffer[first] != (byte)'H' || m_Buffer[first+1] != (byte)'T' || m_Buffer[first+2] != (byte)'T' || m_Buffer[first+3] != (byte)'P' || m_Buffer[first+4] != (byte)'/' || m_Buffer[first+6] != (byte)'.')
                        {
                            return ParseState.Error;
                        }
                        m_HttpListenerWebRequest.ProtocolVersion = new Version(((int)(m_Buffer[first+5]-'0')), ((int)(m_Buffer[first+7]-'0')));
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() ProtocolVersion:" + m_HttpListenerWebRequest.ProtocolVersion.ToString());
                        }
#endif
                        m_ParsedOffset = last+2;
                        m_HttpListenerWebRequest.Headers = new WebHeaderCollection();
                        m_ParserState = ParseState.Headers;
                        goto case ParseState.Headers;
                    case ParseState.Headers:
                        first = SkipWhite(m_ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        header = Encoding.ASCII.GetString(m_Buffer, first, last-first);
                        column = header.IndexOf(':');
                        if (column == -1)
                        {
                            m_ParserState = ParseState.Error;
                            return ParseState.Error;
                        }
                        headerName = header.Substring(0, column).Trim();
                        headerValue = header.Substring(column+1).Trim();
                        m_HttpListenerWebRequest.Headers.Add(headerName, headerValue);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() headerName:[" + headerName + "] headerValue:[" + headerValue + "]");
                        }
#endif
                        if (m_Buffer[last+2] == (byte)'\r' && m_Buffer[last+3] == (byte)'\n')
                        {
                            bool valid = m_HttpListenerWebRequest.InterpretHeaders();
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() InterpretHeaders() returned:" + valid.ToString());
                            }
#endif
                            if (!valid)
                            {
                                m_ParserState = ParseState.Error;
                                return ParseState.Error;
                            }
                            
                            if (Listener.Auto100Continue)
                            {
                                m_HttpListenerWebRequest.Send100Continue();
                            }
                            
                            if (m_ParserState == ParseState.Headers)
                            {
                                m_ParserState = ParseState.Done;
                            }
                            m_ParsedOffset = last+4;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) + "::Parse() returning ParseState.Done m_ParsedOffset:" + m_ParsedOffset.ToString() + " m_EofOffset:" + m_EofOffset.ToString());
                            }
#endif
                            return ParseState.Done;
                        }
                        m_ParsedOffset = last+2;
                        break;
                    default:
                        m_ParserState = ParseState.Error;
                        return ParseState.Error;
                }
            }

            return ParseState.Continue;
        }
    }
}