//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Globalization;

using Terrarium.Configuration;
using Terrarium.Tools;

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
        ///  The list of sprite surfaces.
        /// </summary>
        private Hashtable sprites = null;

        /// <summary>
        ///  Initializes a new sprite surface manager and clears it out.
        ///  Clearing it sets up the necessary internal field values.
        /// </summary>
        internal TerrariumSpriteSurfaceManager()
        {
            Clear();
        }

        /// <summary>
        ///  Determines if the given file name is a valid match to the
        ///  given key.  
        /// </summary>
        /// <param name="name">The name of the file to check.</param>
        /// <param name="key">The key name to check the file against.</param>
        /// <returns>True if the file name is valid given the key, false otherwise.</returns>
        private bool IsSourceValid(string name, string key)
        {
            string fname = Path.GetFileNameWithoutExtension(name);
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
        ///  Counts the number of invalid surfaces in the list of files.
        /// </summary>
        /// <param name="bmps">The collection of bitmaps.</param>
        /// <param name="key">The key used to check the list.</param>
        /// <returns>The number of invalid sources in the list.</returns>
        private int CountInvalidSources(string[] bmps, string key)
        {
            // Remove invalid names
            // e.g Scor, grabbing Scorpion for instance
            int countOff = 0;
            for (int i = 0; i < bmps.Length; i++)
            {
                if (IsSourceValid(bmps[i], key))
                {
                    continue;
                }
            
                countOff++;
            }
        
            return countOff;
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
            if (key == null || key.Length == 0)
            {
                return;
            }
        
            sprites[key] = null;
        
            try
            {
                Int32.Parse(key);
                return;
            }
            catch
            {
            }
        
            TerrariumSpriteSurface tss = new TerrariumSpriteSurface();
            string[] bmps = Directory.GetFiles(GameConfig.MediaDirectory, key + "*.bmp");
            for (int i = 0; i < bmps.Length; i++)
            {
                if (!IsSourceValid(bmps[i], key))
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
                        sprites[key] = tss;
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
            if (key == null || key.Length == 0)
            {
                return;
            }
        
            sprites[key] = null;
        
            try
            {
                Int32.Parse(key);
                return;
            }
            catch
            {
            }

            string basepath = GameConfig.MediaDirectory + "\\" + key;
            string bmppath = basepath + ".bmp";
            if (File.Exists(bmppath))
            {
                TerrariumSpriteSurface tss = new TerrariumSpriteSurface();
                tss.AttachSurface(new DirectDrawSpriteSurface(key, bmppath, xFrames, yFrames));
                sprites[key] = tss;
            }
        }

        /// <summary>
        ///  Clears the sprite surface manager.  Creates a new case
        ///  insensitive hash of sprite surfaces.
        /// </summary>
        internal void Clear()
        {
            sprites = CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

        /// <summary>
        ///  The number of currently loaded sprite surfaces.
        /// </summary>
        internal int Count
        {
            get
            {
                return sprites.Count;
            }
        }

        /// <summary>
        ///  Returns a sprite surface for the given sprite key.
        /// </summary>
        internal TerrariumSpriteSurface this[string key]
        {
            get
            {
                if (key == null || key.Length == 0)
                {
                    return null;
                }

                if (!sprites.ContainsKey(key))
                {
                    Add(key, 10, 40); // This is adding a default sprite
                }
            
                return (TerrariumSpriteSurface) sprites[key];
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
                if (key == null || key.Length == 0)
                {
                    return null;
                }

                if (!sprites.ContainsKey(key))
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
            
                return (TerrariumSpriteSurface) sprites[key];
            }
        }

        /// <summary>
        ///  Removes a sprite surface from the manager.
        /// </summary>
        /// <param name="key">The sprite key of the sprite surface to remove.</param>
        internal void Remove(string key)
        {
            if (key == null || key.Length == 0)
            {
                return;
            }

            if (sprites.ContainsKey(key))
            {
                sprites.Remove(key);
            }
        }

        /// <summary>
        ///  Access to the hashtable of sprites.  This is direct access to
        ///  the underlying holding structure.  You shouldn't directly manipulate
        ///  the hashtable returned by this property.
        /// </summary>
        internal Hashtable Sprites
        {
            get
            {
                return sprites;
            }
        }
    }
}