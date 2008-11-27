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

        public HttpConnectionState(Socket socket, int bufferSize, HttpWebListener httpWebListener)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::ctor() New client connected from: {1}",
                                                        HttpTraceHelper.HashString(this), socket.RemoteEndPoint));
            }
#endif
            Interlocked.Increment(ref _activeConnections);
            Listener = httpWebListener;
            ConnectionSocket = socket;
            ConnectionBuffer = new byte[bufferSize];

            ParserState = ParseState.None;
            EndOfOffset = 0;
            ParsedOffset = 0;
        }

        public static int ActiveConnections
        {
            get { return _activeConnections; }
            set { _activeConnections = value; }
        }

        public byte[] ConnectionBuffer { get; set; }

        public int EndOfOffset { get; set; }

        public ParseState ParserState { get; set; }

        public Socket ConnectionSocket { get; private set; }

        public HttpWebListener Listener { get; private set; }

        public HttpListenerWebRequest Request { get; private set; }

        public int ParsedOffset { get; set; }

        ~HttpConnectionState()
        {
            Close();
        }

        public void Close()
        {
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            var checkSocket = ConnectionSocket;

            // null out the Socket as soon as possible, it's still possible
            // that two or more callers will execute the method below on the same
            // instance. for now that's ok.
            ConnectionSocket = null;
            if (checkSocket != null)
            {
                // gracefully close (allow user to read the data)
                // ignore failures on the socket
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(string.Format(
                                                      "ConnectionState#{0}::ctor() calling Socket.Shutdown()",
                                                      HttpTraceHelper.HashString(this)));
                    }
#endif
                    checkSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(
                            string.Format("ConnectionState#{0}::Close() caught exception in Socket.Shutdown(): {1}",
                                          HttpTraceHelper.HashString(this), exception));
                    }
#endif
                }
#if DEBUG
                if (HttpTraceHelper.Socket.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::Close() calling Socket.Close()",
                                                            HttpTraceHelper.HashString(this)));
                }
#endif
                checkSocket.Close();
            }

            Interlocked.Decrement(ref _activeConnections);
        }

        public void StartReceiving()
        {
            if (EndOfOffset <= ParsedOffset)
            {
                //
                // if we consumed all the data we'll move to the beginning of the block
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine(
                        string.Format("ConnectionState#{0}::StartReceiving() moving to beginning of buffer",
                                      HttpTraceHelper.HashString(this)));
                }
#endif
                ParsedOffset = 0;
                EndOfOffset = 0;
            }
            else if (EndOfOffset >= ConnectionBuffer.Length)
            {
                //
                // if we're at the end of the buffer we have two options
                //
                if (ParsedOffset == 0)
                {
                    //
                    // the buffer is not big enough, double its size.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(
                            string.Format("ConnectionState#{0}::StartReceiving() growing the buffer",
                                          HttpTraceHelper.HashString(this)));
                    }
#endif
                    var newBuffer = new byte[ConnectionBuffer.Length*2];
                    Buffer.BlockCopy(ConnectionBuffer, 0, newBuffer, 0, EndOfOffset);
                    ConnectionBuffer = newBuffer;
                }
                else
                {
                    //
                    // there's space at the head of the buffer, move data.
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(
                            string.Format("ConnectionState#{0}::StartReceiving() moving data at the top of the buffer",
                                          HttpTraceHelper.HashString(this)));
                    }
#endif
                    Buffer.BlockCopy(ConnectionBuffer, ParsedOffset, ConnectionBuffer, 0, EndOfOffset - ParsedOffset);
                }
            }
            else
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine(
                        string.Format("ConnectionState#{0}::StartReceiving() reading in buffer with no changes",
                                      HttpTraceHelper.HashString(this)));
                }
#endif
            }
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::StartReceiving() calling BeginReceive()",
                                                        HttpTraceHelper.HashString(this)));
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            var checkSocket = ConnectionSocket;
            if (checkSocket != null)
            {
                try
                {
#if DEBUG
                    if (HttpTraceHelper.Socket.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(
                            string.Format("ConnectionState#{0}::StartReceiving() calling Socket.BeginReceive()",
                                          HttpTraceHelper.HashString(this)));
                    }
#endif
                    checkSocket.BeginReceive(
                        ConnectionBuffer,
                        EndOfOffset,
                        ConnectionBuffer.Length - EndOfOffset,
                        SocketFlags.None,
                        HttpWebListener.staticReceiveCallback,
                        this);
                }
                catch (Exception exception)
                {
#if DEBUG
                    if (HttpTraceHelper.ExceptionCaught.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine(
                            string.Format(
                                "ConnectionState#{0}::StartReceiving() caught exception in Socket.BeginReceive():{1}",
                                HttpTraceHelper.HashString(this), exception));
                    }
#endif
                    Close();
                }
            }
        }

        private int SkipWhite(int offset)
        {
            while ((ConnectionBuffer[offset] == (byte) ' ' || ConnectionBuffer[offset] == (byte) '\t') &&
                   offset < EndOfOffset)
            {
                offset++;
            }

            if (offset == EndOfOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindWhite(int offset)
        {
            while ((ConnectionBuffer[offset] != (byte) ' ' && ConnectionBuffer[offset] != (byte) '\t') &&
                   offset < EndOfOffset)
            {
                offset++;
            }

            if (offset == EndOfOffset)
            {
                return -1;
            }
            return offset;
        }

        private int FindCrLf(int offset)
        {
            offset++;
            while (offset < EndOfOffset &&
                   (ConnectionBuffer[offset - 1] != (byte) '\r' || ConnectionBuffer[offset] != (byte) '\n'))
            {
                offset++;
            }

            if (offset == EndOfOffset)
            {
                return -1;
            }
            return offset - 1;
        }

        /// <summary>
        /// parse strictly, any exceptions will be treated as errors
        /// </summary>
        /// <returns></returns>
        public ParseState Parse()
        {
            string header;

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine(
                    string.Format(
                        "ConnectionState#{0}::Parse() entering _parsedOffset:{1} _eofOffset:{2} _parserState:{3}",
                        HttpTraceHelper.HashString(this), ParsedOffset, EndOfOffset, ParserState));
            }
#endif

            while (ParsedOffset < EndOfOffset)
            {
                int first;
                int last;
                switch (ParserState)
                {
                    case ParseState.None:
                        Request = new HttpListenerWebRequest(this);
                        ParserState = ParseState.Method;
                        goto case ParseState.Method;
                    case ParseState.Method:
                        first = SkipWhite(ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        Request.Method = Encoding.ASCII.GetString(ConnectionBuffer, first, last - first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::Parse() Method:{1}",
                                                                    HttpTraceHelper.HashString(this), Request.Method));
                        }
#endif
                        ParsedOffset = last;
                        ParserState = ParseState.Uri;
                        goto case ParseState.Uri;
                    case ParseState.Uri:
                        first = SkipWhite(ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindWhite(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        Request.RelativeUri = Encoding.ASCII.GetString(ConnectionBuffer, first, last - first);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::Parse() RelativeUri:{1}",
                                                                    HttpTraceHelper.HashString(this),
                                                                    Request.RelativeUri));
                        }
#endif
                        ParsedOffset = last;
                        ParserState = ParseState.Version;
                        goto case ParseState.Version;
                    case ParseState.Version:
                        first = SkipWhite(ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        if (last - first < 8 || ConnectionBuffer[first] != (byte) 'H' ||
                            ConnectionBuffer[first + 1] != (byte) 'T' ||
                            ConnectionBuffer[first + 2] != (byte) 'T' || ConnectionBuffer[first + 3] != (byte) 'P' ||
                            ConnectionBuffer[first + 4] != (byte) '/' || ConnectionBuffer[first + 6] != (byte) '.')
                        {
                            return ParseState.Error;
                        }
                        Request.ProtocolVersion = new Version(((ConnectionBuffer[first + 5] - '0')),
                                                              ((ConnectionBuffer[first + 7] - '0')));
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine(string.Format("ConnectionState#{0}::Parse() ProtocolVersion:{1}",
                                                                    HttpTraceHelper.HashString(this),
                                                                    Request.ProtocolVersion));
                        }
#endif
                        ParsedOffset = last + 2;
                        Request.Headers = new WebHeaderCollection();
                        ParserState = ParseState.Headers;
                        goto case ParseState.Headers;
                    case ParseState.Headers:
                        first = SkipWhite(ParsedOffset);
                        if (first == -1)
                        {
                            return ParseState.Continue;
                        }
                        last = FindCrLf(first);
                        if (last == -1)
                        {
                            return ParseState.Continue;
                        }
                        header = Encoding.ASCII.GetString(ConnectionBuffer, first, last - first);
                        var column = header.IndexOf(':');
                        if (column == -1)
                        {
                            ParserState = ParseState.Error;
                            return ParseState.Error;
                        }
                        var headerName = header.Substring(0, column).Trim();
                        var headerValue = header.Substring(column + 1).Trim();
                        Request.Headers.Add(headerName, headerValue);
#if DEBUG
                        if (HttpTraceHelper.InternalLog.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine(
                                string.Format("ConnectionState#{0}::Parse() headerName:[{1}] headerValue:[{2}]",
                                              HttpTraceHelper.HashString(this), headerName, headerValue));
                        }
#endif
                        if (ConnectionBuffer[last + 2] == (byte) '\r' && ConnectionBuffer[last + 3] == (byte) '\n')
                        {
                            var valid = Request.InterpretHeaders();
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine(
                                    string.Format("ConnectionState#{0}::Parse() InterpretHeaders() returned:{1}",
                                                  HttpTraceHelper.HashString(this), valid));
                            }
#endif
                            if (!valid)
                            {
                                ParserState = ParseState.Error;
                                return ParseState.Error;
                            }

                            if (Listener.Auto100Continue)
                            {
                                Request.Send100Continue();
                            }

                            if (ParserState == ParseState.Headers)
                            {
                                ParserState = ParseState.Done;
                            }
                            ParsedOffset = last + 4;
#if DEBUG
                            if (HttpTraceHelper.InternalLog.TraceVerbose)
                            {
                                HttpTraceHelper.WriteLine(
                                    string.Format(
                                        "ConnectionState#{0}::Parse() returning ParseState.Done _parsedOffset:{1} _eofOffset:{2}",
                                        HttpTraceHelper.HashString(this), ParsedOffset, EndOfOffset));
                            }
#endif
                            return ParseState.Done;
                        }
                        ParsedOffset = last + 2;
                        break;
                    default:
                        ParserState = ParseState.Error;
                        return ParseState.Error;
                }
            }

            return ParseState.Continue;
        }
    }
}