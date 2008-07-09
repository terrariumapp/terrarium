<%@ register tagprefix="controls" tagname="FooterBar" src="~/Controls/FooterBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ Page Language="c#" %>
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
								
								<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
									<!-- BEGIN TITLE BAR AREA -->
									<tr>
										<td colspan="4" class="TitleBar">
											<controls:HeaderBar id="HeaderBar1" RunAt="server" height="64px"/>
										</td>
									</tr>
									<tr><td colspan="4" height="16px">&nbsp;</td></tr>
									<!-- END TITLE BAR AREA -->
									<tr>
										<td width="24px" height="100%">&nbsp;</td>

										<!-- BEGIN CONTENT AREA -->

										<td width="*" valign="top">
											<table border="0" cellspacing="0" cellpadding="0" style="padding-left: 0px; padding-right: 16px;" width="00%">
												<tr>
													<td width="30000px" align="left" valign="top">
														<table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%" >
															<tr>
																<td>
																	<div align="left" class="SectionTitle">Getting Started</div>
																	<br/>
																	<div>There are plenty of resources available for the first time Terrarium developer.  Use the menu on the right to view the various documents, tutorials and samples.</div>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr><td>&nbsp;</td></tr>
												<tr>
													<td width="30000px" align="left" valign="top">
														<table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%" >
															<tr>
																<td>
																	<div align="left" class="SectionTitle">Referencing OrganismBase</div>
																	<br/>
																	<div>The OrganismBase assembly should be available via the Add References dialog in VS.NET 2005.</div>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr><td>&nbsp;</td></tr>
												<tr>
													<td width="30000px" align="left" valign="top">
														<table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%" >
															<tr>
																<td>
																	<div align="left" class="SectionTitle">Samples</div>
																	<br/>
																	<div>There are several samples available, including skeleton code to get you up and running fast.</div>
																</td>
															</tr>
															</table>
													</td>
												</tr>
												<tr><td>&nbsp;</td></tr>											</table>
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
