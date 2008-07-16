using System;
using System.Collections;
using System.Drawing;
using System.IO;
using OrganismBase;

[assembly : OrganismClass("Exercise3.MyAnimal")]
[assembly : AuthorInformation("Your Name", "someone@microsoft.com")]

namespace Exercise3
{
    [Carnivore(false)]
    [MatureSize(28)]
    [AnimalSkin(AnimalSkinFamily.Inchworm)]
    [MarkingColor(KnownColor.Yellow)]
    [MaximumEnergyPoints(20)]
    [EatingSpeedPointsAttribute(0)]
    [AttackDamagePointsAttribute(12)]
    [DefendDamagePointsAttribute(12)]
    [MaximumSpeedPointsAttribute(16)]
    [CamouflagePointsAttribute(10)]
    [EyesightPointsAttribute(20)]
    public class MyAnimal : Animal
    {
        private PlantState targetPlant = null; // The current plant we're going after

        protected override void Initialize()
        {
            // TODO: Add initialization logic here
            Idle += MyAnimal_Idle;
            Load += MyAnimal_Load;
            Attacked += MyAnimal_Attacked;
            MoveCompleted += MyAnimal_MoveCompleted;
        }

        // First event fired on an organism each turn 
        private void MyAnimal_Load(object sender, LoadEventArgs e)
        {
            try
            {
                if (targetPlant != null)
                {
                    // See if our target plant still exists (it may have died) 
                    // LookFor returns null if it isn't found 
                    targetPlant = (PlantState) LookFor(targetPlant);
                    if (targetPlant == null)

                        // WriteTrace is the best way to debug your creatures. 
                        WriteTrace("Target plant disappeared.");
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
        }

        // Fired if we're being attacked 
        private void MyAnimal_Attacked(object sender, AttackedEventArgs e)
        {
            if (e.Attacker.IsAlive)
            {
                AnimalState TheAttacker = e.Attacker;

                BeginDefending(TheAttacker); //defend against the attacker 
                WriteTrace("Run away to some random point");

                int X = OrganismRandom.Next(0, WorldWidth - 1);
                int Y = OrganismRandom.Next(0, WorldHeight - 1);

                BeginMoving(new MovementVector(new Point(X, Y), 10));
            }
        }

        // Fired when we've finished moving.
        private void MyAnimal_MoveCompleted(object sender, MoveCompletedEventArgs e)
        {
            // Reset the antenna value
            Antennas.AntennaValue = 0;

            // If we've stopped because something is blocking us...
            if (e.Reason == ReasonForStop.Blocked)
            {
                WriteTrace("Something's blocking my way.");
                if (e.BlockingOrganism is AnimalState)
                {
                    AnimalState blockingAnimal = (AnimalState) e.BlockingOrganism;

                    if (blockingAnimal.AnimalSpecies.IsSameSpecies(Species))
                    {
                        // Signal to our friend to move out of our way.
                        WriteTrace("One of my friends is blocking my way.  I'll ask him to move.");
                        Antennas.AntennaValue = 13;
                    }
                }
            }
        }

        // Fired after all other events are fired during a turn 
        private void MyAnimal_Idle(object sender, IdleEventArgs e)
        {
            try
            {
                // Reproduce as often as possible 
                if (CanReproduce)
                    BeginReproduction(null);

                // If we can eat and we have a target plant, eat 
                if (CanEat)
                {
                    WriteTrace("Hungry.");
                    if (!IsEating)
                    {
                        WriteTrace("Not eating: Have target plant?");
                        if (targetPlant != null)
                        {
                            WriteTrace("Yes, Have target plant already.");
                            if (WithinEatingRange(targetPlant))
                            {
                                WriteTrace("Within Range, Start eating.");
                                BeginEating(targetPlant);
                                if (IsMoving)
                                {
                                    WriteTrace("Stop while eating.");
                                    StopMoving();
                                }
                            }
                            else
                            {
                                if (!IsMoving)
                                {
                                    WriteTrace("Move to Target Plant");
                                    BeginMoving(new MovementVector(targetPlant.Position, 2));
                                }
                            }
                        }
                        else
                        {
                            WriteTrace("Don't have target plant.");
                            if (!ScanForTargetPlant())
                                if (!IsMoving)
                                {
                                    WriteTrace("No plant found, so pick a random point and move there");

                                    int RandomX = OrganismRandom.Next(0, WorldWidth - 1);
                                    int RandomY = OrganismRandom.Next(0, WorldHeight - 1);

                                    BeginMoving(new MovementVector(new Point(RandomX, RandomY), 2));
                                }
                                else
                                {
                                    WriteTrace("Moving and Looking...");
                                }
                        }
                    }
                    else
                    {
                        WriteTrace("Eating.");
                        if (IsMoving)
                        {
                            WriteTrace("Stop moving while eating.");
                            StopMoving();
                        }
                    }
                }
                else
                {
                    WriteTrace("Full: do nothing.");
                    if (IsMoving)
                        StopMoving();
                }

                ShouldIMoveForMyFriend();
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
                ArrayList foundCreatures = Scan();

                if (foundCreatures.Count > 0)
                {
                    // Always move after closest plant or defend closest creature if there is one 
                    foreach (OrganismState organismState in foundCreatures)
                    {
                        if (organismState is PlantState)
                        {
                            targetPlant = (PlantState) organismState;
                            BeginMoving(new MovementVector(organismState.Position, 2));
                            return true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
            return false;
        }

        // Routine to move out of the way when blocking our friends.
        private void ShouldIMoveForMyFriend()
        {
            try
            {
                ArrayList foundAnimals = Scan();

                if (foundAnimals.Count > 0)
                {
                    foreach (OrganismState organismState in foundAnimals)
                    {
                        if (organismState is AnimalState)
                        {
                            AnimalState visibleAnimal = (AnimalState) organismState;

                            // Only move if the animal is one of our friends (IsSameSpecies).
                            if (visibleAnimal.Species.IsSameSpecies(Species))
                            {
                                // If the animal's antenna value is 13, it means they're blocked (see the MoveCompletedEvent method).
                                if (visibleAnimal.Antennas.AntennaValue == 13)
                                {
                                    // We're blocking our friend, so we should move.
                                    WriteTrace("I'm blocking one of my friends.  I should move.");

                                    int newX = Position.X - (visibleAnimal.Position.X - Position.X);
                                    int newY = Position.Y - (visibleAnimal.Position.Y - Position.Y);

                                    BeginMoving(new MovementVector(new Point(newX, newY), 2));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                WriteTrace(exc.ToString());
            }
        }

        public override void SerializeAnimal(MemoryStream stream)
        {
            // TODO: Add serialization logic here
        }

        public override void DeserializeAnimal(MemoryStream stream)
        {
            // TODO: Add deserialization logic here
        }
    }
}