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
			<H4>Client FAQ</H4><SPAN class=Normal>
			<OL start=0>
			<LI>Introduction 
			<OL>
			<LI><A href="#0_1">What is explained in this FAQ?</A> 
			<LI><A href="#0_2">Who should use this FAQ?</A> 
			<LI><A href="#0_3">How do I know it is safe?</A> 
			<LI><A href="#0_4">Contents</A> </LI></OL>
			<LI>Initial Installation 
			<OL>
			<LI>Computer Setup 
			<OL>
			<LI><A href="#1_1_1">What kind of computer do I need?</A> 
			<LI><A href="#1_1_2">What OSes are supported/not supported?</A> 
			<LI><A href="#1_1_3">Will my graphics card work?</A> 
			<LI><A href="#1_1_4">What privileges does my user account need?</A> </LI></OL>
			<LI>Software Setup 
			<OL>
			<LI><A href="#1_2_1">What version of the .NET Framework do I need?</A> 
			<LI><A href="#1_2_2">Do I need the .NET Framework SDK?</A> 
			<LI><A href="#1_2_3">Do I need to purchase VS .NET?</A> 
			<LI><A href="#1_2_4">Do I run the setup file or the msi file?</A> 
			</LI></OL></LI></OL>
			<LI>Troubleshooting 
			<OL>
			<LI><A href="#2_1">My graphics have pink boxes around them.</A> 
			<LI><A href="#2_2">Terrarium is detecting mismatched assemblies.</A> 
			<LI><A href="#2_3">Terrarium is detecting that I have Beta 2 installed.</A> 
			<LI><A href="#2_4">My Terrarium shows 0 peers.</A> 
			<LI><A href="#2_5">Why do I get "An error occurred installing the package. 
			Windows Installer returned error 1638." when I try to install the Terrarium?</A> 

			<LI><A href="#2_6">Why do I get "An error occurred installing the package. 
			Windows Installer returned error 1601." when I try to install the Terrarium?</A> 

			<LI><A href="#2_7">Terrarium reports that your machine has an inconsistent set 
			of dlls.</A> </LI></OL>
			<LI>Networking Setup 
			<OL>
			<LI><A href="#3_1">Will my Dial-Up Connection Work?</A> 
			<LI><A href="#3_2">Will my Cable Connection Work?</A> 
			<LI><A href="#3_3">Will my DSL Connection Work?</A> 
			<LI><A href="#3_4">How do I configure my NAT device?</A> </LI></OL>
			<LI>Developer Setup 
			<OL>
			<LI><A href="#4_1">What software do I need to develop creatures for the 
			Terrarium?</A> 
			<LI><A href="#4_2">I don't have VS .NET, how do I compile my creatures?</A> 
			<LI><A href="#4_3">The compilers can't be found, how do I fix it?</A> </LI></OL>
			<LI>UI Q/A 
			<OL>
			<LI><A href="#5_1">I changed my performance settings but nothing happened?</A> 
			<LI><A href="#5_2">How can I change my registered email?</A> 
			<LI><A href="#5_3">How do I change the server I am playing on?</A> 
			<LI><A href="#5_4">Where is the Trace Window?</A> 
			<LI><A href="#5_5">Where is the Properties Window?</A> 
			<LI><A href="#5_6">Where is the Local Statistics Window?</A> 
			<LI><A href="#5_7">The Local Statistics window isn't updating, Why?</A> 
			</LI></OL>
			<LI>Peer Q/A 
			<OL>
			<LI><A href="#6_1">Why are my teleports always getting sent back?</A> 
			<LI><A href="#6_2">How many peers are in my peers list?</A> 
			<LI><A href="#6_3">Why does my EcoSystem have lots of Creature A and no Creature 
			B?</A> 
			<LI><A href="#6_4">What is Peer Blacklisting?</A> 
			<LI><A href="#6_5">How do I reset my bad peers list?</A> 
			<LI><A href="#6_6">My peer teleport lights are red, Why?</A> 
			<LI><A href="#6_7">Why do the teleport lights stay yellow for so long?</A> 
			<LI><A href="#6_8">What am I sending during a teleport?</A> </LI></OL>
			<LI>Cheaters Q/A 
			<OL>
			<LI><A href="#7_1">When I delete my gamestate file how long before I can report 
			again?</A> 
			<LI><A href="#7_2">If another peer denies a received teleport, what happens to 
			them?</A> 
			<LI><A href="#7_3">After a successful teleport what happens to prevent me from 
			getting flooded?</A> 
			<LI><A href="#7_4">How can I prevent other players from decompiling my code?</A> 

			<LI><A href="#7_5">What happens to me if I set up my router to deny incoming 
			traffic?</A> 
			<LI><A href="#7_6">What happens if a peer reports bogus data?</A> </LI></OL>
			<LI>Creature Development 
			<OL>
			<LI>Beginner Development 
			<OL>
			<LI><A href="#8_1_1">Where can I get code for other creatures?</A> 
			<LI><A href="#8_1_2">Where can I ask questions about creature development?</A> 
			<LI><A href="#8_1_3">How do I start programming my first creature?</A> 
			<LI><A href="#8_1_4">Where can I find documentation on developing creatures?</A> 

			<LI><A href="#8_1_5">Are there any creature templates available?</A> 
			<LI><A href="#8_1_6">How do I workaround the error "A *creature* was destroyed 
			because it was blacklisted due to past bad behavior and won't be loaded"?</A> 
			<LI><A href="#8_1_7">How do I workaround the error "Your organism 
			*SomeAnimalName* could not be found in the assembly. Please make sure this class 
			exists." when developing an animal in VB.NET?</A> 
			<LI><A href="#8_1_8">Why am I getting "Assembly failed validation process. 
			Please consult rules for allowed constructs" when I try to introduce an 
			animal?</A> </LI></OL>
			<LI>Intermediate Development 
			<OL>
			<LI><A href="#8_2_1">What is the difference between bigger and smaller 
			creatures?</A> 
			<LI><A href="#8_2_2">Why are there more herbivores than carnivores?</A> 
			<LI><A href="#8_2_3">How do I stock my Terrarium?</A> 
			<LI><A href="#8_2_4">Where can I find EcoSystem creatures?</A> </LI></OL>
			<LI>Advanced Development 
			<OL>
			<LI><A href="#8_3_1">Why do so many creatures die from sickness?</A> 
			<LI><A href="#8_3_2">Why is my creature getting blocked all the time?</A> 
			</LI></OL>
			<LI>Debugging 
			<OL>
			<LI><A href="#8_4_1">What type of information can I use with WriteTrace?</A> 
			<LI><A href="#8_4_2">Why do my WriteTrace calls come out backwards?</A> 
			<LI><A href="#8_4_3">Can I use the VS .NET Debugger?</A> 
			<LI><A href="#8_4_4">How do I use the VS .NET Debugger?</A> 
			<LI><A href="#8_4_5">Can I copy my new creature over top of my old creature when 
			debugging?</A> </LI></OL></LI></OL></LI></OL><BR><BR><A name=0_1><B>0.1 What is 
			explained in this FAQ?</B></A> 
			<DIV>This FAQ is a compilation of questions received multiple times on the 
			forums. Each question is designed to best identify a user's problem so that they 
			can quickly and easily find their issue in the FAQ. Users should feel free to 
			point beginner and intermediate Terrarium users directly to the FAQ whenever a 
			discussed topic arises. Users should also feel free to contact the Terrarium 
			team via the discussion boards if they find a new question or section of 
			questions that should be covered by the FAQ. </DIV>
			<DIV><BR>Note: Please do not link directly into the FAQ using the anchors. At 
			any time FAQ content may change. Enhancements to the readability of the FAQ may 
			also result in some sections becoming their own FAQ pages. If you see a question 
			being asked by a user that you feel is already in the FAQ simply refer them to 
			the Category/Question that they should examine. </DIV><BR><BR><A name=0_2><B>0.2 
			Who should use this FAQ?</B></A> 
			<DIV>The FAQ is designed to walk users through the basic beginning steps of 
			getting a Terrarium client up and running. In addition the FAQ is useful in to 
			advanced developers since several advanced concepts are also considered. 
			</DIV><BR><A name=0_3><B>0.4 How do I know it is safe?</B></A> 
		Teleportation or "mobile code" on the Terrarium is built 
			on top of the .NET Framework and uses the code access security mechanisms built 
			into the Common Language Runtime. An organism being hosted in the terrarium is 
			only permitted to execute code on the machine. This means that the organism code 
			can not perform malicious actions against machine resources such as reading or 
			writing to the file system, accessing the network, or accessing the registry. 
			</DIV><BR><A name=0_4><B>0.4 Contentsntents</B></A> 
			<DIV>The FAQ is designed to walk users through the basic beginning steps of 
			getting a Terrarium client up and running. In addition the FAQ is useful in to 
			advanced developers since several advanced concepts are also considered. 
			</DIV><BR>
			<DIV>All users should examine the FAQ before posting a question relating to a 
			Terrarium issue or problem on the forums. Nevertheless some users will still 
			post on the forums even when a FAQ item exists. To help alleviate this issue 
			please direct these users to the FAQ rather than explain their issue. If they 
			come back for clarification then feel free to both answer the new questions and 
			send the Terrarium team a pointer to the forum topic so that any necessary 
			additions can be made to the FAQ for future users. </DIV><BR><BR><A 
			name=1_1_1><B>1.1.1 What kind of computer do I need?</B></A> 
			<DIV>The Terrarium gaming system is somewhat processor intensive since an 
			arbitrary amount of user coded creatures are given time slices in which to 
			execute code. On each machine a benchmark is run to determine exactly how many 
			creatures can exist on the machine before performance degradation will occur. 
			Because of this performance scaling feature the Terrarium is capable of 
			operating on a fairly minimal set of hardware. The following represents 
			recommended hardware: </DIV><BR>
			<DIV>128 MB RAM Pentium III 500MHz 8 MB Graphics Card with DirectX 7 Drivers 
			</DIV><BR>
			<DIV>The recommended hardware will yield approximately 40-60 creatures after the 
			benchmark test. Hardware with significantly less processor power can also be 
			used, but the graphics card and RAM requirements are generally needed to achieve 
			the appropriate graphics performance. </DIV><BR><BR><A name=1_1_2><B>1.1.2 What 
			OSes are supported/not supported?</B></A> 
			<DIV>The largest OS requirement for Terrarium is that the OS properly supports 
			the .NET Framework. Please review the .NET Framework documentation and make sure 
			your OS is supported by at least the redistributable package. The second OS 
			requirement for Terrarium is that DirectX 7 is supported. The Terrarium graphics 
			make use of DirectX 7 in order to achieve complex scenes with a minimum of CPU 
			and Graphics processing time. Given the above requirements the following OSes 
			are recommended by the Terrarium team for both running a client and for doing 
			creature development.<BR>
			<UL>
			<LI>Windows 2000, Any Flavor. 
			<LI>Windows XP, Home/Pro. </LI></UL>The following OSes are not capable of 
			running the Terrarium due to software constraints.<BR>
			<UL>
			<LI>Windows NT 4.0 </LI></UL>The following OSes have not been heavily tested and 
			may be used, but may have unknown issues or complications.<BR>
			<UL>
			<LI>Windows 98 
			<LI>Windows ME 
			<LI>Windows Server 2003 </LI></UL></DIV><BR><A name=1_1_3><B>1.1.3 Will my 
			graphics card work?</B></A> 
			<DIV>Any graphics card that is DirectX 7 compliant should work well with the 
			Terrarium. In order to achieve decent performance, the card should have a 
			minimum of 8 MB of graphics memory. Cards without 8 MB of graphics memory will 
			still work, but with diminished graphics performance. Some graphics cards also 
			exhibit a pink tinge around the images when viewed in 32-bit color mode. Please 
			see the troubleshooting guide for information on how to fix this issue. 
			</DIV><BR><A name=1_1_4><B>1.1.4 What privileges does my user account 
			need?</B></A> 
			<DIV>When installing the Terrarium you should be logged in as a Power User. You 
			will also need to be logged in as a Power User when running the Terrarium since 
			the client writes to it's installation location. By default this is in the 
			Program Files directory. </DIV><BR><A name=1_2_1><B>1.2.1 What version of the 
			.NET Framework do I need?</B></A> 
			<DIV>The Terrarium client installation page contains a link to install the 
			required version of the .NET Framework. Currently the Terrarium works with the 
			.NET Framework v1.0 or with the .NET Framework v1.0 Service Pack 1. You can get 
			the .NET Framework by installing either the .NET Framework Redist Package, the 
			.NET Framework SDK (recommended for developing creatures), or the VS .NET 
			package (also recommended for developing creatures). </DIV><BR><A 
			name=1_2_2><B>1.2.2 Do I need the .NET Framework SDK?</B></A> 
			<DIV>The .NET Framework SDK is not required, however it is recommended if you 
			are programming creatures. The SDK contains useful documentation for the .NET 
			enabled languages (C#, VB.NET), and also for many useful framework classes that 
			can be used to develop more intelligent creatures. </DIV><BR><A 
			name=1_2_3><B>1.2.3 Do I need to purchase VS .NET?</B></A> 
			<DIV>The VS .NET package is not required, however it is recommended if you are 
			programming creatures. Many features of VS .NET including intellisense, project 
			management, and debugging help to make the creature development easier and more 
			enjoyable. </DIV><BR><A name=1_2_4><B>1.2.4 Do I run the setup file or the msi 
			file?</B></A> 
			<DIV>All users should run the setup file first. If there are issues running the 
			setup file, then the msi file can be run directly. The only requirement for 
			running the msi file directly is that the latest version of the MSI installer 
			components are installed on the machine. Currently this is true of any users 
			that have installed the .NET Framework, but in the future new versions of the 
			MSI components may become available causing the msi to not install properly. 
			</DIV><BR><A name=2_1><B>2.1 My graphics have pink boxes around them.</B></A> 
			<DIV>Some graphics cards show pink tinges around images when being viewed in 
			32-bit color mode. This is due to image dithering features implemented at the 
			card's driver level. To fix this issue you can simply change your Display 
			Settings to 16-bit color mode and everything should work. The following cards 
			have demonstrated this problem: 
			<UL>
			<LI>ViperII Z200 
			<LI>Matrox MGA Millenium </LI></UL></DIV><BR><A name=2_2><B>2.2 Terrarium is 
			detecting mismatched assemblies.</B></A> 
			<DIV>This occurs when the assemblies being loaded for specific CLR types are of 
			different versions. This generally means that there is a configuration problem 
			with your machine resulting from a previous version of the .NET Framework not 
			being uninstalled properly. This can be the case if you installed Beta 2 fo the 
			.NET Framework before upgrading to v1.0. Try to uninstall Beta 2, reinstall v1.0 
			of the .NET Framework and try again. </DIV><BR><A name=2_3><B>2.3 Terrarium is 
			detecting that I have Beta 2 installed.</B></A> 
			<DIV>The released version of the .NET Terrarium requires features that are only 
			available on v1.0 of the .NET Framework. In some cases the Terrarium can detect 
			that these features aren't present and will recommend that you might have Beta 2 
			installed currently, or that some Beta 2 assemblies are being loaded. Try to 
			uninstall Beta 2, reinstall v1.0 of the .NET Framework and try again. 
			</DIV><BR><A name=2_4><B>2.4 My Terrarium shows 0 peers.</B></A> 
			<DIV>A Terrarium can display 0 peers for a number of reasons. The first reason 
			might be that your running in Terrarium mode versus EcoSystem mode. Your current 
			mode is displayed in the upper left hand corner of the Terrarium client. If you 
			are in Terrarium mode, in order to interact with other peers, you'll have to 
			join a peer channel. You'll then need to instruct others to join your peer 
			channel. The Forums is a good place to organize this.<BR><BR>If your in 
			EcoSystem mode, then it is most likely that you are behind a NAT or firewall. In 
			this scenario the Terrarium client is unable to register or connect with the 
			Terrarium server to discover peers and will display 0. You should examine the 
			LEDs (5 lights in the top of the Terrarium), so see if server connections are 
			succeeding or failing. Green means that everything is good, while a red light 
			means something is wrong.<BR><BR>Please review the Network Configuration FAQ 
			Section for more information on enabling the NAT feature. </DIV><BR><A 
			name=2_5><B>2.5 Why do I get "An error occurred installing the package. Windows 
			Installer returned error 1638." when I try to install the Terrarium?</B></A> 
			<DIV>This means that you already have a version of the Terrarium installed. 
			Please remove the older version using Add/Remove Programs in the Control Panel 
			and rerun setup. </DIV><BR><A name=2_6><B>2.6 Why do I get "An error occurred 
			installing the package. Windows Installer returned error 1601." when I try to 
			install the Terrarium? </B></A>
			<DIV>This means that your windows installer service is not running. You can 
			start it by going to Control Panel -&gt; Administrative Tools -&gt; Services 
			-&gt; Windows Installer, right click and choose "Start". Then rerun setup. 
			</DIV><BR><A name=2_7><B>2.7 Terrarium reports that your machine has an 
			inconsistent set of dlls.</B></A> 
			<DIV>Terrarium does a basic check of the assemblies that it will be using as the 
			game is running. During this process the Terrarium loads up one type from each 
			of the assemblies it will need, and then verifies that the version numbers on 
			all of these assemblies is exactly the same. The primary cause of this issue is 
			having a Beta version of the .NET Framework installed on the machine in addition 
			to the release version. You need to make sure that a Beta 2 installation isn't 
			still on your machine. You can check this by running "gacutil /l" and making 
			sure all of the registered assemblies contain the same version number and that 
			none of them contain a version that is smaller than 3705. If you can't find the 
			cause of the problem, then please send email to the Terrarium development team 
			with the output from the gacutil command and the list of directories located in: 
			<PRE>%SYSTEMROOT%\Microsoft.NET\Framework</PRE></DIV><BR><A name=3_1><B>3.1 Will 
			my Dial-Up Connection Work?</B></A> 
			<DIV>Most of the time a direct dial-up connection will work. Direct dial-up 
			connections are relatively slow and certain features of the Terrarium will 
			appear to work improperly. Features that might not work well on a dial-up 
			connection include: 
			<UL>
			<LI>Teleportations of creatures 
			<LI>Downloading of creature lists </LI></UL>Things that might cause a direct 
			dial-up connection to fail are: 
			<UL>
			<LI>Your ISP provides a default firewall on your connection. 
			<LI>Your ISP runs HTTP traffic through a caching or acceleration server 
			</LI></UL>You may also want to make sure your not sharing your connection. 
			Connection sharing devices and software can cause the Terrarium client to fail. 
			If you are behind a connection sharing device then please review the FAQ item on 
			configuring your NAT device. </DIV><BR><A name=3_2><B>3.2 Will my Cable 
			Connection Work?</B></A> 
			<DIV>Cable connections are sufficiently fast enough to enable all Terrarium 
			features without a visible slow-down in the application. Some cable providers do 
			place a NAT device or other connection sharing device on the line which may 
			cause the Terrarium client to not operate in EcoSystem mode. In addition your 
			cable modem or router may have additional blocking features depending on its 
			configuration. Please review the FAQ item on configuration your NAT device if 
			you are having trouble playing behind your cable connection. </DIV><BR><A 
			name=3_3><B>3.3 Will my DSL Connection Work?</B></A> 
			<DIV>DSL connections are sufficiently fast enough to enable all Terrarium 
			features without a visible slow-down in the application. Some DSL providers do 
			place a firewall, NAT device, or other connection sharing device on the line 
			which may cause the Terrarium client to not operate in EcoSystem mode. In 
			addition your DSL modem or router may have additional blocking features 
			depending on its configuration. Please review the FAQ item on configuration your 
			NAT device if you are having trouble playing behind your DSL connection. 
			</DIV><BR><A name=3_4><B>3.4 How do I configure my NAT device?</B></A> 
			<DIV>The .NET Terrarium will run if you are behind a network address translator 
			and it will be able to connect to the discovery server but it will not be able 
			to receive creatures from other peers. This is because the peers cannot directly 
			access a machine that is behind a NAT without making a configuration change to 
			the NAT.<BR><BR>To make the necessary configuration change, right click on the 
			Terrarium screen and select "Game Settings.", click on the "Server" tab on the 
			screen that comes up and check "Enable Nat/Firewall support". You need to modify 
			the NAT device to map the TCP port 50000 on the device to port 50000 on the 
			machine that is running the .NET Terrarium. The steps involved in mapping the 
			port will vary depending on what type of NAT is being used. Check your NAT 
			device documentation for information on how to map ports from the NAT to a 
			private internal machine.<BR><BR>For more information on network address 
			translators, see the NAT Terminology and Considerations RFC. </DIV><BR><A 
			name=4_1><B>4.1 What software do I need to develop creatures for the 
			Terrarium?</B></A> 
			<DIV>At a minimum, you'll need the .NET Framework Redist. It is recommended by 
			the Terrarium development team that you get the .NET Framework SDK or VS .NET if 
			you are truly interested in developing creatures since both packages have 
			helpful features.<BR><BR>Depending on which package you install creatures will 
			have to be compiled using different methods. When compiling using the redist, 
			compilers are on the machine, but the paths are not set up right. Please review 
			the FAQ item on how to find compilers.<BR><BR>The .NET Framework SDK will 
			optionally add the compilers and tools to your path. This is recommended. VS 
			.NET will have a separate command window that sets up the path. This command 
			window will be available on the Start-&gt;Programs menu under the VS 
			.NET-&gt;Tools group. </DIV><BR><A name=4_2><B>4.2 I don't have VS .NET, how do 
			I compile my creature?</B></A> 
			<DIV>Compiling your creature without VS .NET means using the command window and 
			command line compilers. The documentation section of the GDN Terrarium site has 
			a section on compiling your creature using the command line compiler. Please 
			refer to that document when compiling your creature. </DIV><BR><A 
			name=4_3><B>4.3 The compilers can't be found, how do I fix it?</B></A> 
			<DIV>This can occur if the paths to the tools are not set up properly. VS .NET 
			by default does not set up the appropriate paths for the default command window, 
			but provides a special command window available through Start-&gt;Programs in 
			order to set up the appropriate paths.<BR><BR>The .NET Framework SDK will have 
			an option to set the paths up for you during installation. But if you are 
			running the redist, or you chose not to set up the paths, then you'll have to do 
			this step by hand. By default the compilers are placed in the .NET Framework 
			installation directory. This will generally be:<BR><PRE>%SYSTEMROOT%\Microsoft.NET\Framework\{Version}</PRE>Once you find the 
			csc.exe/vbc.exe compiler locations, you can simply append these to your path 
			information. Then use the command line QuickStart available in the documentation 
			section of the GDN Terrarium site to try compiling your creature again. 
			</DIV><BR><A name=5_1><B>5.1 I changed my performance settings but nothing 
			happened?</B></A> 
			<DIV>When you change your performance settings, the world doesn't resize 
			dynamically because of internal data structures that aren't dynamic. The new 
			performance settings will apply to any newly created Terrarium files you 
			generate or any time the EcoSystem file has to be regenerated. If you would like 
			your changes to be reflected immediately on the EcoSystem you can clear your 
			existing EcoSystem by deleting your gamestate.bin file located in the following 
			location: <PRE>%USERPROFILE%\Application Data\Microsoft\Terrarium</PRE>This directory will 
			contain several sub-directories representing the different servers you've 
			connected to and the different versions you've been running. You'll need to find 
			the appropriate directory and then delete the gamestate.bin file. </DIV><BR><A 
			name=5_2><B>5.2 How can I change my registered email?</B></A> 
			<DIV>The Terrarium can register an email address to associate with your IP. This 
			can help the Terrarium development team contact you after you've submitted error 
			reports or feature requests using the built in error reporting tool.<BR><BR>To 
			make the necessary configuration change, right click on the Terrarium screen and 
			select "Game Settings.". There should be a box to change your registered email 
			address. Provide your email address and Apply the settings. Your new email 
			should now be registered. </DIV><BR><A name=5_3><B>5.3 How do I change the 
			server I am playing on?</B></A> 
		You can change the server you are playing on by changing your game settings. To 
                make the necessary configuration change, right click on the Terrarium scree and 
                select &quot;Game Settings.&quot;, and then move to the &quot;Server&quot; tab. In the box provided 
                to you enter the address of the server you wish to connect to. The server will 
                be validated to ensure that it is a Terrarium server and that it is operational. If the server is valid then the Terrarium client 
			will prompt you to shut down and restart so the new changes can take effect. If 
			you've never played on the new server before then you'll get a new EcoSystem. As 
			long as you connect to your previous server within 48 hours you can always go 
			back to your old EcoSystem. In this way you can maintain different EcoSystems on 
			different servers. </DIV><BR><A name=5_4><B>5.4 Where is the Trace 
			Window?indow?</B></A> 
			<DIV>The trace window is available via a UI button on the Terrarium controls 
			panel. The UI documentation available on the GDN Terrarium site explains how to 
			find the button. You may also right-click on the Terrarium client and select 
			"Trace Window" from the list of menu items. </DIV><BR><A name=5_5><B>5.5 Where 
			is the Properties Window?</B></A> 
			<DIV>The properties window is available via a UI button on the Terrarium 
			controls panel. The UI documentation available on the GDN Terrarium site 
			explains how to find the button. You may also right-click on the Terrarium 
			client and select "Properties Window" from the list of menu items. </DIV><BR><A 
			name=5_6><B>5.6 Where is the Local Statistics Window?</B></A> 
			<DIV>The local stats window is not available via the normal UI. However, you may 
			right-click on the Terrarium client and select "Local Stats" from the list of 
			menu items to pop up a view representing your local terrarium statistics. 
			</DIV><BR><A name=5_7><B>5.7 The Local Statistics window isn't updating, 
			Why?</B></A> 
			<DIV>Some versions of Terrarium require that the Refresh button be pressed to 
			get the latest statistics. Newer versions of the Terrarium have a check-box to 
			enable automatic refreshing of the stats window. The stats will refresh every 
			ten seconds using the new feature. If you don't want the stats to refresh 
			automatically you can deslect the checkbox. </DIV><BR><A name=6_1><B>6.1 Why are 
			my teleports always getting sent back?</B></A> 
			<DIV>Teleports between peers have certain constraints to prevent flood based 
			attacks on the EcoSystem. Your teleport may get sent back to your local peer for 
			the following reasons: 
			<UL>
			<LI>Your peer is already sending a creature. 
			<LI>The remote peer is already receiving a creature from another peer. 
			<LI>The remote peer has received a creature from you in the last 30 seconds. 
			<LI>The peer you are trying to send to has left the network. 
			<LI>The remote peer isn't accepting your connection. </LI></UL>Some of these may 
			seem unfair. However, if for any reason your creature is unfairly rejected, your 
			peer will blacklist the other peer and you will neither accept creatures from or 
			send creatures to that peer for an hour. </DIV><BR><A name=6_2><B>6.2 How many 
			peers are in my peers list?</B></A> 
			<DIV>The peer count, and the number of peers in your peers list are drastically 
			different. The peers list in the database is ordered by IP. Then 10 IPs above 
			your peer in the list, and 10 IPs below your peer in the list are added to your 
			peer's list. It is this list that is used when teleporting.<BR><BR>Due to 
			constraints at the database level, the query can result in anywhere from 20 
			peers to 30 peers depending on a peer's location in the list. Peers that are at 
			the top or bottom of the list get a couple of extra peers.<BR><BR></DIV><BR><A 
			name=6_3><B>6.3 Why does my EcoSystem have lots of Creature A and no Creature 
			B?</B></A> 
			<DIV>Because the IP list is sorted by IP, client's in the same geographic region 
			or network segments can often get peered together. This means network issues 
			like a slow line between Europe and America can cause the European based animals 
			to appear on the European network segment, while a completely different creature 
			dominates another network segment. </DIV><BR><A name=6_4><B>6.4 What is Peer 
			Blacklisting?</B></A> 
			<DIV>Peer blacklisting is a special constraint concept to help prevent cheating 
			by disabling incoming traffic, but leaving outgoing traffic enabled. Whenever 
			your peer tries to teleport to another peer and the connection is denied your 
			peer recognizes that something is wrong. At this point your peer blacklists the 
			remote peer for one hour.<BR><BR>Once a peer is blacklisted you neither accept 
			teleports from or send teleports to the peer. In this way the peer can 
			effectively be removed from the EcoSystem. Because the logic is constraint based 
			there are times when a peer that should be blacklisted gets to send a couple of 
			creatures, but they never get more than a few off before they are blacklisted 
			again. </DIV><BR><A name=6_5><B>6.5 How do I reset my bad peers list?</B></A> 
			<DIV>If something happens to your connection and you want to reset your bad 
			peers list because you think you might have blacklisted some peers invalidly 
			then you can clear the list by right-clicking the Terrarium and selecting the 
			"Reset Bad Peer List" menu item. </DIV><BR><A name=6_6><B>6.6 My peer teleport 
			lights are red, Why?</B></A> 
			<DIV>Peer teleports lights turn red when teleportations time out or the actual 
			network connection is not accepted. Depending on the speed of your connection, 
			the teleport lights may actually turn red quite often with network timeouts. 
			This is not a blocking issue. Some successful teleports should still occur both 
			too and from your Terrarium. If this isn't the case then you may also be behind 
			a NAT or firewall. Please check the Networking FAQ items for information on 
			network related issues. </DIV><BR><A name=6_7><B>6.7 Why do the teleport lights 
			stay yellow for so long?</B></A> 
			<DIV>Teleports happen asynchronously with the client. This means a new worker 
			thread is spun off in the background while all of the network IO is happening. 
			Depending on the connection speeds of both your peer and the remote peer the 
			network transfer times can be somewhat long. Yellow lights basically means that 
			work is in progress. Unless the light goes red there is nothing to worry about, 
			and in some cases, a red light is not an issue either. </DIV><BR><A 
			name=6_8><B>6.8 What amd I sending during a teleport?</B></A> 
			<DIV>A teleport is a multi-part conversation with a remote peer. The steps are 
			as follows: 
			<UL>
			<LI>1. Send a peer version request. 
			<UL>
			<LI>Yes, same version, goto 2 
			<LI>No, not same version, end conversation </LI></UL>
			<LI>2. Send an assembly confirmation request. 
			<UL>
			<LI>Yes, I have the assembly, goto 4 
			<LI>No, I don't have the assembly, goto 3 </LI></UL>
			<LI>3. Send the assembly 
			<LI>4. Send the serialized organism state 
			<LI>5. Confirm receipt 
			<UL>
			<LI>Yes, it was received, end conversation 
			<LI>No, it was not received, goto 6 </LI></UL>
			<LI>6. Teleport locally </LI></UL>This means you are sending several messages to 
			the remote peer. But the bulk of the message is the creature assembly if the 
			remote peer does not have it, and the serialized organism which is actually 
			quite small. Most conversations consist of ~5KB worth of data, ~30KB when the 
			organism is not on the remote machine, and a maximum of about ~115KB assuming a 
			maximum sized creature and large serialized state. </DIV><BR><A name=7_1><B>7.1 
			When I delete my gamestate file how long before I can report again?</B></A> 
			<DIV>Deleting your gamestate file means your blacklisted from reporting for 12 
			hours. In most cases this is not long enough to be detrimental to the normal 
			operations of the stats, but is long enough to prevent people from leaving stale 
			gamestates in the database to adversely affect stats. </DIV><BR><A 
			name=7_2><B>7.2 If another peer denies a received teleport, what happens to 
			them?</B></A> 
			<DIV>The answer depends on why the peer denies the teleport. Basically, if the 
			teleport is denied as part of a game function or network timeout, then nothing 
			happens to the peer. If the teleport is denied because the remote peer will not 
			accept a network level connection then the peer is blacklisted by your peer for 
			one hour. Currently the only way this happens is if a user unknowingly has a 
			firewall set up on their connection or has done so on purpose to try to cheat 
			the game. </DIV><BR><A name=7_3><B>7.3 After a successful teleport what happens 
			to prevent me from getting flooded?</B></A> 
			<DIV>Once your peer receives a teleport from a remote peer, it puts that peer on 
			a temporary blacklist of 30 seconds. This prevents the other peer from flooding 
			your peer with creatures. 30 seconds is long enough to prevent viable flooding, 
			but short enough not to affect the game. </DIV><BR><A name=7_4><B>7_4 How can I 
			prevent other players from decompiling my code?</B></A> 
			<DIV>There are many solutions to code decompilation at this point. The most 
			obvious fix for code decompilation is one of the freely available code 
			obfuscators. If you find a code obfuscator that works well with the Terrarium 
			please contact our team with a link and we'll try to get it into the 
			FAQ.<BR><BR>By hand obfuscation is also fairly effective. If you have extra 
			space in your assembly to put unused functions, feel like naming your functions 
			strangely, or make effective use of strangely oriented conditional statements, 
			then it can make your code much harder to decompile.<BR><BR>The true solution, 
			which is not recommended, is writing a small pseudo language for your creature. 
			A pseudo language would be a small interpreted language that your creature runs. 
			Standard organism code written in C# or VB would still be there, but the meat of 
			behavior could be hidden in an encrypted string. This heavily raises the bar for 
			decompilation, but also raises the bar for coding even the simplest creatures. 
			</DIV><BR><A name=7_5><B>7.5 What happens to me if I set up my router to deny 
			incoming traffic?</B></A> 
			<DIV>Your peer gets blacklisted by other peers and your effectively removed from 
			the EcoSystem. By luck of the draw you'll still get a few teleports out before 
			your blacklisted, but you'll eventually be your own island neither sending to or 
			receiving from the rest of the EcoSystem. </DIV><BR><A name=7_6><B>7.6 What 
			happens if a peer reports bogus data?</B></A> 
			<DIV>If a peer reports bogus data then they are blacklisted for 12 hours. This 
			is exactly the same as deleting your gamestate. The method for blacklisting lies 
			in several constraint numbers based on maximums in the Terrarium client. First, 
			a single creature can't report more than a certain population. Second, all 
			creatures can't add up to over a certain population. Finally peers are throttled 
			so they can't report too often. </DIV><BR><A name=8_1_1><B>8.1.1 Where can I get 
			code for other creatures?</B></A> 
			<DIV>Code for other creatures is available through the Animal Farm. Various 
			developers were nice enough to share their creature code with the public, often 
			times sharing prize winning algorithms with the public. You may also make use of 
			the templates provided in the Documentation section of the site, as well as the 
			various code snippets provided in the Advanced Developer Documentation. 
			</DIV><BR><A name=8_1_2><B>8.1.2 Where can I ask questions about creature 
			development?</B></A> 
			<DIV>Questions about creature development should be submitted to the Organism 
			Building Discussion Forum. Please make sure you check all of the FAQ items 
			before posting questions to avoid noise in the forums. In most cases forum users 
			will simply direct you back to the FAQ if they feel your question is answered by 
			a FAQ item.<BR><BR>Questions about the Terrarium client in general can be 
			directed to the General Discussions Forum. </DIV><BR><A name=8_1_3><B>8.1.3 How 
			do I start programming my first creature?</B></A> 
			<DIV>The documentation provides a Quick Start with documentation available in 
			both VS .NET and using the basic command line compilers. The documentation has a 
			series of pre-canned creature templates that will be useful when programming 
			your first creature.<BR><BR>Once your basic template is compiling and ready to 
			go you should feel free to examine the rest of the documentation, search the 
			forums, and simply play with the code. </DIV><BR><A name=8_1_4><B>8.1.4 Where 
			can I find documentation on developing creatures?</B></A> 
			<DIV>The Documentation section of the Terrarium site provides many levels of 
			documentation. There is a basic overview of the Terrarium game, several pages of 
			UI documentation, and a Quick Start document to get your first creature up and 
			running.<BR><BR>Once your ready to go full steam into creature development you 
			can use the Object Model reference to examine the methods and classes available 
			to your creature and the Advanced Developer Documentation for discussions on 
			various aspects of creature behavior. We've also provided an XML doc file that 
			can be used with VS .NET to enable intellisense. </DIV><BR><A 
			name=8_1_5><B>8.1.5 Are there any creature templates available?</B></A> 
			<DIV>Yes there are. The Quick Start contains templates for C# and VB for 
			Herbivores, Carnivores, and Plants. These are very basic templates that 
			represent playable creatures. You may also check the Animal Farm, as some users 
			have submitted more complex templates for advanced creature development. 
			</DIV><BR><A name=8_1_6><B>8.1.6 How do I workaround the error "A *Creature* was 
			destroyed because it was blacklisted due to past bad behavior and won't be 
			loaded" ?</B></A> 
			<DIV>The error may be legitimate, the animal may have done an illegal operation 
			and was killed (like an endlessly recursive function). It may also be due to the 
			fact that the animal was introduced from the Terrarium/Bin folder. If this is 
			the case, rename and rebuild the animal and introduce it from another folder. 
			</DIV><BR><A name=8_1_7><B>8.1.7 How do I workaround the error "Your organism 
			*SomeAnimalName* could not be found in the assembly. Please make sure this class 
			exists." when developing an animal in VB.NET?</B></A> 
			<DIV>VB Projects add a default namespace. You need to right-click on your 
			project, go to properties, and clear out the default namespace. </DIV><BR><A 
			name=8_1_8><B>8.1.8 Why am I getting "Assembly failed validation process. Please 
			consult rules for allowed constructs" when I try to introduce an animal? 
			</B></A>
			<DIV>For security reasons, we do not allow certain constructs, if your animal 
			contains any of the following, you will not be allowed to introduce that 
			animal:<BR><BR><B>Calls to or derivation from any member of any of the following 
			list of types:</B> <PRE>    WCHAR* bannedTypes[] = {
					L"System.Threading.Thread",
					L"System.Threading.ThreadPool",
					L"System.Activator",
					L"System.Threading.Timer",
					L"System.Threading.Mutex",
					L"System.Threading.Monitor",
					L"System.AppDomain",
					L"System.Threading.WaitHandle",
					L"System.GC",
					L"System.LocalDataStoreSlot",
					L"System.Security.SecurityManager",
					L"System.Windows.Forms.MessageBox",
					L"System.Reflection.Assembly",
					L"System.Runtime.Remoting.CallContext",
					L"System.Security.Principal",
					L"System.Diagnostics.Debug",
					L"System.Diagnostics.Debugger",
					L"System.Drawing.Graphics",
					L"System.Reflection.Binder",
					L"System.Reflection.MemberInfo",
					L"System.Reflection.MethodInfo",
					L"System.Reflection.FieldInfo",
					L"System.Security.Cryptography.SymmetricAlgorithm",
					L"System.Security.Cryptography.AsymmetricAlgorithm",
					L"System.Console",
					L"System.Diagnostics.Process",
				};
			</PRE><B>The following aren't allowed:</B><BR>Static members other than readonly 
			fields<BR>Class constructors<BR>Setting any static field anywhere (ban on the 
			stsfld opcode)<BR>Unmanaged assemblies<BR><BR><B>The security policy 
			grants:</B><BR>SecurityPermissionFlag.Execution (anything that requires any 
			permission other than this fails at runtime)<BR><BR><B>You can't run Terrarium 
			if:</B><BR>Security is turned off<BR>Strong name validation is disabled 
			</DIV><BR><A name=8_2_1><B>8.2.1 What is the difference between bigger and 
			smaller creatures?</B></A> 
			<DIV>The following table lists a series of properties along with their values 
			given a bigger or smaller creature: 
			<TABLE width="80%" border=1>
			<TBODY>
			<TR>
			<TD width="20%"><B>Property</B></TD>
			<TD width="40%"><B>Small Creature</B></TD>
			<TD width="40%"><B>Big Creature</B></TD></TR>
			<TR>
			<TD vAlign=top>Growth</TD>
			<TD vAlign=top>Smaller creatures grow to mature size sooner than larger 
			creatures. However, because they don't have as many growth steps to take, they 
			also have a longer period between growth steps meaning they have more time to 
			find food and prepare for growing. This leads to better prepared creatures that 
			don't get stunted in their growth.</TD>
			<TD vAlign=top>Larger creatures grow to mature size later than smaller 
			creatures. Because they have many growth steps to take they have a shorter 
			period between growth steps. This means larger creatures have to be more 
			prepared for growth times to avoid being stunted in their growth. This makes the 
			first few ticks of a larger creature's life much more critical.</TD></TR>
			<TR>
			<TD vAlign=top>Reproduction</TD>
			<TD vAlign=top>Smaller creatures reach maturity faster. This means they can 
			reproduce sooner in game ticks than larger creatures. Even though they can 
			reproduce more quickly, they still reproduce at the same time percentage wise in 
			their lifespans, and they reproduce the same amount of times as a larger 
			creature.</TD>
			<TD vAlign=top>Larger creatures reach maturity quite slowly. This means they can 
			reproduce only half as often as a much smaller creature. Even though they 
			reproduce more slowly, they still reproduce at the same time percentage wise in 
			their lifespans, and they reproduce the same amount of times in their lifespans 
			as a smaller creature.</TD></TR>
			<TR>
			<TD vAlign=top>Combat</TD>
			<TD vAlign=top>All combat modifiers are linked directly to the radius of the 
			creature. This gives smaller creatures a big disadvantage in combat since their 
			radius remains small.</TD>
			<TD vAlign=top>Larger creatures get great combat advantages once they start 
			approaching their mature size. Since the combat modifiers are based on current 
			radius, newly born small and large creatures are on the same footing with 
			regards to combat until the larger creature is able to grow in radius and gain a 
			combat advantage.</TD></TR>
			<TR>
			<TD vAlign=top>Movement</TD>
			<TD vAlign=top>Smaller creatures can store less energy than larger creatures. 
			This means that when a smaller creature is moving it will run out of energy more 
			quickly than a larger creature. At the same time smaller creatures can fit 
			through more confined spaces giving them an intricate movement advantage.</TD>
			<TD vAlign=top>Larger creatures store more energy than smaller creatures. This 
			means that a larger creature can travel much farther than smaller creatures 
			without running out of energy. At the same time larger creatures can't fit 
			through confined spaces and so have a more difficult time navigating between 
			plants and around other creatures.</TD></TR></TBODY></TABLE></DIV><BR><A 
			name=8_2_2><B>8.2.2 Why are there more herbivores than carnivores?</B></A> 
			<DIV>Each carnivore in an EcoSystem is required to eat approximately 4 
			herbivores into order to gain the energy that it will use throughout its life. 
			This points to a generic equation that there can be 1 carnivore for every 4 
			herbivores. In the real EcoSystem these numbers start to spread quite a bit more 
			since it is difficult for the average carnivore to obtain the 1 to 4 ratio. In 
			actuality it is more like a 1:10 ratio. Hopefully as both Herbivores and 
			Carnivores become more sophisticated a more exact number can be locked down. 
			</DIV><BR><A name=8_2_3><B>8.2.3 How do I stock my Terrarium?</B></A> 
			<DIV>You can stock your terrarium in a couple of different ways. First, you 
			should check the organism cache for your EcoSystem. This will be located in the 
			following location: <PRE>%USERPROFILE%\Application Data\Microsoft\Terrarium</PRE>Beneath that 
			directory should be many other directories. You can pull the assemblies out of 
			those directories and use them to stock your Terrarium. If you still don't have 
			enough or didn't find the creature you wanted you can pull them off of the 
			server. When you use the Introduce dialog you can click the "Server List" button 
			(v1.0.21.281 or later), and a list of creatures that can be added to your 
			Terrarium will be displayed. Add as many of these creatures as you would like. 
			</DIV><BR><A name=8_2_4><B>8.2.3 Where can I find EcoSystem creatures?</B></A> 
			<DIV>EcoSystem creatures are cached on your hard drive. Please view FAQ item 
			"How do I stock my Terrarium?" for more information on locating this cache. 
			</DIV><BR><A name=8_3_1><B>8.3.1 Why do so many creatures die from 
			sickness?</B></A> 
			<DIV>Creatures die from sickness because too many creatures on one machine 
			causes a huge performance hit for the game. When the creature count begins to 
			get too high, the game decides to kill off old creatures for sickness. First 
			populations greater than 10 are culled. If numbers are still to high, species 
			are selected at random until the count is below a certain amount.<BR><BR>In the 
			new versions of the Terrarium (1.0.21.281 and later) Carnivores are exempted 
			from this sickness. Hopefully, having more carnivores around will help to 
			naturally cull populations rather than having to resort to the sickness 
			algorithm. </DIV><BR><A name=8_3_2><B>8.3.2 Why is my creature getting blocked 
			all the time?</B></A> 
			<DIV>The Terrarium uses grid locking to determine the space occupied by a 
			creature. The Advanced Developer Documentation has information on how to compute 
			bounding boxes based on this grid system. The Animal Farm also has user 
			submitted code that helps aleviate the blocking issues.<BR><BR>In the new 
			versions of the Terrarium (1.0.21.281 and later) you can turn on bounding boxes 
			for creatures by right-clicking the Terrarium and choosing "Game Settings". The 
			"Graphics" tab will display a series of check boxes for enabling different 
			graphics features, one of which is used to display bounding boxes for creatures. 
			These should help ease the development process for path finding algorithms. 
			</DIV><BR><A name=8_4_1><B>8.4.1 What type of information can I use with 
			WriteTrace?</B></A> 
			<DIV>You can use any string type information with WriteTrace. Make sure that 
			your strings don't extend past 8000 characters or they will be trimmed by the 
			API to 8000 characters causing a loss in data. </DIV><BR><A name=8_4_2><B>8.4.2 
			Why do WriteTrace calls come out backwards?</B></A> 
			<DIV>The Trace Window is implemented in a down to top manner meaning any new 
			traces are added to the beginning rather than the end of the textbox. This 
			causes sequential calls to WriteTrace to appear backwards.<BR><BR>In the new 
			versions of the Terrarium (1.0.21.281 and later) you can use an external debug 
			output monitoring tool to trap traces instead of using the Trace Window. In this 
			configuration traces will come out in sequential order and will not appear 
			backwards. To turn this feature on right-click the Terrarium and choose "Game 
			Settings". The "Performance" tab will display some radio buttons that can be 
			used to control how traces are output to an external debug monitoring 
			tool.<BR><BR>You can use VS as the debug monitoring tool. You may also find some 
			freeware programs on the Internet that are capable of monitoring and logging 
			output from the debug stream. </DIV><BR><A name=8_4_3><B>8.4.3 Can I use the VS 
			.NET Debugger?</B></A> 
			<DIV>Yes, with some setup of the Terrarium client you can use the VS .NET 
			debugger. See FAQ item "How do I use the VS .NET Debugger" for information on 
			how to set the debugger up for use with Terrarium. </DIV><BR><A 
			name=8_4_4><B>8.4.4 How do I use the VS .NET Debugger?</B></A> 
			<DIV>The following steps detail the tested and supported method for using the VS 
			.NET debugger. These steps should work under all circumstances and environments. 
			There are other known methods that do work, but they tend to not work 100% of 
			the time. 
			<OL>
			<LI>Open up VS .NET to your creature's source file. Set any breakpoints you'll 
			want to hit during program execution. This can be done later, as well, once the 
			rest of the steps are performed. 
			<LI>Compile your creature into a new assembly. Make sure your creature is 
			compiled using the Debug configuration so that a .pdb file is produced. 
			<LI>Go to the build location (generally bin\Debug) and verify that both your 
			creature assembly exists (example: creature.dll), and the .pdb file 
			(creature.pdb). It is important that the names match up. 
			<LI>Open the Terrarium and start a new Terrarium game. This can not be an 
			EcoSystem game since debugging is prohibited in the game logic and your peer may 
			be shut down. 
			<LI>Introduce your creature into the Terrarium and immediately pause the game. 
			<LI>Close the Terrarium down and save the current Terrarium game out to file. 
			<LI>Open the Terrarium again and start the previously saved Terrarium. Make sure 
			to pause it again so too many ticks don't pass you by. 
			<LI>Go into the VS .NET IDE and select Tools-&gt;Debug Processes. 
			<LI>A dialog will pop up. Select the terrarium.exe process (make sure not to 
			select the terrariumstart.exe process if it exists) and click Attach. 
			<LI>A new dialog will pop up. Make sure Common Language Runtime is selected to 
			enabled .NET debugging. 
			<LI>You can close down the Processes dialog now, and unpause the Terrarium. Your 
			breakpoints should be hit as they are executed and you can now step through your 
			creature's code. </LI></OL><I>Note: There shortcuts and other means of debugging 
			your creatures. These ways may appear easier, faster, and may even work under 
			your configuration. If you find others that work for you feel free to post them 
			on the discussion boards for others. If we find a simpler process that works on 
			all configurations we'll update this FAQ item.</I> </DIV><BR><A 
			name=8_4_5><B>8.4.5 Can I copy my new creature over top of my old creature when 
			debugging?</B></A> 
			<DIV>This is not recommended. Copying your new creature over top of the old 
			creature does change the assembly used for debugging purposes. However, it does 
			not change the state information that is cached inside of the saved terrarium 
			file. This can cause your creature to be loaded using old attribute values 
			rather than any new values you might have changed. </DIV>										</td>
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
