//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;

namespace OrganismBase 
{
    /// <summary>
    ///  Determines the color used for special markings on the organism (not currently used by Terrarium).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MarkingColorAttribute : System.Attribute
    {
        private KnownColor markingColor;

        /// <param name="markingColor">
        /// The special marking color for the animal.  This could be
        /// the color of the dot on a black widow or the stripes
        /// on some other animal.
        /// </param>
        public MarkingColorAttribute(KnownColor markingColor)
        {
            this.markingColor = markingColor;
        }

        /// <internal/>
        public KnownColor MarkingColor
        {
            get
            {
                return markingColor;
            }
        }
    }
}