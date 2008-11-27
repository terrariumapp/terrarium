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
        private readonly long _contentLength;
        private readonly HttpListenerWebResponse _httpListenerWebResponse;
        private readonly bool _writeChunked;
        private long _sentContentLength;

        public HttpListenerResponseStream(HttpListenerWebResponse httpListenerWebResponse, long contentLength,
                                          bool writeChunked)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::.ctor() contentLength:" + contentLength + " writeChunked:" + writeChunked);
            }
#endif
            _httpListenerWebResponse = httpListenerWebResponse;
            _contentLength = contentLength;
            _writeChunked = writeChunked;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public bool DataAvailable
        {
            get { return false; }
        }

        public override long Length
        {
            get
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                              "::get_Length() throwing: " + exception);
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
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                              "::get_Position() throwing: " + exception);
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
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                              "::set_Position() throwing: " + exception);
                }
#endif
                throw exception;
            }
        }

        ~HttpListenerResponseStream()
        {
            Close();
        }

        public override void Flush()
        {
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
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::Read() throwing: " + exception);
            }
#endif
            throw exception;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::Write() count:" + count);
            }
#endif
            if (_contentLength != -1 && _sentContentLength + count > _contentLength)
            {
                Exception exception = new ProtocolViolationException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                              "::Write() throwing: " + exception);
                }
#endif
                throw exception;
            }

            Socket checkSocket;
            if (!_httpListenerWebResponse.SentHeaders)
            {
                //
                // we didn't send the headers yet, do so now.
                //
                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                checkSocket = _httpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
                if (checkSocket != null)
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                                  "::GetResponseStream() calling Socket.Send() Length:" +
                                                  _httpListenerWebResponse.HeadersBuffer.Length);
                    }
#endif
                    checkSocket.Send(_httpListenerWebResponse.HeadersBuffer);
                    _httpListenerWebResponse.SentHeaders = true;
                }
            }

            var DataToWrite = count;

            if (_writeChunked)
            {
                var ChunkHeader = "0x" + Convert.ToString(count, 16);
                DataToWrite += ChunkHeader.Length + 4;
                var newBuffer = new byte[DataToWrite];

                for (var index = 0; index < ChunkHeader.Length; index++)
                {
                    newBuffer[index] = (byte) ChunkHeader[index];
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
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::GetResponseStream() calling Socket.Send() Length:" + DataToWrite);
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            checkSocket = _httpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                checkSocket.Send(buffer, offset, DataToWrite, SocketFlags.None);
            }

            if (_contentLength != -1)
            {
                //
                // keep track of the data transferred
                //
                _sentContentLength -= count;
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
            if (_writeChunked)
            {
                //
                // send the trailer
                //
                var buffer = new byte[3] {(byte) '0', 0x0D, 0x0A};
#if DEBUG
                if (HttpTraceHelper.Socket.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                              "::Close() calling Socket.Send() Length:3");
                }
#endif
                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                var checkSocket = _httpListenerWebResponse.Request.ConnectionState.ConnectionSocket;
                if (checkSocket != null)
                {
                    checkSocket.Send(buffer, 0, 3, SocketFlags.None);
                }
            }

            _httpListenerWebResponse.HandleConnection();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Exception exception = new NotSupportedException();
#if DEBUG
            if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::Seek() throwing: " + exception);
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
                HttpTraceHelper.WriteLine("ListenerResponseStream#" + HttpTraceHelper.HashString(this) +
                                          "::SetLength() throwing: " + exception);
            }
#endif
            throw exception;
        }
    }
}