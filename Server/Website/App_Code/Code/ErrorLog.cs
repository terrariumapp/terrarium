//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Collections;
using System.Web;
using System.Diagnostics;
using System.Reflection;

namespace Terrarium.Server {
    /*
        Enum:       EventLogEventTypes
        Purpose:    This enum defines all of the various event
        log entry types used by the Terrarium web application.
    */
	public enum EventLogEventTypes : int {
		UnknownError,
		VersionError
	}

    /*
        Enum:       EventLogUnknownErrorCategory
        Purpose:    When an unknown error occurs, this enum
        defines the category or area in which the error
        happened.
    */
	public enum EventLogUnknownErrorCategory : short {
		Unknown,
		Database
	}

    /*
        Enum:       EventLogVersionErrorCategory
        Purpose:    When a client with an incorrect version
        of the Terrarium connects to the server and passes
        invalid parameters this enum is used to define the
        category.
    */
	public enum EventLogVersionErrorCategory : short {
		MissingParameter
	}
}