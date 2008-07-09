//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase 
{
    // This is a class so that we can get the whole set of actions in one chunk because they were remoted 
    // at one point.
    /// <internal/>
    [Serializable]
    public class PendingActions
    {
        Boolean isImmutable = false;
    
        // These pending actions can only be set, never read by the code in 
        // the species.  From the developer's perspective, they just called a method
        // the TickActions class gathers these up and sets them to null
        // This is to ensure that we don't need locks.
        MoveToAction pendingMoveToAction;
        AttackAction pendingAttackAction;
        EatAction pendingEatAction;
        ReproduceAction pendingReproduceAction;
        DefendAction pendingDefendAction;

        /// <internal/>
        public void MakeImmutable()
        {
            isImmutable = true;
        }
    
        /// <internal/>
        public Boolean IsImmutable
        { 
            get
            {
                return isImmutable;
            } 
        }    


        /// <internal/>
        public DefendAction DefendAction
        {
            get
            {
                return pendingDefendAction;
            }
        }

        /// <internal/>
        public void SetDefendAction(DefendAction defendAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            pendingDefendAction = defendAction; 
        }

        /// <internal/>
        public MoveToAction MoveToAction
        {
            get
            {
                return pendingMoveToAction;
            }
        }

        /// <internal/>
        public void SetMoveToAction(MoveToAction moveToAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            pendingMoveToAction = moveToAction; 
        }

        /// <internal/>
        public AttackAction AttackAction
        {
            get
            {
                return pendingAttackAction;
            }
        }
    
        /// <internal/>
        public void SetAttackAction(AttackAction attackAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            pendingAttackAction = attackAction; 
        }    

        /// <internal/>
        public EatAction EatAction
        {
            get
            {
                return pendingEatAction;
            }
        }
    
        /// <internal/>
        public void SetEatAction(EatAction eatAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            pendingEatAction = eatAction; 
        }    

        /// <internal/>
        public ReproduceAction ReproduceAction
        {
            get
            {
                return pendingReproduceAction;
            }
        }
    
        /// <internal/>
        public void SetReproduceAction(ReproduceAction reproduceAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            pendingReproduceAction = reproduceAction; 
        }    
    }
}