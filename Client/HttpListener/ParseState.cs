//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Net
{
    public enum ParseState
    {
        //
        // used by HTTP request parsing
        //
        None,
        Method,
        Uri,
        Version,
        Headers,
        Continue,
        Done,
        //
        // used by chunked request entity body parsing
        //
        Chunk,
        ChunkSize,
        //
        // common
        //
        Error
    }
}