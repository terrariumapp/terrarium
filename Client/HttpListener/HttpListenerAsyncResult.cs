//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Threading;

namespace Terrarium.Net
{
    // Special AsyncResult for asynchronous processing of requests arriving at this Terrarium
    public class HttpListenerAsyncResult : IAsyncResult
    {
        private static readonly WaitOrTimerCallback _staticCallback = GetRequestCallback;
        private readonly AsyncCallback _asyncCallback;
        private readonly object _asyncState;
        private readonly HttpWebListener _httpWebListener;
        private AutoResetEvent _asyncWaitHandle;
        private bool _completedSynchronously;
        private HttpListenerWebRequest _httpListenerWebRequest;
        private bool _isCompleted;

        public HttpListenerAsyncResult(AsyncCallback callback, object userState, HttpWebListener httpWebListener)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) + "::.ctor()");
            }
#endif
            _asyncState = userState;
            _asyncCallback = callback;
            _httpWebListener = httpWebListener;
        }

        public static WaitOrTimerCallback StaticCallback
        {
            get { return _staticCallback; }
        }

        public HttpWebListener Listener
        {
            get { return _httpWebListener; }
        }

        public HttpListenerWebRequest Request
        {
            get { return _httpListenerWebRequest; }

            set { _httpListenerWebRequest = value; }
        }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (_asyncWaitHandle != null)
                {
                    lock (this)
                    {
                        if (_asyncWaitHandle != null)
                        {
                            _asyncWaitHandle = new AutoResetEvent(false);
                        }
                    }
                }
                return _asyncWaitHandle;
            }
        }

        private static void GetRequestCallback(object stateObject, bool signaled)
        {
            HttpListenerAsyncResult asyncResult = stateObject as HttpListenerAsyncResult;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(asyncResult) +
                                          "::GetRequestCallback()");
            }
#endif

            asyncResult.Request = asyncResult._httpWebListener.GetNextRequest();

            if (asyncResult.Request == null)
            {
                //
                // false alarm, request went to another thread, queue again.
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(asyncResult) +
                                              "::GetRequestCallback() calling RegisterWaitForSingleObject()");
                }
#endif

                ThreadPool.RegisterWaitForSingleObject(
                    asyncResult._httpWebListener.RequestReady,
                    StaticCallback,
                    asyncResult,
                    -1,
                    true);
            }
            else
            {
                asyncResult.Complete(!signaled);
            }
        }

        public void Complete(bool completedSynchronously)
        {
            _completedSynchronously = completedSynchronously;
            _isCompleted = true;

            if (_asyncWaitHandle != null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) +
                                              "::GetRequestCallback() Signaling event");
                }
#endif

                _asyncWaitHandle.Set();
            }
            if (_asyncCallback != null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose)
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) +
                                              "::GetRequestCallback() Calling callback");
                }
#endif
                _asyncCallback(this);
            }
        }
    }
}