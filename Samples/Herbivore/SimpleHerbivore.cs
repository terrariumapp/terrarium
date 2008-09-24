using System;
using System.Collections;
using System.Drawing;
using System.IO;
using OrganismBase;

namespace Herbivore
{
    // It's an herbivore
    [Carnivore(false)]
    // This value must be between 24 and 48, 24 means faster reproduction
    // while 48 would give more defense and attack power
    // Make it smaller for reproduction
    [MatureSize(26)]
    // AnimalSkin = AnimalSkinFamilyEnum.Beetle, you can be a Beetle
    // an Ant, a Scorpion, an Inchworm, or a Spider
    // MarkingColor = KnownColor.Red, you can choose to mark your
    // creature with a color.  This does not affect appearance in
    // the game.
    [AnimalSkin(AnimalSkinFamily.Beetle)]
    [MarkingColor(KnownColor.Red)]
    // Point Based Attributes
    // You get 100 points to distribute among these attributes to define
    // what your organism can do.  Choose them based on the strategy your organism
    // will use.  This organism hides and has good eyesight to find plants.
    [MaximumEnergyPoints(0)] // Don't need to increase this as it just sits next to plants
    [EatingSpeedPoints(0)] // Ditto
    [AttackDamagePoints(0)] // Doesn't ever attack
    [DefendDamagePoints(0)] // Doesn't even defend
    [MaximumSpeedPoints(0)] // Doesn't need to move quickly
    [CamouflagePoints(50)] // Try to remain hidden
    [EyesightPoints(50)] // Need this to find plants better
    public class SimpleHerbivore : Animal
    {
        // The current plant we're going after
        private PlantState targetPlant;

        /// <summary>
        /// Set up any event handlers that we want when first initialized
        /// </summary>
        protected override void Initialize()
        {
            Load += LoadEvent;
            Idle += IdleEvent;
        }

        /// <summary>
        /// First event fired on an organism each turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadEvent(object sender, LoadEventArgs e)
        {
            try
            {
                if (targetPlant != null)
                {
                    // See if our target plant still exists (it may have died)
                    // LookFor returns null if it isn't found
                    targetPlant = (PlantState) LookFor(targetPlant);
                }
            }
            catch (Exception exc)
            {
                // WriteTrace is useful in debugging creatures
                WriteTrace(exc.ToString());
            }
        }

        // Fired after all other events are fired during a turn
        private void IdleEvent(object sender, IdleEventArgs e)
        {
            try
            {
                // Our Creature will reproduce as often as possible so
                // every turn it checks CanReproduce to know when it
                // is capable.  If so we call BeginReproduction with
                // a null Dna parameter to being reproduction.
                if (CanReproduce)
                    BeginReproduction(null);

                // Check to see if we are capable of eating
                // If we are then try to eat or find food,
                // else we'll just stop moving.
                if (CanEat && !IsEating)
                {
                    // If we have a Target Plant we can try
                    // to either eat it, or move towards it.
                    // else we'll move to a random vector
                    // in search of a plant.
                    if (targetPlant != null)
                    {
                        // If we are within range start eating
                        // and stop moving.  Else we'll try
                        // to move towards the plant.
                        if (WithinEatingRange(targetPlant))
                        {
                            BeginEating(targetPlant);
                            if (IsMoving)
                                StopMoving();
                        }
                        else
                        {
                            if (!IsMoving)
                                BeginMoving(new MovementVector(targetPlant.Position, 2));
                        }
                    }
                    else
                    {
                        // We'll try try to find a target plant
                        // If we don't find one we'll move to 
                        // a random vector
                        if (!ScanForTargetPlant())
                        {
                            if (!IsMoving)
                            {
                                int RandomX = OrganismRandom.Next(0, WorldWidth - 1);
                                int RandomY = OrganismRandom.Next(0, WorldHeight - 1);
                                BeginMoving(new MovementVector(new Point(RandomX, RandomY), 2));
                            }
                        }
                    }
                }
                else
                {
                    // Since we are Full or we are already eating
                    // We should stop moving.
                    if (IsMoving)
                        StopMoving();
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
        }

        // Looks for target plants, and starts moving towards the first one it finds
        private bool ScanForTargetPlant()
        {
            try
            {
                // Find all Plants/Animals in range
                ArrayList foundAnimals = Scan();

                // If we found some Plants/Animals lets try
                // to weed out the plants.
                if (foundAnimals.Count > 0)
                {
                    foreach (OrganismState organismState in foundAnimals)
                    {
                        // If we found a plant, set it as our target
                        // then begin moving towards it.  Tell the
                        // caller we have a target.
                        if (!(organismState is PlantState)) continue;
                        targetPlant = (PlantState) organismState;
                        BeginMoving(new MovementVector(organismState.Position, 2));
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }

            // Tell the caller we couldn't find a target
            return false;
        }

        /// <summary>
        /// This gets called whenever your animal is being saved -- either the game is closing
        /// or you are being teleported.  Store anything you want to remember when you are
        /// instantiated again in the stream.
        /// </summary>
        /// <param name="m"></param>
        public override void SerializeAnimal(MemoryStream m)
        {
        }

        /// <summary>
        /// This gets called when you are instantiated again after being saved.  You get a 
        /// chance to pull out any information that you put into the stream when you were saved
        /// </summary>
        /// <param name="m"></param>
        public override void DeserializeAnimal(MemoryStream m)
        {
        }
    }
}