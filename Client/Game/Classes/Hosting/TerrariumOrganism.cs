//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System.Drawing;
using System.IO;
using OrganismBase;
using Terrarium.Game;

namespace Terrarium.Hosting
{
    /// <summary>
    /// Test animal whose sole purpose is to throw an exception.
    /// We use it when we are deserializing organisms and find one that won't load
    /// instead of aborting deserialization, we just use this organism so we can continue.
    /// </summary>
    [Carnivore(true)]
    [MatureSize(28)]
    [AnimalSkin(AnimalSkinFamily.Ant)]
    [MarkingColor(KnownColor.Teal)]
    [MaximumEnergyPoints(0)]
    [EatingSpeedPoints(0)]
    [AttackDamagePoints(60)]
    [DefendDamagePoints(0)]
    [MaximumSpeedPoints(10)]
    [CamouflagePoints(0)]
    [EyesightPoints(30)]
    internal class TerrariumOrganism : Animal
    {
        protected override void Initialize()
        {
            Load += LoadEvent;
            throw new OrganismBlacklistedException();
        }

        private static void LoadEvent(object sender, LoadEventArgs e)
        {
            throw new OrganismBlacklistedException();
        }

        public override void SerializeAnimal(MemoryStream m)
        {
        }

        public override void DeserializeAnimal(MemoryStream m)
        {
        }
    }
}