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
        private readonly byte[] _readBuffer = new byte[4096];
        private MemoryStream _buffer = new MemoryStream();
        private HttpListenerWebRequest _httpRequest;
        private HttpListenerWebResponse _httpResponse;
        private int _readBytes;
        private Stream _requestStream;

        public HttpListenerWebRequest HttpRequest
        {
            get { return _httpRequest; }
            set { _httpRequest = value; }
        }

        public HttpListenerWebResponse HttpResponse
        {
            get
            {
                if (_httpResponse == null && _httpRequest != null)
                {
                    _httpResponse = _httpRequest.GetResponse();
                }

                return _httpResponse;
            }
        }

        public byte[] ReadBuffer
        {
            get { return _readBuffer; }
        }

        public Stream RequestStream
        {
            get { return _requestStream; }
            set { _requestStream = value; }
        }

        public int ReadBytes
        {
            get { return _readBytes; }
            set { _readBytes = value; }
        }

        public MemoryStream Buffer
        {
            get { return _buffer; }
        }

        public void Reset()
        {
            _httpRequest = null;
            _requestStream = null;
            _httpResponse = null;
            _readBytes = 0;
            _buffer = new MemoryStream();
        }
    }
}