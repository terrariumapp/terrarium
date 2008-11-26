//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Base class for all serialization binders.  Contains
    ///  methods for validating a number of common assemblies.
    ///  The Binder is being added so that we can validate data being deserialized.  We
    ///  want to make sure someone doesn't insert bogus types into the stream or types
    ///  that might be able to be loaded on the destination client and used for the
    ///  purposes of a hack.
    /// </summary>
    public abstract class OrganismBaseBinder : SerializationBinder
    {
        /// <summary>
        ///  The purpose of this function is to make sure that the assemblies are effectively the
        ///  same, everything except version number.  This is how we check what assemblies
        ///  serialization is trying to deserialize.  
        ///  Sample:
        ///  mscorlib, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        /// </summary>
        /// <param name="asm1">Full Assembly name of the first assembly.</param>
        /// <param name="asm2">Full assembly name of the second assembly.</param>
        /// <returns>True if the assemblies match in part, false otherwise.</returns>
        protected static bool CompareParts(string asm1, string asm2)
        {
            try
            {
                var parts1 = asm1.Split(',');
                var parts2 = asm2.Split(',');
                return (parts1[0] == parts2[0] && parts1[2] == parts2[2]) && parts1[3] == parts2[3];
            }
            catch
            {
                Trace.WriteLine("Error parsing assembly strings");
            }

            return false;
        }

        /// <summary>
        ///  Determines if an assembly full name matches that of OrganismBase.dll
        /// </summary>
        /// <param name="asm">assembly full name to match.</param>
        /// <returns>True if the assembly is OrganismBase.dll</returns>
        protected static bool IsOrganismBase(string asm)
        {
            return CompareParts(typeof (Animal).Assembly.FullName, asm);
        }

        /// <summary>
        ///  Determines if an assembly full name matches that of MSCorLib.dll
        /// </summary>
        /// <param name="asm">assembly full name to match.</param>
        /// <returns>True if the assembly is MSCorLib.dll</returns>
        protected static bool IsMscorlib(string asm)
        {
            return CompareParts(typeof (Guid).Assembly.FullName, asm);
        }

        /// <summary>
        ///  Determines if an assembly full name matches that of System.Drawing.dll
        /// </summary>
        /// <param name="asm">assembly full name to match.</param>
        /// <returns>True if the assembly is System.Drawing.dll</returns>
        protected static bool IsSystemDrawing(string asm)
        {
            return CompareParts(typeof (Point).Assembly.FullName, asm);
        }

        /// <summary>
        ///  Determines if an assembly full name matches that of TerrariumEngine.dll
        /// </summary>
        /// <param name="asm">assembly full name to match.</param>
        /// <returns>True if the assembly is TerrariumEngine.dll</returns>
        protected static bool IsTerrarium(string asm)
        {
            return CompareParts(typeof (GameEngine).Assembly.FullName, asm);
        }

        /// <summary>
        ///  Determines if an assembly full name matches that of System.dll
        /// </summary>
        /// <param name="asm">assembly full name to match.</param>
        /// <returns>True if the assembly is System.dll</returns>
        protected static bool IsSystem(string asm)
        {
            return CompareParts(typeof (Hashtable).Assembly.FullName, asm);
        }
    }
}