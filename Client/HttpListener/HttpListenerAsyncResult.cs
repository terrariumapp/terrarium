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
        private readonly AsyncCallback _asyncCallback;
        private AutoResetEvent _asyncWaitHandle;

        static HttpListenerAsyncResult()
        {
            StaticCallback = GetRequestCallback;
        }

        public HttpListenerAsyncResult(AsyncCallback callback, object userState, HttpWebListener httpWebListener)
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) + "::.ctor()");
            }
#endif
            AsyncState = userState;
            _asyncCallback = callback;
            Listener = httpWebListener;
        }

        public static WaitOrTimerCallback StaticCallback { get; private set; }

        public HttpWebListener Listener { get; private set; }

        public HttpListenerWebRequest Request { get; set; }

        #region IAsyncResult Members

        public object AsyncState { get; private set; }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted { get; private set; }

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

        #endregion

        private static void GetRequestCallback(object stateObject, bool signaled)
        {
            var asyncResult = stateObject as HttpListenerAsyncResult;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose)
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(asyncResult) +
                                          "::GetRequestCallback()");
            }
#endif

            asyncResult.Request = asyncResult.Listener.GetNextRequest();

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
                    asyncResult.Listener.RequestReady,
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
            CompletedSynchronously = completedSynchronously;
            IsCompleted = true;

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