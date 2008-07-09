<%@ Page Language="c#" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ register tagprefix="controls" tagname="FooterBar" src="~/Controls/FooterBar.ascx" %>

<HTML>
	<HEAD>
		<title></title>
		<link rel="stylesheet" type="text/css" href="../theme.css">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	
	<body>
		<!-- BEGIN CENTER ALIGNMENT TABLE -->
		<table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
			<tr>
				<td align="center" valign="top">
								
					<table width="80%" height="100%" border="0" cellpadding="0" cellspacing="0">
						<tr>
						
							<!-- LEFT SHADOW -->
							<td width="30px"><asp:Image ImageUrl="~/images/border_left.png" Width="30px" Height="100%" Runat="server" ID="Image2"/></td>
						
							<td bgcolor="#FFFFFF" width="*" valign="top">
							
								<!-- MAIN LAYOUT -->
								
								<table width="100%" border="0" cellspacing="0" cellpadding="0">
									<!-- BEGIN TITLE BAR AREA -->
									<tr>
										<td colspan="4" class="TitleBar">
											<controls:HeaderBar id="HeaderBar1" RunAt="server" height="64px"/>
										</td>
									</tr>
									<tr><td colspan="2" height="16px"/></tr>
									<!-- END TITLE BAR AREA -->
									<tr>
										<td width="24px" height="100%">&nbsp;</td>

										<!-- BEGIN CONTENT AREA -->
										<td width="*" valign="top">
								<table border="0" cellspacing="0" cellpadding="0" height="320px" width="100%">
									<tr>
										<td class="MainBar">
											<H4>.NET Terrarium 1.2 User Interface Guide</H4>
											<P>The purpose of this document is to familiarize you with the 
											user interface of the Terrarium 1.2 client.</P>
											<H5>Client Window</H5>
											<P>The screenshot below shows what the user interface for the 
											Terrarium 1.2 client looks like.  The window is broken down into several parts, 
											which will be explained in detail later in the document.</P>
											<P align="center"><IMG 
											src="images/image001.jpg"></P>
											<H5>Top Bar</H5>
											<P>The <B>Top Bar</B> is used primarily for display the current 
											state of the Terrarium client.  It also functions as a traditional Windows title 
											bar, providing minimize, maximize and close functionality.</P>
											<P align="center"><IMG 
											src="images/image002.jpg"></P>
											<H6>1. Game Mode</H6>
											<P>This informs you of what mode the 
											client is running in, Terrarium or Ecosystem, If you are in Ecosystem mode, it 
											also informs you what server you are playing on, or if you are in Terrarium 
											mode, what Terrarium file you are currently using.</P>
											<H6>2. Reporting Web Service LED</H6>
											<P>This LED shows the status of the 
											last Reporting Web Service call to the main Terrarium Server. This call tells 
											the central Terrarium server how many of each type of Animal lives in your 
											Terrarium so that the total number of each type of animal can be tallied at the 
											server.</P>
														<P>An LED is a visual indicator of 
														how well that particular function is working.  Green means the last call 
														succeeded.  Yellow means the call is in progress.  Red means the last call 
														failed for some reason.  You can hover over an LED to get a tool tip that will 
														provide more information.</P>
														<H6>3. Peer to Peer Discovery Web 
														Service LED</H6>
														<P>This LED shows the status of the 
														last Peer to Peer discovery web service call. This call lets the central 
														Terrarium server know about the existence of this Terrarium in the 
														Ecosystem.</P>
														<H6>4. Sent Peer to Peer Request 
														LED</H6>
														<P>This LED shows the status of the 
														last sent Peer to Peer Request. This request is made when your Terrarium wants 
														to Teleport an animal to another Peer.</P>
														<H6>5. Received Peer to Peer Request 
														LED</H6>
														<P>This LED shows the status of the 
														last received Peer to Peer Request. This request is made when another Peer wants 
														to Teleport an animal into your Terrarium.</P>
														<H6>6. Environment Statistics</H6>
														<P>This provides details about the 
														current state of the client.  The <B>Animals</B> area shows you the current and 
														maximum population of your client.  The <B>Peers</B> area indicates how many 
														peers are currently in the Ecosystem.  The <B>Teleport</B> area shows various 
														teleport statistics.  The format looks like the following:</P>
														<P><B><SPAN 
														style="FONT-SIZE: 8pt; COLOR: #ff6600">      Teleport: </SPAN></B><B><SPAN 
														style="FONT-SIZE: 8pt">&lt;total&gt;<SPAN style="COLOR: #ff6600"> 
														(</SPAN>&lt;local&gt;<SPAN style="COLOR: #ff6600">/</SPAN>&lt;remote&gt;<SPAN 
														style="COLOR: #ff6600">/</SPAN>&lt;send failed&gt;<SPAN 
														style="COLOR: #ff6600">/</SPAN>&lt;receive failed&gt;<SPAN 
														style="COLOR: #ff6600">)</SPAN></SPAN></B></P>
														<P> </P><SPAN 
														style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana"><BR 
														style="PAGE-BREAK-BEFORE: always" clear=all></SPAN>
														<P><H5>Control Panel</H5></P>
														<P>The <B>Control Panel</B> is how you interact with the 
														Terrarium world.  With it, you will introduce your critters into the world, see 
														a map of your local Terrarium and be informed of critical events that occur.</P>
														<P align="center"><IMG 
														src="images/image003.jpg"></P>
														<H6>1. Join Ecosystem Button</H6>
														<P>This will allow you to join the 
														global Ecosystem if you are currently in Terrarium mode.  If you are in 
														Ecosystem mode already, then this button will be disabled.</P>
														<H6>2. New Terrarium Button</H6>
														<P>This will allow you to create a 
														new Terrarium to use for offline purposes.  If the client is in Ecosystem mode, 
														then this will switch it to Terrarium mode.</P>
														<H6>3. Open Terrarium Button</H6>
														<P>This button allows you to open an 
														existing Terrarium that was previously saved.  If the client is in Ecosystem 
														mode, then this will switch it to Terrarium mode.</P>
														<H6>4. Add Button</H6>
														<P>This enables you to introduce a 
														critter into the Terrarium.  A dialog will be displayed that will allow you to 
														pick a critter from the server, or<B> Browse</B> locally to find your critter.  
														This control will not be displayed if the client is in Ecosystem mode.</P>
														<H6>5. Critter List Combo Box</H6>
														<P>This will contain a list of the 
														different critters that have been introduced previously into this Terrarium.  
														You use this in conjunction with the <B>Insert Critter Button</B> to add more 
														instances of a particular critter to the Terrarium.  This control will not be 
														displayed if the client is in Ecosystem mode.</P>
														<H6>6. Insert Critter Button</H6>
														<P>This adds a new instance of the 
														critter that is currently selected in the <B>Critter List Combo Box</B>.  This 
														control will not be displayed if the client is in Ecosystem mode.</P>
														<H6>7. Pause Button</H6>
														<P>When in Terrarium mode, this 
														allows you to pause the action of the critters.  This is very usefully for 
														debugging.  Pressing it again will resume the critters.</P>
														<H6>8. Minimap</H6>
														<P>This is a zoomed out, overhead 
														view of the world.  Each dot represents a critter, drawn in the marking color 
														the author has chosen.  You can use this as a quick way to judge how well your 
														critter is doing.  You can also use the Minimap as a quick navigation tool by 
														clicking on it.  The game view will center to the area you clicked on.  The 
														white frame on the mini map indicates the current location of the game view. The 
														teleporter is represented by a deep blue dot and plants by green dots. </P>
														<H6>9. Game Trace</H6>
														<P>The Game Trace window gives you 
														details about what animals are being teleported into your Terrarium. It <I>does 
														not</I> show animal Trace information.  You must use the “Trace” button in the 
														Bottom Bar to view animal trace information.</P>
														<H6>10. Introduce Critter Button</H6>
														<P>Use this button to introduce an 
														animal to the Ecosystem. After you create an animal, and test it out on your own 
														Terrarium, this is the way to introduce it into the Ecosystem.  If the client is 
														currently in Terrarium mode, this control will not be visible.</P>
														<H6>11. Reintroduce Critter Button</H6>
														<P>Use this button to reintroduce an 
														animal that has gone extinct (i.e. one that has been killed off from every 
														Terrarium in the Ecosystem). Only extinct animals show up in the Reintroduce 
														Animal window that this button launches.  If the client is currently in 
														Terrarium mode, this control will not be visible.</P><B><SPAN 
														style="FONT-SIZE: 12pt; FONT-FAMILY: Arial"><BR 
														style="PAGE-BREAK-BEFORE: always" clear=all></SPAN></B>
														<H5>Bottom Bar</H5>
														<P>The <B>Bottom Bar</B> provides access to the various dialogs 
														you will use to interact with the client.  It also contains a status bar and the 
														ability to resize the main client window.</P>
														<P> </P>
														<P align="center"><IMG 
														src="images/image004.jpg"></P>
														<H6>1. Settings Button</H6>
														<P>This button will bring up the 
														<B>Game Settings</B> dialog where you can change various settings such as your 
														email address, what server to connect to and the current color scheme used by 
														the client.</P>
														<H6>2. Details Button</H6>
														<P>This button will display the 
														<B>Details</B> dialog.  This allows you to view detailed information about the 
														current state of a critter in your Terrarium.</P>
														<H6>3. Statistics Button</H6>
														<P>Pressing this will display the 
														<B>Statistics</B> dialog.  This displays statistics such as population, births, 
														deaths and teleport numbers for each species living in your Terrarium.</P>
														<H6>4. Trace Button</H6>
														<P>This button will display the 
														<B>Trace</B> dialog.  This dialog displays diagnostic information from the game 
														engine as well as any trace data critters emit via the <B>WriteTrace</B> 
														method.</P>
														<H6>5. Ticker Display</H6>
														<P>This control scrolls through 
														various statistics and information about the Terrarium client and Ecosystem.</P>
														<H6>6. Resize Handle</H6>
														<P>The resize handle allows you to 
														resize the Terrarium client, much like a traditional window.</P>
														<h5>Game View</h5>
														<P>The game view is your window into the Terrarium.  You can 
														watch the battle of life as it unfolds on your desktop!</P>
														<P align="center"><IMG 
														src="images/image005.jpg"></P>
														<H6>1. Background</H6>
														This is the world that the 
														critters in Terrarium walk on.  The graphics used will often have random details 
														in it such as rocks, scratches or other items.  These do not affect game play 
														and are just cosmetic.
														<H6>2. Plant</H6>
														This is a <B>Plant</B> in the 
														Terrarium.  Plants are food for Herbivores.  There are several different 
														graphics used for Plants.
														<H6>3. Critter</H6>
														This is a critter.  These are 
														what you will program and introduce into the Terrarium.   There are several 
														graphics to choose from known as <B>Skins</B>.  This one is using the 
														<B>Beetle</B> skin.
														<H6>4. Teleporter</H6>
														The blue ball is known as the 
														<B>Teleporter</B>.  If it runs over a <B>Plant</B> or a <B>Critter</B>, it will 
														attempt to send it to another client.  If you are in Terrarium mode, or the send 
														fails, you will see it spit the item back out to a random location.

													</td>
												</tr>
											</table>
										</td>
										<!-- END CONTENT AREA -->

										<!-- BEGIN RIGHT MENU BAR -->
										<td valign="top">
											<table border="0" cellpadding="0" cellspacing="0">
												<tr>
													<td class="MenuBar" align="center" valign="top">
														<controls:MenuBar id="MenuBar1" runat="server" />
													</td>
												</tr>
												<tr><td height="24px">&nbsp;</td></tr>
												<tr>
													<td class="MenuBar" align="center" valign="top">
														<controls:InfoBar id="InfoBar1" runat="server" />
													</td>
												</tr>
											</table>
										</td>
										<td width="24px" height="100%">&nbsp;</td>
										<!-- END RIGHT MENU BAR -->
									</tr>
									<tr>
										<td colspan="4">
											<controls:FooterBar id="FooterBar1" runat="server" />
										</td>
									</tr>
								</table>
							</td>
							<td width="30px"><asp:Image ImageUrl="~/images/border_right.png" Width="30px" Height="100%" Runat="server" /></td>

							</tr>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
