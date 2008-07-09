//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;

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
        ///  The sprites associated with each bit of text.
        /// </summary>
        private Hashtable sprites = null;

        /// <summary>
        ///  Represents the rect each piece of text is drawn
        ///  within.  Each text surface is made exactly this
        ///  size.
        /// </summary>
        internal static RECT StandardFontRect;

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
        ///  Adds a new string to the text surface manager.  This creates
        ///  the associated text surface so that text can be rendered with
        ///  a fast Blt rather than with a DrawText call.  Note that caching
        ///  could be done in a much more efficient manner and some text
        ///  surfaces will have identical contents.
        /// </summary>
        /// <param name="key">The string to add.</param>
        internal void Add(string key)
        {
            if (key == null || key.Length == 0)
            {
                return;
            }

            // Set up the surface
            RECT rect = new RECT();
            DirectDrawSurface ddSurface = new DirectDrawSurface(StandardFontRect.Right,StandardFontRect.Bottom);
            ddSurface.TransparencyKey = DirectDrawSurface.MagentaColorKey;

            // Color in the back and add the text
            ddSurface.Surface.BltColorFill(ref rect, DirectDrawSurface.MagentaColorKey.low);
            ddSurface.Surface.SetForeColor(0);

            string text = key;
            if (text.Length > 16)
            {
                text = text.Substring(0,16);
                text += "...";
            }

			IntPtr dcHandle = new IntPtr(ddSurface.Surface.GetDC());
			
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHdc( dcHandle );
			
			System.Drawing.Font font = new System.Drawing.Font( "Verdana", 6.75f, System.Drawing.FontStyle.Regular );
			
			graphics.DrawString( text, font, System.Drawing.Brushes.Black, 1, 1 );
			graphics.DrawString( text, font, System.Drawing.Brushes.WhiteSmoke, 0, 0 );
			
			font.Dispose();
			
			graphics.Dispose();
			
			ddSurface.Surface.ReleaseDC( dcHandle.ToInt32() );

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
        ///  Returns the number of text surfaces currently cached.
        /// </summary>
        internal int Count
        {
            get
            {
                return sprites.Count;
            }
        }

        /// <summary>
        ///  Gets the text surface associated with the given key.  If
        ///  the surface doesn't exist, it is created.
        /// </summary>
        internal DirectDrawSurface this[string key]
        {
            get
            {
                if (key == null || key.Length == 0)
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
        ///  Removes the surface associated with the given key.
        /// </summary>
        /// <param name="key">The key of the surface to remove.</param>
        /// <returns>The DirectDrawSurface being removed.</returns>
        internal DirectDrawSurface Remove(string key)
        {
            DirectDrawSurface ddSurface = null;

            if (key == null || key.Length == 0)
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