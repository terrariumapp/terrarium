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
        private readonly HttpListenerWebRequest _httpListenerWebRequest;
        private bool _bufferHeaders;
        private bool _chunked;
        private long _contentLength;
        private string _contentType;
        private DateTime _date;
        private WebHeaderCollection _headers;
        private byte[] _headersBuffer;
        private bool _keepAlive;
        private Version _protocolVersion;
        private bool _sentHeaders;
        private string _server;
        private HttpStatusCode _statusCode;
        private string _statusDescription;
        private Stream _stream;

        public HttpListenerWebResponse(HttpListenerWebRequest httpListenerRequest)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::.ctor() keepAlive:" + httpListenerRequest.KeepAlive);
            }
#endif
            _httpListenerWebRequest = httpListenerRequest;
            _date = DateTime.MinValue;
            _headers = new WebHeaderCollection();
            _protocolVersion = new Version(1, 1);
            _sentHeaders = false;
            _keepAlive = httpListenerRequest.KeepAlive;
        }

        public byte[] HeadersBuffer
        {
            get { return _headersBuffer; }
        }

        public bool SentHeaders
        {
            get { return _sentHeaders; }
            set { _sentHeaders = value; }
        }

        public HttpListenerWebRequest Request
        {
            get { return _httpListenerWebRequest; }
        }

        public bool BufferHeaders
        {
            get { return _bufferHeaders; }
            set { _bufferHeaders = value; }
        }

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

        public bool Chunked
        {
            get { return _chunked; }
            set { _chunked = value; }
        }

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public Version ProtocolVersion
        {
            get { return _protocolVersion; }
            set { _protocolVersion = value; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }

        public void Close()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) + "::Close()");
            }
#endif
            if (_stream == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::Close() _sentHeaders:" + _sentHeaders);
                }
#endif

                if (!_sentHeaders)
                {
                    SerializeHeaders();
                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    Socket checkSocket = _httpListenerWebRequest.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                      "::Close() calling Socket.Send() Length:" + _headersBuffer.Length);
                        }
#endif
                        checkSocket.Send(_headersBuffer);
                        _sentHeaders = true;
                    }
                }
                HandleConnection();
            }
            else
            {
                _stream.Close();
            }
        }

        public void Close(byte[] entityData)
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::Close(byte[])");
            }
#endif
            if (entityData == null || entityData.Length <= 0 || _stream != null)
            {
                Close();
                return;
            }

            byte[] mergedBuffer;

#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::Close() entityData.Length:" + entityData.Length + " _sentHeaders:" +
                                          _sentHeaders);
            }
#endif

            if (!_sentHeaders)
            {
                SerializeHeaders();
                mergedBuffer = new byte[_headersBuffer.Length + entityData.Length];

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::Close() merging buffers mergedBuffer.Length:" + mergedBuffer.Length);
                }
#endif

                Buffer.BlockCopy(_headersBuffer, 0, mergedBuffer, 0, _headersBuffer.Length);
                Buffer.BlockCopy(entityData, 0, mergedBuffer, _headersBuffer.Length, entityData.Length);
            }
            else
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::SerializeHeaders() no merge: mergedBuffer = entityData");
                }
#endif

                mergedBuffer = entityData;
            }
#if DEBUG
            if (HttpTraceHelper.Socket.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::Close() calling Socket.Send() Length:" + mergedBuffer.Length);
            }
#endif
            // we null out the Socket when we cleanup
            // make a local copy to avoid null reference exceptions
            Socket checkSocket = _httpListenerWebRequest.ConnectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                checkSocket.Send(mergedBuffer);
                _sentHeaders = true;
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
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::HandleConnection() calling HttpListenerWebRequest#" +
                                          HttpTraceHelper.HashString(this) + "::HandleConnection()");
            }
#endif

            _httpListenerWebRequest.HandleConnection();

            if (!_keepAlive)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::HandleConnection() !KeepAlive closing the Connection");
                }
#endif
                _httpListenerWebRequest.ConnectionState.Close();
            }
        }

        private void SerializeHeaders()
        {
            if (_headersBuffer == null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::SerializeHeaders()");
                }
#endif

                //
                // format headers and send them on the wire
                //
                StringBuilder stringBuilder = new StringBuilder(1024);

                stringBuilder.Append("HTTP/");
                stringBuilder.Append(_protocolVersion.Major.ToString());
                stringBuilder.Append(".");
                stringBuilder.Append(_protocolVersion.Minor.ToString());
                stringBuilder.Append(" ");
                stringBuilder.Append(((int) _statusCode).ToString());
                stringBuilder.Append(" ");
                if (_statusDescription == null || _statusDescription.Length == 0)
                {
                    _statusDescription = _statusCode.ToString();
                }
                stringBuilder.Append(_statusDescription);
                stringBuilder.Append("\r\n");

                if (_server != null)
                {
                    _headers["Server"] = _server;
                }
                if (!_date.Equals(DateTime.MinValue))
                {
                    _headers["Date"] = _date.ToString("R");
                }
                if (!_keepAlive)
                {
                    _headers["Connection"] = "Close";
                }
                if (_chunked)
                {
                    _headers.Remove("Content-Length");
                    _headers["Transfer-Encoding"] = "Chunked";
                }
                else
                {
                    _headers["Content-Length"] = _contentLength.ToString("D");
                    _headers.Remove("Transfer-Encoding");
                }

                if (ContentType != null)
                {
                    _headers["Content-Type"] = ContentType;
                }

                if (_headers != null)
                {
                    for (int i = 0; i < _headers.Count; i++)
                    {
                        stringBuilder.Append(_headers.GetKey(i));
                        stringBuilder.Append(": ");
                        stringBuilder.Append(_headers.Get(i));
                        stringBuilder.Append("\r\n");
                    }
                }
                stringBuilder.Append("\r\n");

                if (_httpListenerWebRequest.DelayResponse > 0)
                {
                    //
                    // this feature is useful for testing
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                  "::SerializeHeaders() delaying Response() for:" +
                                                  _httpListenerWebRequest.DelayResponse);
                    }
#endif
                    Thread.Sleep(_httpListenerWebRequest.DelayResponse);
                }

                _headersBuffer = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            }
        }

        public Stream GetResponseStream()
        {
#if DEBUG
            if (HttpTraceHelper.Api.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                          "::GetResponseStream()");
            }
#endif
            if (_stream == null)
            {
                //
                // if _bufferHeaders is true we won't send the headers here, but we'll
                // do it later. at the latest when the user closes the response stream
                //
                if (!_bufferHeaders)
                {
                    //
                    // this call can throw, the exception will be presented to the user
                    // calling GetResponseStream()
                    //
                    SerializeHeaders();

                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    Socket checkSocket = _httpListenerWebRequest.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                      "::GetResponseStream() calling Socket.Send() Length:" +
                                                      _headersBuffer.Length);
                        }
#endif
                        checkSocket.Send(_headersBuffer);
                        _sentHeaders = true;
                    }
                }

                _stream = new HttpListenerResponseStream(this, _contentLength, _chunked);
            }
            return _stream;
        }
    }
}