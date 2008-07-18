//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Net
{
    // Used by HttpNamespaceManager.   To implement a class that get called
    // when a specific URI is reqested of the listener, you implement a class
    // that implements this interface and register it with the HttpNamespaceManager
    public interface IHttpNamespaceHandler
    {
        void ProcessRequest(HttpApplication webApplication);
    }
}