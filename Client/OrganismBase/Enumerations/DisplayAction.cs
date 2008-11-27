//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace OrganismBase
{
    /// <summary>
    ///  Used to determine what the most prominent completed
    ///  action for the previous tick was.  This is used by
    ///  the rendering engine to decide which animation should
    ///  be used.  Note that the values of the enumeration were
    ///  initially set to enable quick render processing, but
    ///  the values are now much less important.
    /// </summary>
    /// <internal/>
    public enum DisplayAction
    {
        /// <summary>
        ///  The creature attacked.  This happens quite a bit and is
        ///  a very prominent display action.
        /// </summary>
        /// <internal/>
        Attacked = -1,
        /// <summary>
        ///  The creature defended.  This also hapens quite a bit
        ///  and is a very prominent display action.
        /// </summary>
        /// <internal/>
        Defended = 7,
        /// <summary>
        ///  The creature was dead in the previous tick and is still
        ///  dead.
        /// </summary>
        /// <internal/>
        Dead = 4000,
        /// <summary>
        ///  The creature died in the previous tick and the dying
        ///  animation should be performed if there is one.
        /// </summary>
        /// <internal/>
        Died = 15,
        /// <summary>
        ///  The creature was eating in the previous tick.
        /// </summary>
        /// <internal/>
        Ate = 23,
        /// <summary>
        ///  The creature was moving in the previous tick.
        /// </summary>
        /// <internal/>
        Moved = 31,
        /// <summary>
        ///  The creature was not performing any actions.
        /// </summary>
        /// <internal/>
        NoAction = 32,
        /// <summary>
        ///  The creature was teleporting in the previous
        ///  tick.  This could be used for a great teleportation
        ///  animation if one existed.
        /// </summary>
        /// <internal/>
        Teleported = 33,
        /// <summary>
        ///  The creature was reproducing in the previous
        ///  tick.  The animation for reproduction should be
        ///  shown if there is one.
        /// </summary>
        /// <internal/>
        Reproduced = 34
    }
}