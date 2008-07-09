//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Terrarium.Net
{
    // The stream that is the body of a response to an HTTP request to this Terrarium
    public class HttpListenerResponseStream : Stream
    {
        private long m_ContentLength;
        private long m_SentContentLength;
        private bool m_WriteChunked;
        private HttpListenerWebResponse m_HttpListenerWebResponse;

        public HttpListenerResponseStream(HttpListenerWebResponse httpListenerWebResponse, long contentLength, bool writeChunked)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::.ctor() contentLength:" + contentLength.ToString() + " writeChunked:" + writeChunked.ToString());
            }
#endif
            m_HttpListenerWebResponse = httpListenerWebResponse;
            m_ContentLength = contentLength;
            m_WriteChunked = writeChunked;
        }

        ~HttpListenerResponseStream()
        {
            Close();
        }

        public override bool CanRead
        {
            get 
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get 
            {
                return true;
            }
        }

        public bool DataAvailable 
        {
            get 
            {
                return false;
            }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get 
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose) 
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::get_Length() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
        }

        public override long Position
        {
            get 
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::get_Position() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
            
            set
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::set_Position() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Read()");
            }
#endif
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Read() throwing: " + exception.ToString());
            }
#endif
            throw exception;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Write() count:" + count.ToString());
            }
#endif
            if (m_ContentLength != -1 && m_SentContentLength + count > m_ContentLength)
            {
                Exception exception = new ProtocolViolationException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Write() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }

            Socket checkSocket;
            if (!m_HttpListenerWebResponse.SentHeaders) 
            {
                //
                // we didn't send the headers yet, do so now.
                //
                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                checkSocket = m_HttpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
                if (checkSocket != null)
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose) 
                    {
                        HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::GetResponseStream() calling Socket.Send() Length:" + m_HttpListenerWebResponse.HeadersBuffer.Length.ToString());
                    }
#endif
                    checkSocket.Send(m_HttpListenerWebResponse.HeadersBuffer);
                    m_HttpListenerWebResponse.SentHeaders = true;
                }
            }

            int DataToWrite = count;

            if (m_WriteChunked) 
            {
                string ChunkHeader = "0x" + Convert.ToString( count, 16 );
                DataToWrite += ChunkHeader.Length + 4;
                byte[] newBuffer = new byte[DataToWrite];

                for (int index=0; index<ChunkHeader.Length; index++)
                {
                    newBuffer[index] = (byte)ChunkHeader[index];
                }

                newBuffer[ChunkHeader.Length] = 0x0D;
                newBuffer[ChunkHeader.Length + 1] = 0x0A;

                Buffer.BlockCopy(buffer, offset, newBuffer, ChunkHeader.Length + 2, count);

                newBuffer[DataToWrite - 2] = 0x0D;
                newBuffer[DataToWrite - 1] = 0x0A;

                buffer = newBuffer;
                offset = 0;
            }
#if DEBUG
            if (HttpTraceHelper.Socket.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::GetResponseStream() calling Socket.Send() Length:" + DataToWrite.ToString()); 
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            checkSocket = m_HttpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                checkSocket.Send(buffer, offset, DataToWrite, SocketFlags.None);
            }

            if (m_ContentLength != -1)
            {
                //
                // keep track of the data transferred
                //
                m_SentContentLength -= count;
            }
        }

        public override void Close()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Close()");
            }
#endif
            if (m_WriteChunked == true)
            {
                //
                // send the trailer
                //
                byte[] buffer = new byte[3] {(byte)'0', 0x0D, 0x0A};
#if DEBUG
                if (HttpTraceHelper.Socket.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Close() calling Socket.Send() Length:3");
                }
#endif
                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                Socket checkSocket = m_HttpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
                if (checkSocket != null) 
                {
                    checkSocket.Send(buffer, 0, 3, SocketFlags.None);
                }
            }

            m_HttpListenerWebResponse.HandleConnection();
        }

        public override long Seek(long offset, SeekOrigin origin) 
        {
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::Seek() throwing: " + exception.ToString());
            }
#endif
            throw exception;
        }

        public override void SetLength(long value)
        {
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) + "::SetLength() throwing: " + exception.ToString());
            }
#endif
            throw exception;
        }
    }
}