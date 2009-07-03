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
    ///  Holds all species information about an animal.  See Species for more information
    /// </summary>
    [Serializable]
    public sealed class AnimalSpecies : Species, IAnimalSpecies
    {
        private readonly int _eatingSpeedPerUnitRadius;

        /// <summary>
        ///  Creates a new Animal species from a CLR Type object.  Initializes
        ///  the new species properties based on various attributes on the Type.
        /// </summary>
        /// <param name="clrType">The type for the organism class.</param>
        public AnimalSpecies(Type clrType) : base(clrType)
        {
            SkinFamily = AnimalSkinFamily.Spider;
            var totalPoints = 0;
            Debug.Assert(clrType != null, "Null type passed to AnimalSpecies");
            Debug.Assert(typeof (Animal).IsAssignableFrom(clrType));

            var skinAttribute =
                (AnimalSkinAttribute) Attribute.GetCustomAttribute(clrType, typeof (AnimalSkinAttribute));
            if (skinAttribute != null)
            {
                SkinFamily = skinAttribute.SkinFamily;
                Skin = skinAttribute.Skin;
            }

            var carnivoreAttribute =
                (CarnivoreAttribute) Attribute.GetCustomAttribute(clrType, typeof (CarnivoreAttribute));
            if (carnivoreAttribute == null)
            {
                throw new AttributeRequiredException("CarnivoreAttribute");
            }
            IsCarnivore = carnivoreAttribute.IsCarnivore;

            var eatingSpeedAttribute =
                (EatingSpeedPointsAttribute) Attribute.GetCustomAttribute(clrType, typeof (EatingSpeedPointsAttribute));
            if (eatingSpeedAttribute == null)
            {
                throw new AttributeRequiredException("EatingSpeedPointsAttribute");
            }
            _eatingSpeedPerUnitRadius = eatingSpeedAttribute.EatingSpeedPerUnitRadius;
            totalPoints += eatingSpeedAttribute.Points;

            var attackDamageAttribute =
                (AttackDamagePointsAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (AttackDamagePointsAttribute));
            if (attackDamageAttribute == null)
            {
                throw new AttributeRequiredException("AttackDamagePointsAttribute");
            }
            if (IsCarnivore)
            {
                MaximumAttackDamagePerUnitRadius =
                    (int)
                    (attackDamageAttribute.MaximumAttackDamagePerUnitRadius*
                     EngineSettings.CarnivoreAttackDefendMultiplier);
            }
            else
            {
                MaximumAttackDamagePerUnitRadius = attackDamageAttribute.MaximumAttackDamagePerUnitRadius;
            }
            totalPoints += attackDamageAttribute.Points;

            var defendDamageAttribute =
                (DefendDamagePointsAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (DefendDamagePointsAttribute));
            if (defendDamageAttribute == null)
            {
                throw new AttributeRequiredException("DefendDamagePointsAttribute");
            }
            if (IsCarnivore)
            {
                MaximumDefendDamagePerUnitRadius =
                    (int)
                    (defendDamageAttribute.MaximumDefendDamagePerUnitRadius*
                     EngineSettings.CarnivoreAttackDefendMultiplier);
            }
            else
            {
                MaximumDefendDamagePerUnitRadius = defendDamageAttribute.MaximumDefendDamagePerUnitRadius;
            }
            totalPoints += defendDamageAttribute.Points;

            var energyAttribute =
                (MaximumEnergyPointsAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (MaximumEnergyPointsAttribute));
            if (energyAttribute == null)
            {
                throw new AttributeRequiredException("MaximumEnergyPointsAttribute");
            }
            totalPoints += energyAttribute.Points;

            var speedAttribute =
                (MaximumSpeedPointsAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (MaximumSpeedPointsAttribute));
            if (speedAttribute == null)
            {
                throw new AttributeRequiredException("MaximumSpeedPointsAttribute");
            }
            totalPoints += speedAttribute.Points;
            MaximumSpeed = speedAttribute.MaximumSpeed;

            var camouflageAttribute =
                (CamouflagePointsAttribute) Attribute.GetCustomAttribute(clrType, typeof (CamouflagePointsAttribute));
            if (camouflageAttribute == null)
            {
                throw new AttributeRequiredException("CamouflagePointsAttribute");
            }
            totalPoints += camouflageAttribute.Points;
            InvisibleOdds = camouflageAttribute.InvisibleOdds;

            var eyesightAttribute =
                (EyesightPointsAttribute) Attribute.GetCustomAttribute(clrType, typeof (EyesightPointsAttribute));
            if (eyesightAttribute == null)
            {
                throw new AttributeRequiredException("EyesightPointsAttribute");
            }
            totalPoints += eyesightAttribute.Points;
            EyesightRadius = eyesightAttribute.EyesightRadius;

            if (totalPoints > EngineSettings.MaxAvailableCharacteristicPoints)
            {
                throw new TooManyPointsException();
            }
        }

        #region IAnimalSpecies Members

        /// <summary>
        ///  The amount of time the creature must wait before they
        ///  can reproduce
        /// </summary>
        public override int ReproductionWait
        {
            get { return MatureRadius*EngineSettings.AnimalReproductionWaitPerUnitRadius; }
        }

        /// <summary>
        ///  Returns the total number of game ticks the creature can live before
        ///  dying of old age.
        /// </summary>
        public override int LifeSpan
        {
            get
            {
                if (IsCarnivore)
                {
                    return MatureRadius*EngineSettings.AnimalLifeSpanPerUnitMaximumRadius*
                           EngineSettings.CarnivoreLifeSpanMultiplier;
                }
                return MatureRadius*EngineSettings.AnimalLifeSpanPerUnitMaximumRadius;
            }
        }

        /// <returns>
        ///  True if the animal is a carnivore, otherwise false.
        /// </returns>
        public bool IsCarnivore { get; private set; }

        /// <returns>
        ///  The speed that the animal can eat.  This is multiplied by the
        ///  radius of the creature to get the real eating speed.
        /// </returns>
        public int EatingSpeedPerUnitRadius
        {
            get { return _eatingSpeedPerUnitRadius; }
        }

        /// <returns>
        ///  The skin family for the organism.
        /// </returns>
        public AnimalSkinFamily SkinFamily { get; private set; }

        /// <returns>
        ///  The maximum damage the species can inflict per unit of its radius.
        /// </returns>
        public int MaximumAttackDamagePerUnitRadius { get; private set; }

        /// <returns>
        ///  The maximum damage the species can defend against per unit of its radius.
        /// </returns>
        public int MaximumDefendDamagePerUnitRadius { get; private set; }

        /// <returns>
        ///  The maximum speed the species can attain.
        /// </returns>
        public int MaximumSpeed { get; private set; }

        /// <returns>
        /// The odds that the species is invisible to a call to Animal.Scan() by another species.
        /// </returns>
        public int InvisibleOdds { get; private set; }

        /// <returns>
        /// The distance animal can see.
        /// </returns>
        public int EyesightRadius { get; private set; }

        #endregion

        /// <summary>
        ///  Generates warnings for attributes that have wasted points.
        /// </summary>
        /// <returns>Message about wasted points, or empty if there aren't any messages.</returns>
        public override string GetAttributeWarnings()
        {
            var warnings = new StringBuilder();
            warnings.Append(base.GetAttributeWarnings());

            var eatingSpeedAttribute =
                (EatingSpeedPointsAttribute) Attribute.GetCustomAttribute(Type, typeof (EatingSpeedPointsAttribute));
            var newWarning = eatingSpeedAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var attackDamageAttribute =
                (AttackDamagePointsAttribute) Attribute.GetCustomAttribute(Type, typeof (AttackDamagePointsAttribute));
            newWarning = attackDamageAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var defendDamageAttribute =
                (DefendDamagePointsAttribute) Attribute.GetCustomAttribute(Type, typeof (DefendDamagePointsAttribute));
            newWarning = defendDamageAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var energyAttribute =
                (MaximumEnergyPointsAttribute) Attribute.GetCustomAttribute(Type, typeof (MaximumEnergyPointsAttribute));
            newWarning = energyAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var speedAttribute =
                (MaximumSpeedPointsAttribute) Attribute.GetCustomAttribute(Type, typeof (MaximumSpeedPointsAttribute));
            newWarning = speedAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var camouflageAttribute =
                (CamouflagePointsAttribute) Attribute.GetCustomAttribute(Type, typeof (CamouflagePointsAttribute));
            newWarning = camouflageAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            var eyesightAttribute =
                (EyesightPointsAttribute) Attribute.GetCustomAttribute(Type, typeof (EyesightPointsAttribute));
            newWarning = eyesightAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            return warnings.ToString();
        }

        /// <summary>
        ///  Initializes a new state given a position and a generation.  This is
        ///  used when creatures give birth, and the state has to effectively
        ///  be cloned.
        /// </summary>
        /// <param name="position">The new position of the creature in the world.</param>
        /// <param name="generation">The family generation for this creature.</param>
        /// <returns>A new state to represent the creature.</returns>
        public override OrganismState InitializeNewState(Point position, int generation)
        {
            // Need to start out hungry so they can't reproduce immediately and just populate the world
            var initialEnergy = EnergyState.Hungry;

            var newState = new AnimalState(Guid.NewGuid().ToString(), this, generation, initialEnergy, InitialRadius) { Position = position };

            newState.ResetGrowthWait();
            return newState;
        }
    }
}