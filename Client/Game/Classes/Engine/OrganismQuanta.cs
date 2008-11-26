//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Drawing;
using Terrarium.Tools;

namespace Terrarium.Game
{
    /// <summary>
    /// This class performance tests the machine against some canonical animal code.
    /// Basically, we try to standardize how much code can get run in a time slice since
    /// we want the same animal to be able to be run on different machines, but we want
    /// to allow faster machines to run more animals.  Thus we attempt to modify the 
    /// size of the time slice based on the speed of the machine.  Quanta is the size
    /// in microseconds of the time slice we're allowing.
    /// </summary>
    internal class OrganismQuanta
    {
        private static ArrayList allQuanta = new ArrayList();
        public static Int64 bestQuanta;
        public static Int64 lastQuanta;
        public static Int64 samples;
        public static Int64 totalQuanta;
        public static Int64 worstQuanta;

        public static void Clear()
        {
            worstQuanta = 0;
            bestQuanta = 0;
            totalQuanta = 0;
            samples = 0;
            lastQuanta = 0;
            allQuanta = new ArrayList();
        }

        public static void TestAnimal(int iterations)
        {
            var tm = new TimeMonitor();

            // Run iterations
            for (var i = 0; i < iterations; i++)
            {
                var cm = new OrganismEmulator();

                tm.Start();
                cm.EmulateOrganism();
                lastQuanta = tm.EndGetMicroseconds();

                allQuanta.Add(lastQuanta);
            }

            // Sort by speed
            allQuanta.Sort();

            // Pick the middle 80% of all iterations
            for (var i = (int) (iterations*0.15); i < (int) (iterations*0.85); i++)
            {
                samples++;
                lastQuanta = (long) allQuanta[i];

                if (worstQuanta == 0 && bestQuanta == 0)
                {
                    worstQuanta = lastQuanta;
                    bestQuanta = lastQuanta;
                }
                else
                {
                    if (lastQuanta > worstQuanta)
                    {
                        worstQuanta = lastQuanta;
                    }

                    if (lastQuanta < bestQuanta)
                    {
                        bestQuanta = lastQuanta;
                    }
                }

                totalQuanta += lastQuanta;
            }
        }

        // This class acts like a fake animal.  It represents how much processing
        // we think an animal should get, and thus we do fake operations we think
        // an animal would actually do to estimate it.

        #region Nested type: OrganismEmulator

        internal class OrganismEmulator
        {
            private readonly Random rand = new Random();
            private ArtificialIntelligence ai;
            private QuantaOrganismState me;

            public void EmulateOrganism()
            {
                // We just need to perform operations that the animal will perform...

                // Initialize brain
                InitAI();

                // Generally speaking, an animal will probably do 50 or so distance
                // checks per round
                DistanceChecks();

                // They will store some information about their food, enemies,
                // friends.
                StoreInformation();
            }

            private void InitAI()
            {
                me = new QuantaOrganismState("me", rand.Next(1, 40), rand.Next(1, 40));
                ai = new ArtificialIntelligence(me);
            }

            private void DistanceChecks()
            {
                for (var i = 0; i < 50; i++)
                {
                    var them = new Point(rand.Next(1, 40), rand.Next(1, 40));

                    var distance = (int) Math.Sqrt((me.Position.X + them.X)*2 + (me.Position.Y + them.Y)*2);

                    if (distance < 10)
                    {
                    }
                }
            }

            private void StoreInformation()
            {
                // Lets add 50 things
                for (var i = 0; i < 50; i++)
                {
                    var them = new Point(rand.Next(1, 40), rand.Next(1, 40));
                    var option = rand.Next(1, 4); // 1 through 3
                    switch (option)
                    {
                        case 1:
                            ai.AddFood(new QuantaOrganismState("food", them));
                            break;
                        case 2:
                            ai.AddEnemy(new QuantaOrganismState("enemy", them));
                            break;
                        case 3:
                            ai.AddFriend(new QuantaOrganismState("me", them));
                            break;
                    }
                }

                // Defend
                var ClosestEnemy = 0;
                foreach (QuantaOrganismState enemy in ai.enemies)
                {
                    var distance =
                        (int) Math.Sqrt((me.Position.X + enemy.Position.X)*2 + (me.Position.Y + enemy.Position.Y)*2);
                    if (ClosestEnemy == 0)
                    {
                        ClosestEnemy = distance;
                    }
                    else
                    {
                        if (ClosestEnemy > distance)
                        {
                            ClosestEnemy = distance;
                        }
                    }
                }

                // Eat
                var ClosestFood = 0;
                foreach (QuantaOrganismState food in ai.food)
                {
                    var distance =
                        (int) Math.Sqrt((me.Position.X + food.Position.X)*2 + (me.Position.Y + food.Position.Y)*2);
                    if (ClosestFood == 0)
                    {
                        ClosestFood = distance;
                    }
                    else
                    {
                        if (ClosestFood > distance)
                        {
                            ClosestFood = distance;
                        }
                    }
                }
            }

            #region Nested type: ArtificialIntelligence

            internal class ArtificialIntelligence
            {
                public ArrayList enemies;
                public ArrayList food;
                public ArrayList friends;
                public QuantaOrganismState me;

                public ArtificialIntelligence(QuantaOrganismState me)
                {
                    enemies = new ArrayList();
                    food = new ArrayList();
                    friends = new ArrayList();
                    this.me = me;
                }

                public void AddEnemy(QuantaOrganismState enemy)
                {
                    enemies.Add(enemy);
                }

                public void AddFood(QuantaOrganismState newFood)
                {
                    food.Add(newFood);
                }

                public void AddFriend(QuantaOrganismState friend)
                {
                    friends.Add(friend);
                }
            }

            #endregion

            #region Nested type: QuantaOrganismState

            internal class QuantaOrganismState
            {
                public Point Position;
                public String Species;

                public QuantaOrganismState(string species, int x, int y)
                {
                    Position = new Point(x, y);
                    Species = species;
                }

                public QuantaOrganismState(string species, Point p)
                {
                    Position = p;
                    Species = species;
                }
            }

            #endregion
        }

        #endregion
    }
}