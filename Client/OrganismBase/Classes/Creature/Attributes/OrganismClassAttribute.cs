//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Use this attribute to identify the name of the class in your assembly that derives
    ///   from either Plant or Animal, and that contains the code required for your creature
    ///   to operate.  This is required for each organism assembly and a load time error
    ///   will be thrown if you try to introduce an assembly without one.
    ///  </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = true, AllowMultiple = false)]
    public sealed class OrganismClassAttribute : Attribute
    {
        /// <summary>
        ///  <para>
        ///   Creates a new attribute that can be used to mark the creature class
        ///   within an assembly.  Only one class per assembly may be marked, and only
        ///   one instance of this attribute per assembly may exist.  The class will
        ///   have to pass other forms of verification as well and the load may still
        ///   fail.
        ///  </para>
        /// </summary>
        /// <param name="className">
        ///  The namespace qualified name of the class that implements your creature.
        /// </param>
        public OrganismClassAttribute(string className)
        {
            ClassName = className;
        }

        /// <summary>
        ///  Provides read-only access to the name of the class that
        ///  represents the creature class within the assembly.
        /// </summary>
        public string ClassName { get; private set; }
    }
}