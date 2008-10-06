//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

namespace Terrarium.PeerToPeer
{
    // The purpose of this class is to make teleportations asynchronous so they don't block the game
    // It is used to call TeleportWorkItem.DoTeleport()
    internal delegate object TeleportDelegate();
}