<%@ Page Language="c#" %>
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


<p class=MsoBodyText>Peer-To-Peer Communication Explanation and Guidelines</p>

<p class=MsoNormal><o:p>&nbsp;</o:p></p>

<p class=MsoNormal><b><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Abstract:<o:p></o:p></span></b></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>One
of the primary facets of the Terrarium application is a multi-player community
where code can be shared and transferred in a secure manner.<span
style='mso-spacerun:yes'>  </span>Each creature represents an untrusted code
entity that has to be successfully transferred to other peers in order to
propagate a species.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Peer-to-peer
interaction is a general programming concept that can be taken advantage of in
many games.<span style='mso-spacerun:yes'>  </span>The peer-to-peer interaction
scheme developed for the Terrarium was a combination of some custom game code,
and some readily available side projects from other developers.<span
style='mso-spacerun:yes'>  </span>At its base the peer networking code relies
on the use of a request/response mechanism centered on the HTTP protocol.<span
style='mso-spacerun:yes'>  </span>Each peer represents a web server or web site
that can be investigated using the simple web classes and namespace provided by
the .NET Framework networking APIs.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
implementation of the web server code is critical to the current scheme
developed for the Terrarium, but isn’t the only mechanism for implementing
asynchronous peer communication.<span style='mso-spacerun:yes'>  </span>For
this reason, the HttpListener code is provided as is, and only very choice
classes will be discussed in this document.<span style='mso-spacerun:yes'> 
</span>The document more focuses on general peer-to-peer networking concepts
like message synchronization and bandwidth issues.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<h1>General Networking Overview</h1>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
Terrarium networking engine can be broken down into several simple areas.<span
style='mso-spacerun:yes'>  </span>The first layer is the networking engine
class.<span style='mso-spacerun:yes'>  </span>This class exports any methods
that the application will need in order to establish both incoming and outgoing
communications.<span style='mso-spacerun:yes'>  </span><o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>In
general, the incoming communications are all request based, and no connections
are held static.<span style='mso-spacerun:yes'>  </span>This works with the web
server based model in that a peer or web browser defines the data to be sent in
a query string or post data request and the serving peer provides a direct and
single response.<span style='mso-spacerun:yes'>  </span>No ongoing
communications are established.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
outgoing communications is where all of the message synchronization happens.<span
style='mso-spacerun:yes'>  </span>The outgoing communications has the concept
of a conversation.<span style='mso-spacerun:yes'>  </span>A conversation is a
series of requests and responses in a certain order that will in turn be used
to pass information back to the gaming engine.<span style='mso-spacerun:yes'> 
</span>The Terrarium has conversations for transferring creature assemblies and
creature states to other machines, but for the most part.<span
style='mso-spacerun:yes'>  </span>This single conversation has many conditional
<b>verbs</b> that control where the conversation will go and which <b>verb</b>
will be executed next.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>At
the receive module location, there is a lot of piled up infrastructure.<span
style='mso-spacerun:yes'>  </span>Each resource that the peer has to offer is
implemented as an http listener handler.<span style='mso-spacerun:yes'> 
</span>The handler is linked directly to a textual namespace representation
that becomes part of the web URL path used to query for the resource.<span
style='mso-spacerun:yes'>  </span>Some resources can be retrieved using the GET
method and are easily access through a standard web browser.<span
style='mso-spacerun:yes'>  </span>Other resources are implemented using the
POST method and expect the client to include some additional information in the
POST data.<span style='mso-spacerun:yes'>  </span>These requests are easily
implemented using the .NET classes inside of the send module code.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Each
of the resource handlers is in turn attached to the web listener classes.<span
style='mso-spacerun:yes'>  </span>The web listener is a pseudo web server
capable of processing requests in the form of an HTTP request.<span
style='mso-spacerun:yes'>  </span>Inside of the Terrarium networking engine,
the web listener classes are wrapped using an HttpNamespaceManager.<span
style='mso-spacerun:yes'>  </span>This class is capable of starting a web listener,
attaching different handlers, and handling any invalid resource requests.<span
style='mso-spacerun:yes'>  </span>Since this is the higher level class
implementation of the http listener it will be discussed rather than the lower
level web listener APIs.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
following diagram describes the different networking objects along with their
locations with respect to the network engine.<span style='mso-spacerun:yes'> 
</span>The application or game engine isn’t shown, but it is implemented just
outside of the network engine and does all network communications through the
engine itself, rather than interacting directly with the communications modules.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><!--[if gte vml 1]><v:shapetype
 id="_x0000_t75" coordsize="21600,21600" o:spt="75" o:preferrelative="t"
 path="m@4@5l@4@11@9@11@9@5xe" filled="f" stroked="f">
 <v:stroke joinstyle="miter"/>
 <v:formulas>
  <v:f eqn="if lineDrawn pixelLineWidth 0"/>
  <v:f eqn="sum @0 1 0"/>
  <v:f eqn="sum 0 0 @1"/>
  <v:f eqn="prod @2 1 2"/>
  <v:f eqn="prod @3 21600 pixelWidth"/>
  <v:f eqn="prod @3 21600 pixelHeight"/>
  <v:f eqn="sum @0 0 1"/>
  <v:f eqn="prod @6 1 2"/>
  <v:f eqn="prod @7 21600 pixelWidth"/>
  <v:f eqn="sum @8 21600 0"/>
  <v:f eqn="prod @7 21600 pixelHeight"/>
  <v:f eqn="sum @10 21600 0"/>
 </v:formulas>
 <v:path o:extrusionok="f" gradientshapeok="t" o:connecttype="rect"/>
 <o:lock v:ext="edit" aspectratio="t"/>
</v:shapetype><v:shape id="_x0000_i1025" type="#_x0000_t75" style='width:6in;
 height:282pt'>
 <v:imagedata src="file:///C:\DOCUME~1\user\LOCALS~1\Temp\msohtml1\01\clip_image001.png"
  o:title="Networking Diagram"/>
</v:shape><![endif]--><![if !vml]><img width=576 height=376
src="file:///C:\DOCUME~1\user\LOCALS~1\Temp\msohtml1\01\clip_image002.jpg"
v:shapes="_x0000_i1025"><![endif]><o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<h1>Network Engine</h1>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
network engine is responsible for providing all peer-to-peer communications to
the primary game engine through an unchanging and easy to use API.<span
style='mso-spacerun:yes'>  </span>The current API set includes methods to start
and stop the network engine, teleport creature states to other machines, and
provide transparent peer availability broadcasts.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>In
order for any peer communications to occur the network engine must first be
enabled.<span style='mso-spacerun:yes'>  </span>Enabling the network engine
performs two things.<span style='mso-spacerun:yes'>  </span>First, a new <u>HttpNamespaceManager</u>
is initialized and started.<span style='mso-spacerun:yes'>  </span>This allows
the network engine to receive communications from other peers.<span
style='mso-spacerun:yes'>  </span>Second, a new thread is forked in order to
handle peer availability announcements.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>This
second method is handled by the <u>AnnounceAndRegisterPeer</u> method.<span
style='mso-spacerun:yes'>  </span>This method creates a new peer discovery web
service wrapper, and calls the peer registration methods.<span
style='mso-spacerun:yes'>  </span>This in turn contacts the central discovery
server that contains peer leases for all of the peers currently
registered.<span style='mso-spacerun:yes'>  </span>The new peer is either
inserted into the registry of peers, or the peer lease is updated so the
discovery service knows the peer is recent.<span style='mso-spacerun:yes'> 
</span>Once this is established, the peer is issued a collection of 20-30 other
peers that it may contact for teleports.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>This
peer list is also used for peer validation.<span style='mso-spacerun:yes'> 
</span>The peer should only allow connections from the peers within its
registered peer list in order to minimize the amount of traffic it actually
processes.<span style='mso-spacerun:yes'>  </span>For teleportation this also
prevents cheating by individuals who hack the network layer in order to send
creatures to all of the peers simultaneously, giving them a population
advantage.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
Terrarium has the concept of good and bad peers.<span
style='mso-spacerun:yes'>  </span>Good peers are peers that exist within a
known peer collection while bad peers exist in a bad peer collection.<span
style='mso-spacerun:yes'>  </span>Whenever a peer performs some action that
might be considered shady or out of line, the peer is moved from the known peer
collection to the bad peer collection.<span style='mso-spacerun:yes'>  </span>Only
known peers can establish accepted communications while bad peers get blocked.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Peer
identities are also used for purposes of throttling.<span
style='mso-spacerun:yes'>  </span>Each peer connection is metered using a time
function so that peers can’t saturate one another.<span
style='mso-spacerun:yes'>  </span>This is performed using a last receipt field
for each peer that logs the last time the peer had a valid communication (valid
communication means a transferred state, not a set of intermediate verbs).<span
style='mso-spacerun:yes'>  </span>Since connections are metered a single hacked
peer can’t be used to send thousands of creatures to a single peer.<span
style='mso-spacerun:yes'>  </span>And with the known peer restriction a hacked
peer can’t send to a significant amount of the ecosystem (only those peers that
are within its list).<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Once
peers are fully registered they can teleport creatures around the
ecosystem.<span style='mso-spacerun:yes'>  </span>Teleports occur based on an
organism state object.<span style='mso-spacerun:yes'>  </span>This object is
removed from the game by the game engine, and then passed into the network
engine for transfer to one of the valid peers within the peers list.<span
style='mso-spacerun:yes'>  </span>Teleports invoke the send module in order to
establish a multi-part conversation.<span style='mso-spacerun:yes'> 
</span>This is more thoroughly discussed in a later section.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>For
the most part, the network engine is fairly simple.<span
style='mso-spacerun:yes'>  </span>It is capable of providing connection
statistics and information on failed and successful conversations.<span
style='mso-spacerun:yes'>  </span>All of these are implemented through easy to
use properties.<span style='mso-spacerun:yes'>  </span>It is also capable of
providing its own status reporting using the <u>TerrariumLed</u> control.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Since
the peer collections are often accessed, various methods are provided that
synchronize the access on the collections and ensure that multiple threads
don’t have access to the collections at the same time, possibly causing
corruption in the lists.<span style='mso-spacerun:yes'>  </span>These functions
can be used to add peers to the known peer list, move peers between the bad
peer list and known peer list, update properties on the peers in the collections
such as the last receipt time, and to make determinations on whether the given
IP should be considered a valid peer and is capable of initiating
conversations.<span style='mso-spacerun:yes'>  </span>Since all peer control is
handled at the network engine level, it provides a series of features that can
be used by the rest of the networking components in order to provide cheat free
and hack free communications.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<h1>Send Module and Conversations</h1>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>All
send module conversations start through the use of the <u>Teleport</u> method
on the network engine.<span style='mso-spacerun:yes'>  </span>This method can
take any object and attempt to send it to another peer.<span
style='mso-spacerun:yes'>  </span>The only object that will be properly
accepted in the current implementation is the <u>TeleportState</u> object.<span
style='mso-spacerun:yes'>  </span>This object wraps up the game’s state object
for a creature, the creature’s state for internal purposes, and other useful
teleportation information into a single object.<span style='mso-spacerun:yes'> 
</span>It also enables delayed deserialization of the creature on the other
end, so that the Terrarium can control when the deserialization process occurs.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>So
how does the <u>Teleport</u> method work?<span style='mso-spacerun:yes'> 
</span>First, all teleportation attempts are asynchronous, so that they happen
simultaneously while the network engine and game engine continue to run.<span
style='mso-spacerun:yes'>  </span>This means when a new object is being
teleported, it is first attached to a <u>TeleportWorkItem</u>, and then the <u>BeginInvoke</u>
method is used to initiate an asynchronous teleport of the work item.<span
style='mso-spacerun:yes'>  </span>The actual method used during the invoke
process is the <u>DoTeleport</u> method.<span style='mso-spacerun:yes'> 
</span>All of the code to actually teleport the creature over and handle the
conversation is present in this method.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
following diagram demonstrates the actual peer conversation that takes place
during a teleport.<span style='mso-spacerun:yes'>  </span>Immediately following
the diagram is an explanation of each of the 4 messages/verbs that are sent and
how each side processes them.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><!--[if gte vml 1]><v:shape
 id="_x0000_i1026" type="#_x0000_t75" style='width:426pt;height:294.75pt'>
 <v:imagedata src="file:///C:\DOCUME~1\user\LOCALS~1\Temp\msohtml1\01\clip_image003.png"
  o:title="Peer Conversation Diagram"/>
</v:shape><![endif]--><![if !vml]><img width=568 height=393
src="file:///C:\DOCUME~1\user\LOCALS~1\Temp\msohtml1\01\clip_image004.jpg"
v:shapes="_x0000_i1026"><![endif]><o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
box on the right represents a <u>TeleportWorkItem</u> object’s <u>DoTeleport</u>
method.<span style='mso-spacerun:yes'>  </span>The boxes on the right represent
individual connections to the web listener on the remote peer.<span
style='mso-spacerun:yes'>  </span>The first step in the process of is a version
request message.<span style='mso-spacerun:yes'>  </span>In this step the
teleporting peer needs to figure out if the remote peer is running the same
version of the game.<span style='mso-spacerun:yes'>  </span>Channel information
is also returned so that the two machines can make sure they are on the same
channel.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Once
the response is retrieved the version and channel are checked against the local
peer.<span style='mso-spacerun:yes'>  </span>If they are not the same version
then the process ends and no teleport occurs.<span style='mso-spacerun:yes'> 
</span>The teleportation is then scheduled to occur locally on the machine so
the creature isn’t lost in space.<span style='mso-spacerun:yes'>  </span>If the
versions do match, then the second step, assembly validation, occurs.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:green'>// Gets the
Terrarium version of a peer<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>NameValueCollection
GetPeerTerrariumInfo(<span style='color:blue'>string</span> address)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>address = &quot;http://&quot; + address +
&quot;:&quot; + httpPort.ToString() + &quot;/version&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>string</span>
content = <span style='color:blue'>null</span>;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>NameValueCollection peerInfo = <span
style='color:blue'>new</span> NameValueCollection();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>HttpWebRequest req =
(HttpWebRequest)WebRequest.Create(address);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>req.UserAgent = &quot;Microsoft .NET
Terrarium&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>req.Timeout = networkTimeoutMsec;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>using</span> (WebResponse
resp = req.GetResponse())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span><span style='color:blue'>using</span>
(StreamReader reader =<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>                </span><span style='color:blue'>new</span>
StreamReader(<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>                    </span>resp.GetResponseStream(),<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>                    </span>Encoding.ASCII))<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>            </span>content = reader.ReadToEnd();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>          </span><span
style='mso-spacerun:yes'>  </span>reader.Close();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span>resp.Close();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>string</span>
tempString = NetworkEngine.GetValueFromContent(<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>            </span>&quot;&lt;major&gt;&quot;,
&quot;&lt;/major&gt;&quot;, content);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>int</span> major =
Convert.ToInt32(tempString);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>tempString =
NetworkEngine.GetValueFromContent(<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>            </span>&quot;&lt;minor&gt;&quot;,
&quot;&lt;/minor&gt;&quot;, content);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>int</span> minor =
Convert.ToInt32(tempString);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>tempString =
NetworkEngine.GetValueFromContent(<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>            </span>&quot;&lt;build&gt;&quot;,
&quot;&lt;/build&gt;&quot;, content);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>int</span> build =
Convert.ToInt32(tempString);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>string</span>
currentChannel = NetworkEngine.GetValueFromContent(<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>            </span>&quot;&lt;channel&gt;&quot;,
&quot;&lt;/channel&gt;&quot;, content);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>peerInfo[&quot;major&quot;] =
major.ToString();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>peerInfo[&quot;minor&quot;] =
minor.ToString();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>peerInfo[&quot;build&quot;] =
build.ToString();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>if</span>
(currentChannel != <span style='color:blue'>null</span>)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>   </span><span style='mso-spacerun:yes'> </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span>peerInfo[&quot;channel&quot;] =
currentChannel;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>}<span style='mso-spacerun:yes'>      
</span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>return</span>
peerInfo;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>During
the assembly validation request, a message is sent to the remote peer
requesting information about whether a specific assembly exists.<span
style='mso-spacerun:yes'>  </span>This assembly name is encoded in the
request.<span style='mso-spacerun:yes'>  </span>Upon receipt by the remote
peer, the organism namespace handler is invoked to determine if the assembly
exists already.<span style='mso-spacerun:yes'>  </span>If it does then the
remote peer returns true, the assembly exists.<span style='mso-spacerun:yes'> 
</span>Else it returns false.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:green'>//1. Check to see
if the peer has this assembly<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>peerAddress =
&quot;http://&quot; + address + &quot;:&quot; + httpPort.ToString() +<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>&quot;/organisms/assemblycheck&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req =
(HttpWebRequest)WebRequest.Create(peerAddress);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req.UserAgent =
&quot;Microsoft .NET Terrarium&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req.Method =
&quot;POST&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req.Timeout = networkTimeoutMsec;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req.ContentType =
&quot;application/text&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>byte</span><span
style='font-size:9.0pt;font-family:"Courier New"'>[] assemblyBuf =
Encoding.ASCII.GetBytes(assemblyName);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:green'>// Scramble the
message<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>MemoryStream originalStream =
<span style='color:blue'>new</span> MemoryStream(assemblyBuf);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>req.ContentLength =
originalStream.Length;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:9.0pt;font-family:"Courier New"'> (Stream stream =
req.GetRequestStream())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>stream.Write(originalStream.GetBuffer(), 0,
(<span style='color:blue'>int</span>) originalStream.Length);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>No
matter what the response the teleport continues.<span
style='mso-spacerun:yes'>  </span>If the assembly already exists then the send
assembly method is skipped and the fourth step is executed.<span
style='mso-spacerun:yes'>  </span>If the assembly does not exist on the target
machine then it needs to be sent.<span style='mso-spacerun:yes'>  </span>During
this phase, the teleporting peer packages the bytes of the assembly into POST
data package and transmits the data to the remote peer.<span
style='mso-spacerun:yes'>  </span>To avoid complex multi-part data, the POST
data packet is limited only to the assembly bytes.<span
style='mso-spacerun:yes'>  </span>The assembly name is sent by using a special
HTTP header, rather than encoding the name as part of the post data.<span
style='mso-spacerun:yes'>  </span>This is possible because of the use of a web
listener on the other end of the stream.<span style='mso-spacerun:yes'> 
</span>Other peer implementations would have to resort to complex multi-part
data or provide some other form to transfer the data in (perhaps SOAP or custom
XML?).<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>On
the other end the peer assembly is saved.<span style='mso-spacerun:yes'> 
</span>The code assumes the assembly is saved and the final step is executed to
send the teleportation object.<span style='mso-spacerun:yes'>  </span>Again,
the object data is serialized and placed in the POST data segment of the
request.<span style='mso-spacerun:yes'>  </span>The receiving side will unpack
the data and insert it directly into the game engine in an asynchronous queued
buffer.<span style='mso-spacerun:yes'>  </span>In this manner the retrieval of
a creature is also asynchronous and doesn’t affect the operation of the game
engine.<span style='mso-spacerun:yes'>  </span>At this point the remote machine
will return and identify whether the state object was successfully inserted
into a local queue.<span style='mso-spacerun:yes'>  </span>If so, the
teleporting machine ends the conversation and exits the teleport work
item.<span style='mso-spacerun:yes'>  </span>If the object teleportation fails
then the state is reinserted into the local game engine so that the creature
isn’t lost.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
Terrarium networking engine only contains a single conversation at
current.<span style='mso-spacerun:yes'>  </span>However, the ease of
implementing a new conversation, set of verbs, etc… is fairly straightforward,
at least on the teleporting side of things.<span style='mso-spacerun:yes'> 
</span>To truly enable a new verb it has to be added to both ends of the
network layer, both receive and send modules.<span style='mso-spacerun:yes'> 
</span>The next section moves onto the use of the <b>HttpNamespaceManager</b>
and what functions it performs for the application.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<h1>HttpNamespaceManager and Web Listeners</h1>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
<u>HttpNamespaceManager</u> is responsible for abstracting the networking logic
from the specialized web server implementation.<span style='mso-spacerun:yes'> 
</span>The namespace manager provides several features enabling an asynchronous
web listener (this occurs when you <u>Start</u> the namespace manager), easily
mapping namespace names to namespace handlers to represent resources, and
providing a default implementation for unsupported namespaces.<span
style='mso-spacerun:yes'>  </span>The following code is used by the network
engine to start the namespace manager used by the Terrarium.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:gray'>///</span><span
style='font-size:9.0pt;font-family:"Courier New";color:green'> </span><span
style='font-size:9.0pt;font-family:"Courier New";color:gray'>&lt;summary&gt;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:gray'>///</span><span
style='font-size:9.0pt;font-family:"Courier New";color:green'><span
style='mso-spacerun:yes'>  </span>Start the HTTP Listener used for Peer to Peer
interaction.<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:gray'>///</span><span
style='font-size:9.0pt;font-family:"Courier New";color:green'> </span><span
style='font-size:9.0pt;font-family:"Courier New";color:gray'>&lt;/summary&gt;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>public</span><span
style='font-size:9.0pt;font-family:"Courier New"'> <span style='color:blue'>void</span>
StartHttpNamespaceManager()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>namespaceManager = <span style='color:blue'>new</span>
HttpNamespaceManager();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>VersionNamespaceHandler versionHandler = <span
style='color:blue'>new</span> VersionNamespaceHandler();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>OrganismsNamespaceHandler organismsHandler
=<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>        </span><span style='color:blue'>new</span>
OrganismsNamespaceHandler();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:green'>// Starting the
manager starts the http listener<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>namespaceManager.Start(HostIP, httpPort,
receivedPeerConnectionLed);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:green'>// Register the
namespaces we intend to service<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>namespaceManager.RegisterNamespace(&quot;version&quot;,
versionHandler);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>   
</span>namespaceManager.RegisterNamespace(&quot;organisms&quot;,
organismsHandler);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>   
</span>namespaceManager.RegisterNamespace(&quot;organisms/&quot;,
organismsHandler);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>In
reality the web listener doesn’t support namespaces at all.<span
style='mso-spacerun:yes'>  </span>It simply cracks the incoming request into a
set of HTTP objects.<span style='mso-spacerun:yes'>  </span>It is the namespace
manager that examines the properties on these objects to figure out which
resources are actually being requested.<span style='mso-spacerun:yes'> 
</span>Once it figures out the target resource it makes sure the resource
exists in the namespace handler table, and then invokes the appropriate
handler.<span style='mso-spacerun:yes'>  </span>If no handler exists, then
depending on the namespace requested and the verb used a default error response
is formulated.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>internal</span><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>  </span><span style='color:blue'>void</span>
HandleBadRequest(HttpApplication state, <span style='color:blue'>string</span>
message)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.StatusCode =
HttpStatusCode.BadRequest;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.StatusDescription =
&quot;Bad Request&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.Server = &quot;Microsoft
.Net Terrarium&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.Date = DateTime.Now;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.ContentType =
&quot;text/html&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.KeepAlive = <span
style='color:blue'>false</span>;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>string</span> body
= message;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>byte</span>[]
bodyBytes = Encoding.ASCII.GetBytes(body);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.ContentLength = (<span
style='color:blue'>long</span>)bodyBytes.Length;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>state.HttpResponse.Close(bodyBytes);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>When
a valid namespace is received and a valid method has been implemented (a POST
has occurred and there is actual data, GET should always succeed), the
underlying namespace handler is invoked.<span style='mso-spacerun:yes'> 
</span>The <u>ProcessRequest</u> method is called on the handler and the entire
<u>HttpApplication</u> state object is passed in.<span
style='mso-spacerun:yes'>  </span>This gives the handlers a lot of power to
work with the request as they see fit.<span style='mso-spacerun:yes'> 
</span>The next section examines one of the Terrarium handlers to see how it
operates.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<h1>HTTP Listener Handler Implementation</h1>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>A
HTTP Handler is implemented through the <u>IHttpNamespaceHandler</u>
interface.<span style='mso-spacerun:yes'>  </span>The easiest handler
implemented by the Terrarium is the <u>VersionNamespaceHandler</u>.<span
style='mso-spacerun:yes'>  </span>This is the handler responsible for returning
version and channel information during the first step of the Terrarium object
teleportation conversation.<span style='mso-spacerun:yes'>  </span>Implementing
the required interface is quite easy since it only exports a single method, <u>ProcessRequest</u>.<span
style='mso-spacerun:yes'>  </span>The following code shows a skeletal
implementation.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>internal</span><span
style='font-size:9.0pt;font-family:"Courier New"'> <span style='color:blue'>class</span>
VersionNamespaceHandler : IHttpNamespaceHandler<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span><span style='color:blue'>public</span> <span
style='color:blue'>void</span> ProcessRequest(HttpApplication webapp)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:9.0pt;font-family:"Courier New"'>}</span><span
style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
<u>ProcessRequest</u> method is where everything happens.<span
style='mso-spacerun:yes'>  </span>Handlers should control method
implementation, processing of POST data, and reporting of errors.<span
style='mso-spacerun:yes'>  </span>In the case of the version namespace handler
the first step is to set up any headers and response properties that will
enable the handler to operate efficiently.<span style='mso-spacerun:yes'> 
</span>Items like the <u>Server</u> are set to identify this server as a
Terrarium server, and a default content-type is set to “text/xml” since that is
the type of data the handler will be returning.<span style='mso-spacerun:yes'> 
</span>Most of the server response variables will be the same between all of
your namespace handlers.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.Server =
&quot;Microsoft .Net Terrarium&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.Date =
DateTime.Now;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.ContentType
= &quot;text/xml&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.StatusCode
= HttpStatusCode.OK;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.StatusDescription
= &quot;OK&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.KeepAlive
= <span style='color:blue'>false</span>;<o:p></o:p></span></p>

<p class=MsoNormal><u><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p><span
 style='text-decoration:none'>&nbsp;</span></o:p></span></u></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>The
application then attempts to process the request method.<span
style='mso-spacerun:yes'>  </span>The version namespace handler only properly
supports the GET method.<span style='mso-spacerun:yes'>  </span>When the GET method
is invoked an XML message is built up and returned as the response.<span
style='mso-spacerun:yes'>  </span>This is the XML that later gets parsed by the
teleporting peer and becomes the basis of further conversation verbs.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>if</span><span
style='font-size:9.0pt;font-family:"Courier New"'> (webapp.httpRequest.Method
== &quot;GET&quot;)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body =
&quot;&lt;version&gt;&lt;build&gt;&quot; + peerVersion.Build.ToString() +
&quot;&lt;/build&gt;&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body +=&quot;&lt;major&gt;&quot; +
peerVersion.Major.ToString() + &quot;&lt;/major&gt;\t&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body +=&quot;&lt;minor&gt;&quot; +
peerVersion.Minor.ToString() + &quot;&lt;/minor&gt;\t&lt;/version&gt;&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body +=&quot;&lt;channel&gt;&quot; +
GameEngine.Current.PeerChannel + &quot;&lt;/channel&gt;&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>For
the POST method, the terrarium implements a form of reflection, in which the
original POST data sent in, is encoded in ASCII, and finally sent back to the
user as part of an HTML page.<span style='mso-spacerun:yes'>  </span>The <u>ContenType</u>
is also reset to “text/html” since HTML is now being returned instead of
XML.<span style='mso-spacerun:yes'>  </span>This response isn’t truly valid and
won’t help further the conversation.<span style='mso-spacerun:yes'>  </span>For
this reason, the get method needs to be used whenever talking with the version
handler.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'>Additionally,
any other method is implemented by sending back an error message.<span
style='mso-spacerun:yes'>  </span>The <u>StatusCode</u> is set to the <u>MethodNotAllowed</u>
value and an HTML status message is generated.<span style='mso-spacerun:yes'> 
</span>Normally, IE will display friendly messages so this won’t be seen, but
for other browsers, or someone using the web methods to interact with the
Terrarium this message can provide concrete information on why the handler
failed.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;mso-bidi-font-size:12.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>else<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>webapp.HttpResponse.StatusCode =
HttpStatusCode.MethodNotAllowed;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>webapp.HttpResponse.StatusDescription =
&quot;Method Not Allowed&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body = &quot;&lt;HTML&gt;&lt;BODY&gt;&quot;
+ &quot;The method &quot; + webapp.httpRequest.Method;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>body += &quot; is not
allowed.&lt;/BODY&gt;&lt;/HTML&gt;&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>    </span>webapp.HttpResponse.ContentType =
&quot;text/html&quot;;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt'>Once the body has been built
up the response stream is written and closed off.<span
style='mso-spacerun:yes'>  </span>That ends the namespace handler.<span
style='mso-spacerun:yes'>  </span>With this information you can easily
implement your own handler’s, conversations and verbs.<span
style='mso-spacerun:yes'>  </span>You can even use the code provided to equip
your own game with peer to peer functionality.<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:blue'>byte</span><span
style='font-size:9.0pt;font-family:"Courier New"'>[] bodyBytes =
Encoding.ASCII.GetBytes(body);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.ContentLength
= (<span style='color:blue'>long</span>)bodyBytes.Length;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New";color:green'>// code to write
back response goes here<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:9.0pt;font-family:"Courier New"'>webapp.HttpResponse.Close(bodyBytes);<o:p></o:p></span></p>

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
