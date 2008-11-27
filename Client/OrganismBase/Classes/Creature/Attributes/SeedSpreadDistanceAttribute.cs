//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  This attribute would control the distance that a plant could spread it
    ///  seeds were it used.  Every time a plant was ready to reproduce a new
    ///  plant would be generated within the radius specified by this attribute.
    ///  (not currently used by Terrarium)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class SeedSpreadDistanceAttribute : Attribute
    {
        /// <summary>
        ///  Creates a new attribute that can be used on plants to specify
        ///  how far new children can appear from the plant.  This attribute
        ///  isn't implemented in the current Terrarium.
        /// </summary>
        /// <param name="seedSpreadDistance">The distance seeds can spread.</param>
        public SeedSpreadDistanceAttribute(int seedSpreadDistance)
        {
            if (seedSpreadDistance > EngineSettings.MaxSeedSpreadDistance)
            {
                throw new ApplicationException(
                    "You have placed too many points into SeedSpreadDistance.  Please limit this number to " +
                    EngineSettings.MaxSeedSpreadDistance + ".");
            }

            SeedSpreadDistance = seedSpreadDistance;
        }

        /// <summary>
        ///  Read-only access to the distacnce seeds can be spread.
        /// </summary>
        public int SeedSpreadDistance { get; private set; }
    }
}