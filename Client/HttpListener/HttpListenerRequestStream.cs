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
        private const int DrainBufferSize = 1024;
        private static readonly byte[] _drainBuffer = new byte[DrainBufferSize];
        private readonly HttpConnectionState _connectionState;
        private readonly long _contentLength;
        private readonly bool _readChunked;
        private ParseState _chunkParserState;
        private bool _closeCalled;
        private int _currentChunkSize;
        private long _leftToRead;
        private long _read;

        public HttpListenerRequestStream(HttpConnectionState connectionState, long contentLength, bool readChunked)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::.ctor() contentLength:" + contentLength + " readChunked:" + readChunked);
            }
#endif
            _connectionState = connectionState;
            _contentLength = contentLength;
            _leftToRead = contentLength;
            _chunkParserState = ParseState.ChunkSize;
            _readChunked = readChunked;
            DataAvailable = _connectionState.Request.Uploading;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public bool DataAvailable { get; private set; }

        public override long Length
        {
            get { return _contentLength >= 0 ? _contentLength : -1; }
        }

        public override long Position
        {
            get { return _read; }
            set
            {
                Exception exception = new NotSupportedException();
#if DEBUG
                if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::set_Position() throwing: " + exception);
                }
#endif
                throw exception;
            }
        }

        public override void Flush()
        {
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
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::Write() throwing: " + exception);
            }
#endif
            throw exception;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::Read() count:" + count + " _readChunked:" + _readChunked +
                                          " _leftToRead:" + _leftToRead + " _moreToRead:" + DataAvailable +
                                          " _currentChunkSize:" + _currentChunkSize);
            }
#endif
            //
            // if reading 0 or past EOF, just return 0
            //
            if (count == 0 || !DataAvailable)
            {
                return 0;
            }

            var read = 0;

            if (!_readChunked)
            {
                //
                // make sure we don't try reading more than content length
                //
                if (_leftToRead > 0 && count > _leftToRead)
                {
                    count = (int) _leftToRead;
                }
                //
                // make sure there's data in the buffer
                //
                var available = ReadMore(false);
                //
                // now we certainly have some data in the buffer
                // copy as much as we can in the user's buffer
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::Read() _connectionState.EndOfOffset:" + _connectionState.EndOfOffset +
                                              " _connectionState.m_ParsedOffset:" + _connectionState.ParsedOffset);
                }
#endif
                read = Math.Min(available, count);
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::Read() read:" + read);
                }
#endif

                if (read > 0)
                {
                    Buffer.BlockCopy(
                        _connectionState.ConnectionBuffer,
                        _connectionState.ParsedOffset,
                        buffer,
                        offset,
                        read);

                    //
                    // update the offset for the buffered data for subsequent calls
                    // this is needed for pipelining support, since in the buffered
                    // data we could already have another request
                    //
                    _connectionState.ParsedOffset += read;

#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                  "::Read() after copy _connectionState.EndOfOffset:" +
                                                  _connectionState.EndOfOffset + " _connectionState.m_ParsedOffset:" +
                                                  _connectionState.ParsedOffset);
                    }
#endif

                    if (_leftToRead > 0)
                    {
                        _leftToRead -= read;
                        DataAvailable = _leftToRead > 0;
                    }
                }
            }
            else
            {
                //
                // we're going to read chunked data first figure out what the size of the current
                // chunk is. note that everytime we reach the end of a chunk we must and will set
                // _chunkParserState to ParseState.ChunkSize, and that when we see the final 0 size
                // chunk we must and will _moreToRead to false.
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
                while (read == 0 && DataAvailable && _chunkParserState != ParseState.Error)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                  "::Read() main while loop _chunkParserState:" + _chunkParserState +
                                                  " _currentChunkSize:" + _currentChunkSize);
                    }
#endif

                    switch (_chunkParserState)
                    {
                        case ParseState.ChunkSize:
                            int thisByte;
                            while (_connectionState.ParsedOffset < _connectionState.EndOfOffset &&
                                   (thisByte = _connectionState.ConnectionBuffer[_connectionState.ParsedOffset]) !=
                                   '\r')
                            {
#if DEBUG
                                if (HttpTraceHelper.InternalLog.TraceVerbose)
                                {
                                    HttpTraceHelper.WriteLine("ListenerRequestStream#" +
                                                              HttpTraceHelper.HashString(this) +
                                                              "::Read() current byte:" + thisByte + " at:" +
                                                              _connectionState.ParsedOffset + " _currentChunkSize:" +
                                                              _currentChunkSize);
                                }
#endif
                                var thisChunkDigit = (thisByte <= '9')
                                                         ? (thisByte - '0')
                                                         : (((thisByte <= 'F') ? (thisByte - 'A') : (thisByte - 'a')) +
                                                            10);
                                if (thisChunkDigit < 0 || thisChunkDigit > 15)
                                {
                                    _chunkParserState = ParseState.Error;
                                    break;
                                }
                                _currentChunkSize = _currentChunkSize*16 + thisChunkDigit;
                                _connectionState.ParsedOffset++;
                            }
                            if (_connectionState.ParsedOffset == _connectionState.EndOfOffset)
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
                            _connectionState.ParsedOffset++;
                            //
                            // we have parsed the chunk size. make sure we also have the final '\n'
                            // otherwise we'll need to read that as well. if we can't error out.
                            //
                            if (_connectionState.ConnectionBuffer[_connectionState.ParsedOffset] != '\n')
                            {
                                _chunkParserState = ParseState.Error;
                                break;
                            }
                            //
                            // skip the '\n'
                            //
                            _connectionState.ParsedOffset++;
                            _chunkParserState = ParseState.Chunk;
                            goto case ParseState.Chunk;
                        case ParseState.Chunk:
                            if (_currentChunkSize == 0)
                            {
                                //
                                // this is the last chunk.
                                //
                                DataAvailable = false;
                                break;
                            }
                            var available = _connectionState.EndOfOffset - _connectionState.ParsedOffset;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                          "::Read() ParseState.Chunk available:" + available);
                            }
#endif
                            if (available == 0)
                            {
                                available = ReadMore(false);
                                if (available == 0)
                                {
                                    _chunkParserState = ParseState.Error;
                                    break;
                                }
                            }
                            //
                            // copy all data available up to count without exceeding the current chunk
                            //
                            read = Math.Min(available, count);
                            read = Math.Min(read, _currentChunkSize);
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                          "::Read() will read:" + read + " available:" + available +
                                                          " count:" + count + " _currentChunkSize:" +
                                                          _currentChunkSize);
                            }
#endif
                            Buffer.BlockCopy(
                                _connectionState.ConnectionBuffer,
                                _connectionState.ParsedOffset,
                                buffer,
                                offset,
                                read);

                            //
                            // update the offset for the buffered data for subsequent calls
                            // this is needed for pipelining support, since in the buffered
                            // data we could already have another request
                            //
                            _connectionState.ParsedOffset += read;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                          "::Read() after copy _connectionState.EndOfOffset:" +
                                                          _connectionState.EndOfOffset +
                                                          " _connectionState.m_ParsedOffset:" +
                                                          _connectionState.ParsedOffset);
                            }
#endif

                            _currentChunkSize -= read;
                            if (_currentChunkSize == 0)
                            {
                                //
                                // need to swallow the extra '\r\n' here.
                                //
                                var i = 0;
                                available = _connectionState.EndOfOffset - _connectionState.ParsedOffset;
                                while (available < 2)
                                {
                                    available = ReadMore(false);
                                    if (++i == 2)
                                    {
                                        //
                                        // can't read more data but need to skip the '\r\n' at the end of the chunk
                                        //
                                        _chunkParserState = ParseState.Error;
                                        break;
                                    }
                                }
                                _connectionState.ParsedOffset += 2;
                                _chunkParserState = ParseState.ChunkSize;
                            }
                            break;
                    }
                }

                if (_chunkParserState == ParseState.Error)
                {
                    var exception = new Exception("Error parsing chunked request stream");
#if DEBUG
                    if (HttpTraceHelper.ExceptionThrown.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                  "::Read() throwing: " + exception);
                    }
#endif
                    throw exception;
                }
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::Read() exiting the while loop _chunkParserState:" + _chunkParserState +
                                              " _currentChunkSize:" + _currentChunkSize);
                }
#endif
            }

            _read += read;

            if (!DataAvailable)
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
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::Read() returning read:" + read + " _leftToRead:" + _leftToRead +
                                          " _moreToRead:" + DataAvailable);
            }
#endif

            return read;
        }

        private int ReadMore(bool forceRead)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::ReadMore() reading forceRead:" + forceRead);
            }
#endif

            var dataAvailable = _connectionState.EndOfOffset - _connectionState.ParsedOffset;

            if (dataAvailable == 0)
            {
                //
                // we don't have buffered data read more from the socket.
                // we can move to the beginning of the buffer
                //
                _connectionState.ParsedOffset = 0;
                _connectionState.EndOfOffset = 0;
            }
            else if (forceRead && _connectionState.EndOfOffset == _connectionState.ConnectionBuffer.Length)
            {
                //
                // we have buffered data but buffer is full. move data to the head
                //
                Buffer.BlockCopy(
                    _connectionState.ConnectionBuffer,
                    _connectionState.ParsedOffset,
                    _connectionState.ConnectionBuffer,
                    0,
                    dataAvailable);

                _connectionState.ParsedOffset = 0;
                _connectionState.EndOfOffset = dataAvailable;
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
                var dataToRead = _connectionState.ConnectionBuffer.Length - _connectionState.EndOfOffset;
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::ReadMore() dataToRead:" + dataToRead);
                }
#endif
                var dataRead = 0;

                // we null out the Socket when we cleanup
                // make a local copy to avoid null reference exceptions
                var checkSocket = _connectionState.ConnectionSocket;
                if (checkSocket != null)
                {
                    try
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                      "::ReadMore() calling Socket.Receive()");
                        }
#endif
                        dataRead = checkSocket.Receive(_connectionState.ConnectionBuffer, _connectionState.EndOfOffset,
                                                       dataToRead, SocketFlags.None);

                        dataAvailable += dataRead;
                        _connectionState.EndOfOffset += dataRead;
                    }
                    catch (Exception exception)
                    {
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                      "::ReadMore() Socket.Receive() threw:" + exception);
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
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::ReadMore() Socket.Receive() returned:" + _connectionState.EndOfOffset);
                }
#endif
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::ReadMore() returning dataAvailable:" + dataAvailable);
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
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
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
                HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                          "::Seek() throwing: " + exception);
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
            if (_closeCalled)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                              "::Close() stream already closed returning");
                }
#endif
                return;
            }

            _closeCalled = true;
            if (DataAvailable)
            {
                //
                // drain stream data
                //
                for (;;)
                {
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                  "::Close() draining LeftToRead:" + _leftToRead);
                    }
#endif
                    int read;
                    try
                    {
                        read = Read(_drainBuffer, 0, _drainBuffer.Length);
                    }
                    catch (Exception exception)
                    {
                        read = -1;
#if DEBUG
                        if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ListenerRequestStream#" + HttpTraceHelper.HashString(this) +
                                                      "::Close() caught exception in Stream.Read(): " + exception);
                        }
#endif
                    }
                    if (read <= 0)
                    {
                        break;
                    }
                }
            }

            _connectionState.Request.Close();
        }
    }
}