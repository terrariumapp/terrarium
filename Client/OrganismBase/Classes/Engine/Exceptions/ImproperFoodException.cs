//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    // Action Exceptions
    /// <summary>
    /// Organism tried to eat an improper food: Carnivores must eat meat, and herbivores must eat plants.
    /// </summary>
    public class ImproperFoodException : OrganismException
    {
        internal ImproperFoodException()
            : base("Organism tried to eat an improper food: Carnivores must eat meat, and herbivores must eat plants.")
        {
        }
    }
}