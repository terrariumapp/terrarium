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
<style>
<!--
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
h1
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	page-break-after:avoid;
	font-size:16.0pt;
	font-family:Arial;
	font-weight:bold;}
h2
	{margin-top:14.0pt;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:0in;
	margin-bottom:.0001pt;
	line-height:14.0pt;
	page-break-after:avoid;
	font-size:14.0pt;
	font-family:Arial;
	font-weight:bold;}
h3
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	page-break-after:avoid;
	font-size:11.0pt;
	font-family:Arial;
	font-weight:bold;}
h4
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	page-break-after:avoid;
	font-size:11.0pt;
	font-family:Arial;
	font-weight:bold;}
h5
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	font-size:13.0pt;
	font-family:Arial;
	font-weight:bold;
	font-style:italic;}
h6
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;
	font-weight:bold;}
p.MsoToc1, li.MsoToc1, div.MsoToc1
	{margin-top:14.0pt;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:0in;
	margin-bottom:.0001pt;
	line-height:14.0pt;
	font-size:10.0pt;
	font-family:Arial;
	text-transform:uppercase;
	font-weight:bold;}
p.MsoToc2, li.MsoToc2, div.MsoToc2
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:12.0pt;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:Arial;}
p.MsoToc3, li.MsoToc3, div.MsoToc3
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:24.0pt;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:Arial;}
p.MsoToc4, li.MsoToc4, div.MsoToc4
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.5in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoToc5, li.MsoToc5, div.MsoToc5
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:48.0pt;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoToc6, li.MsoToc6, div.MsoToc6
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:60.0pt;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoToc7, li.MsoToc7, div.MsoToc7
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:1.0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoToc8, li.MsoToc8, div.MsoToc8
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:84.0pt;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoToc9, li.MsoToc9, div.MsoToc9
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:96.0pt;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoFootnoteText, li.MsoFootnoteText, div.MsoFootnoteText
	{margin-top:3.0pt;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:9.0pt;
	font-family:"Arial Narrow";}
p.MsoCommentText, li.MsoCommentText, div.MsoCommentText
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:Arial;}
p.MsoHeader, li.MsoHeader, div.MsoHeader
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoFooter, li.MsoFooter, div.MsoFooter
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoCaption, li.MsoCaption, div.MsoCaption
	{margin-top:6.0pt;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:10.0pt;
	font-family:Arial;
	font-weight:bold;}
span.MsoFootnoteReference
	{vertical-align:super;}
p.MsoBodyText, li.MsoBodyText, div.MsoBodyText
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoBodyTextIndent, li.MsoBodyTextIndent, div.MsoBodyTextIndent
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.25in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoBodyTextIndent2, li.MsoBodyTextIndent2, div.MsoBodyTextIndent2
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.75in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:"Comic Sans MS";
	font-weight:bold;
	font-style:italic;}
p.MsoDocumentMap, li.MsoDocumentMap, div.MsoDocumentMap
	{margin:0in;
	margin-bottom:.0001pt;
	background:navy;
	font-size:11.0pt;
	font-family:Tahoma;}
p
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:8.0pt;
	font-family:Tahoma;}
p.Body-noindent, li.Body-noindent, div.Body-noindent
	{margin-top:6.0pt;
	margin-right:-.7pt;
	margin-bottom:0in;
	margin-left:0in;
	margin-bottom:.0001pt;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.Bullet1, li.Bullet1, div.Bullet1
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.25in;
	margin-bottom:.0001pt;
	text-indent:-.25in;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.Aftergraphicortable, li.Aftergraphicortable, div.Aftergraphicortable
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.25in;
	margin-bottom:.0001pt;
	font-size:8.0pt;
	font-family:Arial;}
p.Bullet3, li.Bullet3, div.Bullet3
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:14.0pt;
	margin-left:45.0pt;
	text-indent:-27.0pt;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.Bodynoindent, li.Bodynoindent, div.Bodynoindent
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	line-height:-116%;
	font-size:9.0pt;
	font-family:Arial;}
p.Number, li.Number, div.Number
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.15in;
	margin-bottom:.0001pt;
	text-indent:-.15in;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.NumberedList, li.NumberedList, div.NumberedList
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:99.0pt;
	margin-bottom:.0001pt;
	text-indent:-.25in;
	line-height:14.0pt;
	font-size:10.0pt;
	font-family:Arial;}
p.Note, li.Note, div.Note
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	line-height:-116%;
	font-size:10.0pt;
	font-family:Arial;
	font-style:italic;}
p.Bullet4, li.Bullet4, div.Bullet4
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.5in;
	margin-bottom:.0001pt;
	text-indent:-.25in;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
span.H1Char
	{font-family:Arial;
	font-weight:bold;}
p.Bodyindent, li.Bodyindent, div.Bodyindent
	{margin-top:0in;
	margin-right:-.7pt;
	margin-bottom:6.0pt;
	margin-left:.15in;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.Numberedstep, li.Numberedstep, div.Numberedstep
	{margin-top:0in;
	margin-right:-.7pt;
	margin-bottom:6.0pt;
	margin-left:0in;
	line-height:14.0pt;
	font-size:9.5pt;
	font-family:Arial;}
p.Picture2Med, li.Picture2Med, div.Picture2Med
	{margin-top:6.0pt;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.25in;
	page-break-after:avoid;
	font-size:10.0pt;
	font-family:"Century Schoolbook";}
p.ProductDescriptor, li.ProductDescriptor, div.ProductDescriptor
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:52.0pt;
	margin-left:.55in;
	line-height:10.0pt;
	font-size:10.0pt;
	font-family:Arial;
	font-style:italic;}
p.Legalese-Space, li.Legalese-Space, div.Legalese-Space
	{margin-top:271.5pt;
	margin-right:0in;
	margin-bottom:3.5pt;
	margin-left:188.4pt;
	line-height:7.0pt;
	font-size:6.5pt;
	font-family:Arial;
	font-style:italic;}
p.Legalese, li.Legalese, div.Legalese
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:3.5pt;
	margin-left:188.4pt;
	line-height:7.0pt;
	font-size:6.5pt;
	font-family:Arial;
	font-style:italic;}
p.Table, li.Table, div.Table
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:9.0pt;
	font-family:Arial;}
p.tablenormal, li.tablenormal, div.tablenormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	font-size:9.0pt;
	font-family:Verdana;}
p.Exercisesteps, li.Exercisesteps, div.Exercisesteps
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.75in;
	text-indent:-.5in;
	font-size:11.0pt;
	font-family:Arial;}
p.ExerciseScreenShot, li.ExerciseScreenShot, div.ExerciseScreenShot
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:12.0pt;
	margin-left:0in;
	text-align:center;
	font-size:11.0pt;
	font-family:Arial;}
p.LabTasklist, li.LabTasklist, div.LabTasklist
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.25in;
	margin-bottom:.0001pt;
	text-indent:-.25in;
	line-height:14.0pt;
	font-size:8.0pt;
	font-family:Arial;}
p.LabTablecolhdr, li.LabTablecolhdr, div.LabTablecolhdr
	{margin-top:6.0pt;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	text-align:center;
	font-size:10.0pt;
	font-family:Arial;
	font-weight:bold;}
p.ExerciseIntro, li.ExerciseIntro, div.ExerciseIntro
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.LabTableinfo, li.LabTableinfo, div.LabTableinfo
	{margin-top:3.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.LabTableending, li.LabTableending, div.LabTableending
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:0in;
	font-size:8.0pt;
	font-family:Arial;}
p.LabNumandTitle, li.LabNumandTitle, div.LabNumandTitle
	{margin-top:12.0pt;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:0in;
	page-break-before:always;
	page-break-after:avoid;
	font-size:16.0pt;
	font-family:Arial;
	font-weight:bold;}
p.ExerciseafterlaststepNumOFF, li.ExerciseafterlaststepNumOFF, div.ExerciseafterlaststepNumOFF
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:6.0pt;
	font-family:Arial;}
p.ExerciseNote, li.ExerciseNote, div.ExerciseNote
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.75in;
	font-size:11.0pt;
	font-family:Arial;
	font-weight:bold;}
p.Code, li.Code, div.Code
	{margin:0in;
	margin-bottom:.0001pt;
	background:#E6E6E6;
	font-size:9.0pt;
	font-family:"Courier New";}
p.ExerciseCode, li.ExerciseCode, div.ExerciseCode
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:1.0in;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:Courier;}
p.StyleCode11ptBold, li.StyleCode11ptBold, div.StyleCode11ptBold
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:.4in;
	margin-bottom:.0001pt;
	background:#E6E6E6;
	font-size:11.0pt;
	font-family:"Courier New";
	font-weight:bold;}
p.Steps, li.Steps, div.Steps
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:3.0pt;
	margin-left:.5in;
	text-indent:-.25in;
	font-size:9.0pt;
	font-family:Verdana;}
span.CodeChar
	{font-family:"Courier New";}
span.NoteChar
	{font-family:Verdana;
	font-style:italic;}
p.StyleStepsArial11ptBold, li.StyleStepsArial11ptBold, div.StyleStepsArial11ptBold
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.5in;
	text-indent:-.25in;
	font-size:11.0pt;
	font-family:Arial;
	font-weight:bold;}
p.StyleStepsArial11ptCharCharCharChar, li.StyleStepsArial11ptCharCharCharChar, div.StyleStepsArial11ptCharCharCharChar
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.5in;
	text-indent:-.25in;
	font-size:11.0pt;
	font-family:Arial;}
span.tablenormalChar
	{font-family:Verdana;}
span.StepsChar
	{font-family:Verdana;}
span.StyleStepsArial11ptCharCharCharCharChar
	{font-family:Arial;}
p.StyleStepsArial11pt1, li.StyleStepsArial11pt1, div.StyleStepsArial11pt1
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:6.0pt;
	margin-left:.5in;
	text-indent:-.25in;
	font-size:11.0pt;
	font-family:Arial;}
span.StyleStepsArial11pt1Char
	{font-family:Arial;}
p.StyleBody-noindent11pt, li.StyleBody-noindent11pt, div.StyleBody-noindent11pt
	{margin-top:0in;
	margin-right:-.7pt;
	margin-bottom:6.0pt;
	margin-left:0in;
	line-height:14.0pt;
	font-size:11.0pt;
	font-family:Arial;}
p.TOc3, li.TOc3, div.TOc3
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:Arial;}
p.HOLDescription, li.HOLDescription, div.HOLDescription
	{margin:0in;
	margin-bottom:.0001pt;
	page-break-after:avoid;
	border:none;
	padding:0in;
	font-size:12.0pt;
	font-family:"Times New Roman";
	font-style:italic;}
p.HOLTitle1, li.HOLTitle1, div.HOLTitle1
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:36.0pt;
	font-family:"Arial Black";}
p.HOLTitle2, li.HOLTitle2, div.HOLTitle2
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:12.0pt;
	margin-left:0in;
	page-break-after:avoid;
	font-size:28.0pt;
	font-family:"Arial Narrow";
	font-weight:bold;}
p.lab2thf, li.lab2thf, div.lab2thf
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.lab2th, li.lab2th, div.lab2th
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.lab2tpn, li.lab2tpn, div.lab2tpn
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.lab2tpl, li.lab2tpl, div.lab2tpl
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.le, li.le, div.le
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.code0, li.code0, div.code0
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:8.0pt;
	font-family:"Courier New";}
p.lab2lb1, li.lab2lb1, div.lab2lb1
	{margin-right:0in;
	margin-left:0in;
	font-size:11.0pt;
	font-family:Arial;}
p.ButtonLabel, li.ButtonLabel, div.ButtonLabel
	{margin:0in;
	margin-bottom:.0001pt;
	text-align:center;
	font-size:9.0pt;
	font-family:Arial;
	font-weight:bold;}
span.ButtonLabelChar
	{font-family:Arial;
	font-weight:bold;}
span.codeChar0
	{font-family:"Courier New";}
span.CodeChar1
	{font-family:"Courier New";}
 /* Page Definitions */
 @page Section1
	{size:8.5in 11.0in;
	margin:1.25in .75in .5in .75in;}
div.Section1
	{page:Section1;}
 /* List Definitions */
 ol
	{margin-bottom:0in;}
ul
	{margin-bottom:0in;}
-->
</style>
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

<h1><a name="_Toc70324472">Lab 1: Creating a Herbivore</a></h1>

<h2><a name="_Toc70324473">Lab Objective</a>  </h2>

<p class=Body-noindent><span style='font-size:11.0pt'>The objective of this lab
is to build a fully functional organism for Terrarium.  You will begin by
creating a simple organism and then add more advanced functionality.  Once you
are happy with the way it is performing, you can introduce it to the Terrarium Ecosystem
and have it compete against other attendees’ organisms.</span></p>

<h2><a name="_Toc70324474">Exercise 1 – Create a Simple Herbivore</a></h2>

<h2><a name="_Toc70324475">Create a Simple Herbivore</a> </h2>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>An animal is simply a class you write that derives from a
special class (Animal) exposed by the Terrarium.&nbsp; The code in this class
gets executed by the game and controls the behavior and characteristics of your
animal in the game. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>In this exercise, you will create a simple Herbivore Animal
in Visual Basic. You will use Microsoft Visual Studio .NET 2003 to create,
compile, and run this lab. </p>

<p class=MsoNormal>&nbsp;</p>

<p><b><i><span style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>Please
substitute your name wherever you see the text &lt;YOUR NAME&gt; below since
each animal introduced into the .NET Terrarium must have a unique name.</span></i></b><span
style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'> </span></p>

<p><span style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>&nbsp;</span></p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324476"></a><a
name="_Toc69891990"><span style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>1.1<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span></span>Create a new Terrarium
Animal project in Visual Studio</a> </h3>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-size:10.0pt;font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>Click <b>Start</b>-&gt;<b>All</b> <b>Programs</b>-&gt;<b>Microsoft</b>
<b>Visual</b> <b>Studio</b> <b>.NET 2003</b>-&gt;<b>Microsoft</b> <b>Visual</b>
<b>Studio .NET 2003</b></p>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>Click <b>File-&gt;New-&gt;Project</b> </p>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>In the Project Types pane, click <b>Visual C# Projects</b> </p>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>In the Templates pane, click <b>Terrarium Animal</b> </p>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>Type <b>&lt;YOUR NAME&gt;</b> in the Name field.</p>

<p class=MsoNormal style='margin-left:45.0pt;text-indent:-9.0pt'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;
</span></span>Click <b>OK</b></p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324477"><span
style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>1.2<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span></span>Add your name and
email address to your animal.</a>  <br>
<br>
</h3>

<p class=MsoNormal>Update the AuthorInformation assembly attribute with your
name and email address.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>&lt;Assembly: AuthorInformation(&quot;Your Name&quot;,
&quot;someone@microsoft.com&quot;)&gt;</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324478">1.3<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Set the CarnivoreAttribute
to false.</a>  </h3>

<p class=MsoNormal>The Carnivore attribute indicates that your animal is an
Herbivore.  Set the CarnivoreAttribute to false. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code> Carnivore(<span style='color:blue'>False</span>), _</p>

<p class=lab2tpn><span style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>&nbsp;</span></p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324479">1.4<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Select an AnimalSkin for
your animal.</a>  </h3>

<p class=MsoNormal>The AnimalSkin attribute defines what kind of creature your
animal looks like.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>AnimalSkin(AnimalSkinFamily.Beetle), _</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324480"><span
style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>1.5<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span></span>Assign values to each
of your animal characteristic properties.  The sum of the property values must
not exceed 100.</a></h3>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>MaximumEnergyPoints(20), _</p>

<p class=Code>EatingSpeedPointsAttribute(0), _</p>

<p class=Code>AttackDamagePointsAttribute(12), _</p>

<p class=Code>DefendDamagePointsAttribute(12), _</p>

<p class=Code>MaximumSpeedPointsAttribute(16), _</p>

<p class=Code>CamouflagePointsAttribute(10), _</p>

<p class=Code>EyesightPointsAttribute(20) _</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324481">1.6<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Add the targetPlant field to
the MyAnimal class.</a>  </h3>

<p class=MsoNormal>Because your animal is a Herbivore, it eats plants. 
targetPlant stores the reference to the plant your herbivore wants to eat. 
Place this above the Initialize method.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>' The current plant we're going after<span style='color:blue'> </span></p>

<p class=Code><span style='color:blue'>Dim</span> targetPlant <span
style='color:blue'>As</span> PlantState = <span style='color:blue'>Nothing</span> 
</p>

<p class=Code>  </p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324482">1.7<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Subscribe to the IdleEvent.</a> 
</h3>

<p class=MsoNormal>The IdleEvent is the last event fired on every turn.  This
line tells the game engine to call the MyAnimal_Idle method each time the
IdleEvent is fired.  Add the following line to the Initialize method.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code><span style='color:blue'>AddHandler</span> Idle, <span
style='color:blue'>AddressOf</span> MyAnimal_Idle</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324483">1.8<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Add the IdleEvent handler.</a> 
</h3>

<p class=MsoNormal>This method provides most of the logic for your simple
herbivore.  First, if your animal is ready to reproduce, it begins a
reproduction cycle.  Next it attempts to eat.  If it already has a targetPlant,
it walks to the targetPlant and begins eating.  If it doesn’t have a target
plant, it tries to find one by scanning its surroundings.  Add the following
two methods to the MyAnimal class.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>' Fired after all other events are fired during a turn </p>

<p class=Code><span style='color:blue'>Sub</span> MyAnimal_Idle(<span
style='color:blue'>ByVal</span> sender <span style='color:blue'>As</span> <span
style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e <span
style='color:blue'>As</span> IdleEventArgs)</p>

<p class=Code>    <span style='color:blue'>Try</span></p>

<p class=Code>        ' Reproduce as often as possible </p>

<p class=Code>        <span style='color:blue'>If</span> (CanReproduce) <span
style='color:blue'>Then</span></p>

<p class=Code>            BeginReproduction(<span style='color:blue'>Nothing</span>)</p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>&nbsp;</p>

<p class=Code>        ' If we can eat and we have a target plant, eat </p>

<p class=Code>        <span style='color:blue'>If</span> (CanEat) <span
style='color:blue'>Then</span></p>

<p class=Code>            WriteTrace(&quot;Hungry.&quot;)</p>

<p class=Code>            <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (IsEating) <span style='color:blue'>Then</span></p>

<p class=Code>                WriteTrace(&quot;Not eating: Have target
plant?&quot;)</p>

<p class=Code>                <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (targetPlant <span style='color:blue'>Is</span> <span
style='color:blue'>Nothing</span>) <span style='color:blue'>Then</span></p>

<p class=Code>                    WriteTrace(&quot;Yes, Have target plant
already.&quot;)</p>

<p class=Code>                    <span style='color:blue'>If</span>
(WithinEatingRange(targetPlant)) <span style='color:blue'>Then</span></p>

<p class=Code>                        WriteTrace(&quot;Within Range, Start
eating.&quot;)</p>

<p class=Code>                        BeginEating(targetPlant)</p>

<p class=Code>                        <span style='color:blue'>If</span>
(IsMoving) <span style='color:blue'>Then</span></p>

<p class=Code>                            WriteTrace(&quot;Stop while
eating.&quot;)</p>

<p class=Code>                            StopMoving()</p>

<p class=Code>                        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                    <span style='color:blue'>Else</span></p>

<p class=Code>                        <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (IsMoving) <span style='color:blue'>Then</span></p>

<p class=Code>                            WriteTrace(&quot;Move to Target
Plant&quot;)</p>

<p class=Code>                            BeginMoving(<span style='color:blue'>New</span>
MovementVector(targetPlant.Position, 2))</p>

<p class=Code>                        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                    <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                <span style='color:blue'>Else</span></p>

<p class=Code>                    WriteTrace(&quot;Don't have target
plant.&quot;)</p>

<p class=Code>                    <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (ScanForTargetPlant()) <span style='color:blue'>Then</span></p>

<p class=Code>                        <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (IsMoving) <span style='color:blue'>Then</span></p>

<p class=Code>                            WriteTrace(&quot;No plant found, so
pick a random point and move there&quot;)</p>

<p class=Code>                            <span style='color:blue'>Dim</span>
RandomX <span style='color:blue'>As</span> <span style='color:blue'>Integer</span>
= OrganismRandom.Next(0, WorldWidth - 1)</p>

<p class=Code>                            <span style='color:blue'>Dim</span>
RandomY <span style='color:blue'>As</span> <span style='color:blue'>Integer</span>
= OrganismRandom.Next(0, WorldHeight - 1)</p>

<p class=Code>                            BeginMoving(<span style='color:blue'>New</span>
MovementVector(<span style='color:blue'>New</span> Point(RandomX, RandomY), 2))</p>

<p class=Code>                        <span style='color:blue'>Else</span></p>

<p class=Code>                            WriteTrace(&quot;Moving and
Looking...&quot;)</p>

<p class=Code>                        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                    <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>            <span style='color:blue'>Else</span></p>

<p class=Code>                WriteTrace(&quot;Eating.&quot;)</p>

<p class=Code>                <span style='color:blue'>If</span> (IsMoving) <span
style='color:blue'>Then</span></p>

<p class=Code>                    WriteTrace(&quot;Stop moving while
eating.&quot;)</p>

<p class=Code>                    StopMoving()</p>

<p class=Code>                <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>        <span style='color:blue'>Else</span></p>

<p class=Code>            WriteTrace(&quot;Full: do nothing.&quot;)</p>

<p class=Code>            <span style='color:blue'>If</span> (IsMoving) <span
style='color:blue'>Then</span></p>

<p class=Code>                StopMoving()</p>

<p class=Code>            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>    <span style='color:blue'>Catch</span> exc <span
style='color:blue'>As</span> Exception</p>

<p class=Code>        WriteTrace(exc.ToString())</p>

<p class=Code>    End Try</p>

<p class=Code>End Sub</p>

<p class=Code>&nbsp;</p>

<p class=Code>' Looks for target plants, and starts moving towards </p>

<p class=Code>' the first one it finds </p>

<p class=Code><span style='color:blue'>Function</span> ScanForTargetPlant() <span
style='color:blue'>As</span> <span style='color:blue'>Boolean</span></p>

<p class=Code>    <span style='color:blue'>Try</span></p>

<p class=Code>        <span style='color:blue'>Dim</span> foundCreatures <span
style='color:blue'>As</span> System.Collections.ArrayList = Scan()</p>

<p class=Code>        <span style='color:blue'>If</span> (foundCreatures.Count
&gt; 0) <span style='color:blue'>Then</span></p>

<p class=Code>            <span style='color:blue'>Dim</span> orgState <span
style='color:blue'>As</span> OrganismState</p>

<p class=Code>            ' Always move after closest plant </p>

<p class=Code>            ' or defend closest creature if there is one </p>

<p class=Code>            <span style='color:blue'>For</span> <span
style='color:blue'>Each</span> orgState <span style='color:blue'>In</span>
foundCreatures</p>

<p class=Code>                <span style='color:blue'>If</span> (<span
style='color:blue'>TypeOf</span> orgState <span style='color:blue'>Is</span>
PlantState) <span style='color:blue'>Then</span></p>

<p class=Code>                    targetPlant = <span style='color:blue'>CType</span>(orgState,
PlantState)</p>

<p class=Code>                    BeginMoving(<span style='color:blue'>New</span>
MovementVector(orgState.Position, 2))</p>

<p class=Code>                    <span style='color:blue'>Return</span> <span
style='color:blue'>True</span></p>

<p class=Code>                <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>            <span style='color:blue'>Next</span></p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>    <span style='color:blue'>Catch</span> exc <span
style='color:blue'>As</span> Exception</p>

<p class=Code>        WriteTrace(exc.ToString())</p>

<p class=Code>    End Try</p>

<p class=Code>    Return False</p>

<p class=Code>End Function</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324484">1.9<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Subscribe to the LoadEvent.</a> 
</h3>

<p class=MsoNormal>The LoadEvent is the first event fired on every turn.  This
line calls the MyAnimal_Load method (defined in Task 1.7) each time the
LoadEvent is fired.  Add the following line to the Initialize method.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code><span style='color:blue'>AddHandler</span> Load, <span
style='color:blue'>AddressOf</span> MyAnimal_Load</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324485">1.10<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>Add the LoadEvent handler.</a> </h3>

<p class=MsoNormal>In the LoadEvent, your animal verifies the targetPlant still
exists (it may have been teleported or eaten).  Add the following method to the
MyAnimal class.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code>    ' First event fired on an organism each turn </p>

<p class=Code>    <span style='color:blue'>Sub</span> MyAnimal_Load(<span
style='color:blue'>ByVal</span> sender <span style='color:blue'>As</span> <span
style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e <span
style='color:blue'>As</span> LoadEventArgs)</p>

<p class=Code>        <span style='color:blue'>Try</span></p>

<p class=Code>            <span style='color:blue'>If</span> <span
style='color:blue'>Not</span> (targetPlant <span style='color:blue'>Is</span> <span
style='color:blue'>Nothing</span>) <span style='color:blue'>Then</span></p>

<p class=Code>                ' See if our target plant still exists </p>

<p class=Code>                ' (it may have died) </p>

<p class=Code>                ' LookFor returns null if it isn't found </p>

<p class=Code>                targetPlant = <span style='color:blue'>CType</span>(LookFor(targetPlant),
PlantState)</p>

<p class=Code>                <span style='color:blue'>If</span> (targetPlant <span
style='color:blue'>Is</span> <span style='color:blue'>Nothing</span>) <span
style='color:blue'>Then</span></p>

<p class=Code>&nbsp;</p>

<p class=Code>                    ' WriteTrace is the best way to debug your
creatures. </p>

<p class=Code>                    WriteTrace(&quot;Target plant
disappeared.&quot;)</p>

<p class=Code>                <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>        <span style='color:blue'>Catch</span> exc <span
style='color:blue'>As</span> Exception</p>

<p class=Code>            WriteTrace(exc.ToString())</p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>Try</span></p>

<p class=Code>    <span style='color:blue'>End</span> <span style='color:blue'>Sub</span></p>

<p class=lab2tpn><span style='font-size:10.0pt;font-family:"Microsoft Sans Serif"'>&nbsp;</span></p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324486">1.11<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>Build the animal DLL.</a>  </h3>

<p class=MsoNormal>Build the animal DLL by selecting <b>Build | Make Solution</b>
</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324487"></a><a
name="_Toc69892001">1.12<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span>Introduce the Herbivore into the Terrarium in Terrarium Mode.</a>  </h3>

<p class=MsoNormal>Terrarium supports two game modes, Terrarium mode and
Ecosystem mode.  In Ecosystem mode, your animal competes against animals
submitted by other developers.  </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Terrarium mode is the Terrarium test mode.  In Terrarium
mode you can test your animal in a controlled environment.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Start the Terrarium by clicking <b>Start-&gt;Programs-&gt;.NET
Terrarium 1.2-&gt;.NET Terrarium 1.2.  </b>The Terrarium 1.2 client will open
in Ecosystem mode.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>There are two modes that Terrarium 1.2 can run in.  The
default is Ecosystem mode.  This is where everyone who is running the client in
this mode participates together.  The peers will discover each other, teleport
organisms to each other and report their statistics to the server.  This mode
is the more interesting of the two.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>The other mode is called Terrarium mode.  This is an
“offline” mode that is perfect for testing your organisms.  In Terrarium mode,
you are not communicating with other peers, so no teleportation occurs.  You
also don’t report your statistics to the server.  You can save the state of the
Terrarium whenever you like, create a new one to start fresh whenever, or open
an existing one.  You can see how this mode really does aid in creating and
testing your organisms.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>For this exercise, we do not want to introduce the animal
into the Ecosystem since the animal may not be advanced enough to survive long.
Instead, we change into Terrarium mode and this allows you to test your animal
before introducing it to the Ecosystem. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>To switch to Terrarium Mode, click the <b>New Terrarium</b>
button.  Enter <b>Lab01</b> as the name and click <b>Save</b>.  This will
restart the client in Terrarium mode.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'><img width=139 height=34
src="images/tutorial_image001.jpg"></span></span></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>New Terrarium</span></span><br>
<br>
</p>

<p class=MsoNormal>The next step is to add some plants into the Terrarium so
that your Herbivore will have something to eat when it is introduced. To do
that, click on the <b>Add</b> button, then click the <b>Server </b>List button
and you will get a list of several animals that have already been introduced.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image002.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Add Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>For the purpose of this lab, and as is usual in the real
world, there are several plant species you can choose from. Simply select any
of the species of Type “plant” and click <b>OK</b>. This will introduce 10
plants of the type you selected to the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Add several more plants by selecting the plant in the drop
down box and clicking the <b>Insert</b> button so you have a lot of plants in
the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image003.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Insert Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>Finally introduce your animal. To do this, click on <b>Add </b>button
click <b>Browse</b> and browse to the dll you created (<b>&lt;YOUR NAME&gt;</b>.dll).
This dll will be located in the Bin folder of your project.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=lab2tpn>You should see 10 instances of your creature in the Terrarium. 
To add more, you can use the Combo Box and <b>Insert</b> button like you did
with the plants.</p>

<h2><a name="_Toc70324488">Note</a></h2>

<p class=MsoNormal>The base characteristics of your animal&nbsp;can't be
changed when the code is running -- capabilities characteristics like: how fast
it can move, how well it attacks, how large it gets, etc.&nbsp; These are
defined on your animal by applying special attributes to the class you
create.&nbsp; You can see the list of available characteristic attributes in
the <b>Attributes</b> section of the <b>Organism Documentation</b>.&nbsp; Some
characteristics require points to be applied to decide &quot;how much&quot; of
them you get.&nbsp; Each animal has 100 points to divide among these
attributes.&nbsp; So, for example, you could decide that your animal will be
really fast by applying many points to the <span style='font-family:"Courier New"'>MaximumSpeedPointsA</span><span
style='font-family:"Courier New"'>ttribute</span>, but it won't have good
eyesight since you’ll have fewer points left to apply to the <span
style='font-family:"Courier New"'>EyesightPointsA</span><span style='font-family:
"Courier New"'>ttribute</span>. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Once you've defined the characteristics your animal will
have, then you write code that controls the behavior of the animal.&nbsp; Look
at the methods, properties and events on the <b>Animal</b> and <b>Organism</b>
classes (<b>Animal</b> derives from <b>Organism</b>) in the <b>Organism
Documentation</b> to see what you can do in your code.&nbsp; These methods
define all the actions your animal can take.&nbsp; Note that many of the method
names start with &quot;Begin...&quot;.&nbsp; These methods are
asynchronous.&nbsp; When you call them, code execution will return immediately
from the method.&nbsp; When the action completes, the corresponding event will
fire on your organism to tell you it is done. </p>

<h2><a name="_Toc70324489">Lab Summary</a></h2>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>In this
lab, you’ve created a simple Herbivore, tested it in your local Terrarium.</span></p>

<b><span style='font-size:14.0pt;font-family:Arial'><br clear=all
style='page-break-before:always'>
</span></b>

<h2><a name="_Toc70324490">Exercise 2 – Handling an Attacked Event</a></h2>

<p class=Body-noindent>&nbsp;</p>

<p class=MsoNormal>In this hands-on lab you will use the event model of the
Terrarium to handle an Attacked event.  An Attacked event is fired when you are
attacked by another animal. This is just one of the many events that you can
handle, to see the full list, refer to the <b>Organism Documentation</b>.</p>

<p class=MsoNormal> </p>

<p class=MsoNormal>Please substitute your name wherever you see the text <b>&lt;YOUR
NAME&gt;</b> below since each animal introduced into the .NET Terrarium must
have a unique name. </p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324491"></a><a
name="_Toc69892005">2.1<span style='font:7.0pt "Times New Roman"'>&nbsp; </span>Open
the animal you created in Exercise 1 (Simple Herbivore)</a> </h3>

<ul style='margin-top:0in' type=disc>
 <li class=MsoNormal>Click <b>Start</b>-&gt;<b>All</b> <b>Programs</b>-&gt;<b>Microsoft</b>
     <b>Visual</b> <b>Studio</b> <b>.NET 2003</b>-&gt;<b>Microsoft</b> <b>Visual</b>
     <b>Studio</b>  <b>.NET 2003</b></li>
 <li class=MsoNormal>Select <b>File</b> | <b>Open</b> | <b>Project</b>.&nbsp;
     This will open the standard file open dialogue. </li>
 <li class=MsoNormal>Browse to the folder where you saved the project in
     Exercise 1. </li>
 <li class=MsoNormal>Click the file named <b>&lt;YOUR NAME&gt;</b> and then
     click <b>Open</b>.</li>
 <li class=MsoNormal>If the source file does not open in the main window,
     double-click the MyAnimal.cs icon from the Solution Explorer window. The
     source then opens in the main window</li>
</ul>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324492">2.2<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Subscribe to the
AttackedEvent.</a>  </h3>

<p class=MsoNormal>The AttackedEvent is fired when your animal is attacked by
another animal.  This line tells the game <span style='font-size:10.0pt;
font-family:"Microsoft Sans Serif"'>engine to call the MyAnimal_Attacked method
each time the AttackedEvent is fired.  </span>Add the following line to the
Initialize method: </p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code><span style='color:blue'>AddHandler</span> Attacked, <span
style='color:blue'>AddressOf</span> MyAnimal_Attacked</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324493">2.3<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Add the AttackedEvent
handler.</a>  </h3>

<p class=MsoNormal>When your animal is attacked, it first tries to defend
itself against its attacker.  It then tries to run away from the attacker by
moving to a random location on the game board.  Add the following method to the
MyAnimal class. <br>
<br>
</p>

<p class=Code><span style='color:green'>    ' Fired if we're being attacked </span></p>

<p class=Code>    <span style='color:blue'>Sub</span> MyAnimal_Attacked(<span
style='color:blue'>ByVal</span> sender <span style='color:blue'>As</span> <span
style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e <span
style='color:blue'>As</span> AttackedEventArgs)</p>

<p class=Code>        <span style='color:blue'>If</span> (e.Attacker.IsAlive) <span
style='color:blue'>Then</span></p>

<p class=Code>            <span style='color:blue'>Dim</span> TheAttacker <span
style='color:blue'>As</span> AnimalState = e.Attacker</p>

<p class=Code>            BeginDefending(TheAttacker) 'defend against the
attacker </p>

<p class=Code>&nbsp;</p>

<p class=Code>            WriteTrace(&quot;Run away to some random point&quot;)</p>

<p class=Code>            <span style='color:blue'>Dim</span> x <span
style='color:blue'>As</span> <span style='color:blue'>Integer</span> =
OrganismRandom.Next(0, WorldWidth - 1)</p>

<p class=Code>            <span style='color:blue'>Dim</span> y <span
style='color:blue'>As</span> <span style='color:blue'>Integer</span> =
OrganismRandom.Next(0, WorldHeight - 1)</p>

<p class=Code>            BeginMoving(<span style='color:blue'>New</span>
MovementVector(<span style='color:blue'>New</span> Point(x, y), 10))</p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>    <span style='color:blue'>End</span> <span style='color:blue'>Sub</span></p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324494">2.4<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Rename and Build the animal
DLL.</a></h3>

<ul style='margin-top:0in' type=disc>
 <li class=MsoNormal>Select <b>Project</b> <b>Properties</b>.  This will open
     the Project Properties dialog.</li>
 <li class=MsoNormal>In <b>Common Properties</b> | <b>General</b>, change the <b>Assembly
     Name</b> field to <b>&lt;YOUR NAME&gt;_Ex2</b> and click <b>OK</b>.</li>
</ul>

<p class=lab2tpn style='margin-left:.5in;text-indent:-.25in'><span
style='font-size:10.0pt;font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>Build the animal DLL from within the IDE by selecting <b>Build | Make
Solution</b></p>

<h3><a name="_Toc70324495"></a><a name="_Toc69892009">2.5 Introduce the
Herbivore into the Terrarium in Terrarium Mode.</a>  </h3>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Terrarium supports two game modes, Terrarium mode and
Ecosystem mode.  </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>In Ecosystem mode, your animal competes against animals
submitted by other developers.  </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Terrarium mode is the Terrarium test mode.  In Terrarium
mode you can test your animal in a controlled environment.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>To switch to Terrarium Mode, click the <b>New Terrarium</b>
button.  Enter <b>Lab01</b> as the name and click <b>Save</b>.  This will
restart the client in Terrarium mode.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'><img width=139 height=34
src="images/tutorial_image001.jpg"></span></span></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>New Terrarium</span></span><br>
<br>
</p>

<p class=MsoNormal>The next step is to add some plants into the Terrarium so
that your Herbivore will have something to eat when it is introduced. To do
that, click on the <b>Add</b> button, then click the <b>Server </b>List button
and you will get a list of several animals that have already been introduced.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image002.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Add Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>For the purpose of this lab, and as is usual in the real
world, there are several plant species you can choose from. Simply select any
of the species of Type “plant” and click <b>OK</b>. This will introduce 10
plants of the type you selected to the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Add several more plants by selecting the plant in the drop
down box and clicking the <b>Insert</b> button so you have a lot of plants in
the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image003.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Insert Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>Finally introduce your animal. To do this, click on <b>Add </b>button
click <b>Browse</b> and browse to the dll you created (<b>&lt;YOUR NAME&gt;</b>.dll).
This dll will be located in the Bin folder of your project.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=lab2tpn>You should see 10 instances of your creature in the
Terrarium.  To add more, you can use the Combo Box and <b>Insert</b> button
like you did with the plants.</p>

<h2><a name="_Toc70324496">Lab Summary</a></h2>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>&nbsp;</span></p>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>In this
lab, you’ve modified your Herbivore to be able to detect when it is being
attacked and to respond to this attack. </span></p>

<b><span style='font-size:11.0pt;font-family:Arial'><br clear=all
style='page-break-before:always'>
</span></b>

<h2><a name="_Toc70324497">Exercise 3 – Communication</a></h2>

<p class=Body-noindent>&nbsp;</p>

<p class=MsoNormal>In this hands-on lab you will use the animal’s Antenna
property to communicate with other animals.  The state of the antenna is
visible to all animals.  By changing the value of the antenna your animal can
communicate with the other animals in the Ecosystem.  In this lab, you will
update your animal to signal when another animal is blocking a target plant.  In
response, the blocking animal will move out of the way, giving your animal
access to the target plant.</p>

<p class=MsoNormal> </p>

<p class=MsoNormal>Please substitute your name wherever you see the text <b>&lt;YOUR
NAME&gt;</b> below since each animal introduced into the .NET Terrarium must
have a unique name. </p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324498"></a><a
name="_Toc69892012">3.1<span style='font:7.0pt "Times New Roman"'>&nbsp; </span>Open
the animal you created in Exercise 1 (Simple Herbivore)</a> </h3>

<p class=MsoNormal style='margin-left:.5in;text-indent:-.25in'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>Click <b>Start</b>-&gt;<b>All</b> <b>Programs</b>-&gt;<b>Microsoft</b>
<b>Visual</b> <b>Studio</b> <b>.NET 2003</b>-&gt;<b>Microsoft</b> <b>Visual</b>
<b>Studio</b>  <b>.NET 2003</b></p>

<p class=MsoNormal style='margin-left:.5in;text-indent:-.25in'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>Select <b>File</b> | <b>Open</b> | <b>Project</b>.&nbsp; This
will open the standard file open dialogue. </p>

<p class=MsoNormal style='margin-left:.5in;text-indent:-.25in'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>Browse to the folder where you saved the project in Exercise 2. </p>

<p class=MsoNormal style='margin-left:.5in;text-indent:-.25in'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>Click the file named <b>&lt;YOUR NAME&gt;</b> and then click <b>Open</b>.</p>

<p class=MsoNormal style='margin-left:.5in;text-indent:-.25in'><span
style='font-family:Symbol'>·<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span>If the source file does not open in the main window, double-click
the MyAnimal.cs icon from the Solution Explorer window. The source then opens
in the main window</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324499">3.2<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Subscribe to the
MoveCompletedEvent.</a>  </h3>

<p class=MsoNormal>The MoveCompletedEvent is fired when your animal has stopped
moving.  This line tells the game engine to call the MyAnimal_MoveCompleted
method each time the MoveCompletedEvent is fired.</p>

<p class=MsoNormal>Add the following line to the Initialize method: </p>

<p class=MsoNormal>&nbsp;</p>

<p class=Code><span style='color:blue'>AddHandler</span> MoveCompleted, <span
style='color:blue'>AddressOf</span> MyAnimal_MoveCompleted</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324500">3.3<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Add the MoveCompletedEvent
handler.</a> </h3>

<p class=MsoNormal>There are two reasons an animal will complete its move
event; the animal has reached its destination or its path is blocked.  If the
animal’s path is being blocked, we want to signal the blocking animal to move
out of the way.  The signal we use is the antenna value of 13.  Add the
following method to the MyAnimal class. <br>
<br>
</p>

<p class=Code>    ' Fired when we've finished moving.</p>

<p class=Code>    <span style='color:blue'>Sub</span> MyAnimal_MoveCompleted(<span
style='color:blue'>ByVal</span> sender <span style='color:blue'>As</span> <span
style='color:blue'>Object</span>, <span style='color:blue'>ByVal</span> e <span
style='color:blue'>As</span> MoveCompletedEventArgs)</p>

<p class=Code>        ' Reset the antenna value</p>

<p class=Code>        Antennas.AntennaValue = 0</p>

<p class=Code>&nbsp;</p>

<p class=Code>        ' If we've stopped because something is blocking us...</p>

<p class=Code>        <span style='color:blue'>If</span> (e.Reason =
ReasonForStop.Blocked) <span style='color:blue'>Then</span></p>

<p class=Code>            WriteTrace(&quot;Something's blocking my way.&quot;)</p>

<p class=Code>            <span style='color:blue'>If</span> (<span
style='color:blue'>TypeOf</span> e.BlockingOrganism <span style='color:blue'>Is</span>
AnimalState) <span style='color:blue'>Then</span></p>

<p class=Code>                <span style='color:blue'>Dim</span>
blockingAnimal <span style='color:blue'>As</span> AnimalState = <span
style='color:blue'>CType</span>(e.BlockingOrganism, AnimalState)</p>

<p class=Code>                <span style='color:blue'>If</span>
(blockingAnimal.AnimalSpecies.IsSameSpecies(<span style='color:blue'>Me</span>.Species))
<span style='color:blue'>Then</span></p>

<p class=Code>                    ' Signal to our friend to move out of our
way.</p>

<p class=Code>                    WriteTrace(&quot;One of my friends is
blocking my way.  I'll ask him to move.&quot;)</p>

<p class=Code>                    Antennas.AntennaValue = 13</p>

<p class=Code>                <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>    <span style='color:blue'>End</span> <span style='color:blue'>Sub</span></p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324501">3.4<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Add the logic to move out of
the way when blocking our friends.</a>  </h3>

<p class=MsoNormal>On every turn, we need to look around for animals signaling
they’re being blocked.  The ShouldIMoveForMyFriend method looks for animals
signaling “13.”  If any are found, the animal moves out of the way of the
blocked animal.  Add the following method to the MyAnimal class. <br>
<br>
</p>

<p class=Code>    ' Routine to move out of the way when blocking our friends.</p>

<p class=Code>    <span style='color:blue'>Sub</span> ShouldIMoveForMyFriend()</p>

<p class=Code>        <span style='color:blue'>Try</span></p>

<p class=Code>            <span style='color:blue'>Dim</span> foundAnimals <span
style='color:blue'>As</span> System.Collections.ArrayList = Scan()</p>

<p class=Code>            <span style='color:blue'>If</span>
(foundAnimals.Count &gt; 0) <span style='color:blue'>Then</span></p>

<p class=Code>&nbsp;</p>

<p class=Code>                <span style='color:blue'>Dim</span> orgState <span
style='color:blue'>As</span> OrganismState</p>

<p class=Code>                <span style='color:blue'>For</span> <span
style='color:blue'>Each</span> orgState <span style='color:blue'>In</span>
foundAnimals</p>

<p class=Code>                    <span style='color:blue'>If</span> (TypeOf
orgState Is AnimalState) <span style='color:blue'>Then</span></p>

<p class=Code>                        <span style='color:blue'>Dim</span>
visibleAnimal <span style='color:blue'>As</span> AnimalState = <span
style='color:blue'>CType</span>(orgState, AnimalState)</p>

<p class=Code>                        ' Only move if the animal is one of our
friends (IsSameSpecies).</p>

<p class=Code>                        <span style='color:blue'>If</span>
(visibleAnimal.Species.IsSameSpecies(<span style='color:blue'>Me</span>.Species))
<span style='color:blue'>Then</span></p>

<p class=Code>                            ' If the animal's antenna value is
13, it means they're blocked (see the MoveCompletedEvent method).</p>

<p class=Code>                            <span style='color:blue'>If</span>
(visibleAnimal.Antennas.AntennaValue = 13) <span style='color:blue'>Then</span></p>

<p class=Code>                                ' We're blocking our friend, so
we should move.</p>

<p class=Code>                                WriteTrace(&quot;I'm blocking one
of my friends.  I should move.&quot;)</p>

<p class=Code>&nbsp;</p>

<p class=Code>                                <span style='color:blue'>Dim</span>
newX <span style='color:blue'>As</span> <span style='color:blue'>Integer</span>
= <span style='color:blue'>Me</span>.Position.X - (visibleAnimal.Position.X - <span
style='color:blue'>Me</span>.Position.X)</p>

<p class=Code>                                <span style='color:blue'>Dim</span>
newY <span style='color:blue'>As</span> <span style='color:blue'>Integer</span>
= <span style='color:blue'>Me</span>.Position.Y - (visibleAnimal.Position.Y - <span
style='color:blue'>Me</span>.Position.Y)</p>

<p class=Code>&nbsp;</p>

<p class=Code>                                BeginMoving(<span
style='color:blue'>New</span> MovementVector(<span style='color:blue'>New</span>
Point(newX, newY), 2))</p>

<p class=Code>                                <span style='color:blue'>Return</span></p>

<p class=Code>                            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                        <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                    <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>                <span style='color:blue'>Next</span></p>

<p class=Code>            <span style='color:blue'>End</span> <span
style='color:blue'>If</span></p>

<p class=Code>&nbsp;</p>

<p class=Code>        <span style='color:blue'>Catch</span> exc <span
style='color:blue'>As</span> Exception</p>

<p class=Code>            WriteTrace(exc.ToString())</p>

<p class=Code>        <span style='color:blue'>End</span> <span
style='color:blue'>Try</span></p>

<p class=Code>    <span style='color:blue'>End</span> <span style='color:blue'>Sub</span></p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324502">3.5<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Call the
ShouldIMoveForMyFriend() method at the end of each turn.</a>  </h3>

<p class=MsoNormal>At the end of every turn, we need to call the
ShouldIMoveForMyFriend method to unblock any of our friends.  Since the
IdleEvent is fired on each turn, add a call to the ShouldIMoveForMyFriend
method at the end of the MyAnimal_Idle method.  Add the following method call
as the last line of the MyAnimal_Idle method. <br>
<br>
</p>

<p class=Code>ShouldIMoveForMyFriend()</p>

<p class=MsoNormal>&nbsp;</p>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324503">3.6<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span>Rename and Build the animal
DLL.</a></h3>

<ul style='margin-top:0in' type=disc>
 <li class=MsoNormal>Select <b>Project</b> | <b>Properties</b>.  This will open
     the Project Properties dialog.</li>
 <li class=MsoNormal>In <b>Common Properties</b> | <b>General</b>, change the <b>Assembly
     Name</b> field to <b>&lt;YOUR NAME&gt;_Ex3</b>.</li>
 <li class=MsoNormal>Build the animal DLL from within the IDE by selecting <b>Build
     | Make Solution</b></li>
</ul>

<h3 style='margin-left:.25in;text-indent:-.25in'><a name="_Toc70324504"></a><a
name="_Toc69892018">3.7<span style='font:7.0pt "Times New Roman"'>&nbsp; </span>Introduce
the Herbivore into the Terrarium in Terrarium Mode.</a>  </h3>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Terrarium supports two game modes, Terrarium mode and Ecosystem
mode.  </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>In Ecosystem mode, your animal competes against animals
submitted by other developers.  </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Terrarium mode is the Terrarium test mode.  In Terrarium
mode you can test your animal in a controlled environment.</p>

<p class=MsoNormal style='margin-left:.25in'>&nbsp;</p>

<p class=MsoNormal>To switch to Terrarium Mode, click the <b>New Terrarium</b>
button.  Enter <b>Lab01</b> as the name and click <b>Save</b>.  This will
restart the client in Terrarium mode.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'><img width=139 height=34
src="images/tutorial_image001.jpg"></span></span></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>New Terrarium</span></span><br>
<br>
</p>

<p class=MsoNormal>The next step is to add some plants into the Terrarium so
that your Herbivore will have something to eat when it is introduced. To do
that, click on the <b>Add</b> button, then click the <b>Server </b>List button
and you will get a list of several animals that have already been introduced.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image002.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Add Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>For the purpose of this lab, and as is usual in the real
world, there are several plant species you can choose from. Simply select any
of the species of Type “plant” and click <b>OK</b>. This will introduce 10
plants of the type you selected to the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Add several more plants by selecting the plant in the drop
down box and clicking the <b>Insert</b> button so you have a lot of plants in
the Terrarium. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=99
height=35 src="images/tutorial_image003.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Insert Animal</span></span></p>

<p class=MsoNormal align=center style='text-align:center'>&nbsp;</p>

<p class=MsoNormal>Finally introduce your animal. To do this, click on <b>Add </b>button
click <b>Browse</b> and browse to the dll you created (<b>&lt;YOUR NAME&gt;</b>.dll).
This dll will be located in the Bin folder of your project.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=lab2tpn>You should see 10 instances of your creature in the
Terrarium.  To add more, you can use the Combo Box and <b>Insert</b> button
like you did with the plants.</p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>To test the new functionality, increase the population of
your animal by clicking the <b>Insert</b> button.  Notice that when one of your
animals is blocked from a food source, the blocking animal moves out of the
way.</p>

<p class=MsoNormal>&nbsp;</p>

<h2><a name="_Toc70324505">Lab Summary</a></h2>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>&nbsp;</span></p>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>Your
Herbivores are now talking to each other.  You could now take communication and
combine it with the attacked event logic.  Maybe you can have your herbivore
signal to all of his friends that he is being attacked and they should all come
and help.  Or they should all run away!</span></p>

<b><span style='font-size:11.0pt;font-family:Arial'><br clear=all
style='page-break-before:always'>
</span></b>

<h2><a name="_Toc70324506">Exercise 3 – Introduction Into the Ecosystem</a></h2>

<p class=lab2lb1>In <span lang=PT-BR>this hands-on lab, you will introduce your
animal into the Terrarium Ecosystem. The Ecosystem is what makes the Terrarium
such an interesting game. You get a chance here to match your animal making
skills against others. If you run this on a machine on the Internet, the
Ecosystem is the Internet and other any Terrariums on the Internet will be able
to safely exchange Animals with your Terrarium.</span> </p>

<p class=lab2tpl>Start the Terrarium by clicking <b>Start-&gt;All Programs-&gt;.NET
Terrarium 1.2-&gt;.NET Terrarium 1.2 </b></p>

<p class=MsoNormal>The Terrarium will open in Ecosystem mode. </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>To introduce your animal into the Ecosystem, click on the <b>Introduce
Animal</b> button, </p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal align=center style='text-align:center'><img width=141
height=35 src="images/tutorial_image004.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span
class=ButtonLabelChar><span style='font-size:9.0pt'>Introduce Animal</span></span></p>

<p class=MsoNormal>&nbsp;</p>

<p class=MsoNormal>Browse to the dll you created (<b>&lt;YOUR NAME&gt;_Ex3</b>.dll).
This dll will be located in the Bin folder of your project. After selecting
your animal, click <b>Open</b>, your animal will be introduced into the
Ecosystem, you should be able to see 10 of your animals on the screen. </p>

<p class=lab2lb1>You can only insert an animal once into the Ecosystem, so make
sure you’ve tested it thoroughly in Terrarium mode before introducing it.</p>

<h2><a name="_Toc70324507">Lab Summary</a></h2>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>&nbsp;</span></p>

<p class=Bodynoindent><span style='font-size:11.0pt;line-height:-116%'>Your
animal is now in the Ecosystem battling against dozens of other animals, where
your coding skills will determine its fate!</span></p>

<span style='font-size:11.0pt;font-family:Arial'><br clear=all
style='page-break-before:always'>
</span>

<p class=MsoNormal style='margin-left:.25in'>&nbsp;</p>

<p class=MsoNormal>&nbsp;</p>

<h1>&nbsp;</h1>

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
