//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    // This uses params, even though it's slower than normal arguments, because it only gets called if there is
    // a listener, and then it doesn't matter if it's slow
    /// <summary>
    ///    <para>For system use only.</para>
    /// </summary>
    public delegate void TraceEventHandler(object sender, params object[] items);
}