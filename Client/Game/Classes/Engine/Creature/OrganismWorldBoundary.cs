//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using OrganismBase;
using Terrarium.Hosting;

namespace Terrarium.Game
{
    /// <summary>
    ///  Represents an organisms world view.  Implements the IOrganismWorldBoundary
    ///  interface which is available to creatures that link to the OrganismBase
    ///  library.  See description of AnimalWorldBoundary to see how this hides information
    ///  from creatures.
    /// </summary>
    public class OrganismWorldBoundary : IOrganismWorldBoundary
    {
        /// <summary>
        ///  Initializes a new world boundary given the original organism and their ID.
        /// </summary>
        /// <param name="organism">The organism used to init this world boundary.</param>
        /// <param name="ID">The Unique ID of the organism.</param>
        internal OrganismWorldBoundary(Organism organism, string ID)
        {
            Organism = organism;
            this.ID = ID;
        }

        /// <summary>
        ///  The original organism this world boundary was created for.
        /// </summary>
        protected Organism Organism { get; set; }

        #region IOrganismWorldBoundary Members

        /// <summary>
        ///  Returns an AnimalState that represents your current state in the world.
        /// </summary>
        public OrganismState CurrentOrganismState
        {
            get { return AppMgr.CurrentScheduler.CurrentState.GetOrganismState(ID); }
        }

        /// <summary>
        ///  Returns the organism's ID.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        ///  Returns the width of the world in game units (pixels).
        /// </summary>
        public int WorldWidth
        {
            get { return GameEngine.Current.WorldWidth; }
        }

        /// <summary>
        ///  Returns the width of the world in game units (pixels).
        /// </summary>
        public int WorldHeight
        {
            get { return GameEngine.Current.WorldHeight; }
        }

        #endregion

        /// <summary>
        ///  Sets the Unique ID of the creature in the system.
        /// </summary>
        protected void SetOrganismID(string id)
        {
            ID = id;
        }

        /// <summary>
        ///  Sets the world boundary for a specific creature.
        /// </summary>
        /// <param name="organism">The organism the boundary is for.</param>
        /// <param name="id">The Unique ID of the organism.</param>
        internal static void SetWorldBoundary(Organism organism, string id)
        {
            if (organism is Animal)
            {
                organism.SetWorldBoundary(new AnimalWorldBoundary(organism, id));
            }
            else
            {
                organism.SetWorldBoundary(new PlantWorldBoundary(organism, id));
            }
        }
    }
}