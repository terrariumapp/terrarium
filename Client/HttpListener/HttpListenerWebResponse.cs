//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Terrarium.Net
{
    public class HttpListenerWebResponse
    {
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
            Request = httpListenerRequest;
            Date = DateTime.MinValue;
            Headers = new WebHeaderCollection();
            ProtocolVersion = new Version(1, 1);
            SentHeaders = false;
            KeepAlive = httpListenerRequest.KeepAlive;
        }

        public byte[] HeadersBuffer { get; private set; }

        public bool SentHeaders { get; set; }

        public HttpListenerWebRequest Request { get; private set; }

        public bool BufferHeaders { get; set; }

        public bool KeepAlive { get; set; }

        public bool Chunked { get; set; }

        public string Server { get; set; }

        public Version ProtocolVersion { get; set; }

        public long ContentLength { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public DateTime Date { get; set; }

        public string ContentType { get; set; }

        public WebHeaderCollection Headers { get; set; }

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
                                              "::Close() _sentHeaders:" + SentHeaders);
                }
#endif

                if (!SentHeaders)
                {
                    SerializeHeaders();
                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    var checkSocket = Request.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                      "::Close() calling Socket.Send() Length:" + HeadersBuffer.Length);
                        }
#endif
                        checkSocket.Send(HeadersBuffer);
                        SentHeaders = true;
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
                                          SentHeaders);
            }
#endif

            if (!SentHeaders)
            {
                SerializeHeaders();
                mergedBuffer = new byte[HeadersBuffer.Length + entityData.Length];

#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::Close() merging buffers mergedBuffer.Length:" + mergedBuffer.Length);
                }
#endif

                Buffer.BlockCopy(HeadersBuffer, 0, mergedBuffer, 0, HeadersBuffer.Length);
                Buffer.BlockCopy(entityData, 0, mergedBuffer, HeadersBuffer.Length, entityData.Length);
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
            var checkSocket = Request.ConnectionState.ConnectionSocket;
            if (checkSocket != null)
            {
                checkSocket.Send(mergedBuffer);
                SentHeaders = true;
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

            Request.HandleConnection();

            if (!KeepAlive)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                              "::HandleConnection() !KeepAlive closing the Connection");
                }
#endif
                Request.ConnectionState.Close();
            }
        }

        private void SerializeHeaders()
        {
            if (HeadersBuffer == null)
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
                var stringBuilder = new StringBuilder(1024);

                stringBuilder.Append("HTTP/");
                stringBuilder.Append(ProtocolVersion.Major.ToString());
                stringBuilder.Append(".");
                stringBuilder.Append(ProtocolVersion.Minor.ToString());
                stringBuilder.Append(" ");
                stringBuilder.Append(((int) StatusCode).ToString());
                stringBuilder.Append(" ");
                if (StatusDescription == null || StatusDescription.Length == 0)
                {
                    StatusDescription = StatusCode.ToString();
                }
                stringBuilder.Append(StatusDescription);
                stringBuilder.Append("\r\n");

                if (Server != null)
                {
                    Headers["Server"] = Server;
                }
                if (!Date.Equals(DateTime.MinValue))
                {
                    Headers["Date"] = Date.ToString("R");
                }
                if (!KeepAlive)
                {
                    Headers["Connection"] = "Close";
                }
                if (Chunked)
                {
                    Headers.Remove("Content-Length");
                    Headers["Transfer-Encoding"] = "Chunked";
                }
                else
                {
                    Headers["Content-Length"] = ContentLength.ToString("D");
                    Headers.Remove("Transfer-Encoding");
                }

                if (ContentType != null)
                {
                    Headers["Content-Type"] = ContentType;
                }

                if (Headers != null)
                {
                    for (var i = 0; i < Headers.Count; i++)
                    {
                        stringBuilder.Append(Headers.GetKey(i));
                        stringBuilder.Append(": ");
                        stringBuilder.Append(Headers.Get(i));
                        stringBuilder.Append("\r\n");
                    }
                }
                stringBuilder.Append("\r\n");

                if (Request.DelayResponse > 0)
                {
                    //
                    // this feature is useful for testing
                    //
#if DEBUG
                    if (HttpTraceHelper.InternalLog.TraceVerbose)
                    {
                        HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                  "::SerializeHeaders() delaying Response() for:" +
                                                  Request.DelayResponse);
                    }
#endif
                    Thread.Sleep(Request.DelayResponse);
                }

                HeadersBuffer = Encoding.ASCII.GetBytes(stringBuilder.ToString());
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
                if (!BufferHeaders)
                {
                    //
                    // this call can throw, the exception will be presented to the user
                    // calling GetResponseStream()
                    //
                    SerializeHeaders();

                    // we null out the Socket when we cleanup
                    // make a local copy to avoid null reference exceptions
                    var checkSocket = Request.ConnectionState.ConnectionSocket;
                    if (checkSocket != null)
                    {
#if DEBUG
                        if (HttpTraceHelper.Socket.TraceVerbose)
                        {
                            HttpTraceHelper.WriteLine("HttpListenerWebResponse#" + HttpTraceHelper.HashString(this) +
                                                      "::GetResponseStream() calling Socket.Send() Length:" +
                                                      HeadersBuffer.Length);
                        }
#endif
                        checkSocket.Send(HeadersBuffer);
                        SentHeaders = true;
                    }
                }

                _stream = new HttpListenerResponseStream(this, ContentLength, Chunked);
            }
            return _stream;
        }
    }
}