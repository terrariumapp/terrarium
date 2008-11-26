//------------------------------------------------------------------------------ 
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using OrganismBase;
using Terrarium.Hosting;
using Terrarium.Tools;

namespace Terrarium.Game
{
    /// <summary>
    ///  Base class for animal and plant species.  These
    ///  objects contains all the information about the characteristics
    ///  and abilities of a given species.
    /// </summary>
    /// <threadsafe/>
    [Serializable]
    public abstract class Species : ISpecies
    {
        /// <summary>
        ///  The full assembly name of the containing assembly for the typeName
        /// </summary>
        private readonly string _assemblyFullName;

        /// <summary>
        ///  The maximum amount of energey this species can attain.
        /// </summary>
        private readonly int _maximumEnergyPerUnitRadius;

        /// <summary>
        ///  The name of the CLR Type that represents this creature.
        /// </summary>
        private readonly string _typeName;

        /// <summary>
        ///  Cached Type object used to quickly compare species together.
        ///  This class is serialized, but this member is left out since
        ///  when the object is deserialized sometimes the assembly doesn't exist.
        /// </summary>
        [NonSerialized] private Type _speciesType;

        /// <summary>
        ///  Creates a new instance of the species class initialized with
        ///  the given CLR Type pulling various fields out of the Type's
        ///  attributes.
        /// </summary>
        /// <param name="clrType">The CLR Type to initialize this species.</param>
        protected Species(Type clrType)
        {
            MarkingColor = KnownColor.Black;
            _typeName = clrType.AssemblyQualifiedName;
            _assemblyFullName = clrType.Assembly.FullName;

            // Cache attributes because it is expensive to load them
            var energyAttribute =
                (MaximumEnergyPointsAttribute)
                Attribute.GetCustomAttribute(clrType, typeof (MaximumEnergyPointsAttribute));
            if (energyAttribute == null)
            {
                throw new AttributeRequiredException("MaximumEnergyPointsAttribute");
            }
            _maximumEnergyPerUnitRadius = energyAttribute.MaximumEnergyPerUnitRadius;

            var matureSizeAttribute =
                (MatureSizeAttribute) Attribute.GetCustomAttribute(clrType, typeof (MatureSizeAttribute));
            if (matureSizeAttribute == null)
            {
                throw new AttributeRequiredException("MatureSizeAttribute");
            }
            MatureRadius = matureSizeAttribute.MatureRadius;

            var markingColorAttribute =
                (MarkingColorAttribute) Attribute.GetCustomAttribute(clrType, typeof (MarkingColorAttribute));
            if (markingColorAttribute != null)
            {
                MarkingColor = markingColorAttribute.MarkingColor;
            }

            var authInfo =
                (AuthorInformationAttribute)
                Attribute.GetCustomAttribute(clrType.Assembly, typeof (AuthorInformationAttribute));
            if (authInfo == null || authInfo.AuthorName.Length == 0)
            {
                throw new AttributeRequiredException("AuthorInformationAttribute");
            }

            AuthorName = authInfo.AuthorName;
            AuthorEmail = authInfo.AuthorEmail;

            // The assembly name is the name of the organism
            Name = PrivateAssemblyCache.GetAssemblyShortName(clrType.Assembly.FullName);
        }

        /// <summary>
        ///  Return the type for this species.  Normally this value
        ///  is cached and very quick to return, otherwise the value
        ///  has to be looked up using the assembly resolver handlers.
        /// </summary>
        internal Type Type
        {
            get
            {
                if (_speciesType != null)
                {
                    return _speciesType;
                }

                // Since typeName is assembly qualified, we will get a chance to load it from
                // the PAC if the CLR doesn't find it
                try
                {
                    _speciesType = Type.GetType(_typeName);
                }
                catch (Exception e)
                {
                    ErrorLog.LogHandledException(e);
                }
                if (_speciesType == null)
                {
                    _speciesType = typeof (TerrariumOrganism);
                }
                return _speciesType;
            }
        }

        /// <summary>
        /// Returns the author's name
        /// </summary>
        public string AuthorName { get; private set; }

        /// <summary>
        /// Returns the author's email
        /// </summary>
        public string AuthorEmail { get; private set; }

        /// <summary>
        /// The custom coloring for the organism
        /// </summary>
        public KnownColor MarkingColor { get; private set; }

        /// <summary>
        /// The initial Radius of the organism.
        /// </summary>
        public int InitialRadius
        {
            get
            {
                // Ensure that the organism starts out and at least needs to grow a little to reach maturity
                return (EngineSettings.MinMatureSize/2) - 1;
            }
        }

        /// <summary>
        /// The name of the organism.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///  Retrieves assembly information about the current organism.
        /// </summary>
        public OrganismAssemblyInfo AssemblyInfo
        {
            get
            {
                return new OrganismAssemblyInfo(_assemblyFullName,
                                                PrivateAssemblyCache.GetAssemblyShortName(_assemblyFullName));
            }
        }

        #region ISpecies Members

        /// <summary>
        ///  The size of the organism when mature.
        /// </summary>
        public int MatureRadius { get; private set; }

        /// <summary>
        /// The custom skin name for the organism.
        /// </summary>
        public string Skin { get; protected set; }

        /// <summary>
        /// How long the organism must wait between reproduction.
        /// </summary>
        public abstract int ReproductionWait { get; }

        /// <summary>
        /// The lifespan of the organism.
        /// </summary>
        public abstract int LifeSpan { get; }

        /// <summary>
        /// How long the organism must wait to grow.
        /// </summary>
        public int GrowthWait
        {
            get
            {
                // This ensures that they can reach maximum size halfway through their life
                return (LifeSpan/2)/(MatureRadius - InitialRadius);
            }
        }

        /// <summary>
        /// The maximum amount of energy the organism can store.
        /// </summary>
        public int MaximumEnergyPerUnitRadius
        {
            get { return _maximumEnergyPerUnitRadius; }
        }

        /// <summary>
        ///  This method can be used to compare another species to this species
        ///  and determine if they match.
        /// </summary>
        /// <param name="species">The species to be compared</param>
        /// <returns>True if the species match, false otherwise.</returns>
        public bool IsSameSpecies(ISpecies species)
        {
            return ((Species) species).Type == Type;
        }

        #endregion

        /// <summary>
        ///  Gets attribute warnings for specific attributes when points are being wasted
        ///  or points are out of bounds.
        /// </summary>
        /// <returns>A message containing attribute warnings, or an empty string.</returns>
        public virtual string GetAttributeWarnings()
        {
            var warnings = new StringBuilder();

            var energyAttribute =
                (MaximumEnergyPointsAttribute) Attribute.GetCustomAttribute(Type, typeof (MaximumEnergyPointsAttribute));
            var newWarning = energyAttribute.GetWarnings();
            if (newWarning.Length != 0)
            {
                warnings.Append(newWarning);
                warnings.Append(Environment.NewLine);
            }

            return warnings.ToString();
        }

        /// <summary>
        ///  Required method for any derived classes that can be used to
        ///  create a new state object based on the species.
        /// </summary>
        /// <param name="position">The position for the new state.</param>
        /// <param name="generation">The creature's generation for the new state.</param>
        /// <returns>A state object based on the species object.</returns>
        public abstract OrganismState InitializeNewState(Point position, int generation);

        /// <summary>
        ///  Called to create a new species object from the given assembly.  This
        ///  should be the only entry point for creating a new species object.
        /// </summary>
        /// <param name="organismAssembly">The assembly to generate the species from.</param>
        /// <returns>A new species object generated from the assembly.</returns>
        public static Species GetSpeciesFromAssembly(Assembly organismAssembly)
        {
            var hasOrganismClassAttribute = false;

            var attributes = Attribute.GetCustomAttributes(organismAssembly);
            if (attributes.Length == 0)
            {
                throw new AttributeRequiredException("OrganismClassAttribute");
            }

            foreach (var attribute in attributes)
            {
                if (attribute.GetType().Name != "OrganismClassAttribute") continue;
                hasOrganismClassAttribute = true;
                break;
            }

            var classAttribute =
                (OrganismClassAttribute) Attribute.GetCustomAttribute(organismAssembly, typeof (OrganismClassAttribute));
            if (classAttribute == null)
            {
                // When an organism has the OrganismClassAttribute but GetCustomAttribute returns null, it means
                // that they have a different version of the attribute.
                if (hasOrganismClassAttribute)
                {
                    throw new GameEngineException(
                        "Your organism is built against a different version of Terrarium than you are running.  Try rebuilding it.");
                }
                throw new AttributeRequiredException("OrganismClassAttribute");
            }

            Type clrType;

            try
            {
                clrType = organismAssembly.GetType(classAttribute.ClassName, true);
            }
            catch (TypeLoadException)
            {
                throw new GameEngineException(
                    string.Format(
                        "Your organism {0} could not be found in the assembly.  Please make sure this class exists.",
                        classAttribute.ClassName));
            }

            if (typeof (Plant).IsAssignableFrom(clrType))
            {
                return new PlantSpecies(clrType);
            }
            if (typeof (Animal).IsAssignableFrom(clrType))
            {
                return new AnimalSpecies(clrType);
            }

            throw new GameEngineException(
                string.Format("Class specified in OrganismClassAttribute ({0}) doesn't derive from Animal or Plant",
                              classAttribute.ClassName));
        }
    }
}