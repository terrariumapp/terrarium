<%@ Page Language="c#" AutoEventWireup="false" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>

<HTML>
	<HEAD>
		<title></title>
		<link rel="stylesheet" type="text/css" href="/Terrarium/theme.css">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	
	<body background="/Terrarium/images/background.png">
		<!-- BEGIN CENTER ALIGNMENT TABLE -->
		<table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
			<tr>
				<td align="center" valign="top">
				
					<!-- BEGIN MAIN LAYOUT TABLE -->
				
					<table border="0" cellpadding="0" cellspacing="4px" width="100%" ID="Table1">
				
						<!-- BEGIN TITLE BAR AREA -->
						<tr>
							<td colspan="3" class="TitleBar">
								<controls:HeaderBar id="HeaderBar1" RunAt="server"/>
							</td>
						</tr>
						<!-- END TITLE BAR AREA -->
												
						<tr>
							<!-- BEGIN LEFT MENU BAR -->
							<td class="MenuBar" align="center" valign="top" width="160px">
								<controls:MenuBar id="menuBar" runat="server"/>
							</td>
							<!-- END LEFT MENU BAR -->

							<!-- BEGIN CONTENT AREA -->
							<td width="*" valign="top">
								<table border="0" cellspacing="0" cellpadding="0" height="320px" width="100%">
									<tr>
										<td class="MainBar">
<H4>Terrarium Web Services Whitepaper</H4>

<H5>Goals</H5>
After reading this Whitepaper you should understand the unique operation of each of the Terrarium Web Services.
    
<H5>Terrarium Web Services Overview</H5>
The Terrarium Client uses Web Services in order to interact with the Terrarium Web Application.  For this reason a series of Web Services was developed to harness each of the unique operations that needed to occur between the Client and the Terrarium Web Server and Database backend.  These operations include normal client operations such as registering as a peer and reporting population data.
    
In addition to the Terrarium Client usages, there are also several web services that are used to gain Administration information.  These services are protected under the Administration section of the Web Server and are only useful to server administrators.
    
<H5>Discovery Web Service</H5>
The Discovery Web Service contains methods to register a peer, get information about other peers in the Ecosystem or Terrarium channel, and validates whether the peer is behind a NAT device or publicly visible IP.
    
The first method of importance is the ValidatePeer method.  This method is trivial and simply returns the IP address that the server received from the HTTP Request.  The Client will then check to see if it owns the IP.  If it does own the IP then the Client will continue registering.  If not the client will not register and will report that it is behind a NAT.  A special configuration switch can override this process.
    
The second method of importance is the method for registering a peer. This method performs the act of registering a peer on a given Terrarium Channel or within the Ecosystem.  It also returns to the peer a list of IP addresses that the Peer can contact for teleportation purposes and the total number of clients currently on that channel.
    
The registration process uses the peers current version number, the ip address, the channel the peer wishes to connect to, and the GUID of the peer's current game state file in order to fully register.  In addition to the normal operation, admins on the server can disable Clients based on version.  If the version of the peer matches a disabled Client during registration then that peer is not registered and is told to shut down.
    
<H5>Reporting Web Service</H5>
The Reporting Web Service collects all the species data in the Terrarium and inserts it into the History table for later processing into global data points.
    
The Reporting service takes the current tick of the client, the current GUID of the client's state, and the DataSet of species data the client has collected over the previous 6 minutes (on the average).  At this point the Reporting service can deny the data submission if the Client is reporting too often.  The Client can also deny the submission if the GUID for the client has changed.  If the GUID is changed then the client won't be able to have its reporting data accepted for 12 hours.  If the current tick the client is reporting for is not more recent than the last tick reported for then the data is also rejected.  These rejections help prevent lots of DOS attacks and cheating scenarios.
    
With these basic checks performed the each row in the DataSet is inserted into the database.  Each row is also checked for a valid number.  In this way extreme values can't be reported by a hacked client.  In addition to each row, the totals for all rows are summed and checked.  If any of the basic constraints of the service are breached then the transaction is rolled back and the reporting data is rejected.
    
Assuming that none of the validation logic is triggered, the rows will make it into the database successfully and later be rolled up into an Ecosystem wide data point by the NonPageServices.
    
<H5>Species Web Service</H5>
The Species Web Service is used to insert new species into the Ecosystem. It also returns list of all inserted species, all inserted species that are extinct, and can return the assembly for any submitted creatures.
    
Primarily this service is used to add species.  For this reason the process of adding a species has several constraints.  The first constraint is that a user may submit only one creature every 5 minutes.  This help drastically in cutting down server traffic.  Another feature is that the user may only submit 30 creatures in a 24 hour period.  This in turn helps cut down on the amount of creatures placed onto the disk and into the database.
    
The Species Retrieval services are also used quite frequently.  Each time a user uses the reintroduction dialog the species service gets called to return a species list.  This is also true of the Introduction dialog that is presented in Terrarium mode.  During the process of reintroduction, if your peer doesn't already have the appropriate assembly, the creature assembly is downloaded for you.  Normally this is done on a peer to peer basis during teleportation, but in the case of a reintroduction there isn't a peer to get the assembly from, so the server's copy of the assembly is downloaded to the peer for use.
    
<H5>Watson Web Service</H5>
The Watson Web Service is the interface used for client based bug reports. Any time a bug gets trapped and/or reported by a client the information gets passed through the Watson Web Service.
    
The Watson Web Service formats the data from the client into a form that can be inserted into the database and later recalled by a Terrarium developer. There are three types of issues that get logged:

<ul>
<li><B>Assertion Failures</B> Special Debug code in the Terrarium somehow skipped outside of the bounds of a system constraint.  This generally happens when an unexpected value is somehow obtained from a function or calculation.<br/><br/></li>
<li><B>Unhandled Exceptions</B> These are areas where exceptions were allowed to filter all the way to the top of the Terrarium client exception handling routines.  Normally exceptions are caught at a lower level where they are expected, but some sneak to the top, primarily access exceptions, out of memory exceptions, and other sporadic items fall in this category.<br/><br/></li>
<li><B>User Bugs</B> These are bugs that users submit using the feedback methods in the Terrarium client.  These can be anything from bugs the Terrarium was unable to catch, user suggestions, or complaints.</li>
</ul>

            
            
										</td>
									</tr>
								</table>
							</td>
							<!-- END CONTENT AREA -->
							
							<!-- BEGIN RIGHT MENU BAR -->
							<td class="MenuBar" align="center" valign="top">
								<controls:InfoBar id="InfoBar1" runat="server"/>
							</td>
							<!-- END RIGHT MENU BAR -->
						
						</tr>
						
					</table>
				</td>
			</tr>
		</table>
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
