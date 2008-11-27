//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    ///<summary>
    /// This is a class so that we can get the whole set of actions in 
    /// one chunk because they were remoted at one point.
    /// <para>
    /// These pending actions can only be set, never read by the code in 
    /// the species.  From the developer's perspective, they just called a method
    /// the TickActions class gathers these up and sets them to null
    /// This is to ensure that we don't need locks.
    /// </para>
    ///</summary>
    [Serializable]
    public class PendingActions
    {

        ///<summary>
        ///</summary>
        public bool IsImmutable { get; private set; }

        ///<summary>
        ///</summary>
        public DefendAction DefendAction { get; private set; }

        ///<summary>
        ///</summary>
        public MoveToAction MoveToAction { get; private set; }

        ///<summary>
        ///</summary>
        public AttackAction AttackAction { get; private set; }

        ///<summary>
        ///</summary>
        public EatAction EatAction { get; private set; }

        ///<summary>
        ///</summary>
        public ReproduceAction ReproduceAction { get; private set; }

        ///<summary>
        ///</summary>
        public void MakeImmutable()
        {
            IsImmutable = true;
        }

        ///<summary>
        ///</summary>
        ///<param name="defendAction"></param>
        ///<exception cref="ApplicationException"></exception>
        public void SetDefendAction(DefendAction defendAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            DefendAction = defendAction;
        }

        ///<summary>
        ///</summary>
        ///<param name="moveToAction"></param>
        ///<exception cref="ApplicationException"></exception>
        public void SetMoveToAction(MoveToAction moveToAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            MoveToAction = moveToAction;
        }

        ///<summary>
        ///</summary>
        ///<param name="attackAction"></param>
        ///<exception cref="ApplicationException"></exception>
        public void SetAttackAction(AttackAction attackAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            AttackAction = attackAction;
        }

        ///<summary>
        ///</summary>
        ///<param name="eatAction"></param>
        ///<exception cref="ApplicationException"></exception>
        public void SetEatAction(EatAction eatAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            EatAction = eatAction;
        }

        ///<summary>
        ///</summary>
        ///<param name="reproduceAction"></param>
        ///<exception cref="ApplicationException"></exception>
        public void SetReproduceAction(ReproduceAction reproduceAction)
        {
            if (IsImmutable)
            {
                throw new ApplicationException("PendingActions must be mutable to modify actions.");
            }

            ReproduceAction = reproduceAction;
        }
    }
}