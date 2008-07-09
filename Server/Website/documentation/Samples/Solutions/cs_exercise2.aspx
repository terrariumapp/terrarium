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
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.Collections;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.Diagnostics;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.Drawing;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> System.IO;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>using</span><span
style='font-size:10.0pt;font-family:"Courier New"'> OrganismBase;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'>[assembly:
OrganismClass(&quot;Exercise2.MyAnimal&quot;)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'>[assembly:
AuthorInformation(&quot;Your Name&quot;, &quot;someone@microsoft.com&quot;)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New";color:blue'>namespace</span><span
style='font-size:10.0pt;font-family:"Courier New"'> Exercise2<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[Carnivore(<span style='color:blue'>false</span>)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[MatureSize(28)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[AnimalSkin(AnimalSkinFamily.Inchworm)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[MarkingColor(KnownColor.Yellow)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[MaximumEnergyPoints(20)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[EatingSpeedPointsAttribute(0)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[AttackDamagePointsAttribute(12)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[DefendDamagePointsAttribute(12)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[MaximumSpeedPointsAttribute(16)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[CamouflagePointsAttribute(10)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>[EyesightPointsAttribute(20)]<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style='color:blue'>public</span>
<span style='color:blue'>class</span> MyAnimal : Animal<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>PlantState
targetPlant = <span style='color:blue'>null</span>;<span
style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp; </span><span style='color:green'>//
The current plant we're going after<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>protected</span> <span style='color:blue'>override</span> <span
style='color:blue'>void</span> Initialize()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='mso-tab-count:1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// TODO: Add initialization logic here<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>Idle
+= <span style='color:blue'>new</span> IdleEventHandler(MyAnimal_Idle); <o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>Load
+= <span style='color:blue'>new</span> LoadEventHandler(MyAnimal_Load);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>Attacked
+= <span style='color:blue'>new</span> AttackedEventHandler(MyAnimal_Attacked);
<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// First event fired on an organism each turn <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>void</span> MyAnimal_Load(<span style='color:blue'>object</span>
sender, LoadEventArgs e)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (targetPlant != <span style='color:blue'>null</span>)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// See if our target plant still exists (it may have died) <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// LookFor returns null if it isn't found <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>targetPlant
= (PlantState)LookFor(targetPlant);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (targetPlant == <span style='color:blue'>null</span>)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// WriteTrace is the best way to debug your creatures. <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Target
plant disappeared.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>catch</span> (Exception exc)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(exc.ToString());<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// Fired if we're being attacked <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>void</span> MyAnimal_Attacked(<span style='color:blue'>object</span>
sender, AttackedEventArgs e)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (e.Attacker.IsAlive)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>AnimalState
TheAttacker = e.Attacker;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginDefending(TheAttacker);
<span style='color:green'>//defend against the attacker <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Run
away to some random point&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>int</span> X = OrganismRandom.Next(0, WorldWidth - 1);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>int</span> Y = OrganismRandom.Next(0, WorldHeight - 1);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginMoving(<span
style='color:blue'>new</span> MovementVector(<span style='color:blue'>new</span>
Point(X, Y), 10));<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// Fired after all other events are fired during a turn <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>void</span> MyAnimal_Idle(<span style='color:blue'>object</span>
sender, IdleEventArgs e)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// Reproduce as often as possible <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (CanReproduce)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginReproduction(<span
style='color:blue'>null</span>);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// If we can eat and we have a target plant, eat <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (CanEat)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Hungry.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (!IsEating)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Not
eating: Have target plant?&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (targetPlant != <span style='color:blue'>null</span>)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Yes,
Have target plant already.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (WithinEatingRange(targetPlant))<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Within
Range, Start eating.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginEating(targetPlant);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (IsMoving)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Stop
while eating.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>StopMoving();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (!IsMoving)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Move
to Target Plant&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginMoving(<span
style='color:blue'>new</span> MovementVector(targetPlant.Position, 2));<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Don't
have target plant.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (!ScanForTargetPlant())<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (!IsMoving)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;No
plant found, so pick a random point and move there&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>int</span> RandomX = OrganismRandom.Next(0, WorldWidth - 1);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>int</span> RandomY = OrganismRandom.Next(0, WorldHeight -
1);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginMoving(<span
style='color:blue'>new</span> MovementVector(<span style='color:blue'>new</span>
Point(RandomX, RandomY), 2));<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
9'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Moving
and Looking...&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
8'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Eating.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (IsMoving)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='mso-tab-count:4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Stop
moving while eating.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>StopMoving();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>else<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(&quot;Full:
do nothing.&quot;);<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (IsMoving)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>StopMoving();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>catch</span> (Exception exc)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(exc.ToString());<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// Looks for target plants, and starts moving towards the
first one it finds <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>bool</span> ScanForTargetPlant()<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>try<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>ArrayList
foundCreatures = Scan();<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (foundCreatures.Count &gt; 0)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// Always move after closest plant or defend closest
creature if there is one <o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>foreach</span> (OrganismState organismState <span
style='color:blue'>in</span> foundCreatures)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>if</span> (organismState <span style='color:blue'>is</span>
PlantState)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>targetPlant
= (PlantState)organismState;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>BeginMoving(<span
style='color:blue'>new</span> MovementVector(organismState.Position, 2));<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
7'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>return</span> <span style='color:blue'>true</span>;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
6'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
5'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>catch</span> (Exception exc)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>WriteTrace(exc.ToString());<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>return</span> <span style='color:blue'>false</span>;<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>public</span> <span style='color:blue'>override</span> <span
style='color:blue'>void</span> SerializeAnimal(MemoryStream stream)<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='mso-tab-count:1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// TODO: Add serialization logic here<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><o:p>&nbsp;</o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:blue'>public</span> <span style='color:blue'>override</span> <span
style='color:blue'>void</span> DeserializeAnimal( MemoryStream stream )<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>{<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
3'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span
style='color:green'>// TODO: Add deserialization logic here<o:p></o:p></span></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
2'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal style='mso-layout-grid-align:none;text-autospace:none'><span
style='font-size:10.0pt;font-family:"Courier New"'><span style='mso-tab-count:
1'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span>}<o:p></o:p></span></p>

<p class=MsoNormal><span style='font-size:10.0pt;font-family:"Courier New"'>}</span></p>

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
