//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.IO;

namespace Terrarium.Net
{
    // HttpApplication is used for representing the state of a HttpListener
    // request/response pair. It is used by the HttpNamespaceManager
    // for queueing up listeners and encapsulating their state.
    public class HttpApplication
    {
        private byte[] readBuffer = new byte[4096];
        private HttpListenerWebRequest httpRequest;
        private Stream requestStream;
        private HttpListenerWebResponse httpResponse;
        private int readBytes;
        private MemoryStream buffer = new MemoryStream();

        public HttpListenerWebRequest HttpRequest
        {
            get
            {
                return httpRequest;
            }

            set
            {
                httpRequest = value;
            }
        }

        public HttpListenerWebResponse HttpResponse
        {
            get
            {
                if (httpResponse == null && httpRequest != null)
                {
                    httpResponse = httpRequest.GetResponse();
                }

                return httpResponse;
            }
        }

        public byte[] ReadBuffer 
        {
            get 
            {
                return readBuffer;
            }
        }

        public Stream RequestStream
        {
            get
            {
                return requestStream;
            }

            set
            {
                requestStream = value;
            }
        }

        public int ReadBytes
        {
            get
            {
                return readBytes;
            }

            set
            {
                readBytes = value;
            }
        }

        public MemoryStream Buffer
        {
            get
            {
                return buffer;
            }
        }

        public void Reset()
        {
            httpRequest = null;
            requestStream = null;
            httpResponse = null;
            readBytes = 0;
            buffer = new MemoryStream();
        }
    }
}