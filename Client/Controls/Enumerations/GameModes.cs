//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

namespace Terrarium.Forms 
{
    /// <summary>
    ///  Enumerates available terrarium game modes.  Currently
    ///  only EcoSystem mode and Terrarium mode exist.
    /// </summary>
    public enum GameModes
    {
        /// <summary>
        ///  EcoSystem mode where multiple peers connect over the
        ///  Internet.
        /// </summary>
        Ecosystem = 0,
        /// <summary>
        ///  Terrarium mode where a single peer operates by itself
        ///  or several peers operate on a custom channel.
        /// </summary>
        Terrarium = 1,
    }
}