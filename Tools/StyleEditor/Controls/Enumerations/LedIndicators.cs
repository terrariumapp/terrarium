//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

namespace Terrarium.Forms 
{
    /// <summary>
    ///  Used by the TerrariumTopPanel and other classes to
    ///  index into the leds array.
    /// </summary>
    public enum LedIndicators
    {
        /// <summary>
        ///  Reporting service LED
        /// </summary>
        ReportWebService = 0,
        /// <summary>
        ///  Discovery service LED
        /// </summary>
        DiscoveryWebService = 1,
        /// <summary>
        ///  Outgoing peer service connection LED
        /// </summary>
        PeerConnection = 2,
        /// <summary>
        ///  Incoming peer service connection LED
        /// </summary>
        PeerReceivedConnection = 3
    }
}