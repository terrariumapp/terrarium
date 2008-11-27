//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   Provides access to a creature's Antenna.  Each creature has two
    ///   Antenna that can be placed in 10 different positions each.  This
    ///   enables a multitude of states that can be used to communicate
    ///   with other creatures of the same species or even creatures of
    ///   different species.
    ///  </para>
    ///  <para>
    ///   By default both Antenna are positioned to the AntennaPosition.Left.
    ///   In addition to setting the state of each individual Antenna.  The
    ///   author can also use the AntennaValue property to set a number from
    ///   0 to 99.  This is provided for ease of use since most author developers
    ///   will use this structure for passing data (cell data maybe?) rather than
    ///   simple flags or states.
    ///  </para>
    /// </summary>
    [Serializable]
    public class AntennaState
    {
        private bool immutable;
        private AntennaPosition leftAntenna;
        private AntennaPosition rightAntenna;

        /// <summary>
        ///  <para>
        ///   Constructs a new AntennaState given the initial values for
        ///   the LeftAntenna and RightAntenna.  If the values are not
        ///   within the ranged allowed by the AntennaState they will
        ///   be defaulted to AntennaPosition.Left;
        ///  </para>
        /// </summary>
        /// <param name="left">
        ///  AntennaPosition that will be assigned to LeftAntenna.
        /// </param>
        /// <param name="right">
        ///  AntennaPosition that will be assigned to RightAntenna.
        /// </param>
        public AntennaState(AntennaPosition left, AntennaPosition right)
        {
            leftAntenna = verifyAntenna(left) ? left : AntennaPosition.Left;
            rightAntenna = verifyAntenna(right) ? right : AntennaPosition.Left;
        }

        /// <summary>
        ///  <para>
        ///   Constructs a new AntennaState given the initial values from
        ///   a pre-existing AntennaState.  This effectively makes a copy
        ///   without making the new state immutable.  This can be used to
        ///   copy states from another creature and then modified slightly.
        ///  </para>
        /// </summary>
        /// <param name="state">
        ///  AntennaState used to initialize the LeftAntenna and RightAntenna properties.
        /// </param>
        public AntennaState(AntennaState state)
        {
            leftAntenna = AntennaPosition.Left;
            rightAntenna = AntennaPosition.Left;

            if (state != null)
            {
                if (verifyAntenna(state.LeftAntenna))
                {
                    leftAntenna = state.LeftAntenna;
                }

                if (verifyAntenna(state.RightAntenna))
                {
                    rightAntenna = state.RightAntenna;
                }
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to get the position of the LeftAntenna.  You can also
        ///   set the position of the LeftAntenna if the AntennaState is
        ///   not set to immutable.  By default the AntennaState located on
        ///   the Animal class is never marked immutable, and the AntennaState
        ///   located on the AnimalState class is always immutable.
        ///  </para>
        ///  <para>
        ///   You may also use the AntennaValue property if you are just trying
        ///   to pass simple numbers or are organizing a state machine that has
        ///   more than 10 specific states.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AntennaPosition representing the location of the LeftAntenna
        /// </returns>
        public AntennaPosition LeftAntenna
        {
            get { return leftAntenna; }

            set
            {
                if (!immutable)
                {
                    if (verifyAntenna(value))
                    {
                        leftAntenna = value;
                    }
                }
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to get the position of the RightAntenna.  You can also
        ///   set the position of the RightAntenna if the AntennaState is
        ///   not set to immutable.  By default the AntennaState located on
        ///   the Animal class is never marked immutable, and the AntennaState
        ///   located on the AnimalState class is always immutable.
        ///  </para>
        ///  <para>
        ///   You may also use the AntennaValue property if you are just trying
        ///   to pass simple numbers or are organizing a state machine that has
        ///   more than 10 specific states.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AntennaPosition representing the location of the LeftAntenna
        /// </returns>
        public AntennaPosition RightAntenna
        {
            get { return rightAntenna; }

            set
            {
                if (!immutable)
                {
                    if (verifyAntenna(value))
                    {
                        rightAntenna = value;
                    }
                }
            }
        }

        /// <summary>
        ///  <para>
        ///   Used to get a numeric value between 0 and 99 that represents the
        ///   AntennaPosition of both the LeftAntenna and RightAntenna.  You can
        ///   also set the position of both Antenna by specifying a new value
        ///   between 0 and 99 if the AntennaState is not set to immutable.  By
        ///   default the AntennaState located on the Animal class is never
        ///   marked immutable, and the AntennaState located on the AnimalState
        ///   class is always immutable.
        ///  </para>
        ///  <para>
        ///   You may also set each of the Antenna separately rather than using
        ///   this special value that represents both Antenna numerically.
        ///  </para>
        /// </summary>
        /// <returns>
        ///  AntennaPosition representing the location of the LeftAntenna
        /// </returns>
        public int AntennaValue
        {
            get
            {
                var leftVal = (int) leftAntenna;
                var rightVal = (int) rightAntenna;

                return leftVal*10 + rightVal;
            }

            set
            {
                if (!immutable)
                {
                    if (value >= 0 && value < 100)
                    {
                        leftAntenna = (AntennaPosition) (value/10);
                        rightAntenna = (AntennaPosition) (value%10);
                    }
                }
            }
        }

        /// <internal />
        public void MakeImmutable()
        {
            immutable = true;
        }

        private static bool verifyAntenna(AntennaPosition pos)
        {
            var antennaValue = (int) pos;
            return antennaValue >= 0 && antennaValue < 10;
        }
    }
}