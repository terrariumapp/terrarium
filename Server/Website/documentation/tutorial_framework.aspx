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
<H4>Building Animals Quickstart </H4>
<P>This page gives a brief overview of the steps involved in building and 
deploying an animal.   </P>
<H5>1. Write Code That Controls the Behavior of your Animal </H5>
<P>An animal is simply a class you write that derives from a special class 
(Animal) exposed by the Terrarium.  The code in this class gets executed by the 
game and controls the behavior and characteristics of your animal in the game. 
</P>
<P>The base characteristics of your animal can't be changed when the code is 
running -- capabilities characteristics like: how fast it can move, how well it 
attacks, how large it gets, etc.  These are defined on your animal by applying 
special attributes to the class you create.  You can see the list of available 
characteristic attributes in the "Attributes" section of the Class Reference Guide.  Some characteristics require 
points to be applied to decide "how much" of them you get.  Each animal has 100 
points to divide among these attributes.  So, for example, you could decide that 
your animal will be really fast by applying many points to that attribute, but 
it won't have good eyesight since you have less points left to apply there. </P>
<P>Once you've defined the characteristics your animal will have, then you write 
code that controls the behavior of the animal.  Look at the methods, properties 
and events on the Animal and Organism classes (Animal derives from Organism) in 
the Class Reference Guide to see what you can do 
in your code.  These methods define all the actions your animal can take.  Note 
that many of the method names start with "Begin...".  These methods are 
asynchronous.  When you call them, code execution will return immediately from 
the method.  When the action completes, the corresponding event will fire on 
your organism to tell you it is done. </P>
<BLOCKQUOTE dir=ltr style="MARGIN-RIGHT: 0px">
<P><EM>The easiest way to get started writing your code is to modify one of the existing skeletons found on the samples homepage.</EM></P></BLOCKQUOTE>
<H5>2. Build into an Assembly </H5>
<P>In order to introduce your animal into the game, you first need to build it 
into an assembly.  The name you give your assembly will be the name your animal 
has in the game.  Note that when you finally introduce it into the Ecosystem for 
real (after you are done testing), you won't be able to introduce the same 
assembly again.  You'll need to rename the assembly. </P>
<P>When you build your code into an assembly you'll need to tell the compiler to 
reference the Terrarium assembly named "OrganismBase.dll".  It is located in the 
directory where you installed Terrarium. This assembly contains the base Animal 
class, as well as any other classes from the game you may have used. 
<P><PRE class=code>csc /t:library /debug+ /out:MyAnimal.dll
	/r:System.dll,System.Drawing.dll,OrganismBase.dll MyAnimal.cs
</PRE><PRE class=code>vbc /t:library /debug+ /out:MyAnimal.dll
	/r:System.dll,System.Drawing.dll,OrganismBase.dll MyAnimal.vb
</PRE>
<H5>3. Debug Your Organism </H5>
<P>Terrarium has two modes that it runs in: </P>
<UL>
<LI>Ecosystem Mode - This is the actual game.  The only thing you can do in this 
mode is introduce new organisms and watch what happens. 
<LI>Terrarium Mode - This is useful for debugging.  In this mode, you can 
introduce organisms, pause the game, add as many instances of whatever organisms 
you want, view their debugging traces, etc. </LI></UL>
<P>Your current mode is displayed in the upper left corner of the game window. 
</P>
<P>When you first build an animal, it is a good idea to try it out in terrarium 
mode first to make sure it does what you expect.  Create a new Terrarium by 
clicking the controls on the bottom left corner of the game screen.  Choose the 
"Introduce Animal" command and point at the assembly you've created.  When you 
first introduce the animal, 10 instances get created.  You can create more by 
choosing your animal from the dropdown and pushing the "Add button". </P>
<P>Of course, you'll need other plants and animals for your organism to interact 
with when you're debugging. If your using the Introduce Species dialog you'll 
notice that you can use any of the animals from the EcoSystem server your peer 
is using. You can even introduce plants and animals more than once when in 
Terrarium mode. </P>
<P>Once your animals are introduced, there are a few ways to debug them.  Click 
on the trace window button in the lower left corner of the screen to show the 
trace window and select one of your animals.  Anything you traced using the 
Organism.WriteTrace method will get shown in the trace window.  In addition, you 
can bring up the property sheet to see the current properties on your Animal, 
see why they died, see how much energy they have, etc. </P>
<P>If your organism throws an exception, it will be destroyed by the game, and 
if it takes too long to run it's code (dependent upon your machine, but normally 
2 milliseconds) it will get destroyed as well. </P>
<P>You can set up several machines in a private peer to peer network by typing a 
string into the "Peer Channel" textbox and tabbing away.  All machines that use 
the same string will eventually link up on the same peer to peer network and 
teleport organisms to each other.  This is a useful way to debug your organism 
to see how it behaves when it is teleported. </P>
<H5>4. Deploy Your Organism and See How it Fares </H5>
<P>Once you've decided that your organism is ready to experience the real world, 
switch to Ecosystem mode and introduce it there.  Once you introduce it, you 
won't be able to introduce it again using the same assembly name. </P>
<P>You can go to the Terrarium Website and chart the population of your animal 
or statistics like why your animals die (do they starve? get eaten? throw 
exceptions? etc). </P>
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
