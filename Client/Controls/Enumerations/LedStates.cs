//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

namespace Terrarium.Forms 
{
    /// <summary>
    ///  Identifies the states available in a TerrariumLed control.
    ///  The led acts just like a US stoplight with values for good (green),
    ///  in flux (yellow), and failed (red).
    /// </summary>
    public enum LedStates
    {
        /// <summary>
        ///  The LED is in a failed state and is painted red.
        /// </summary>
        Failed = 0,
        /// <summary>
        ///  The LED is in a waiting state and is painted yellow
        /// </summary>
        Waiting = 1,
        /// <summary>
        ///  The LED is in an idle state and is painted green.
        /// </summary>
        Idle = 2,
    }
}