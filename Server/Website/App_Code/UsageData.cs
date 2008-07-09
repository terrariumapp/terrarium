//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using Terrarium.Server;

namespace Terrarium.Server
{
    public class UsageData
    {
        /// <remarks/>
        public string Alias;

        /// <remarks/>
        public string Domain;

        /// <remarks/>
        public DateTime TickTime;

        /// <remarks/>
        public int UsageMinutes;

        /// <remarks/>
        public string IPAddress;

        /// <remarks/>
        public string GameVersion;

        /// <remarks/>
        public string PeerChannel;

        /// <remarks/>
        public int PeerCount;

        /// <remarks/>
        public int AnimalCount;

        /// <remarks/>
        public int MaxAnimalCount;

        /// <remarks/>
        public int WorldWidth;

        /// <remarks/>
        public int WorldHeight;

        /// <remarks/>
        public string MachineName;

        /// <remarks/>
        public string OSVersion;

        /// <remarks/>
        public int ProcessorCount;

        /// <remarks/>
        public string ClrVersion;

        /// <remarks/>
        public int WorkingSet;

        /// <remarks/>
        public int MaxWorkingSet;

        /// <remarks/>
        public int MinWorkingSet;

        /// <remarks/>
        public int ProcessorTimeInSeconds;

        /// <remarks/>
        public System.DateTime ProcessStartTime;
    }
}
