//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------


using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DxVBLib;

namespace Terrarium.Renderer.DirectX
{
    /// <summary>
    ///  Managed Wrapper for a DirectX7 DirectDraw Surface.
    /// </summary>
    public class DirectDrawSurface
    {
        /// <summary>
        ///  Defines a default surface description
        /// </summary>
        public static DDSURFACEDESC2 DefaultSurfaceDescription;

        /// <summary>
        ///  Defines a surface description used for image surfaces
        /// </summary>
        public static DDSURFACEDESC2 ImageSurfaceDescription;

        /// <summary>
        ///  Defines a surface description for a system memory surface.
        /// </summary>
        public static DDSURFACEDESC2 SystemMemorySurfaceDescription;

        /// <summary>
        ///  Pointer to the surface description used to create this surface.
        /// </summary>
        private DDSURFACEDESC2 descriptor;

        /// <summary>
        ///  File based image used to initialize this surface.
        /// </summary>
        private String image;

        /// <summary>
        ///  The size of the surface.
        /// </summary>
        private RECT rect;

        /// <summary>
        ///  Pointer to the real DirectDrawSurface7 class
        /// </summary>
        private DirectDrawSurface7 surface;

        /// <summary>
        ///  Determines if transparency is enabled for this surface.
        /// </summary>
        private bool transparencyEnabled;

        /// <summary>
        ///  The transparency key for this surface
        /// </summary>
        private DDCOLORKEY transparencyKey;

        /// <summary>
        ///  Static constructor used to intialize static surface description fields.
        /// </summary>
        static DirectDrawSurface()
        {
            // Setup default Surface Description
            DefaultSurfaceDescription = new DDSURFACEDESC2();
            DefaultSurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            DefaultSurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;

            // Setup default Surface Description
            ImageSurfaceDescription = new DDSURFACEDESC2();
            ImageSurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            ImageSurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;

            SystemMemorySurfaceDescription = new DDSURFACEDESC2();
            SystemMemorySurfaceDescription.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
            SystemMemorySurfaceDescription.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN |
                                                           CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
        }


        /// <summary>
        ///  Creates a new DirectDrawSurface given a width and height.
        /// </summary>
        /// <param name="x">The width of the surface.</param>
        /// <param name="y">The height of the surface.</param>
        public DirectDrawSurface(int x, int y)
        {
            descriptor = new DDSURFACEDESC2();
            descriptor.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT |
                                CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
            descriptor.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
            descriptor.lWidth = x;
            descriptor.lHeight = y;
            image = "";

            CreateSurface();
        }

        /// <summary>
        ///  Create a new surface given a surface description.
        /// </summary>
        /// <param name="surfaceDescription">Surface Description</param>
        public DirectDrawSurface(DDSURFACEDESC2 surfaceDescription) : this("", surfaceDescription)
        {
        }

        /// <summary>
        ///  Create a new surface given an image path
        /// </summary>
        /// <param name="imagePath">Path to an image file.</param>
        public DirectDrawSurface(String imagePath) : this(imagePath, DefaultSurfaceDescription)
        {
        }

        /// <summary>
        ///  Create a new surface from an image and a surface description.
        /// </summary>
        /// <param name="imagePath">Path to an image file.</param>
        /// <param name="surfaceDescription">Surface Description.</param>
        public DirectDrawSurface(String imagePath, DDSURFACEDESC2 surfaceDescription)
        {
            descriptor = surfaceDescription;
            image = imagePath;
            CreateSurface();
        }

        /// <summary>
        ///  Initialize a new surface based on a previously created surface
        /// </summary>
        /// <param name="directDrawSurface">The native DirectDraw surface used as reference.</param>
        public DirectDrawSurface(DirectDrawSurface7 directDrawSurface)
        {
            directDrawSurface.GetSurfaceDesc(ref descriptor);
            surface = directDrawSurface;
        }

        /// <summary>
        ///  The default transparency color key.  Points to the MagentaColorKey
        /// </summary>
        public static DDCOLORKEY DefaultColorKey
        {
            get { return MagentaColorKey; }
        }

        /// <summary>
        ///  Creates a transparency key for the color Magenta.
        /// </summary>
        public static DDCOLORKEY MagentaColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ManagedDirectX.DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 253;
                    ddck.high = 253;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lBBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lBBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Creates a transparency key for the color White
        /// </summary>
        public static DDCOLORKEY WhiteColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ManagedDirectX.DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 255;
                    ddck.high = 255;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lGBitMask +
                               ddsd2.ddpfPixelFormat.lBBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lRBitMask + ddsd2.ddpfPixelFormat.lGBitMask +
                                ddsd2.ddpfPixelFormat.lBBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Create a transparency key for the color Lime
        /// </summary>
        public static DDCOLORKEY LimeColorKey
        {
            get
            {
                var ddck = new DDCOLORKEY();
                var ddsd2 = new DDSURFACEDESC2();
                ManagedDirectX.DirectDraw.GetDisplayMode(ref ddsd2);

                if ((ddsd2.ddpfPixelFormat.lFlags & CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8) ==
                    CONST_DDPIXELFORMATFLAGS.DDPF_PALETTEINDEXED8)
                {
                    ddck.low = 250;
                    ddck.high = 250;
                }
                else
                {
                    ddck.low = ddsd2.ddpfPixelFormat.lGBitMask;
                    ddck.high = ddsd2.ddpfPixelFormat.lGBitMask;
                }

                return ddck;
            }
        }

        /// <summary>
        ///  Determines if the surface is in video memory
        ///  or system memory.
        /// </summary>
        public bool InVideo
        {
            get
            {
                if (surface != null)
                {
                    var ddsc = new DDSCAPS2();
                    surface.GetCaps(ref ddsc);
                    if ((ddsc.lCaps & CONST_DDSURFACECAPSFLAGS.DDSCAPS_VIDEOMEMORY) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        ///  Path to the image used to initialize this surface if one exists.
        /// </summary>
        public String ImagePath
        {
            get { return image; }
            set
            {
                image = value;
                CreateSurface();
            }
        }

        /// <summary>
        ///  Determines if this is a transparent surface.
        /// </summary>
        public bool TransparentSurface
        {
            get { return transparencyEnabled; }
        }

        /// <summary>
        ///  Sets the transparency key for this surface.
        /// </summary>
        public DDCOLORKEY TransparencyKey
        {
            get { return transparencyKey; }
            set
            {
                transparencyKey = value;
                transparencyEnabled = true;
                if (surface != null)
                {
                    surface.SetColorKey(CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref transparencyKey);
                }
            }
        }

        /// <summary>
        ///  Modifies the Surface Description
        /// </summary>
        public DDSURFACEDESC2 Descriptor
        {
            get { return descriptor; }
            set { descriptor = value; }
        }

        /// <summary>
        ///  Retrieves the size of this surface.
        /// </summary>
        public RECT Rect
        {
            get { return rect; }
        }

        /// <summary>
        ///  Provides access to the native surface object
        /// </summary>
        public DirectDrawSurface7 Surface
        {
            get { return surface; }
        }

        /// <summary>
        ///  Attempts to generate a transparency key from an r,g,b byte color.
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        /// <returns></returns>
        public static DDCOLORKEY GenerateColorKey(byte r, byte g, byte b)
        {
            // This may not be perfect since we are going to have to average
            // different bit depths together
            var ddck = new DDCOLORKEY();
            var ddsd2 = new DDSURFACEDESC2();
            ManagedDirectX.DirectDraw.GetDisplayMode(ref ddsd2);

            var bBitCount = CountBits(ddsd2.ddpfPixelFormat.lBBitMask);
            var gBitCount = CountBits(ddsd2.ddpfPixelFormat.lGBitMask);
            var rBitCount = CountBits(ddsd2.ddpfPixelFormat.lRBitMask);

            var bBitMask = ddsd2.ddpfPixelFormat.lBBitMask;
            var gBitMask = ddsd2.ddpfPixelFormat.lGBitMask >> bBitCount;
            var rBitMask = ddsd2.ddpfPixelFormat.lRBitMask >> (gBitCount + bBitCount);

            var bValue = (b/255)*bBitMask;
            var gValue = (g/255)*gBitMask;
            var rValue = (r/255)*rBitMask;

            ddck.low = (rValue << (gBitCount + bBitCount)) + (gValue << bBitCount) + bValue;
            ddck.high = ddck.low;

            return ddck;
        }

        /// <summary>
        ///  Helper function for counting bits used when creating transparency keys.
        /// </summary>
        /// <param name="number">The number of bits</param>
        /// <returns>The number specified by the number of bits.</returns>
        private static int CountBits(int number)
        {
            var bits = 0;

            while (number != 0)
            {
                if ((number & 1) == 1)
                {
                    bits++;
                }
                number >>= 1;
            }

            return bits;
        }

        /// <summary>
        ///  Recreate the surface in the instance that the image
        ///  memory is lost do to a video mode switch.
        /// </summary>
        public void RestoreSurface()
        {
            if (surface == null || surface.isLost() != 0)
            {
                CreateSurface();
            }
        }

        /// <summary>
        ///  Helper function used to complete initialization of a surface.
        /// </summary>
        private void CreateSurface()
        {
#if TRACE
            ManagedDirectX.Profiler.Start("CreateSurface");
#endif
            try
            {
                if (string.IsNullOrEmpty(image))
                {
                    surface = ManagedDirectX.DirectDraw.CreateSurface(ref descriptor);
                    if (surface != null)
                    {
                        rect.Bottom = descriptor.lHeight;
                        rect.Right = descriptor.lWidth;
                    }
                }
                else
                {
                    try
                    {
                        Trace.WriteLine(image);
                        try
                        {
                            surface = ManagedDirectX.DirectDraw.CreateSurfaceFromFile(image, ref descriptor);
                        }
                        catch (ArgumentException)
                        {
                            descriptor = SystemMemorySurfaceDescription;
                            surface = ManagedDirectX.DirectDraw.CreateSurfaceFromFile(image, ref descriptor);
                        }
                    }
                    catch (COMException e)
                    {
                        // File Not Found
                        switch ((uint) e.ErrorCode)
                        {
                            case 0x800A0035:
                                Trace.WriteLine(
                                    "Could not find the file '" + image +
                                    "'.  This must be placed in the current directory.", "Picture Not Found");
                                break;
                            case 0x8876024E:
                                Trace.WriteLine(
                                    "The graphics card is in an unsupported mode.  We will try to initalize again later.");
                                throw new DirectXException(
                                    "Error Creating a DirectDraw Surface because of unsupported graphics mode.", e);
                            default:
                                Trace.WriteLine("Unexpected exception: " + e, "Unexpected Exception");
                                break;
                        }
                    }
                    rect.Bottom = descriptor.lHeight;
                    rect.Right = descriptor.lWidth;
                }
            }
            catch (Exception exc)
            {
                throw new DirectXException("Error Creating a DirectDraw Surface", exc);
            }
#if TRACE
            ManagedDirectX.Profiler.End("DirectDrawSurface.CreateSurface");
#endif
        }
    }
}