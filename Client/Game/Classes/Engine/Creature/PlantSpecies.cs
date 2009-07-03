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
        /// <summary>
        ///  Creates a new species object from a CLR Type by
        ///  pulling information off of the type's attributes.
        /// </summary>
        /// <param name="clrType">The Type representing the Plant class.</param>
        public PlantSpecies(Type clrType) : base(clrType)
        {
            SkinFamily = PlantSkinFamily.Plant;
            Debug.Assert(clrType != null, "Null type passed to PlantSpecies");
            Debug.Assert(typeof (Plant).IsAssignableFrom(clrType));

            var skinAttribute =
                (PlantSkinAttribute) Attribute.GetCustomAttribute(clrType, typeof (PlantSkinAttribute));
            if (skinAttribute != null)
            {
                SkinFamily = skinAttribute.SkinFamily;
                Skin = skinAttribute.Skin;
            }

            var seedSpreadAttribute =
                (SeedSpreadDistanceAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (SeedSpreadDistanceAttribute));
            if (seedSpreadAttribute == null)
            {
                throw new AttributeRequiredException("SeedSpreadDistanceAttribute");
            }

            SeedSpreadDistance = seedSpreadAttribute.SeedSpreadDistance;
        }

        /// <summary>
        /// The maximum distance that seeds can go from the parent plant when reproducing.
        /// </summary>
        public int SeedSpreadDistance { get; private set; }

        #region IPlantSpecies Members

        /// <summary>
        /// The skin family for the organism.
        /// </summary>
        public PlantSkinFamily SkinFamily { get; private set; }

        /// <summary>
        /// See Species.LifeSpan for information about this member.
        /// </summary>
        public override int LifeSpan
        {
            get { return MatureRadius*EngineSettings.PlantLifeSpanPerUnitMaximumRadius; }
        }

        /// <summary>
        /// See Species.ReproductionWait for information about this member.
        /// </summary>
        public override int ReproductionWait
        {
            get { return MatureRadius*EngineSettings.PlantReproductionWaitPerUnitRadius; }
        }

        #endregion

        /// <summary>
        ///  Retrieves attribute warnings.  The Plant class implements no warnings
        ///  and so delegates to the base class for warnings.
        /// </summary>
        /// <returns>A message with available attribute warnings.</returns>
        public override string GetAttributeWarnings()
        {
            var warnings = new StringBuilder();
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
            // Need to start out hungry so they can't reproduce immediately and just populate the world
            var initialEnergy = EnergyState.Hungry;

            var newState = new PlantState(Guid.NewGuid().ToString(), this, generation, initialEnergy, InitialRadius) { Position = position };

            newState.ResetGrowthWait();
            return newState;
        }
    }
}