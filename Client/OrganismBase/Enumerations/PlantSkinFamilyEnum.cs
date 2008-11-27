//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Used by the PlantSkin attribute.  Similar to a font family,
    ///   this enum specifies which family of skins to use when displaying an
    ///   creature whose custom skin can't be loaded.
    ///  </para>
    /// </summary>
    public enum PlantSkinFamily
    {
        /// <summary>
        ///  <para>
        ///   Use the Plant skin (grass number 1) if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        Plant,
        /// <summary>
        ///  <para>
        ///   Use the PlantOne skin (grass number 1) if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        PlantOne,
        /// <summary>
        ///  <para>
        ///   Use the PlantTwo skin (grass number 2) if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        PlantTwo,
        /// <summary>
        ///  <para>
        ///   Use the PlantThree skin (grass number 3) if the custom skin can't be loaded.
        ///  </para>
        /// </summary>
        PlantThree
    }
}