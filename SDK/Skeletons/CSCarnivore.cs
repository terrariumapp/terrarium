using System;
using System.Drawing;
using System.Collections;
using System.IO;
using OrganismBase;

// Sample Carnivore
// Strategy: This animal stands still until it sees something tasty and then
// bites it.  If any dead animals are within range it eats them.

// The following Assembly attributes must be applied to each Organism Assembly
// The class that derives from Animal
[assembly: OrganismClass("CSharpCreatures.SimpleCarnivore")]
// Provide Author Information
// Author Name, Email
[assembly: AuthorInformation("Your Name", "someone@microsoft.com")]

namespace CSharpCreatures 
{
    // It's an herbivore
    [CarnivoreAttribute(true)]
    // This value must be between 24 and 48, 24 means faster reproduction
    // while 48 would give more defense and attack power
    // Make it smaller for reproduction, but not too small
    [MatureSize(30)]

    // AnimalSkin = AnimalSkinFamilyEnum.Scorpion, you can be a Beetle
    // an Ant, a Scorpion, an Inchworm, or a Spider
    // MarkingColor = KnownColor.Red, you can choose to mark your
    // creature with a color.  This does not affect appearance in
    // the game.
    [AnimalSkin(AnimalSkinFamily.Beetle)]
    [MarkingColor(KnownColor.Red)]

    // Point Based Attributes
    // You get 100 points to distribute among these attributes to define
    // what your organism can do.  Choose them based on the strategy your organism
    // will use.  This organism stays still and waits for creatures to come.
    [MaximumEnergyPoints(0)]        // No need to store energy.
    [EatingSpeedPoints(0)]          // No need to eat fast.
    [AttackDamagePoints(52)]        // Needs to deal some good damage
    [DefendDamagePoints(0)]         // Attacks, so no need for defense
    [MaximumSpeedPoints(28)]        // Moves somewhat fast to catch prey
    [CamouflagePoints(0)]           // Doesn't mind if noticed
    [EyesightPoints(20)]            // Needs to see prey and food

    public class SimpleCarnivore : Animal 
    {
        AnimalState targetAnimal;

        protected override void Initialize() 
        {
            Load += new LoadEventHandler(LoadEvent);
            Idle += new IdleEventHandler(IdleEvent);
        }

        // First event fired on an organism each turn
        void LoadEvent(object sender, LoadEventArgs e)
        {
            try 
            {
                if(targetAnimal != null) 
                {
                    // See if our target animal still exists (it may have died)
                    // LookFor returns null if it isn't found
                    targetAnimal = (AnimalState) LookFor(targetAnimal);
                }
            }
            catch(Exception exc) 
            {
                // WriteTrace is useful in debugging creatures
                WriteTrace(exc.ToString());
            }
        }

        // Fired after all other events are fired during a turn
        void IdleEvent(object sender, IdleEventArgs e) 
        {
            try 
            {
                // Our Creature will reproduce as often as possible so
                // every turn it checks CanReproduce to know when it
                // is capable.  If so we call BeginReproduction with
                // a null Dna parameter to being reproduction.
                if(CanReproduce)
                    BeginReproduction(null);

                // If we are already doing something, then we don't
                // need to do something else.  Lets leave the
                // function.
                if ( IsAttacking || IsMoving || IsEating ) 
                {
                    return;
                }

                // Try to find a new target if we need one.
                if(targetAnimal == null) 
                {
                    FindNewTarget();
                } 

                // If we have a target animal then lets check him out
                if(targetAnimal != null ) 
                {
                    // If the target is alive, then we need to kill it
                    // else we can immediately eat it.
                    if ( targetAnimal.IsAlive ) 
                    {
                        // If we are within attacking range, then
                        // lets eat the creature.  Else we'll
                        // try to move into range
                        if ( WithinAttackingRange(targetAnimal) ) 
                        {
                            BeginAttacking(targetAnimal);
                        } 
                        else 
                        {
                            MoveToTarget();
                        }
                    } 
                    else 
                    {
                        // If the creature is dead then we can try to eat it.
                        // If we are within eating range then we'll try to
                        // eat the creature, else we'll move towards it.
                        if ( WithinEatingRange(targetAnimal) ) 
                        {
                            if (CanEat) 
                            {
                                BeginEating(targetAnimal);
                            } 
                        } 
                        else 
                        {
                            MoveToTarget();
                        }
                    }
                } 
                else 
                {
                    // If we stop moving we conserve energy.  Sometimes
                    // this works better than running around.
                    StopMoving();
                }
            } 
            catch(Exception exc) 
            {
                WriteTrace(exc.ToString());
            }
        }

        void FindNewTarget() 
        {
            try 
            {
                ArrayList foundAnimals = Scan();

                // We should see at least a couple of animals.
                if(foundAnimals.Count > 0) 
                {
                    foreach(OrganismState organismState in foundAnimals) 
                    {
                        // We are looking for any organism that is an animal
                        // and that isn't of our species so we can eat them.
                        if(organismState is AnimalState && !IsMySpecies(organismState)) 
                        {
                            targetAnimal = (AnimalState) organismState;
                        }
                    }
                }
            }
            catch(Exception exc) 
            {
                WriteTrace(exc.ToString());
            }
        }

        // Function used to move towards our prey or meal
        void MoveToTarget() 
        {
            try 
            {
                // Make sure we aren't moving towards a null target
                if ( targetAnimal == null ) 
                {
                    return;
                }

                // Follow our target as quickly as we can
                BeginMoving(new MovementVector(targetAnimal.Position, Species.MaximumSpeed));
            }
            catch(Exception exc) 
            {
                WriteTrace(exc.ToString());
            }
        }

        // This gets called whenever your animal is being saved -- either the game is closing
        // or you are being teleported.  Store anything you want to remember when you are
        // instantiated again in the stream.
        public override void SerializeAnimal(MemoryStream m) 
        {
        }

        // This gets called when you are instantiated again after being saved.  You get a 
        // chance to pull out any information that you put into the stream when you were saved
        public override void DeserializeAnimal(MemoryStream m) 
        {
        }
    }
}
