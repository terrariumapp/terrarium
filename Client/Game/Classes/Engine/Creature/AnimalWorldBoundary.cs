//------------------------------------------------------------------------------   
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections;
using OrganismBase;
using Terrarium.Hosting;

namespace Terrarium.Game
{
    /// <summary>
    /// This class implements the IAnimalWorldBoundary which is used by a creature
    /// to get information about the surrounding area. It is the interface between 
    /// the animal and the world. This class is passed from the game engine to the 
    /// actual creature, but we don't want them to have access to all the members.
    /// So, we implement an interface that is defined in the OrganismBase assembly, 
    /// and implement the class in the TerrariumEngine assembly. Since the 
    /// TerrariumEngine assembly doesn't have the AllowPartiallyTrustedCallers
    /// attribute animals won't be able to call the class directly and will only 
    /// be able to call the members of the interface.
    /// </summary>
    public class AnimalWorldBoundary : OrganismWorldBoundary, IAnimalWorldBoundary
    {
        /// <summary>
        ///  Creates a new animal world boundary for a given animal
        /// </summary>
        /// <param name="animal">The actual class representing the creature.</param>
        /// <param name="ID">The unique ID of the creature.</param>
        internal AnimalWorldBoundary(Organism animal, string ID) : base(animal, ID)
        {
        }

        #region IAnimalWorldBoundary Members

        /// <summary>
        ///  While immutable animal properties are stored directly on the creature
        ///  class, any game engine properties are stored on the state objects.
        ///  Creatures can get access to their state object through the CurrentAnimalState
        ///  property.
        /// </summary>
        public AnimalState CurrentAnimalState
        {
            get { return (AnimalState) AppMgr.CurrentScheduler.CurrentState.GetOrganismState(ID); }
        }

        /// <summary>
        ///  <para>
        ///   Scans the world in a circle around your animal's current location and returns a list of what was seen.
        ///   The radius of the circle your animal looks in is determined by the number of points you applied to the
        ///   EyesightPoints attribute.
        ///  </para>
        ///  <para>
        ///   Animals can be hidden by camouflage so subsequent calls to this method may return different sets of objects
        ///   even if the world hasn't changed at all.
        ///  </para>
        ///  <para>
        ///   You can hold onto references to the OrganismState objects that are returned by this method indefinitely.
        ///   However, they will reflect the organisms state at the point where you saw the animal -- they are not refreshed
        ///   to reflect an organisms state over time.  Use the LookFor() method to get an up-to-date OrganismState
        ///   object.
        ///  </para>
        /// </summary>
        /// <returns>Returns an ArrayList of OrganismState objects. One for each plant or animal that was seen.</returns>
        public ArrayList Scan()
        {
            // grab the state now so that it doesn't change over the course of the function
            var worldState = AppMgr.CurrentScheduler.CurrentState;
            var thisState = worldState.GetOrganismState(ID);

            // Look around
            var foundList = worldState.FindOrganismsInView(CurrentAnimalState,
                                                           ((AnimalSpecies) thisState.Species).EyesightRadius);

            // Remove the organism that is scanning
            foundList.Remove(thisState);

            // Remove any camouflaged animals
            for (var index = 0; index < foundList.Count;)
            {
                var state = (OrganismState) foundList[index];

                // Dead animals aren't hidden
                if (state is AnimalState && state.IsAlive)
                {
                    var invisible = Organism.OrganismRandom.Next(1, 100);
                    if (invisible <= ((AnimalSpecies) state.Species).InvisibleOdds)
                    {
                        foundList.Remove(state);
                        Organism.WriteTrace("#Camouflage hid animal from organism");
                        continue;
                    }
                }
                index++;
            }

            return foundList;
        }

        /// <summary>
        ///  Refreshes an organism state to the latest available state.  Organism
        ///  state objects can be held by an organism for many ticks, and they don't
        ///  automatically update themselves.  The reference held is immutable and so
        ///  only represents the creature's state at the time the Scan was made and
        ///  not necessarily the latest state.
        /// </summary>
        /// <param name="organismState">The organism state that needs to be updated</param>
        /// <returns>An updated state if the creature is still visible and alive, else null</returns>
        public OrganismState LookFor(OrganismState organismState)
        {
            if (organismState == null)
            {
                throw new ArgumentNullException("organismState", "The argument organismState cannot be null");
            }

            var targetOrganism = LookForNoCamouflage(organismState);

            if (targetOrganism != null)
            {
                if (targetOrganism is AnimalState)
                {
                    // See if the camouflage hides it
                    var invisible = Organism.OrganismRandom.Next(1, 100);
                    if (invisible <= ((AnimalSpecies) targetOrganism.Species).InvisibleOdds)
                    {
                        Organism.WriteTrace("#Camouflage hid animal from organism");
                        return null;
                    }
                }

                return targetOrganism;
            }
            return null;
        }

        /// <summary>
        ///  Provides the same features as LookFor, except does not take camouflage
        ///  into account.
        /// </summary>
        /// <param name="organismState">The organism state that needs to be updated.</param>
        /// <returns>An updated state if the creature is still alive and within range, null otherwise.</returns>
        /// <internal/>
        public OrganismState LookForNoCamouflage(OrganismState organismState)
        {
            if (organismState != null)
            {
                var worldState = AppMgr.CurrentScheduler.CurrentState;

                organismState = worldState.GetOrganismState(organismState.ID);
                OrganismState thisOrganism = CurrentAnimalState;

                if (organismState == null ||
                    !thisOrganism.IsWithinRect(((AnimalSpecies) thisOrganism.Species).EyesightRadius, organismState))
                {
                    organismState = null;
                }
            }

            return organismState;
        }

        /// <summary>
        ///  Refresh's a state based on ID rather than a stored state object.
        ///  This can be used to optimize the amount of memory required for
        ///  storing creature information during serialization.
        /// </summary>
        /// <param name="organismID">The Unique ID of the organism.</param>
        /// <returns>The state object for the creature if visible to LookFor, null otherwise.</returns>
        public OrganismState RefreshState(string organismID)
        {
            if (organismID == null)
            {
                throw new ArgumentNullException("organismID", "The argument organismID cannot be null");
            }

            var org = AppMgr.CurrentScheduler.CurrentState.GetOrganismState(organismID);
            if (org != null)
            {
                org = LookFor(org);
            }

            return org;
        }

        #endregion
    }
}