//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Terrarium.Configuration;
using Terrarium.Renderer.DirectX;

namespace Terrarium.Renderer
{
    /// <summary>
    ///  The Terrarium sprite surface manager manages keyed terrarium
    ///  sprite surfaces.  It attempts to load resources first from
    ///  disk based image, then from assembly based resources.
    /// </summary>
    internal class TerrariumSpriteSurfaceManager
    {
        /// <summary>
        ///  Initializes a new sprite surface manager and clears it out.
        ///  Clearing it sets up the necessary internal field values.
        /// </summary>
        internal TerrariumSpriteSurfaceManager()
        {
            Clear();
        }

        /// <summary>
        ///  The number of currently loaded sprite surfaces.
        /// </summary>
        internal int Count
        {
            get { return Sprites.Count; }
        }

        /// <summary>
        ///  Returns a sprite surface for the given sprite key.
        /// </summary>
        internal TerrariumSpriteSurface this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                if (!Sprites.ContainsKey(key))
                {
                    Add(key, 10, 40); // This is adding a default sprite
                }

                return (TerrariumSpriteSurface) Sprites[key];
            }
        }

        /// <summary>
        ///  Returns an optimally sized sprite surface for a given sprite
        ///  key, a given ideal size, and whether or not the type of sprite
        ///  is that of a creature or plant.
        /// </summary>
        internal TerrariumSpriteSurface this[string key, int size, bool animal]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                if (!Sprites.ContainsKey(key))
                {
                    if (animal)
                    {
                        AddSizedSurface(key, 10, 40); // This is adding a default sprite
                    }
                    else
                    {
                        AddSizedSurface(key, 1, 1); // This is adding a default sprite
                    }
                }

                return (TerrariumSpriteSurface) Sprites[key];
            }
        }

        /// <summary>
        ///  Access to the hashtable of sprites.  This is direct access to
        ///  the underlying holding structure.  You shouldn't directly manipulate
        ///  the hashtable returned by this property.
        /// </summary>
        internal Hashtable Sprites { get; private set; }

        /// <summary>
        ///  Determines if the given file name is a valid match to the
        ///  given key.  
        /// </summary>
        /// <param name="name">The name of the file to check.</param>
        /// <param name="key">The key name to check the file against.</param>
        /// <returns>True if the file name is valid given the key, false otherwise.</returns>
        private static bool isSourceValid(string name, string key)
        {
            var fname = Path.GetFileNameWithoutExtension(name);
            if (fname.Length > key.Length)
            {
                // Strip off the prefix.  We should be left
                // with a set of numbers now.
                fname = fname.Substring(key.Length);
                try
                {
                    // If we parse it then we need to move on.
                    Int32.Parse(fname);
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        ///  Adds a new sized surface to the sprite manager.  A sized surface
        ///  attempts to load multiple surfaces for the various sizes of a keyed
        ///  sprite.
        /// </summary>
        /// <param name="key">The name of the sprite surface.</param>
        /// <param name="xFrames">The number of frames of animation.</param>
        /// <param name="yFrames">The number of frame types.</param>
        internal void AddSizedSurface(string key, int xFrames, int yFrames)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Sprites[key] = null;

            try
            {
                Int32.Parse(key);
                return;
            }
            catch
            {
            }

            var tss = new TerrariumSpriteSurface();
            var bmps = Directory.GetFiles(GameConfig.MediaDirectory, key + "*.bmp");
            for (var i = 0; i < bmps.Length; i++)
            {
                if (!isSourceValid(bmps[i], key))
                {
                    continue;
                }

                DirectDrawSpriteSurface dds;

                try
                {
                    dds = new DirectDrawSpriteSurface(
                        Path.GetFileNameWithoutExtension(bmps[i]),
                        bmps[i],
                        xFrames,
                        yFrames
                        );
                }
                catch
                {
                    dds = null;
                }

                if (dds != null)
                {
                    // Only if frames are between 8 and 48 pixels.
                    if (dds.FrameWidth > 7 && dds.FrameWidth < 49)
                    {
                        tss.AttachSurface(dds, dds.FrameWidth);
                        Sprites[key] = tss;
                    }
                }
            }
        }

        /// <summary>
        ///  Add a single size surface.
        /// </summary>
        /// <param name="key">The key of the sprite surface.</param>
        /// <param name="xFrames">The number of frames of animation.</param>
        /// <param name="yFrames">The number of frame types.</param>
        internal void Add(string key, int xFrames, int yFrames)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Sprites[key] = null;

            try
            {
                Int32.Parse(key);
                return;
            }
            catch
            {
            }

            var basepath = GameConfig.MediaDirectory + "\\" + key;
            var bmppath = basepath + ".bmp";
            if (File.Exists(bmppath))
            {
                var tss = new TerrariumSpriteSurface();
                tss.AttachSurface(new DirectDrawSpriteSurface(key, bmppath, xFrames, yFrames));
                Sprites[key] = tss;
            }
        }

        /// <summary>
        ///  Clears the sprite surface manager.  Creates a new case
        ///  insensitive hash of sprite surfaces.
        /// </summary>
        internal void Clear()
        {
            Sprites = CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

        /// <summary>
        ///  Removes a sprite surface from the manager.
        /// </summary>
        /// <param name="key">The sprite key of the sprite surface to remove.</param>
        internal void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            if (Sprites.ContainsKey(key))
            {
                Sprites.Remove(key);
            }
        }
    }
}