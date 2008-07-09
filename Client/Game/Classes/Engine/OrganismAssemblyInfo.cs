//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace Terrarium.Game 
{
    /// <summary>
    ///  Contains assembly information about an organism.
    /// </summary>
    /// <threadsafe/>
    public class OrganismAssemblyInfo
    {
        /// <summary>
        ///  The full name of the assembly used for loading the assembly.
        /// </summary>
        string assemblyFullName;

        /// <summary>
        ///  The short name of the assembly used for identification purposes.
        /// </summary>
        string assemblyShortName;
    
        /// <summary>
        ///  Creates a new info object using the full name of the assembly
        ///  and the short name.
        /// </summary>
        /// <param name="assemblyFullName">The full assembly name.</param>
        /// <param name="assemblyShortName">The assembly short name.</param>
        public OrganismAssemblyInfo(string assemblyFullName, string assemblyShortName)
        {
            this.assemblyFullName = assemblyFullName;
            this.assemblyShortName = assemblyShortName;
        }
    
        /// <summary>
        ///  Returns the assembly full name that was saved on this info object.
        /// </summary>
        public string FullName
        {
            get
            {
                return assemblyFullName;
            }
        }
    
        /// <summary>
        ///  Returns the assembly short name that was saved on this info object.
        /// </summary>
        public string ShortName
        {
            get
            {
                return assemblyShortName;
            }
        }

        /// <summary>
        ///  Returns a string representation of this organism info object.  This
        ///  is primarily useful for when the class is used as a list item.
        /// </summary>
        /// <returns>The assembly short name.</returns>
        public override string ToString()
        {
            return assemblyShortName;
        }
    }
}