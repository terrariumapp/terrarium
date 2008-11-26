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
    ///  Rolls up and provides access to all actions requested by animals in a given tick.
    /// </summary>
    [Serializable]
    public class TickActions
    {
        /// <summary>
        ///  All of the attack actions that organisms have performed in this tick.
        /// </summary>
        private readonly Hashtable _attackActions = new Hashtable();

        /// <summary>
        ///  All of the defend actions that organisms have performed in this tick.
        /// </summary>
        private readonly Hashtable _defendActions = new Hashtable();

        /// <summary>
        ///  All of the eat actions that organisms have performed in this tick.
        /// </summary>
        private readonly Hashtable _eatActions = new Hashtable();

        /// <summary>
        ///  All of the movement actions that organisms have performed in this tick.
        /// </summary>
        private readonly Hashtable _moveToActions = new Hashtable();

        /// <summary>
        ///  All of the reproduce actions that organisms have performed in this tick.
        /// </summary>
        private readonly Hashtable _reproduceActions = new Hashtable();

        /// <summary>
        ///  Provides access to the Movement actions.
        /// </summary>
        public Hashtable MoveToActions
        {
            get { return (Hashtable) _moveToActions.Clone(); }
        }

        /// <summary>
        ///  Provides access to the Attack actions.
        /// </summary>
        public Hashtable AttackActions
        {
            get { return (Hashtable) _attackActions.Clone(); }
        }

        /// <summary>
        ///  Provides access to the Eat actions.
        /// </summary>
        public Hashtable EatActions
        {
            get { return (Hashtable) _eatActions.Clone(); }
        }

        /// <summary>
        ///  Provides access to the Reproduction actions.
        /// </summary>
        public Hashtable ReproduceActions
        {
            get { return (Hashtable) _reproduceActions.Clone(); }
        }

        /// <summary>
        ///  Provides access to the Defend actions.
        /// </summary>
        public Hashtable DefendActions
        {
            get { return (Hashtable) _defendActions.Clone(); }
        }

        /// <summary>
        ///  Rips through all organisms and wraps their pending actions
        ///  up into the hashtables on a per action type basis.
        /// </summary>
        /// <param name="scheduler">The game scheduler that has all of the organisms.</param>
        internal void GatherActionsFromOrganisms(IGameScheduler scheduler)
        {
            foreach (Organism organism in scheduler.Organisms)
            {
                var pendingActions = organism.GetThenErasePendingActions();
                if (pendingActions.MoveToAction != null)
                {
                    _moveToActions[organism.ID] = pendingActions.MoveToAction;
                }

                if (pendingActions.AttackAction != null)
                {
                    _attackActions[organism.ID] = pendingActions.AttackAction;
                }

                if (pendingActions.EatAction != null)
                {
                    _eatActions[organism.ID] = pendingActions.EatAction;
                }

                if (pendingActions.ReproduceAction != null)
                {
                    _reproduceActions[organism.ID] = pendingActions.ReproduceAction;
                }

                if (pendingActions.DefendAction != null)
                {
                    _defendActions[organism.ID] = pendingActions.DefendAction;
                }
            }
        }
    }
}