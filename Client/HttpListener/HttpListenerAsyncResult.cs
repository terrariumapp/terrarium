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
        private object m_AsyncState;
        private AutoResetEvent m_AsyncWaitHandle;
        private bool m_CompletedSynchronously;
        private bool m_IsCompleted;
        private AsyncCallback m_AsyncCallback;
        private HttpWebListener m_HttpWebListener;
        private HttpListenerWebRequest m_HttpListenerWebRequest;

        private static WaitOrTimerCallback m_StaticCallback = new WaitOrTimerCallback(GetRequestCallback);
        
        public HttpListenerAsyncResult(AsyncCallback callback, object userState, HttpWebListener httpWebListener) 
        {
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) + "::.ctor()");
            }
#endif
            m_AsyncState = userState;
            m_AsyncCallback = callback;
            m_HttpWebListener = httpWebListener;
        }

        public static WaitOrTimerCallback StaticCallback
        {
            get
            {
                return m_StaticCallback;
            }
        }

        public HttpWebListener Listener 
        {
            get 
            {
                return m_HttpWebListener;
            }
        }

        public HttpListenerWebRequest Request 
        {
            get 
            {
                return m_HttpListenerWebRequest;
            }
            
            set 
            {
                m_HttpListenerWebRequest = value;
            }
        }

        public object AsyncState 
        {
            get 
            {
                return m_AsyncState;
            }
        }

        public bool CompletedSynchronously 
        {
            get
            {
                return m_CompletedSynchronously;
            }
        }

        public bool IsCompleted 
        {
            get 
            {
                return m_IsCompleted;
            }
        }

        private static void GetRequestCallback(object stateObject, bool signaled)
        {
            HttpListenerAsyncResult asyncResult = stateObject as HttpListenerAsyncResult;
#if DEBUG
            if (HttpTraceHelper.InternalLog.TraceVerbose) 
            {
                HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(asyncResult) + "::GetRequestCallback()");
            }
#endif

            asyncResult.Request = asyncResult.m_HttpWebListener.GetNextRequest();

            if (asyncResult.Request == null)
            {
                //
                // false alarm, request went to another thread, queue again.
                //
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose) 
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(asyncResult) + "::GetRequestCallback() calling RegisterWaitForSingleObject()");
                }
#endif

                ThreadPool.RegisterWaitForSingleObject(
                    asyncResult.m_HttpWebListener.RequestReady,
                    StaticCallback,
                    asyncResult,
                    -1,
                    true );
            }
            else
            {
                asyncResult.Complete(!signaled);
            }
        }

        public void Complete(bool completedSynchronously) 
        {
            m_CompletedSynchronously = completedSynchronously;
            m_IsCompleted = true;
            
            if (m_AsyncWaitHandle != null)
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose) 
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) + "::GetRequestCallback() Signaling event");
                }
#endif

                m_AsyncWaitHandle.Set();
            }
            if (m_AsyncCallback != null) 
            {
#if DEBUG
                if (HttpTraceHelper.InternalLog.TraceVerbose) 
                {
                    HttpTraceHelper.WriteLine("ListenerAsyncResult#" + HttpTraceHelper.HashString(this) + "::GetRequestCallback() Calling callback");
                }
#endif
				m_AsyncCallback(this);
            }
        }

        public WaitHandle AsyncWaitHandle 
        {
            get 
            {
                if (m_AsyncWaitHandle != null) 
                {
                    lock (this)
                    {
                        if (m_AsyncWaitHandle != null)
                        {
                            m_AsyncWaitHandle = new AutoResetEvent(false);
                        }
                    }
                }
                return m_AsyncWaitHandle;
            }
        }
    }
}