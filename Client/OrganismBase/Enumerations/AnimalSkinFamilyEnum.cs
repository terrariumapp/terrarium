//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Used by the AnimalSkin attribute.  Similar to a "font family" in windows,
    ///   this enum specifies which family of skins to use when displaying an
    ///   creature whose custom skin can't be loaded.
    ///  </para>
    /// </summary>
    public enum AnimalSkinFamily
    {
        /// <summary>
        ///  <para>
        ///   Use the Ant skin if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Ant,
        /// <summary>
        ///  <para>
        ///   Use the Beetle skin if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Beetle,
        /// <summary>
        ///  <para>
        ///   Use the Spider skin if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Spider,
        /// <summary>
        ///  <para>
        ///   Use the Inchworm skin if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Inchworm,
        /// <summary>
        ///  <para>
        ///   Use the Scorpion skin if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Scorpion
    }
}