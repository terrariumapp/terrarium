//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using OrganismBase;

namespace Terrarium.Game
{
    /// <summary>
    ///  Creates a teleporter object that can move around the Terrarium
    ///  and teleport contained creatures.  This is the only non organism
    ///  object that can exist in the Terrarium currently.
    /// </summary>
    [Serializable]
    public class Teleporter
    {
        /// <summary>
        ///  Contains an array of teleportation zones that move around
        ///  the Terrarium.
        /// </summary>
        private readonly TeleportZone[] _teleportZones;

        /// <summary>
        ///  A random number generator that can be used to generate
        ///  the new locations for zones.
        /// </summary>
        [NonSerialized] private Random _random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        ///  Creates a Teleporter with the specified number of zones.
        /// </summary>
        /// <param name="zoneCount">The number of moving teleport zones.</param>
        public Teleporter(int zoneCount)
        {
            if (zoneCount == 0)
            {
                zoneCount = 1;
            }

            _teleportZones = new TeleportZone[zoneCount];

            for (var i = 0; i < zoneCount; i++)
            {
                _teleportZones[i] = new TeleportZone(new Rectangle(225, 225, 48, 48), null, i);
            }
        }

        /// <summary>
        ///  Clones a teleporter to create a new copy.
        /// </summary>
        /// <returns>A new copy of the Teleporter</returns>
        public Teleporter Clone()
        {
            var teleporter = new Teleporter(_teleportZones.Length);

            for (var i = 0; i < _teleportZones.Length; i++)
            {
                teleporter._teleportZones[i] = _teleportZones[i];
            }

            return teleporter;
        }

        /// <summary>
        ///  Moves all teleporation zones around the board.
        /// </summary>
        public void Move()
        {
            for (var i = 0; i < _teleportZones.Length; i++)
            {
                var teleportZone = _teleportZones[i];
                if (teleportZone == null) continue;
                if (teleportZone.Vector == null ||
                    teleportZone.Rectangle.Contains(teleportZone.Vector.Destination))
                {
                    // find a new place to move to
                    if (GameEngine.Current != null)
                    {
                        _teleportZones[i] =
                            teleportZone.SetVector(

                                new MovementVector(new Point(_random.Next(0, GameEngine.Current.WorldWidth),
                                                             _random.Next(0, GameEngine.Current.WorldHeight)), 5));
                    }
                }
                else
                {
                    var currentRectangle = teleportZone.Rectangle;
                    var vector = Vector.Subtract(currentRectangle.Location, teleportZone.Vector.Destination);
                    if (vector.Magnitude <= teleportZone.Vector.Speed)
                    {
                        // We've arrived
                        _teleportZones[i] = teleportZone.SetVector(null);
                    }
                    else
                    {
                        var unitVector = vector.GetUnitVector();
                        var speedVector = unitVector.Scale(teleportZone.Vector.Speed);
                        currentRectangle.Location = Vector.Add(currentRectangle.Location, speedVector);
                        _teleportZones[i] = teleportZone.SetRectangle(currentRectangle);
                    }
                }
            }
        }

        /// <summary>
        ///  Determines if a given creature exists within any of the teleport
        ///  zones and if so notifies the caller.
        /// </summary>
        /// <param name="state">The organism state to be checked</param>
        /// <returns>True if the organism is in the teleporter, false otherwise.</returns>
        public Boolean IsInTeleporter(OrganismState state)
        {
            foreach (var teleportZone in _teleportZones)
            {
                if (teleportZone.Contains(state))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///  Provides access to the teleportation zones by ID.
        /// </summary>
        /// <param name="ID">The ID for the teleporter to retrieve.</param>
        /// <returns>The teleportzone</returns>
        public TeleportZone GetTeleportZone(int ID)
        {
            return _teleportZones[ID];
        }

        /// <summary>
        ///  Provides access to the collection of TeleportZone objects.
        /// </summary>
        /// <returns>The collection of TeleportZone objects.</returns>
        public TeleportZone[] GetTeleportZones()
        {
            return (TeleportZone[]) _teleportZones.Clone();
        }
    }
}