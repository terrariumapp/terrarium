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

														<H4>What is Terrarium?</H4>
														
														<P>Terrarium, a sample application built 
														by Microsoft®, is game for software developers that provides a great 
														introduction to software development on the .NET Framework.  In Terrarium, 
														developers create herbivores, carnivores, or plants and then introduce them into 
														a peer-to-peer, networked ecosystem for a survival-of-the-fittest type 
														competition.  The game provides both a competitive medium for testing your 
														software development and strategy skills as well as a realistic evolutionary 
														biology/artificial intelligence model for evaluating the role that various 
														behaviors and traits can play in the fight for survival.  Terrarium also 
														demonstrates some of the features of the .NET Framework, including the Windows 
														Forms integration with DirectX® for generating powerful user interface (UI); XML 
														Web services; support for peer-to-peer networking; support for multiple 
														programming languages; the capability to update <I>smart client</I>, or 
														Windows-based, applications via a remote Web server; and the evidence-based and 
														code access security infrastructure that protects participating computers from 
														the mobile code they are running.  This paper will focus on the technological 
														merits of Terrarium, presenting a high-level overview of the role that the above 
														features of the .NET Framework play in the game.</P>
														
														<H5>Game Overview</H5>
														
														<P>Before diving into the technical 
														details, let’s present a brief look at the actual flow-of-action involved in 
														running the game.  The game can run in two modes:</P>
														<P class=Normal style="MARGIN: 12pt 0in 0pt 0.5in; TEXT-INDENT: -0.5in"><SPAN 
														style="FONT-FAMILY: Symbol">·<SPAN style="FONT: 7pt 'Times New Roman'">        
														</SPAN></SPAN>Terrarium Mode – This gives the user two options.  The user may 
														run alone, without peers.  In this case, the ecosystem presented on the screen 
														represents the whole of the ecosystem.  This is good for creature testing 
														purposes.  The user may also elect to join with a select group of peers, 
														expanding the ecosystem across all of the participating peer computers.  This is 
														simple to do.  Each participating user opts into a special, private network by 
														entering an agreed upon character string in the “channel” textbox on the 
														Terrarium console.  Upon entering that string, the user’s computer is matched 
														with only those computers which also entered that same string.</P>
														<P class=Normal style="MARGIN: 12pt 0in 0pt 0.5in; TEXT-INDENT: -0.5in"><SPAN 
														style="FONT-FAMILY: Symbol">·<SPAN style="FONT: 7pt 'Times New Roman'">        
														</SPAN></SPAN>Ecosystem Mode – This is the standard mode, in which the user’s 
														computer runs just a small slice of an ecosystem which spans all of the 
														participating peer computers, worldwide.  </P>
														
														<P>In both modes, developers are free to 
														develop their own creatures, using the Terrarium class libraries coupled with 
														the .NET Framework SDK or Visual Studio .NET.  Alternatively, they may simply 
														watch as other developers’ creatures battle it out for survival by running 
														Terrarium as a standalone application or as a screensaver.</P>
														
														<P>In creating a creature, developers have 
														complete control over everything from genetic traits (eyesight, speed, defensive 
														power, attacking power, etc.) to behavior (the algorithms for locating prey, 
														moving, attacking, etc.) to reproduction (how often a creature will give birth 
														and what “genetic information,” if any, will be passed on to its offspring).  
														Upon completing the development process, the code is compiled into an assembly 
														(dynamically linked library, or DLL) that can be loaded into the local ecosystem 
														slice, viewable through the Terrarium console.  When a creature is initially 
														introduced in Ecosystem Mode, ten instances of it are scattered throughout the 
														local ecosystem.  No more instances of that creature may be introduced by that 
														user or any other on the network until the creature has died off completely.  By 
														contrast, if running in Terrarium Mode, an infinite number of instances of a 
														given creature may be entered into the environment.</P>
														
														<P>Once the creature is loaded into 
														Terrarium, it acts on the instructions supplied by its code.  Each creature is 
														only granted between 2 and 5 milliseconds (depending on the speed of the 
														machine) to act before it is destroyed.  This prevents any one creature from 
														hogging the processor and halting the game, in the case of an infinite loop, for 
														example.</P>
														
														<P>Within each peer in the network, a blue 
														“teleporter” ball rolls randomly about.  If the user is running with active 
														peers logged in (either in Ecosystem Mode or using a private channel in 
														Terrarium Mode), whenever this blue ball rolls over a creature that creature is 
														transported to a randomly selected peer machine.</P>
														
														<P>A central, “master” server provides 
														peer discovery and reporting features.</P>
														
														<H5>User Interface</H5>
														
														<P align="center">
														<IMG src="images/screenshot.jpg">
														</P>
														
														<P>The screenshot above is of the 
														Terrarium console and shows a small piece of the ecosystem as well as some of 
														the controls available to clients running the game which enable introducing a 
														creature, providing reporting information, etc.  </P>
														
														<P>The form and buttons on the screen 
														above were all generated using Windows Forms, the classes of the .NET Framework 
														for Windows-based UI development.  These developer-friendly, extensible 
														libraries effectively combine the ease-of-use of Visual Basic with the power of 
														C++.</P>
														
														<P>The ecosystem graphics – both the 
														creatures and the landscape, which are presented at a rate of 20 frames per 
														second – were developed using DirectX, a set of graphics libraries which gives 
														developers direct access to the system’s video card, enabling powerful, 
														optimized graphics performance.  Interestingly, at the time of development, the 
														DirectX SDK had not been implemented for managed, or .NET Framework-based, 
														code.  As a result, the developers of Terrarium took advantage of COM Interop, a 
														.NET Framework service used to tie into existing unmanaged code from within a 
														.NET Framework-based application.  The ability to call unmanaged code is a 
														powerful one and can enable developers to leverage existing, legacy components 
														as they go forward in their application development.  </P>
														
														<H5>XML Web Services</H5>
														
														<P>An XML Web service is an application that exposes its 
														functionality programmatically over the Internet or intranet using Internet 
														protocols and standards, such as SOAP, WSDL, and XML.  Essentially, XML Web 
														services provide a loosely-coupled, message-oriented, platform-neutral model for 
														distributed computing, allowing a client machine running any platform to invoke 
														functionality on a remote server machine running any platform, even through a 
														firewall.  </P>
														
														<P>XML Web services are used throughout Terrarium.  While the game 
														itself runs on a peer-to-peer network, in which every participant is equal, 
														acting as both a client and a server, there is one master server used for 
														purposes such as peer discovery and reporting.  The peer computers interact with 
														this master server using XML Web services.  </P>
														
														<H6>Validating the IP Address</H6>
														
														<P>When a peer computer first loads its instance of Terrarium, it 
														calls an XML Web service hosted by the master server to find out what IP address 
														is exposed to the master server.  By comparing the returned IP address (the one 
														exposed to the world outside) to the IP address the computer is using locally, 
														the game is able to find out if its IP address is being changed in a way that 
														would prevent other peers from communicating with it.  This may happen, for 
														example, if the client computer goes through a proxy server or if Network 
														Address Translation is at work. If this is the case, the peer computer will not 
														be allowed to join the ecosystem, as its peers will not be able to communicate 
														directly with it.</P>
														
														<H6>Registration and Peer Discovery</H6>
														
														<P>Assuming the peer computer has a static, public IP address, it 
														calls another XML Web service on the master server that will register it on the 
														list of enrolled peers.  In turn, the peer computer is returned a count of all 
														the peers participating as well as a “peer contact list” of 20-30 IP addresses 
														representing geographically close peers which will be used for creature exchange 
														later in the game.  The list of 20-30 peers returned overlaps in such a way that 
														a fully connected network is formed – meaning that there are no “islands” of 
														peers that are isolated from the rest of the network. A given peer will reject 
														any connection attempts from a peer that is not in its “peer contact list”.  
														Each peer computer renews its registration status every five minutes.  If a peer 
														computer fails to renew its registration status in over 15 minutes, that peer 
														computer is assumed to have left the network and is removed from the 
														registration list and replaced in all of the “peer contact lists” which 
														contained it.</P>
														
														<H6>Loading a Creature Assembly</H6>
														
														<P>Once developers have written their creature assemblies, they may 
														use the “Introduce Animal” button to load the creature into the ecosystem.  
														Behind the scenes, the assembly’s code is quickly scanned to ensure all the 
														critical methods are in place and that functionality which might support 
														cheating has not been implemented.  If the assembly passes this test, the peer 
														computer calls an XML Web service on the master server to register the creature 
														and actually sends along a copy of the assembly for storage on the server.  (If 
														the creature ever becomes extinct, a user may reintroduce the creature into the 
														ecosystem, using the version stored on the master server.)  Only after all this 
														has taken place will 10 copies of the creature actually be loaded into the local 
														Terrarium ecosystem slice.</P>
														
														<P>When the creatures are deployed into the Terrarium ecosystem, 
														they are hosted securely using the .NET Framework’s evidence-based and code 
														access security infrastructure.  This prevents them for accessing and 
														potentially tampering with any resources on the local machine.  See the section 
														below entitled “Evidence-based and Code Access Security” for more information on 
														how this works.</P>
														
														<H6>Reporting</H6>
														
														<P>Roughly every six minutes, each peer computer gathers together 
														information on the number and types of creatures alive in its environment, rolls 
														this data up into a data set, and sends this to the central server, where it 
														will be aggregated with the data sets from the other participating peers and 
														published to a public website for statistical reporting purposes.</P>
														
														<H5>Peer-to-Peer Networking</H5>
														
														<P>Peer-to-peer networking functionality is implemented via the 
														System.Net and System.IO classes in the .NET Framework.  When the teleporter 
														ball rolls over a creature, one peer computer is selected at random from the 
														20-30 listed in the machine’s server-assigned “peer contact list.”  This random 
														peer is queried by the sending peer to see if it has the assembly for the 
														creature in question.  If not, the assembly is streamed across to the receiving 
														peer using a network stream, which is easy to set up as a developer, using the 
														functionality provided by the System.Net classes.  Once the assembly is on the 
														receiving computer’s local disk, the creature’s state object (which contains 
														information on its present size, energy level, etc.) is serialized, using the 
														System.Runtime.Serialization classes, and sent across to the receiving peer over 
														another network stream.  It is then deserialized and associated with the 
														creature assembly.  The resulting creature, an exact replica of the one 
														transported out of the sending peer, is then inserted into the receiving peer’s 
														ecosystem slice and activated.</P>
														
														<H5>Support for Multiple Programming Languages</H5>
														
									The .NET Framework supports over 20 programming languages.  Developers can 
														select the language that best meets their needs and skill sets and know that the 
														code they write will be able to transparently communicate with and even inherit 
														from classes written in any of the 20+ other .NET Framework programming 
														languages.  At present, creatures for Terrarium can be developed in either C# or 
														Visual Basic .NET.  This is to prevent cheating.  As mentioned earlier, when a 
														creature is loaded, its code is scanned by the Terrarium client to ensure that 
														it does not harbor any functionality that could give it an unfair advantage over 
														the other creatures.  Static methods, threading calls, and deconstructors could 
														all be used to effectively cheat.  Unfortunately, some language compilers 
														automatically generate a static constructor method, for example.  The Terrarium 
														code scan picks up on this and denies that code the right to run.  In the 
														future, Terrarium will support more programming languages. </P>
														
														<H5>Version Updates from a Remote Web Serverb Server</H5>
														
														<P>The .NET Framework greatly improves the 
														deployment process for <I>smart client</I>, or Windows-based, applications, by 
														preventing DLL conflicts and allowing systems administrators to both deploy and 
														update these applications via a remote Web server.  To accomplish this, 
														Terrarium actually uses another sample application, the .NET Application 
														Updater, to transparently handle all of the work involved.  This component 
														(which will be released along with documentation on MSDN in February) calls an 
														XML Web service hosted by the master server to see if an update for the 
														Terrarium application is available.  This XML Web services is called 30 seconds 
														after the Terrarium application is launched on the peer computer and every 15 
														minutes thereafter. The XML Web service simply takes with it the peer computer’s 
														application version number and compares it to the latest version number 
														available.  If there is a new version available, it returns to the peer computer 
														the URL where the new version is available for download.   Using the System.Net 
														classes, the peer computer will download the new files into a new folder, while 
														continuing to run the older version.  Digital signatures ensure that the new 
														version files are authentic and have not been tampered with.  Once the download 
														is complete, the configuration file which directs the Terrarium application 
														executable stub to the folder with the assemblies which provide the game’s UI 
														and functionality is overwritten so as to point to the new folder containing the 
														new assemblies.  The next time the Terrarium application is launched, it will 
														launch with the new version.  The folder containing the last version is always 
														kept around in case there is an error with the new version.  Any older version 
														folders will be deleted to recycle disk space.</P>
														
														<H5>Evidence-based and Code Access Security</H5>
														
														<P>Creatures represent mobile code that is 
														transmitted from one machine to the next in the peer-to-peer Terrarium network.  
														Thanks to the .NET Framework’s evidence-based and code access security 
														infrastructure, machines are kept safe from code that might be intentionally or 
														unintentionally harmful.  </P>
														
														<P>Generally speaking, evidence-based 
														security in the .NET Framework enables code to be trusted to varying degrees, 
														depending upon its origin and other characteristics such as the identity of the 
														author.  These characteristics represent evidence which is used to assign the 
														code in question to a code group, or category, each of which has a permission 
														set which determines what resources can and cannot be accessed by that code.  At 
														runtime, thanks to a technology known as code access security, the .NET 
														Framework’s common language runtime (CLR) performs low-level security checks, 
														ensuring that code executes only those operations allowed by the security 
														permissions it has been granted.  In doing so, the CLR checks not only the 
														permissions allotted to the assembly attempting a particular operation, but 
														those of all the other assemblies in the stack that might be calling the active 
														assembly to act on their behalf.  Only operations approved in this comprehensive 
														stack walk will be performed.  Others will not. </P>
														
														<P>Code groups and their corresponding 
														permission sets can be set at the enterprise, machine, user, or application 
														domain level.  At runtime, the permissions are intersected, so effectively, 
														machine level settings can only be more restrictive than enterprise settings and 
														so on.  With Terrarium, the principal security settings take place at the 
														application domain level.  Within the Terrarium application domain, creature 
														code is given execute-only security permissions.  That is to say, the code may 
														run, but it is forbidden access to any system resources, such as the local disk, 
														the system registry, etc.  While highly restrictive, this policy grants 
														creatures the freedom they require to exist and act in the Terrarium ecosystem, 
														while providing users with the protection they need to participate in this 
														peer-to-peer network.</P>
														
														<H5>Conclusion</H5>
														
														<P>Terrarium provides a great way to gain 
														an introduction to .NET Framework programming, a showcase for some of the many 
														technological advancements built into the .NET Framework, and a powerful 
														modeling tool for use by those studying evolutionary biology or artificial 
														intelligence.  First and foremost, though, it is a game.  Developing a creature 
														can be as easy or as challenging, as leisurely or as competitive, as you wish to 
														make it.  Have fun and good luck.</P>

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
							<td width="30px"><asp:Image ImageUrl="~/images/border_right.png" Width="30px" Height="100%" Runat="server"/></td>

							</tr>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
