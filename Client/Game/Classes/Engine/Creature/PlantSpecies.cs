//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    /// Holds all species information about a plant.  See Species for more information.
    /// </summary>
    [Serializable]
    public sealed class PlantSpecies : Species, IPlantSpecies 
    {
        PlantSkinFamily skinFamily = PlantSkinFamily.Plant;
        int seedSpreadDistance;

        /// <summary>
        ///  Creates a new species object from a CLR Type by
        ///  pulling information off of the type's attributes.
        /// </summary>
        /// <param name="clrType">The Type representing the Plant class.</param>
        public PlantSpecies(Type clrType) : base(clrType)
        {
            Debug.Assert(clrType != null, "Null type passed to PlantSpecies");
            Debug.Assert(typeof(Plant).IsAssignableFrom(clrType));

            PlantSkinAttribute skinAttribute = (PlantSkinAttribute) Attribute.GetCustomAttribute(clrType, typeof(PlantSkinAttribute));
            if (skinAttribute != null)
            {
                skinFamily = skinAttribute.SkinFamily;
                animalSkin = skinAttribute.Skin;
            }

            SeedSpreadDistanceAttribute seedSpreadAttribute = (SeedSpreadDistanceAttribute) Attribute.GetCustomAttribute(clrType, typeof(SeedSpreadDistanceAttribute));
            if (seedSpreadAttribute == null)
            {
                throw new AttributeRequiredException("SeedSpreadDistanceAttribute");
            }

            seedSpreadDistance = seedSpreadAttribute.SeedSpreadDistance;
        }

        /// <summary>
        ///  Retrieves attribute warnings.  The Plant class implements no warnings
        ///  and so delegates to the base class for warnings.
        /// </summary>
        /// <returns>A message with available attribute warnings.</returns>
        public override string GetAttributeWarnings() 
        {
            StringBuilder warnings = new StringBuilder();
            warnings.Append(base.GetAttributeWarnings());

            return warnings.ToString();
        }

        /// <summary>
        ///  Initializes a new state object with the given position and family generation
        /// </summary>
        /// <param name="position">The position of the new PlantState</param>
        /// <param name="generation">The family generation of the new Plant</param>
        /// <returns>A state object initialized to the given position and generation.</returns>
        public override OrganismState InitializeNewState(Point position, int generation)
        {
            PlantState newState = new PlantState(Guid.NewGuid().ToString(), this, generation);
            newState.Position = position;
            newState.IncreaseRadiusTo(InitialRadius);

            // Need to start out hungry so they can't reproduce immediately and just populate the world
            newState.StoredEnergy = newState.UpperBoundaryForEnergyState(EnergyState.Hungry);
            newState.ResetGrowthWait();
            return newState;
        }

        /// <summary>
        /// The skin family for the organism.
        /// </summary>
        public PlantSkinFamily SkinFamily
        {
            get
            {
                return skinFamily;
            }
        }


        /// <summary>
        /// See Species.LifeSpan for information about this member.
        /// </summary>
        public override int LifeSpan
        {
            get
            {
                return MatureRadius * EngineSettings.PlantLifeSpanPerUnitMaximumRadius;
            }
        }

        /// <summary>
        /// See Species.ReproductionWait for information about this member.
        /// </summary>
        public override int ReproductionWait
        {
            get 
            {
                return MatureRadius * EngineSettings.PlantReproductionWaitPerUnitRadius;
            }
        }

        /// <summary>
        /// The maximum distance that seeds can go from the parent plant when reproducing.
        /// </summary>
        public int SeedSpreadDistance 
        {
            get 
            {
                return seedSpreadDistance;
            }
        }
    }
}