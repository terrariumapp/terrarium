<%@ Page Language="c#" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ register tagprefix="controls" tagname="FooterBar" src="~/Controls/FooterBar.ascx" %>

<HTML>
	<HEAD>
		<title></title>
		<link rel="stylesheet" type="text/css" href="../../../theme.css">
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
<div class=Section1>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Imports</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Imports</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.Drawing<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Imports</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.Collections<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Imports</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.IO<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Imports</span><span
style='font-size:10.0pt;font-family:"Courier New"'> OrganismBase<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' Sample
Carnivore<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' Strategy: This
animal stands still until it sees something tasty and then<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' bites it.<span
style='mso-spacerun:yes'>&nbsp; </span>If any dead animals are within range it
eats them.<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' The following
Assembly attributes must be applied to each Organism Assembly<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' The class that
derives from Animal<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' Provide Author
Information<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'>' Author Name,
Email<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:green'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>Namespace</span><span
style='font-size:10.0pt;font-family:"Courier New"'> VBCreatures<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
Carnivore = True, It's a Carnivore<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
MatureSize = 30, Must be between 24 and 48, 24 means faster<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
reproduction while 48 would give more defense and attack power<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
AnimalSkin = AnimalSkinFamilyEnum.Scorpion, you can be a Beetle<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
an Ant, a Scorpion, an Inchworm, or a Spider<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
MarkingColor = KnownColor.Red, you can choose to mark your<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
creature with a color.<span style='mso-spacerun:yes'>&nbsp; </span>This does
not affect appearance in<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
the game.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
Point Based Attributes<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
You get 100 points to distribute among these attributes to define<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
what your organism can do.<span style='mso-spacerun:yes'>&nbsp; </span>Choose
them based on the strategy<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
your organism will use.<span style='mso-spacerun:yes'>&nbsp; </span>This
organism hides and has good eyesight<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
to find plants.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
MaximumEnergyPoints = 0, No need to store energy.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
EatingSpeedPoints = 0, No need to eat fast.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
AttackDamagePoints = 52, Needs to deal some good damage<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
DefendDamagePoints = 0, Attacks, so no need for defense<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
MaximumSpeedPoints = 28, Moves somewhat fast to catch prey<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
CamouflagePoints = 0, Doesn't mind if noticed<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>'
EyesightPoints = 20, Needs to see prey and food<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span>&lt; _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>CarnivoreAttribute(<span style='color:blue'>True</span>), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MatureSize(30), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>AnimalSkin(AnimalSkinFamily.Beetle),
_<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MarkingColor(KnownColor.Red), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MaximumEnergyPoints(0), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>EatingSpeedPoints(0), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>AttackDamagePoints(52), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>DefendDamagePoints(0), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MaximumSpeedPoints(28), _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>CamouflagePoints(0),
_<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>EyesightPoints(20) _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span>&gt; _<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:blue'>Public</span>
<span style='color:blue'>Class</span> SimpleCarnivore : <span style='color:
blue'>Inherits</span> Animal<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Dim</span> targetAnimal <span style='color:blue'>As</span>
AnimalState<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Protected</span> <span style='color:blue'>Overloads</span> <span
style='color:blue'>Overrides</span> <span style='color:blue'>Sub</span>
Initialize()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>AddHandler</span> Load, <span style='color:
blue'>New</span> LoadEventHandler(<span style='color:blue'>AddressOf</span>
LoadEvent)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>AddHandler</span> Idle, <span style='color:
blue'>New</span> IdleEventHandler(<span style='color:blue'>AddressOf</span>
IdleEvent)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' First event fired on an organism each turn<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Private</span> <span style='color:blue'>Sub</span>
LoadEvent(<span style='color:blue'>ByVal</span> Sender <span style='color:blue'>As</span>
<span style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e
<span style='color:blue'>As</span> LoadEventArgs)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> <span style='color:blue'>Not</span>
targetAnimal <span style='color:blue'>Is</span> <span style='color:blue'>Nothing</span>
<span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' See if our target animal still exists (it
may have died)<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' LookFor returns null if it isn't found<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>targetAnimal = <span style='color:blue'>CType</span>(LookFor(targetAnimal),
AnimalState)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Catch</span> exc <span style='color:blue'>As</span>
Exception<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' WriteTrace is useful in debugging creatures<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>WriteTrace(exc.ToString())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' Fired after all other events are fired during a turn<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Private</span> <span style='color:blue'>Sub</span>
IdleEvent(<span style='color:blue'>ByVal</span> Sender <span style='color:blue'>As</span>
<span style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e
<span style='color:blue'>As</span> IdleEventArgs)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' Our Creature will reproduce as often as
possible so<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' every turn it checks CanReproduce to know
when it<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' is capable.<span
style='mso-spacerun:yes'>&nbsp; </span>If so we call BeginReproduction with<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' a null Dna parameter to being reproduction.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> CanReproduce <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>BeginReproduction(<span style='color:blue'>Nothing</span>)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If we are already doing something, then we
don't<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' need to do something else.<span
style='mso-spacerun:yes'>&nbsp; </span>Lets leave the<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' function.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> IsAttacking <span style='color:blue'>Or</span>
IsMoving <span style='color:blue'>Or</span> IsEating <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Return<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' Try to find a new target if we need one.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> targetAnimal <span style='color:blue'>Is</span>
<span style='color:blue'>Nothing</span> <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>FindNewTarget()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span
style='color:green'>' If we have a target animal then lets check him out<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> <span style='color:blue'>Not</span>
targetAnimal <span style='color:blue'>Is</span> <span style='color:blue'>Nothing</span>
<span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If the target is alive, then we need to kill
it<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' else we can immediately eat it.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='mso-spacerun:yes'>&nbsp;&nbsp;</span><span
style='color:blue'>If</span> targetAnimal.IsAlive <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If we are within attacking range, then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' lets eat the creature.<span
style='mso-spacerun:yes'>&nbsp; </span>Else we'll<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' try to move into range<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> WithinAttackingRange(targetAnimal) <span
style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>BeginAttacking(targetAnimal)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MoveToTarget()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If the creature is dead then we can try to
eat it.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If we are within eating range then we'll try
to<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' eat the creature, else we'll move towards
it.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> WithinEatingRange(targetAnimal) <span
style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> CanEat <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>BeginEating(targetAnimal)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>MoveToTarget()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span
style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' If we stop moving we conserve energy.<span
style='mso-spacerun:yes'>&nbsp; </span>Sometimes<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' this works better than running around.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>StopMoving()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Catch</span> exc <span style='color:blue'>As</span>
Exception<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>WriteTrace(exc.ToString())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Private</span> <span style='color:blue'>Sub</span>
FindNewTarget()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Dim</span> foundAnimals <span style='color:
blue'>As</span> ArrayList = Scan()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' We should see at least a couple of animals.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> foundAnimals.Count &gt; 0 <span
style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Dim</span> orgState <span style='color:blue'>As</span>
OrganismState<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>For</span> <span style='color:blue'>Each</span>
orgState <span style='color:blue'>In</span> foundAnimals<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' We are looking for any organism that is an
animal<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' and that isn't of our species so we can eat
them.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> <span style='color:blue'>TypeOf</span>
orgState <span style='color:blue'>Is</span> AnimalState <span style='color:
blue'>And</span> <span style='color:blue'>Not</span> IsMySpecies(orgState) <span
style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>targetAnimal = <span style='color:blue'>CType</span>(orgState,
AnimalState)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span
style='color:blue'>Next<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Catch</span> exc <span style='color:blue'>As</span>
Exception<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>WriteTrace(exc.ToString())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' Function used to move towards our prey or meal<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Private</span> <span style='color:blue'>Sub</span>
MoveToTarget()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;</span><span
style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' Make sure we aren't moving towards a null
target<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>If</span> targetAnimal <span style='color:blue'>Is</span>
<span style='color:blue'>Nothing</span> <span style='color:blue'>Then<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Return<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>If<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:green'>' Follow our target as quickly as we can<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>BeginMoving(<span style='color:blue'>New</span>
MovementVector(targetAnimal.Position, Species.MaximumSpeed))<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>Catch</span> exc <span style='color:blue'>As</span>
Exception<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>WriteTrace(exc.ToString())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span><span style='color:blue'>End</span> <span style='color:blue'>Try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' This gets called whenever your animal is being saved --
either the game is closing<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' or you are being teleported.<span
style='mso-spacerun:yes'>&nbsp; </span>Store anything you want to remember when
you are<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' instantiated again in the stream.<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Public</span> <span style='color:blue'>Overloads</span> <span
style='color:blue'>Overrides</span> <span style='color:blue'>Sub</span>
SerializeAnimal(<span style='color:blue'>ByVal</span> m <span style='color:
blue'>As</span> MemoryStream)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' This gets called when you are instantiated again after
being saved.<span style='mso-spacerun:yes'>&nbsp; </span>You get a <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>' chance to pull out any information that you put into the
stream when you were saved<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>Public</span> <span style='color:blue'>Overloads</span> <span
style='color:blue'>Overrides</span> <span style='color:blue'>Sub</span>
DeserializeAnimal(<span style='color:blue'>ByVal</span> m <span
style='color:blue'>As</span> MemoryStream)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>End</span> <span style='color:blue'>Sub<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:blue'>End</span>
<span style='color:blue'>Class<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>End</span><span
style='font-size:10.0pt;font-family:"Courier New"'> <span style='color:blue'>Namespace<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal><o:p>&nbsp;</o:p></p>

</div>


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
