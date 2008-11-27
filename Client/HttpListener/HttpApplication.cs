//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System.IO;

namespace Terrarium.Net
{
    // HttpApplication is used for representing the state of a HttpListener
    // request/response pair. It is used by the HttpNamespaceManager
    // for queueing up listeners and encapsulating their state.
    public class HttpApplication
    {
        private HttpListenerWebResponse _httpResponse;

        public HttpApplication()
        {
            Buffer = new MemoryStream();
            ReadBuffer = new byte[4096];
        }

        public HttpListenerWebRequest HttpRequest { get; set; }

        public HttpListenerWebResponse HttpResponse
        {
            get
            {
                if (_httpResponse == null && HttpRequest != null)
                {
                    _httpResponse = HttpRequest.GetResponse();
                }

                return _httpResponse;
            }
        }

        public byte[] ReadBuffer { get; private set; }

        public Stream RequestStream { get; set; }

        public int ReadBytes { get; set; }

        public MemoryStream Buffer { get; private set; }

        public void Reset()
        {
            HttpRequest = null;
            RequestStream = null;
            _httpResponse = null;
            ReadBytes = 0;
            Buffer = new MemoryStream();
        }
    }
}