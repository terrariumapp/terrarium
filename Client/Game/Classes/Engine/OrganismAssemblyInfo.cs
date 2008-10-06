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
        ///  The full name of the assembly used for loading the assembly.
        /// </summary>
        private readonly string _assemblyFullName;

        /// <summary>
        ///  The short name of the assembly used for identification purposes.
        /// </summary>
        private readonly string _assemblyShortName;

        /// <summary>
        ///  Creates a new info object using the full name of the assembly
        ///  and the short name.
        /// </summary>
        /// <param name="assemblyFullName">The full assembly name.</param>
        /// <param name="assemblyShortName">The assembly short name.</param>
        public OrganismAssemblyInfo(string assemblyFullName, string assemblyShortName)
        {
            _assemblyFullName = assemblyFullName;
            _assemblyShortName = assemblyShortName;
        }

        /// <summary>
        ///  Returns the assembly full name that was saved on this info object.
        /// </summary>
        public string FullName
        {
            get { return _assemblyFullName; }
        }

        /// <summary>
        ///  Returns the assembly short name that was saved on this info object.
        /// </summary>
        public string ShortName
        {
            get { return _assemblyShortName; }
        }

        /// <summary>
        ///  Returns a string representation of this organism info object.  This
        ///  is primarily useful for when the class is used as a list item.
        /// </summary>
        /// <returns>The assembly short name.</returns>
        public override string ToString()
        {
            return _assemblyShortName;
        }
    }
}