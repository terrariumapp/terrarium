//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Sockets;

namespace Terrarium.Net
{
    // The stream that is the body of an HTTP request to this Terrarium
    public class HttpListenerRequestStream : Stream
    {
        private long m_ContentLength;
        private long m_LeftToRead;
        private bool m_MoreToRead;
        private bool m_ReadChunked;
        private long m_Read;
        private bool m_CloseCalled;
        private int m_CurrentChunkSize;
        private ParseState m_ChunkParserState;
        private HttpConnectionState m_ConnectionState;
        private const int DrainBufferSize = 1024;
        private static readonly byte[] DrainBuffer = new byte[DrainBufferSize];

        public HttpListenerRequestStream(HttpConnectionState connectionState, long contentLength, bool readChunked)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::.ctor() contentLength:" + contentLength.ToString() + " readChunked:" + readChunked.ToString());
            }
#endif
            m_ConnectionState = connectionState;
            m_ContentLength = contentLength;
            m_LeftToRead = contentLength;
            m_ChunkParserState = ParseState.ChunkSize;
            m_ReadChunked = readChunked;
            m_MoreToRead = m_ConnectionState.Request.Uploading;
        }

        public override bool CanRead
        {
            get
            {
                return true;
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
                return false;
            }
        }

        public bool DataAvailable
        {
            get
            {
                return m_MoreToRead;
            }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get
            {
                return m_ContentLength>=0 ? m_ContentLength : -1;
            }
        }

        public override long Position
        {
            get
            {
                return m_Read;
            }
            set 
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::set_Position() throwing: " + exception.ToString());
                }
#endif
                throw exception;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Write()");
            }
#endif
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Write() throwing: " + exception.ToString());
            }
#endif
            throw exception;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() count:" + count.ToString() + " m_ReadChunked:" + m_ReadChunked.ToString() + " m_LeftToRead:" + m_LeftToRead.ToString() + " m_MoreToRead:" + m_MoreToRead.ToString() + " m_CurrentChunkSize:" + m_CurrentChunkSize.ToString());
            }
#endif
            //
            // if reading 0 or past EOF, just return 0
            //
            if (count == 0 || !m_MoreToRead)
            {
                return 0;
            }

            int read = 0;

            if (!m_ReadChunked)
            {
                //
                // make sure we don't try reading more than content length
                //
                if (m_LeftToRead > 0 && count > m_LeftToRead)
                {
                    count = (int)m_LeftToRead;
                }
                //
                // make sure there's data in the buffer
                //
                int available = ReadMore(false);
                //
                // now we certainly have some data in the buffer
                // copy as much as we can in the user's buffer
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() m_ConnectionState.EndOfOffset:" + m_ConnectionState.EndOfOffset.ToString() + " m_ConnectionState.m_ParsedOffset:" + m_ConnectionState.ParsedOffset.ToString());
                }
#endif
                read = Math.Min(available, count);
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() read:" + read.ToString());
                }
#endif

                if (read > 0)
                {
                    Buffer.BlockCopy(
                        m_ConnectionState.ConnectionBuffer,
                        m_ConnectionState.ParsedOffset,
                        buffer,
                        offset,
                        read );

                    //
                    // update the offset for the buffered data for subsequent calls
                    // this is needed for pipelining support, since in the buffered
                    // data we could already have another request
                    //
                    m_ConnectionState.ParsedOffset += read;

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() after copy m_ConnectionState.EndOfOffset:" + m_ConnectionState.EndOfOffset.ToString() + " m_ConnectionState.m_ParsedOffset:" + m_ConnectionState.ParsedOffset.ToString());
                    }
#endif

                    if (m_LeftToRead > 0)
                    {
                        m_LeftToRead -= read;
                        m_MoreToRead = m_LeftToRead>0;
                    }
                }
            }
            else
            {
                //
                // we're going to read chunked data first figure out what the size of the current
                // chunk is. note that everytime we reach the end of a chunk we must and will set
                // m_ChunkParserState to ParseState.ChunkSize, and that when we see the final 0 size
                // chunk we must and will m_MoreToRead to false.
                // Our parsing code will be based on a state machine that sits in a while loop until:
                // 1) we have read at least 1 byte of data.
                // 2) we have reached the end of the the data.
                //
                // here's the meaning of the status of the state machine
                //
                // ChunkSize : we're parsing the ChunkSize
                // Chunk : we know the size of the current chunk, but we have more data to read from it
                // Error : error while parsing chunks
                //
                while (read == 0 && m_MoreToRead && m_ChunkParserState != ParseState.Error)
                {

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() main while loop m_ChunkParserState:" + m_ChunkParserState.ToString() + " m_CurrentChunkSize:" + m_CurrentChunkSize.ToString());
                    }
#endif

                    switch (m_ChunkParserState)
                    {
                        case ParseState.ChunkSize:
                            int thisByte;
                            int thisChunkDigit;
                            while (m_ConnectionState.ParsedOffset < m_ConnectionState.EndOfOffset && (thisByte=(int)m_ConnectionState.ConnectionBuffer[m_ConnectionState.ParsedOffset]) != (int)'\r')
                            {
#if DEBUG
                                if (HttpTraceHelper.InternalLog.TraceVerbose)
                                {
                                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() current byte:" + thisByte.ToString() + " at:" + m_ConnectionState.ParsedOffset.ToString() + " m_CurrentChunkSize:" + m_CurrentChunkSize.ToString());
                                }
#endif
                                thisChunkDigit = (thisByte<=(int)'9') ? (thisByte-(int)'0') : (((thisByte<=(int)'F') ? (thisByte-(int)'A') : (thisByte-(int)'a')) + 10);
                                if (thisChunkDigit < 0 || thisChunkDigit > 15)
                                {
                                    m_ChunkParserState = ParseState.Error;
                                    break;
                                }
                                m_CurrentChunkSize = m_CurrentChunkSize*16 + thisChunkDigit;
                                m_ConnectionState.ParsedOffset++;
                            }
                            if (m_ConnectionState.ParsedOffset == m_ConnectionState.EndOfOffset)
                            {
                                //
                                // need more data.
                                //
                                ReadMore(false);
                                break;
                            }
                            //
                            // skip the '\r'
                            //
                            m_ConnectionState.ParsedOffset++;
                            //
                            // we have parsed the chunk size. make sure we also have the final '\n'
                            // otherwise we'll need to read that as well. if we can't error out.
                            //
                            if (m_ConnectionState.ConnectionBuffer[m_ConnectionState.ParsedOffset] != (int)'\n')
                            {
                                m_ChunkParserState=ParseState.Error;
                                break;
                            }
                            //
                            // skip the '\n'
                            //
                            m_ConnectionState.ParsedOffset++;
                            m_ChunkParserState=ParseState.Chunk;
                            goto case ParseState.Chunk;
                        case ParseState.Chunk:
                            if (m_CurrentChunkSize == 0)
                            {
                                //
                                // this is the last chunk.
                                //
                                m_MoreToRead = false;
                                break;
                            }
                            int available = m_ConnectionState.EndOfOffset-m_ConnectionState.ParsedOffset;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() ParseState.Chunk available:" + available.ToString());
                            }
#endif
                            if (available == 0)
                            {
                                available = ReadMore(false);
                                if (available == 0)
                                {
                                    m_ChunkParserState=ParseState.Error;
                                    break;
                                }
                            }
                            //
                            // copy all data available up to count without exceeding the current chunk
                            //
                            read = Math.Min(available, count);
                            read = Math.Min(read, m_CurrentChunkSize);
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() will read:" + read.ToString() + " available:" + available.ToString() + " count:" + count.ToString() + " m_CurrentChunkSize:" + m_CurrentChunkSize.ToString());
                            }
#endif
                            Buffer.BlockCopy(
                                m_ConnectionState.ConnectionBuffer,
                                m_ConnectionState.ParsedOffset,
                                buffer,
                                offset,
                                read );

                            //
                            // update the offset for the buffered data for subsequent calls
                            // this is needed for pipelining support, since in the buffered
                            // data we could already have another request
                            //
                            m_ConnectionState.ParsedOffset += read;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() after copy m_ConnectionState.EndOfOffset:" + m_ConnectionState.EndOfOffset.ToString() + " m_ConnectionState.m_ParsedOffset:" + m_ConnectionState.ParsedOffset.ToString());
                            }
#endif

                            m_CurrentChunkSize -= read;
                            if (m_CurrentChunkSize==0)
                            {
                                //
                                // need to swallow the extra '\r\n' here.
                                //
                                int i=0;
                                available = m_ConnectionState.EndOfOffset-m_ConnectionState.ParsedOffset;
                                while (available < 2)
                                {
                                    available = ReadMore(false);
                                    if (++i == 2)
                                    {
                                        //
                                        // can't read more data but need to skip the '\r\n' at the end of the chunk
                                        //
                                        m_ChunkParserState=ParseState.Error;
                                        break;
                                    }
                                }
                                m_ConnectionState.ParsedOffset+=2;
                                m_ChunkParserState=ParseState.ChunkSize;
                            }
                            break;
                    }
                }

                if (m_ChunkParserState == ParseState.Error)
                {
                    Exception exception = new Exception("Error parsing chunked request stream");
#if DEBUG
                    if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() throwing: " + exception.ToString());
                    }
#endif
                    throw exception;
                }
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() exiting the while loop m_ChunkParserState:" + m_ChunkParserState.ToString() + " m_CurrentChunkSize:" + m_CurrentChunkSize.ToString());
                }
#endif
            }

            m_Read += read;

            if (!m_MoreToRead)
            {
                //
                // if we read all the data, we'll call Close() so that we start
                // processing pipelined requests as soon as possible.
                //
                Close();
            }

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Read() returning read:" + read.ToString() + " m_LeftToRead:" + m_LeftToRead.ToString() + " m_MoreToRead:" + m_MoreToRead.ToString());
            }
#endif

            return read;
        }

        private int ReadMore(bool forceRead)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() reading forceRead:" + forceRead.ToString());
            }
#endif

            int dataAvailable = m_ConnectionState.EndOfOffset - m_ConnectionState.ParsedOffset;

            if (dataAvailable == 0)
            {
                //
                // we don't have buffered data read more from the socket.
                // we can move to the beginning of the buffer
                //
                m_ConnectionState.ParsedOffset = 0;
                m_ConnectionState.EndOfOffset = 0;
            }
            else if (forceRead && m_ConnectionState.EndOfOffset == m_ConnectionState.ConnectionBuffer.Length)
            {
                //
                // we have buffered data but buffer is full. move data to the head
                //
                Buffer.BlockCopy(
                    m_ConnectionState.ConnectionBuffer,
                    m_ConnectionState.ParsedOffset,
                    m_ConnectionState.ConnectionBuffer,
                    0,
                    dataAvailable );

                m_ConnectionState.ParsedOffset = 0;
                m_ConnectionState.EndOfOffset = dataAvailable;
            }

            if (dataAvailable == 0 || forceRead)
            {
                //
                // here we could just read up to count. instead we try to fill the whole buffer
                // so that we minimize the Socket calls.
                // we may read more than what we need, so if there are pipelined requests
                // these will be held in our public buffer and we will need to kick off parsing
                // after we complete reading the stream.
                //
                int dataToRead = m_ConnectionState.ConnectionBuffer.Length - m_ConnectionState.EndOfOffset;
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() dataToRead:" + dataToRead.ToString());
                }
#endif
                int dataRead = 0;

                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                Socket checkSocket = m_ConnectionState.ConnectionSocket;
                if (checkSocket != null)
                {
                    try
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() calling Socket.Receive()");
                        }
#endif
                        dataRead = checkSocket.Receive(m_ConnectionState.ConnectionBuffer, m_ConnectionState.EndOfOffset, dataToRead, SocketFlags.None);

                        dataAvailable += dataRead;
                        m_ConnectionState.EndOfOffset += dataRead;
                    }
                    catch (Exception exception)
                    {
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() Socket.Receive() threw:" + exception.ToString());
                        }
#endif
                    }
                }

                if (dataRead == 0)
                {
                    // socket was shutdown by the remote peer. let Read() handle this.
                }
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() Socket.Receive() returned:" + m_ConnectionState.EndOfOffset.ToString());
                }
#endif
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::ReadMore() returning dataAvailable:" + dataAvailable.ToString());
            }
#endif

            return dataAvailable;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Seek() throwing: " + exception.ToString());
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
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Seek() throwing: " + exception.ToString()); 
            }
#endif
            throw exception;
        }

        public override void Close()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Close()");
            }
#endif
            if (m_CloseCalled)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Close() stream already closed returning");
                }
#endif
                return;
            }

            m_CloseCalled = true;
            if (m_MoreToRead)
            {
                //
                // drain stream data
                //
                int read;
                for (;;)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Close() draining LeftToRead:" + m_LeftToRead.ToString());
                    }
#endif
                    try
                    {
                        read = Read(DrainBuffer, 0, DrainBuffer.Length);
                    }
                    catch (Exception exception)
                    {
                        read = -1;
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) + "::Close() caught exception in Stream.Read(): " + exception.ToString());
                        }
#endif
                    }
                    if (read <= 0)
                    {
                        break;
                    }
                }
            }

            m_ConnectionState.Request.Close();
        }
    }
}