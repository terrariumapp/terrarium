//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.Game
{
    /// <summary>
    ///  Contains assembly information about an organism.
    /// </summary>
    /// <threadsafe/>
    public class OrganismAssemblyInfo
    {
        /// <summary>
        ///  Creates a new info object using the full name of the assembly
        ///  and the short name.
        /// </summary>
        /// <param name="assemblyFullName">The full assembly name.</param>
        /// <param name="assemblyShortName">The assembly short name.</param>
        public OrganismAssemblyInfo(string assemblyFullName, string assemblyShortName)
        {
            FullName = assemblyFullName;
            ShortName = assemblyShortName;
        }

        /// <summary>
        ///  Returns the assembly full name that was saved on this info object.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        ///  Returns the assembly short name that was saved on this info object.
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        ///  Returns a string representation of this organism info object.  This
        ///  is primarily useful for when the class is used as a list item.
        /// </summary>
        /// <returns>The assembly short name.</returns>
        public override string ToString()
        {
            return ShortName;
        }
    }
}