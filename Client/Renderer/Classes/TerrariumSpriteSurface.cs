//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using Terrarium.Configuration;
using Terrarium.Renderer.DirectX;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  Manages Terrarium Sprite Surfaces.  Sprite Surfaces
    ///  are linked to one or more actual surfaces.  Each surface
    ///  can be used to represent a creature's size.  Each sprite
    ///  surface is uniquely identified by name, and is capable of
    ///  returning the appropriate internal sprite sheet based on
    ///  the look-up method.
    /// </summary>
    internal class TerrariumSpriteSurface
    {
        /// <summary>
        ///  A collection of surfaces attached to this sprite surface.
        /// </summary>
        private readonly DirectDrawSpriteSurface[] surfaces;

        /// <summary>
        ///  Determines if this instance supports sized surfaces.
        /// </summary>
        private bool sizedSurfaces;

        /// <summary>
        ///  Creates a new sprite surface, initializing the size
        ///  array to an initial 49 slots.
        /// </summary>
        internal TerrariumSpriteSurface()
        {
            surfaces = new DirectDrawSpriteSurface[49];
        }

        /// <summary>
        ///  Attaches a single, unsized surface to this instance.
        ///  If sizedSurface was previously set, it is now unset.
        /// </summary>
        /// <param name="ddss">The surface to attach.</param>
        internal void AttachSurface(DirectDrawSpriteSurface ddss)
        {
            sizedSurfaces = false;
            surfaces[0] = ddss;
        }

        /// <summary>
        ///  Attaches a sized surface to this instance.  No bounds
        ///  checking is performed.  The size should be between 0 and 48.
        ///  The sizedSurfaces field is set to true.
        /// </summary>
        /// <param name="ddss">The sprite surface to attach to this instance.</param>
        /// <param name="size">The size of this sprite surface.</param>
        internal void AttachSurface(DirectDrawSpriteSurface ddss, int size)
        {
            sizedSurfaces = true;
            surfaces[size] = ddss;
        }

        /// <summary>
        ///  Gets the default surface.  This only works for unsized surfaces.
        /// </summary>
        /// <returns>The default, non-sized surface.</returns>
        internal DirectDrawSpriteSurface GetDefaultSurface()
        {
            return surfaces[0];
        }

        /// <summary>
        ///  Attempts to look-up the surface closest to the given size.
        /// </summary>
        /// <param name="size">The ideal size of the surface requrested.</param>
        /// <returns>A surface that closesly resembles the ideal surface in size.</returns>
        internal DirectDrawSpriteSurface GetClosestSurface(int size)
        {
            DirectDrawSpriteSurface close = null;

            if (!sizedSurfaces)
            {
                return surfaces[0];
            }

            for (var i = 1; i <= 48; i++)
            {
                if (surfaces[i] == null) continue;
                close = surfaces[i];

                if (GameConfig.UseLargeGraphics) continue;
                if (i >= size)
                {
                    break;
                }
            }

            return close;
        }
    }
}