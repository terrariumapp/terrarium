//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>Chooses whether your animal is an herbivore or a carnivore.</summary>
    /// <remarks>
    /// <para>
    /// <list type="bullet">
    /// <listheader>Pros for herbivores</listheader>
    /// <item><term>Easier to find food, but need to eat more of it because plants give less energy than meat</term></item>
    /// </list>
    /// </para>
    /// <para>
    /// <list type="bullet">
    /// <listheader>Pros for carnivores</listheader>
    /// <item><term>Harder to find food, but need to less of it because meat gives more energy than plants</term></item>
    /// </list>
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class CarnivoreAttribute : Attribute
    {
        /// <summary>
        ///  Creates a new carnivore attribute determining if the target creature
        ///  should be a carnivore or not.
        /// </summary>
        /// <param name="isCarnivore">True if your organism is a carnivore, false if they are an herbivore.</param>
        public CarnivoreAttribute(Boolean isCarnivore)
        {
            IsCarnivore = isCarnivore;
        }

        /// <summary>
        ///  Read-only access to whether or not the attribute is used to specify
        ///  a Carnivore type creature.
        /// </summary>
        public bool IsCarnivore { get; private set; }
    }
}