//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

namespace Terrarium.Forms 
{
    /// <summary>
    ///  Represents the various running modes of the Terrarium.
    ///  Since Terrarium is primarily run as a screen saver, most
    ///  of these values correspond to the different screen saver
    ///  modes.
    /// </summary>
    public enum ScreenSaverMode
    {
        /// <summary>
        ///  Show a settings dialog
        /// </summary>
        ShowSettingsModeless,
        /// <summary>
        ///  Show a settings dialog
        /// </summary>
        ShowSettingsModal,
        /// <summary>
        ///  Show a preview mode
        /// </summary>
        Preview,
        /// <summary>
        ///  Run as a screen saver
        /// </summary>
        Run,
        /// <summary>
        ///  Run in application mode
        /// </summary>
        NoScreenSaver,
        /// <summary>
        ///  Run the Terrarium in Terrarium game mode
        /// </summary>
        RunLoadTerrarium,
        /// <summary>
        ///  Run the Terrarium in Ecosystem game mode
        /// </summary>
        RunNewTerrarium
    }
}