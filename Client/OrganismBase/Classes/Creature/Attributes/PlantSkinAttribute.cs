//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase 
{
    /// <summary>
    /// Determines the skin used to display the organism on screen.</summary>
    /// <remarks>
    /// <para>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class PlantSkinAttribute : System.Attribute
    {
        private PlantSkinFamily skinFamily = PlantSkinFamily.Plant;
        private string skin = string.Empty;


        /// <summary>
        ///    <para>Use this constructor if you don't want to specify a Skin Family to use if
        ///       your custom skin doesn't exist.</para>
        /// </summary>
        /// <param name='skin'>The name of the assembly the contains the skin you want to use</param>
        public PlantSkinAttribute(string skin)
        {
            this.skin = skin;
        }

        /// <summary>
        ///    <para>Use this constructor if you don't have a custom skin you want to use with
        ///       your organism.</para>
        /// </summary>
        /// <param name='skinFamily'>A PlantSkinFamilyEnum value that specifies the skin to use for this organism.</param>
        public PlantSkinAttribute(PlantSkinFamily skinFamily)
        {
            this.skinFamily = skinFamily;
        }

        /// <summary>
        ///    <para>Use this overload if you want to specify a custom skin, and specify the
        ///       family it belongs to so that something reasonable gets displayed if the skin
        ///       can't be loaded.</para>
        /// </summary>
        /// <param name='skinFamily'>An AnimalSkinFamily value that specifies the skin to use for this organism if the custom one can't be loaded.</param>
        /// <param name='skin'>The name of the assembly the contains the skin you want to use.</param>
        public PlantSkinAttribute(PlantSkinFamily skinFamily, string skin)
        {
            this.skinFamily = skinFamily;
            this.skin = skin;
        }

        /// <internal/>
        public string Skin
        {
            get
            {
                return skin;
            }
        }

        /// <internal/>
        public PlantSkinFamily SkinFamily
        {
            get
            {
                return skinFamily;
            }
        }
    }
}