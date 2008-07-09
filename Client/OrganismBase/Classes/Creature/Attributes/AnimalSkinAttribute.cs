//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase 
{
    /// <summary>
    ///  Determines the skin used to display the organism on screen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AnimalSkinAttribute : System.Attribute
    {
        /// <summary>
        ///  The skin family defined by this attribute.  The default is the
        ///  Ant skin.  Think of this like a font family.
        /// </summary>
        private AnimalSkinFamily skinFamily = AnimalSkinFamily.Ant;

        /// <summary>
        ///  The custom skin defined by this attribute.  The default is no
        ///  default skin.
        /// </summary>
        private string skin = string.Empty;

        /// <summary>
        ///    <para>Use this constructor if you don't want to specify a Skin Family to use if
        ///       your custom skin doesn't exist.</para>
        /// </summary>
        /// <param name='skin'>The name of the assembly the contains the skin you want to use.</param>
        public AnimalSkinAttribute(string skin)
        {
            this.skin = skin;
        }

        /// <summary>
        ///    <para>Use this constructor if you don't have a custom skin you want to use with
        ///       your organism.</para>
        /// </summary>
        /// <param name='skinFamily'>An AnimalSkinFamily value that specifies the skin to use for this organism.</param>
        public AnimalSkinAttribute(AnimalSkinFamily skinFamily)
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
        public AnimalSkinAttribute(AnimalSkinFamily skinFamily, string skin)
        {
            this.skinFamily = skinFamily;
            this.skin = skin;
        }

        /// <summary>
        ///  Provides read-only access to the string value specifying the skin.
        /// </summary>
        public string Skin
        {
            get
            {
                return skin;
            }
        }

        /// <summary>
        ///  Provides read-only access to the enumeration value representing the family of skins.
        /// </summary>
        public AnimalSkinFamily SkinFamily
        {
            get
            {
                return skinFamily;
            }
        }
    }
}