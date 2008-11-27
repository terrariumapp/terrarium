//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using DxVBLib;
using Terrarium.Renderer.DirectX;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Manages text based surfaces.  Each surface is keyed
    ///  to the original text that creates it.  For performance
    ///  lengthy strings are concatenated.
    /// </summary>
    internal class TerrariumTextSurfaceManager
    {
        /// <summary>
        ///  Represents the rect each piece of text is drawn
        ///  within.  Each text surface is made exactly this
        ///  size.
        /// </summary>
        internal static RECT StandardFontRect;

        /// <summary>
        ///  The sprites associated with each bit of text.
        /// </summary>
        private Hashtable sprites;

        /// <summary>
        ///  Initialize the standard font rectangle.
        /// </summary>
        static TerrariumTextSurfaceManager()
        {
            StandardFontRect = new RECT();
            StandardFontRect.Top = 0;
            StandardFontRect.Left = 0;
            StandardFontRect.Bottom = 15;
            StandardFontRect.Right = 100;
        }

        /// <summary>
        ///  Initialize a new text surface manager and any internal
        ///  fields.
        /// </summary>
        internal TerrariumTextSurfaceManager()
        {
            Clear();
        }

        /// <summary>
        ///  Returns the number of text surfaces currently cached.
        /// </summary>
        internal int Count
        {
            get { return sprites.Count; }
        }

        /// <summary>
        ///  Gets the text surface associated with the given key.  If
        ///  the surface doesn't exist, it is created.
        /// </summary>
        internal DirectDrawSurface this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                if (!sprites.ContainsKey(key))
                {
                    Add(key);
                }

                return (DirectDrawSurface) sprites[key];
            }
        }

        /// <summary>
        ///  Adds a new string to the text surface manager.  This creates
        ///  the associated text surface so that text can be rendered with
        ///  a fast Blt rather than with a DrawText call.  Note that caching
        ///  could be done in a much more efficient manner and some text
        ///  surfaces will have identical contents.
        /// </summary>
        /// <param name="key">The string to add.</param>
        internal void Add(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            // Set up the surface
            var rect = new RECT();
            var ddSurface = new DirectDrawSurface(StandardFontRect.Right, StandardFontRect.Bottom)
                                {
                                    TransparencyKey = DirectDrawSurface.MagentaColorKey
                                };

            // Color in the back and add the text
            ddSurface.Surface.BltColorFill(ref rect, DirectDrawSurface.MagentaColorKey.low);
            ddSurface.Surface.SetForeColor(0);

            var text = key;
            if (text.Length > 16)
            {
                text = text.Substring(0, 16);
                text += "...";
            }

            var dcHandle = new IntPtr(ddSurface.Surface.GetDC());

            var graphics = Graphics.FromHdc(dcHandle);

            var font = new Font("Verdana", 6.75f, FontStyle.Regular);

            graphics.DrawString(text, font, Brushes.Black, 1, 1);
            graphics.DrawString(text, font, Brushes.WhiteSmoke, 0, 0);

            font.Dispose();

            graphics.Dispose();

            ddSurface.Surface.ReleaseDC(dcHandle.ToInt32());

            sprites.Add(key, ddSurface);
        }

        /// <summary>
        ///  Clears out any existing text surfaces and reinitializes the
        ///  hash table for storing keyed surfaces.
        /// </summary>
        internal void Clear()
        {
            sprites = CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

        /// <summary>
        ///  Removes the surface associated with the given key.
        /// </summary>
        /// <param name="key">The key of the surface to remove.</param>
        /// <returns>The DirectDrawSurface being removed.</returns>
        internal DirectDrawSurface Remove(string key)
        {
            DirectDrawSurface ddSurface = null;

            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (sprites.ContainsKey(key))
            {
                ddSurface = (DirectDrawSurface) sprites[key];
                sprites.Remove(key);
            }

            return ddSurface;
        }
    }
}