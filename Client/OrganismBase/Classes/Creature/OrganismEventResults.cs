//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    // This class represents all of the events that will get sent to an animal for a given tick.
    // The Terrarium engine fills it in and then hands it over when it is finished calculating a turn.
    /// <internal/>
    [Serializable]
    public class OrganismEventResults
    {
        private readonly AttackedEventArgsCollection attackedCollection = new AttackedEventArgsCollection();
        private AttackCompletedEventArgs attackCompleted;
        private BornEventArgs born;
        private DefendCompletedEventArgs defendCompleted;
        private EatCompletedEventArgs eatCompleted;

        private MoveCompletedEventArgs moveCompleted;
        private ReproduceCompletedEventArgs reproduceCompleted;
        private TeleportedEventArgs teleported;

        /// <internal/>
        public Boolean IsImmutable { get; private set; }

        /// <internal/>
        public BornEventArgs Born
        {
            get { return born; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                born = value;
            }
        }

        /// <internal/>
        public ReproduceCompletedEventArgs ReproduceCompleted
        {
            get { return reproduceCompleted; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                reproduceCompleted = value;
            }
        }

        /// <internal/>
        public TeleportedEventArgs Teleported
        {
            get { return teleported; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                teleported = value;
            }
        }

        /// <internal/>
        public AttackedEventArgsCollection AttackedEvents
        {
            get { return attackedCollection; }
        }

        /// <internal/>
        public MoveCompletedEventArgs MoveCompleted
        {
            get { return moveCompleted; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                moveCompleted = value;
            }
        }

        /// <internal/>
        public AttackCompletedEventArgs AttackCompleted
        {
            get { return attackCompleted; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                attackCompleted = value;
            }
        }

        /// <internal/>
        public EatCompletedEventArgs EatCompleted
        {
            get { return eatCompleted; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                eatCompleted = value;
            }
        }

        /// <internal/>
        public DefendCompletedEventArgs DefendCompleted
        {
            get { return defendCompleted; }

            set
            {
                if (IsImmutable)
                {
                    throw new ApplicationException("Object is immutable.");
                }

                defendCompleted = value;
            }
        }

        /// <internal/>
        public void MakeImmutable()
        {
            IsImmutable = true;
            attackedCollection.MakeImmutable();
        }
    }
}