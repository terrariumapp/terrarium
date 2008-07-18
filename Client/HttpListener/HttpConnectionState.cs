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
        private static int _activeConnections;
        private readonly HttpWebListener _httpWebListener;
        private byte[] _buffer;
        private int _eofOffset;
        private HttpListenerWebRequest _httpListenerWebRequest;
        private int _parsedOffset;
        private ParseState _parserState;
        private Socket _socket;

        public HttpConnectionState(Socket socket, int bufferSize, HttpWebListener httpWebListener)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                          "::ctor() New client connected from: " + socket.RemoteEndPoint);
            }
#endif
            Interlocked.Increment(ref _activeConnections);
            _httpWebListener = httpWebListener;
            _socket = socket;
            _buffer = new byte[bufferSize];

            _parserState = ParseState.None;
            _eofOffset = 0;
            _parsedOffset = 0;
        }

        public static int ActiveConnections
        {
            get { return _activeConnections; }
            set { _activeConnections = value; }
        }

        public byte[] ConnectionBuffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public int EndOfOffset
        {
            get { return _eofOffset; }
            set { _eofOffset = value; }
        }

        public ParseState ParserState
        {
            get { return _parserState; }
            set { _parserState = value; }
        }

        public Socket ConnectionSocket
        {
            get { return _socket; }
        }

        public HttpWebListener Listener
        {
            get { return _httpWebListener; }
        }

        public HttpListenerWebRequest Request
        {
            get { return _httpListenerWebRequest; }
        }

        public int ParsedOffset
        {
            get { return _parsedOffset; }
            set { _parsedOffset = value; }
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
            _socket = null;
            if (checkSocket != null)
            {
                // gracefully close (allow user to read the data)
                // ignore failures on the socket
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::ctor() calling Socket.Shutdown()");
                    }
#endif
                    checkSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::Close() caught exception in Socket.Shutdown(): " + exception);
                    }
#endif
                }
#if DEBUG
                if (HttpTraceHelper.Socket.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                              "::Close() calling Socket.Close()");
                }
#endif
                checkSocket.Close();
            }

            Interlocked.Decrement(ref _activeConnections);
        }

        public void StartReceiving()
        {
            if (_eofOffset <= _parsedOffset)
            {
                //
                // if we consumed all the data we'll move to the beginning of the block
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                              "::StartReceiving() moving to beginning of buffer");
                }
#endif
                _parsedOffset = 0;
                _eofOffset = 0;
            }
            else if (_eofOffset >= _buffer.Length)
            {
                //
                // if we're at the end of the buffer we have two options
                //
                if (_parsedOffset == 0)
                {
                    //
                    // the buffer is not big enough, double its size.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::StartReceiving() growing the buffer");
                    }
#endif
                    byte[] newBuffer = new byte[_buffer.Length*2];
                    Buffer.BlockCopy(_buffer, 0, newBuffer, 0, _eofOffset);
                    _buffer = newBuffer;
                }
                else
                {
                    //
                    // there's space at the head of the buffer, move data.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::StartReceiving() moving data at the top of the buffer");
                    }
#endif
                    Buffer.BlockCopy(_buffer, _parsedOffset, _buffer, 0, _eofOffset - _parsedOffset);
                }
            }
            else
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                              "::StartReceiving() reading in buffer with no changes");
                }
#endif
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                          "::StartReceiving() calling BeginReceive()");
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
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::StartReceiving() calling Socket.BeginReceive()");
                    }
#endif
                    checkSocket.BeginReceive(
                        _buffer,
                        _eofOffset,
                        _buffer.Length - _eofOffset,
                        SocketFlags.None,
                        HttpWebListener.staticReceiveCallback,
                        this);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                  "::StartReceiving() caught exception in Socket.BeginReceive():" +
                                                  exception);
                    }
#endif
                    Close();
                }
            }
        }

        private int SkipWhite(int offset)
        {
            while ((_buffer[offset] == (byte) ' ' || _buffer[offset] == (byte) '\t') && offset < _eofOffset)
            {
                offset++;
            }

            if (offset == _eofOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindWhite(int offset)
        {
            while ((_buffer[offset] != (byte) ' ' && _buffer[offset] != (byte) '\t') && offset < _eofOffset)
            {
                offset++;
            }

            if (offset == _eofOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindCrLf(int offset)
        {
            offset++;
            while (offset < _eofOffset && (_buffer[offset - 1] != (byte) '\r' || _buffer[offset] != (byte) '\n'))
            {
                offset++;
            }

            if (offset == _eofOffset)
            {
                return -1;
            }
            return offset - 1;
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
                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                          "::Parse() entering _parsedOffset:" + _parsedOffset + " _eofOffset:" +
                                          _eofOffset + " _parserState:" + _parserState);
            }
#endif

            while (_parsedOffset < _eofOffset)
            {
                switch (_parserState)
                {
                    case ParseState.None:
                        _httpListenerWebRequest = new HttpListenerWebRequest(this);
                        _parserState = ParseState.Method;
                        goto case ParseState.Method;
                    case ParseState.Method:
                        first = SkipWhite(_parsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        _httpListenerWebRequest.Method = Encoding.ASCII.GetString(_buffer, first, last - first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                      "::Parse() Method:" + _httpListenerWebRequest.Method);
                        }
#endif
                        _parsedOffset = last;
                        _parserState = ParseState.Uri;
                        goto case ParseState.Uri;
                    case ParseState.Uri:
                        first = SkipWhite(_parsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        _httpListenerWebRequest.RelativeUri = Encoding.ASCII.GetString(_buffer, first, last - first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                      "::Parse() RelativeUri:" + _httpListenerWebRequest.RelativeUri);
                        }
#endif
                        _parsedOffset = last;
                        _parserState = ParseState.Version;
                        goto case ParseState.Version;
                    case ParseState.Version:
                        first = SkipWhite(_parsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        if (last - first < 8 || _buffer[first] != (byte) 'H' || _buffer[first + 1] != (byte) 'T' ||
                            _buffer[first + 2] != (byte) 'T' || _buffer[first + 3] != (byte) 'P' ||
                            _buffer[first + 4] != (byte) '/' || _buffer[first + 6] != (byte) '.')
                        {
                            return ParseState.Error;
                        }
                        _httpListenerWebRequest.ProtocolVersion = new Version(((_buffer[first + 5] - '0')),
                                                                               ((_buffer[first + 7] - '0')));
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                      "::Parse() ProtocolVersion:" +
                                                      _httpListenerWebRequest.ProtocolVersion);
                        }
#endif
                        _parsedOffset = last + 2;
                        _httpListenerWebRequest.Headers = new WebHeaderCollection();
                        _parserState = ParseState.Headers;
                        goto case ParseState.Headers;
                    case ParseState.Headers:
                        first = SkipWhite(_parsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        header = Encoding.ASCII.GetString(_buffer, first, last - first);
                        column = header.IndexOf(':');
                        if (column == -1)
                        {
                            _parserState = ParseState.Error;
                            return ParseState.Error;
                        }
                        headerName = header.Substring(0, column).Trim();
                        headerValue = header.Substring(column + 1).Trim();
                        _httpListenerWebRequest.Headers.Add(headerName, headerValue);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                      "::Parse() headerName:[" + headerName + "] headerValue:[" +
                                                      headerValue + "]");
                        }
#endif
                        if (_buffer[last + 2] == (byte) '\r' && _buffer[last + 3] == (byte) '\n')
                        {
                            bool valid = _httpListenerWebRequest.InterpretHeaders();
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                          "::Parse() InterpretHeaders() returned:" + valid);
                            }
#endif
                            if (!valid)
                            {
                                _parserState = ParseState.Error;
                                return ParseState.Error;
                            }

                            if (Listener.Auto100Continue)
                            {
                                _httpListenerWebRequest.Send100Continue();
                            }

                            if (_parserState == ParseState.Headers)
                            {
                                _parserState = ParseState.Done;
                            }
                            _parsedOffset = last + 4;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine("ConnectionState#" + HttpTraceHelper.HashString(this) +
                                                          "::Parse() returning ParseState.Done _parsedOffset:" +
                                                          _parsedOffset + " _eofOffset:" + _eofOffset);
                            }
#endif
                            return ParseState.Done;
                        }
                        _parsedOffset = last + 2;
                        break;
                    default:
                        _parserState = ParseState.Error;
                        return ParseState.Error;
                }
            }

            return ParseState.Continue;
        }
    }
}