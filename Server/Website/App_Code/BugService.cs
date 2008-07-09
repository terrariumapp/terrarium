//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for BugService
/// </summary>


    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BugService : System.Web.Services.WebService
    {
        [WebMethod]
        public void ReportBug(Bug bug)
        {
            //TODO: Add code to report bugs
        }
    }

    public class Bug
    {
        public string Title;
        public string Description;
        public string Path;
        public string Alias;
        public string Version;
    }


