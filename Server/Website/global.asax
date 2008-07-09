<%@ Import Namespace="System.Reflection"%>
<%@ Import Namespace="System.IO"%>
<%@ Import Namespace="System.Diagnostics"%>
<%@ Import Namespace="System.Text"%>
<%@ Import Namespace="Terrarium.Server"%>
<%--
//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------
--%>

<script language="C#" runat="server">
    /*
        Method:     Application_Start
        Purpose:    This method is called whenever the web application is
        first started.  This could be during a worker process rollover,
        restarting of IIS, or a change to the web.config that forces the
        application to restart.
        
        This method is responsible for starting the NonPageServices which
        do reporting rollups and generation of new blackboxes.
        
        This method is also responsible for loading the BlackBoxEncoder
        with the DefaultBlackBoxFile.
    */
    void Application_Start(Object sender, EventArgs E) {
    	InstallerInfo.WriteEventLog("Webservice", "Terrarium Website Started", EventLogEntryType.Information);
    	
    	NonPageServices.Current.Start();
    	
    }

    /*
        Method:     Application_End
        Purpose:    This method is called whenever the web application is
        shutting down.
    */
    void Application_End(Object sender, EventArgs E) {
        InstallerInfo.WriteEventLog("Webservice", "Terrarium Website Stopped", EventLogEntryType.Information);
    }

    /*
        Method:     Application_Error
        Purpose:    This method is called whenever the web application needs to
        report an error condition.  The most common call to this function is
        when a page fails to compile because of incorrect code changes.
    */
    void Application_Error(Object sender, EventArgs E) {
		StringBuilder builder = new StringBuilder();
		Exception [] exceptions = Context.AllErrors;
		builder.Append("Error accessing: " + Context.Request.RawUrl + "\r\n");
		foreach(Exception e in exceptions)
			builder.Append("Exception:\r\n" + e.ToString() + "\r\n");

		InstallerInfo.WriteEventLog("Webservice", builder.ToString());
    }

</script>


