<%@ Page Language="c#" AutoEventWireup="false" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<HTML xmlns:o>
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
					<table border="0" cellpadding="0" cellspacing="4" width="100%" ID="Table1">
						<!-- BEGIN TITLE BAR AREA -->
						<tr>
							<td colspan="3" class="TitleBar">
								<controls:HeaderBar id="HeaderBar1" RunAt="server" />
							</td>
						</tr>
						<!-- END TITLE BAR AREA -->
						<tr>
							<!-- BEGIN LEFT MENU BAR -->
							<td class="MenuBar" align="center" valign="top" width="160">
								<controls:MenuBar id="menuBar" runat="server" />
							</td>
							<!-- END LEFT MENU BAR -->
							<!-- BEGIN CONTENT AREA -->
							<td width="*" valign="top">
								<table border="0" cellspacing="0" cellpadding="0" height="320" width="100%">
									<tr>
										<td class="MainBar">
											<H4>Terrarium Tools and Configuration Whitepaper</H4>
											<H5>Abstract</H5>
											<p>The Terrarium application has many supporting tool classes and a strongly typed 
												configuration class.<span style='mso-spacerun:yes'>&nbsp; </span>The tools 
												encapsulate methods for high performance timers, various safe native methods 
												that are used by different portions of the Terrarium application, and a special 
												set of error logging classes that are built on top of the debug trace listening 
												facilities in the CLR.<span style='mso-spacerun:yes'>&nbsp; </span>This 
												document attempts to describe the use of each of the tools provided.<span style='mso-spacerun:yes'>&nbsp;
												</span>The document also discusses the need and importance of the strongly 
												typed configuration class.</p>
											<H5>High Performance Timing and Profiling</H5>
											<p>The Terrarium defines and implements its own set of high performance timing and 
												profiling API’s.<span style='mso-spacerun:yes'>&nbsp; </span>The API’s are 
												built on top of the Windows high performance timers and are bound to the 
												Windows operating system for proper function.<span style='mso-spacerun:yes'>&nbsp; </span>
												Timing in most cases is conditionally compiled into the operation through the 
												use of the DEBUG and TRACE flags.<span style='mso-spacerun:yes'>&nbsp; </span>Some 
												performance timers are enabled constantly in order to display engine 
												performance to the user.<span style='mso-spacerun:yes'>&nbsp; </span>Generally 
												basic timings are enabled all the time, but more complex timings can be made 
												available when needed to look for engine and graphics drawing bottlenecks.<span style='mso-spacerun:yes'>&nbsp;
												</span>The timing classes are also used to time creatures.<span style='mso-spacerun:yes'>&nbsp;
												</span>Since creatures only execute for some amount of microseconds the high 
												performance timers are critical.</p>
											<p>The Terrarium implements high performance timers through the use of the <b>TimeMonitor</b>
												class.<span style='mso-spacerun:yes'>&nbsp; </span>The <b>TimeMonitor</b> class 
												encapsulates all of the code required to call the system level APIs for 
												measuring time.<span style='mso-spacerun:yes'>&nbsp; </span>These system level 
												APIs are implemented through the use of the CLR PInvoke facilities.<span style='mso-spacerun:yes'>&nbsp;
												</span>A PInvoke is declared by using the extern modifier on a method when 
												using C#.<span style='mso-spacerun:yes'>&nbsp; </span>This tells the compiler 
												that the method will be linked in at run-time.<span style='mso-spacerun:yes'>&nbsp; </span>
												Further, the DllImportAttribute needs to be applied so that the CLR knows which 
												system library to find the function in.</p>
											<P style="MARGIN-LEFT: 24px"><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;</SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>Used to query the system timer frequency.<SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>Returns counts per</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>second.<SPAN style="mso-spacerun: yes">&nbsp; </SPAN>Ref parameter is a 
													LARGE_INTEGER</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">DllImport("kernel32", 
													CharSet=CharSet.Auto)]</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
													<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
														int</SPAN> QueryPerformanceFrequency(<SPAN style="COLOR: blue">ref</SPAN> <SPAN style="COLOR: blue">
														double</SPAN> quadpart);</SPAN><br>
												<br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>Used to query the number of elapsed intervals in the system</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>timer.</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">[DllImport("kernel32", 
													CharSet=CharSet.Auto)]</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
													<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
														int</SPAN> QueryPerformanceCounter(<SPAN style="COLOR: blue">ref</SPAN> <SPAN style="COLOR: blue">
														double</SPAN> quadpart);
													<o:p></o:p></SPAN></P>
											<p>These methods can then be called inside of the <b>TimeMonitor</b> class 
												directly.<span style='mso-spacerun:yes'>&nbsp; </span>Since the <b>TimeMonitor</b>
												encapsulates the timing APIs, it provides a set of timing methods, <b>Start</b> 
												which begins timing by noting the current system tick count, and the <b>EndGetMicroseconds</b>
												method which returns the number of microseconds elapsed since the previous call 
												to the <b>Start</b> method.</p>
											<p>That is the <b>TimeMonitor</b> at its most basic level.<span style='mso-spacerun:yes'>&nbsp;
												</span>Since timing in the Terrarium application needs to be extremely precise, 
												some additional facilities are provided by the class in terms of API overhead 
												removal.<span style='mso-spacerun:yes'>&nbsp; </span>The time returned by the <b>TimeMonitor</b>
												factors out the amount of time used to call the underlying performance counter 
												APIs.<span style='mso-spacerun:yes'>&nbsp; </span>This makes the <b>TimeMonitor</b>
												that much more accurate.<span style='mso-spacerun:yes'>&nbsp; </span>In 
												addition, the constructor for the time monitor does a simple warm-up of the 
												APIs by calling the methods.<span style='mso-spacerun:yes'>&nbsp; </span>This 
												ensures all methods are JIT’ed and ready to go the first time they are called.</p>
											<p>The Terrarium doesn’t stop at the <b>TimeMonitor</b> class though.<span style='mso-spacerun:yes'>&nbsp;
												</span>It provides several other timing features through the use of the <b>Profiler</b>
												and <b>ProfilerNode</b> classes.<span style='mso-spacerun:yes'>&nbsp; </span>The 
												basic premise behind the <b>TimeMonitor</b> class is a single timing around a 
												loop or piece of code.<span style='mso-spacerun:yes'>&nbsp; </span>The <b>TimeMonitor</b>
												is only capable of monitoring a single time slice at a time.<span style='mso-spacerun:yes'>&nbsp;
												</span>For most simple usages this is enough.<span style='mso-spacerun:yes'>&nbsp; </span>
												For more complex usages, such as function timings and timings of separate parts 
												of a loop, multiple timings are needed.</p>
											<p>The <b>Profiler</b> class supports <b>Start</b> and <b>End</b> methods for doing 
												arbitrary timings.<span style='mso-spacerun:yes'>&nbsp; </span>Each node is 
												identified by a unique name, and a call to <b>Start</b> along with a successive 
												call to <b>End</b> represents a single iteration.<span style='mso-spacerun:yes'>&nbsp;
												</span>When the methods are called another time an internal counter on the node 
												is incremented and multiple timings for the same node are saved so they can 
												later be averaged out.<span style='mso-spacerun:yes'>&nbsp; </span>This means 
												you can time individual function calls, compute average execution time, examine 
												the amount of time taken by the last call, and determine how many times the 
												function was called.<span style='mso-spacerun:yes'>&nbsp; </span>All of this 
												information is useful when profiling different portions of a game engine.</p>
											<p>For a detail usage of the <b>TimeMonitor</b>, <b>Profiler</b>, and <b>ProfilerNode</b>
												classes take a look at the Graphics Engine classes.<span style='mso-spacerun:yes'>&nbsp;
												</span>The profilers are liberally used in order to examine various bottlenecks 
												in the graphics engine and figure out where improvements can be made.<span style='mso-spacerun:yes'>&nbsp;
												</span>It is also useful to figure out how many times each of the functions or 
												code blocks are called in order to minimize the amount of iteration.</p>
											<H5>PInvoke Native Methods</H5>
											<p>The Terrarium makes use of many native methods in order to provide a fully 
												functional game.<span style='mso-spacerun:yes'>&nbsp; </span>Though the 
												Terrarium could be implemented without the use of the PInvoke layers, they help 
												enable increased functionality that wouldn’t otherwise be available.<span style='mso-spacerun:yes'>&nbsp;
												</span>The first set of PInvoke native methods are used by the high speed 
												timing APIs.<span style='mso-spacerun:yes'>&nbsp; </span>They enable access to 
												the high performance timing counters available on most modern day computers.</p>
											<p>The Hosting API is augmented by a set of PInvoke calls that enable timing of 
												multiple threads.<span style='mso-spacerun:yes'>&nbsp; </span>Whenever an 
												organism is run it is run on a separate thread.<span style='mso-spacerun:yes'>&nbsp;
												</span>The thread handles are obtained using native method calls, the thread 
												handle is cloned, and finally a call to <b>GetThreadTimes</b> is made in order 
												to get the time a thread has run.<span style='mso-spacerun:yes'>&nbsp; </span>This 
												ensures that organisms are actually given time to execute on their thread.<span style='mso-spacerun:yes'>&nbsp;
												</span>Assuming that organism thread gets executed can’t be assumed in a 
												multi-threaded environment.</p>
											<p style="MARGIN-LEFT: 24px">
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>Used to get thread times for the current thread.<SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>This enables</SPAN><br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
													</SPAN>the Hosting code to time out creatures. </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
												</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;
												</SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">[DllImport("kernel32", 
													CharSet=CharSet.Auto)] </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
													<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
														int</SPAN> GetThreadTimes( </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN>IntPtr hThread, </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN><SPAN style="COLOR: blue">ref</SPAN> FILETIME 
													lpCreationTime, </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN><SPAN style="COLOR: blue">ref</SPAN> FILETIME 
													lpExitTime, </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN><SPAN style="COLOR: blue">ref</SPAN> FILETIME 
													lpKernelTime, </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN><SPAN style="COLOR: blue">ref</SPAN> FILETIME 
													lpUserTime </SPAN>
												<br>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">
														&nbsp;&nbsp;&nbsp; </SPAN>); </SPAN>
											</p>
											<P>And finally the IL scanning code is implemented as an external unmanaged 
												library.<span style='mso-spacerun:yes'>&nbsp; </span>The IL scanning API 
												exports several methods which are used to check different aspects of an 
												assembly for validity.<span style='mso-spacerun:yes'>&nbsp; </span>The <b>CheckAssembly</b>
												method can be used to report information about invalid IL constructs, the <b>CheckAssemblyWithReporting</b>
												can be used to enable XML reporting of errors found during an IL scan.<span style='mso-spacerun:yes'>&nbsp;
												</span>If an error is found the <b>Report</b> method can be called to get the 
												name of the XML file generated.</P>
											<p style="margin-left: 24px;">
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
														</SPAN>Used for assembly validation in AsmCheck
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
													</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">[DllImport("asmcheck.dll", 
														CharSet=CharSet.Unicode)]
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
														<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
															int</SPAN> CheckAssembly(String assemblyName);
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
													</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
														</SPAN>Used for assembly validation with XML reporting in AsmCheck
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
													</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">[DllImport("asmcheck.dll", 
														CharSet=CharSet.Unicode)]
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
														<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
															int</SPAN> CheckAssemblyWithReporting(String assemblyName, String xmlFile);
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
													</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;summary&gt;
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'"><SPAN style="mso-spacerun: yes">&nbsp;
														</SPAN>Used for getting the name of the XML report in AsmCheck
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">///</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: green; FONT-FAMILY: 'Courier New'">
													</SPAN><SPAN style="FONT-SIZE: 9pt; COLOR: gray; FONT-FAMILY: 'Courier New'">&lt;/summary&gt;
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">[DllImport("asmcheck.dll", 
														CharSet=CharSet.Unicode)]
														</SPAN><br/>
												<SPAN style="FONT-SIZE: 9pt; COLOR: blue; FONT-FAMILY: 'Courier New'">public</SPAN><SPAN style="FONT-SIZE: 9pt; FONT-FAMILY: 'Courier New'">
														<SPAN style="COLOR: blue">static</SPAN> <SPAN style="COLOR: blue">extern</SPAN> <SPAN style="COLOR: blue">
															string</SPAN> Report();
														</SPAN><br/>
											</p>
											<H5>Error Logging and Trace Listeners</H5>
											<p>In a large application like the 
													Terrarium error logging, error reporting, and exception handling are extremely 
													important to the development and maintenance process. At the first level, you 
													might not want any errors to get to the user without first passing through some 
													of your own processing code.<span style='mso-spacerun:yes'>&nbsp; </span>The 
													Terrarium performs this operation with application level exception handling and 
													a custom trace listener.<span style='mso-spacerun:yes'>&nbsp; </span>At a 
													higher level, you’d like to give the user the option of logging exception data 
													to either a central server or through email so that you can be notified of the 
													issue at hand.<span style='mso-spacerun:yes'>&nbsp; </span>The Watson services 
													provide this second level feature.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>To begin, all errors in Terrarium are 
													reported by using the <b>ErrorLog</b> class.<span style='mso-spacerun:yes'>&nbsp; </span>
													The <b>ErrorLog</b> class has methods for logging handled exceptions (a handled 
													exception is an exception that was expected, and so shouldn’t be an issue).<span style='mso-spacerun:yes'>&nbsp;
													</span>By logging handled exceptions the logging class can build up a context 
													of exceptions that lead up to the real exception.<span style='mso-spacerun:yes'>&nbsp;
													</span>A real exception is logged by using the <b>LogUnhandledException</b> method.<span style='mso-spacerun:yes'>&nbsp;
													</span>Exceptions logged using this method result in a special error dialog 
													that allows the user to report the issue.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>There isn’t much to talk about the <b>ErrorLog</b>
													class because it is simply easy to implement and easy to use.<span style='mso-spacerun:yes'>&nbsp;
													</span>The most complex aspect of the <b>ErrorLog</b> is synchronizing the UI 
													since it can be called from multiple threads.<span style='mso-spacerun:yes'>&nbsp; </span>
													This was accomplished through the use of a <b>Mutex</b> that only allows one 
													instance of the error form to be popped up at a time.<span style='mso-spacerun:yes'>&nbsp;
													</span>Any additional error logs forms will have to wait until the first form 
													is closed out by the user.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>The Watson dialog is simple.<span style='mso-spacerun:yes'>&nbsp;
													</span>It consists of several text boxes that can be manipulated by the user.<span style='mso-spacerun:yes'>&nbsp;
													</span>The user is allowed to send an email address with the error report so 
													that they can be easily contacted.<span style='mso-spacerun:yes'>&nbsp; </span>A 
													read-only text box is used to hold the actual error log information.<span style='mso-spacerun:yes'>&nbsp;
													</span>This is generally the stack trace for the unhandled exception, along 
													with any previous logged handled tracings (as mentioned before they can help 
													establish context).<span style='mso-spacerun:yes'>&nbsp; </span>The user is 
													also allowed to enter their own opinions on what happened in the form which 
													will be included with the final error report sent to the server.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>If the user chooses to send the 
													report to the server, a special service, the Watson Service, is invoked in 
													order to send a DataSet detailing the entire issue.<span style='mso-spacerun:yes'>&nbsp;
													</span>On the server side this DataSet is written directly into the database 
													where it can later be mined by support personnel.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>The custom trace listener is an 
													excellent feature provided by the .NET Framework.<span style='mso-spacerun:yes'>&nbsp;
													</span>Any calls to the <b>Trace</b> or <b>Debug</b> classes end up sending 
													their message information to the trace listener class.<span style='mso-spacerun:yes'>&nbsp;
													</span>The trace listener can then perform any actions based on these messages 
													that it sees fit.<span style='mso-spacerun:yes'>&nbsp; </span>In the case of 
													the Terrarium trace listener they are added to a buffer which is also sent 
													along with any error reports.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>If an error actually gets through 
													that needs to be shown to the user, then it is thrown with an Abort, Retry, or 
													Cancel set of buttons.<span style='mso-spacerun:yes'>&nbsp; </span>Depending on 
													the response, the application is either exited, or the application continues to 
													run even though an error has occurred.<span style='mso-spacerun:yes'>&nbsp; </span>
													In most instances the application will succeed to run even after an error.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<H5>Strongly Typed Configuration Class</H5>
											<p>The Terrarium application decided 
													early on to use an XML based configuration file for static persistence of 
													variables that users could change.<span style='mso-spacerun:yes'>&nbsp; </span>Rather 
													than use the application configuration file accessible through the 
													configuration APIs, the Terrarium makes use of a separate XML file and a custom 
													class to represent the configuration variables.<span style='mso-spacerun:yes'>&nbsp;
													</span>If you need to know why this method was chosen, it is because the 
													configuration APIs available through .NET don’t support read/write 
													configuration variables, instead they can only be read.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>The configuration methods are 
													responsible for loading the configuration file, setting values, and retrieving 
													values.<span style='mso-spacerun:yes'>&nbsp; </span>The methods for setting and 
													retrieving properties should never be called directly within the application.<span style='mso-spacerun:yes'>&nbsp;
													</span>Instead a static property on the configuration class is created which 
													interacts with these methods when setting or retrieving a property value.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>Each property is made static for 
													quick and easy access anywhere within the Terrarium application.<span style='mso-spacerun:yes'>&nbsp;
													</span>There are currently 3 types of configuration variables.<span style='mso-spacerun:yes'>&nbsp;
													</span>There is a cached boolean variable that is used to represent a 
													configuration switch.<span style='mso-spacerun:yes'>&nbsp; </span>Most of the 
													Terrarium variables are cached boolean values.<span style='mso-spacerun:yes'>&nbsp; </span>
													The cached boolean reads in the value string from the XML file, and attempts to 
													convert the value to boolean, and if the conversion fails, it simply sets the 
													value to a default.<span style='mso-spacerun:yes'>&nbsp; </span>Some properties 
													are simple strings and are read and written directly to the configuration file.<span style='mso-spacerun:yes'>&nbsp;
													</span>And there is currently one numeric property.<span style='mso-spacerun:yes'>&nbsp;
													</span>A numeric property has to be parsed and set to a default value I the 
													parse fails as well.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>The <b>CachedBooleanConfig</b> class 
													is a special helper class that enables working with boolean values.<span style='mso-spacerun:yes'>&nbsp;
													</span>To prevent reading from the configuration file every time the property 
													is requested, the value is only read the first time, and then cached on an 
													internal field the rest of the time.<span style='mso-spacerun:yes'>&nbsp; </span>
													This enables fast boolean look-ups without having to parse the string every 
													time, possibly throwing exceptions.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
											<p>There are also two strongly typed 
													classes for working with States and Countries.<span style='mso-spacerun:yes'>&nbsp; </span>
													These two classes <b>StateSettings</b> and <b>CountrySettings</b> can be used 
													to read states and countries from the configuration file.<span style='mso-spacerun:yes'>&nbsp;
													</span>If the value is outside of the range of the possible states and 
													countries then the value is set to &lt;Unknown&gt;.<span style='mso-spacerun:yes'>&nbsp;
													</span>This is useful in verifying that a user can’t set arbitrary values, such 
													as cusswords or inflammatory phrases.<span style='mso-spacerun:yes'>&nbsp; </span>
													If the data is strongly typed then it can be used to send to other clients and 
													can be displayed somewhere in the UI to enable neat features.<span style='mso-spacerun:yes'>&nbsp;
													</span>This was never enabled in the Terrarium itself, but it is already sent 
													as part of the TeleportState and could easily be enabled.<o:p></o:p></span></p>
											<p><o:p></o:p></span></p>
										</td>
									</tr>
								</table>
							</td>
							<!-- END CONTENT AREA -->
							<!-- BEGIN RIGHT MENU BAR -->
							<td class="MenuBar" align="center" valign="top">
								<controls:InfoBar id="InfoBar1" runat="server" />
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
