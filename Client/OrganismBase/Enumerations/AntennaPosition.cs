//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Provides the various positions that the LeftAntenna and RightAntenna
    ///   may be in.  Values outside of the allowable range are thrown away
    ///   and will always default to AntennaPosition.Left.
    ///  </para>
    /// </summary>
    [Serializable]
    public enum AntennaPosition
    {
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Left, this demonstrates a numeric value
        ///   of 0 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Left,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Right, this demonstrates a numeric value
        ///   of 1 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Right,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Up, this demonstrates a numeric value
        ///   of 2 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Top,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Down, this demonstrates a numeric value
        ///   of 3 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Bottom,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing to the Upper Left, this demonstrates a numeric value
        ///   of 4 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        UpperLeft,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing to the Upper Right, this demonstrates a numeric value
        ///   of 5 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        UpperRight,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing to the Bottom Left, this demonstrates a numeric value
        ///   of 6 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        BottomLeft,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing to the Bottom Right, this demonstrates a numeric value
        ///   of 7 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        BottomRight,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Forward, this demonstrates a numeric value
        ///   of 8 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Forward,
        /// <summary>
        ///  <para>
        ///   The Antenna will be facing Backward, this demonstrates a numeric value
        ///   of 9 when using the AntennaValue property of AntennaState.
        ///  </para>
        /// </summary>
        Backward
    }
}