<%@ Page Language="c#" Inherits="Terrarium.Server.Charts.ChartNew" CodeFile="default.aspx.cs" %>
<%@ register tagprefix="controls" tagname="FooterBar" src="~/Controls/FooterBar.ascx" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ Import Namespace="Terrarium.Server" %>
<%@ Import Namespace="System.Data.SqlTypes" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System" %>
<HTML>
	<HEAD>
		<title></title>
		<link rel="stylesheet" type="text/css" href="../theme.css">
	</HEAD>
	<body>
		<!-- BEGIN CENTER ALIGNMENT TABLE -->
		<table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
			<tr>
				<td align="center" valign="top">
					<table width="80%" height="100%" border="0" cellpadding="0" cellspacing="0">
						<TBODY>
							<tr>
								<!-- LEFT SHADOW -->
								<td width="30"><asp:Image ImageUrl="~/images/border_left.png" Width="30px" Height="100%" Runat="server" /></td>
								<td bgcolor="#ffffff" width="*" valign="top">
									<!-- MAIN LAYOUT -->
									<table width="100%" border="0" cellspacing="0" cellpadding="0">
										<!-- BEGIN TITLE BAR AREA -->
										<tr>
											<td colspan="4" class="TitleBar">
												<controls:HeaderBar id="HeaderBar1" RunAt="server" height="64px" />
											</td>
										</tr>
										<tr>
											<td colspan="2" height="16" />
										<!-- END TITLE BAR AREA -->
										<tr>
											<td width="24" height="100%">&nbsp;</td>
											<!-- BEGIN CONTENT AREA -->
											<td width="*" valign="top">
												<table border="0" cellspacing="0" cellpadding="0" height="320" width="100%">
													<tr>
														<td class="MainBar">
															<form runat="server">
																<table width="100%" border="0" cellspacing="0" cellpadding="4">
																	<tr>
																		<td width="*" class="ChartTitle"><span>Full Creature List</span></td>
																	</tr>
																	<tr>
																		<td valign="top" width="*" align="center">
																			<ASP:DataGrid id="notIncluded" runat="server" Width="100%" AllowPaging="true" AllowSorting="true"
																				PageSize="10" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Right" PagerStyle-NextPageText="Next"
																				PagerStyle-PrevPageText="Prev" OnPageIndexChanged="notIncluded_Page" BorderColor="black" BorderWidth="1"
																				GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" HeaderStyle-Font-Bold="True"
																				Font-Size="0.7em" HeaderStyle-BackColor="#808080" AlternatingItemStyle-BackColor="#f0f0f0"
																				ItemStyle-BackColor="#FFFFFF" ItemStyle-Font-Bold="False" AlternatingItemStyle-Font-Bold="False"
																				AutoGenerateColumns="false" OnItemCommand="notIncluded_Command" OnSortCommand="notIncluded_Sort">
																				<PagerStyle Mode="NumericPages" HorizontalAlign="Left" />
																				<Columns>
																					<asp:ButtonColumn CommandName="Add" Text="Add" HeaderText="Charting" HeaderStyle-Width="100px" HeaderStyle-ForeColor="white" />
																					<asp:TemplateColumn HeaderStyle-Width="100px">
																						<HeaderTemplate>
																							<asp:LinkButton runat="server" style="color: white; text-align: center;" CommandName="Sort" CommandArgument="Population" Text='<%# MakeHeader("Population") %>' />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<%# DataBinder.Eval(Container.DataItem, "Population") %>
																						</ItemTemplate>
																					</asp:TemplateColumn>
																					<asp:TemplateColumn HeaderStyle-Width="125px">
																						<HeaderTemplate>
																							<asp:LinkButton runat="server" style="color: white; text-align: center;" CommandName="Sort" CommandArgument="SpeciesName" Text='<%# MakeHeader("Species Name") %>' />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<%# HttpUtility.HtmlEncode((string) DataBinder.Eval(Container.DataItem, "SpeciesName")) %>
																						</ItemTemplate>
																					</asp:TemplateColumn>
																					<asp:TemplateColumn>
																						<HeaderTemplate>
																							<asp:LinkButton runat="server" style="color: white; text-align: center;" CommandName="Sort" CommandArgument="AuthorName" Text='<%# MakeHeader("Author Name") %>' />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<%# HttpUtility.HtmlEncode((string) DataBinder.Eval(Container.DataItem, "AuthorName")) %>
																						</ItemTemplate>
																					</asp:TemplateColumn>
																					<asp:TemplateColumn>
																						<HeaderTemplate>
																							<asp:LinkButton runat="server" style="color: white; text-align: center;" CommandName="Sort" CommandArgument="Type" Text='<%# MakeHeader("Type") %>' />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<%# DataBinder.Eval(Container.DataItem, "Type") %>
																						</ItemTemplate>
																					</asp:TemplateColumn>
																				</Columns>
																			</ASP:DataGrid>
																		</td>
																	</tr>
																	<tr>
																		<td align="left" valign="middle">
																			<table border="0" cellpadding="0" cellspacing="0">
																				<tr>
																					<td align="center" valign="top">
																						<span class="ChartLabel">Filter Column:</span>
																					</td>
																					<td>&nbsp;</td>
																					<td align="center" valign="middle">
																						<asp:DropDownList CssClass="ChartValue" id="FilterColumn" runat="server">
																							<asp:ListItem>Population</asp:ListItem>
																							<asp:ListItem>SpeciesName</asp:ListItem>
																							<asp:ListItem>AuthorName</asp:ListItem>
																							<asp:ListItem>Type</asp:ListItem>
																						</asp:DropDownList>
																					</td>
																					<td>&nbsp;</td>
																					<td align="center" valign="top">
																						<span class="ChartLabel">Filter:</span>
																					</td>
																					<td>&nbsp;</td>
																					<td align="center" valign="middle">
																						<asp:TextBox CssClass="ChartValue" id="FilterValue" runat="server" />
																					</td>
																					<td>&nbsp;</td>
																					<td align="center" valign="top">
																						<span class="ChartLabel">Version:</span>
																					</td>
																					<td>&nbsp;</td>
																					<td align="center" valign="middle">
																						<asp:DropDownList id="Version" runat="server" CssClass="ChartValue"></asp:DropDownList>
																					</td>
																					<td>&nbsp;&nbsp;</td>
																					<td align="center" valign="middle">
																						<asp:Button id="applyFilterButton" Text="Apply Filter" OnClick="applyFilterButton_Click" runat="server"
																							CssClass="ChartLabel" Height="24" />
																					</td>
																				</tr>
																			</table>
																		</td>
																	</tr>
																</table>
																</SPAN> 
																<!-- Selected Species Row -->
																<p>
																	<span class="Panel">
																		<table width="100%" border="0" cellspacing="0" cellpadding="4">
																			<tr>
																				<td colspan="3" width="*" class="ChartTitle"><span>Selected Creatures</span></td>
																			</tr>
																			<tr>
																				<td width="*" valign="top" colspan="3">
																					<ASP:DataGrid id="included" runat="server" Width="100%" BorderColor="black" BorderWidth="1" GridLines="Both"
																						CellPadding="3" CellSpacing="0" Font-Name="Verdana" Font-Size="0.7em" HeaderStyle-Font-Bold="True"
																						HeaderStyle-BackColor="#808080" AlternatingItemStyle-BackColor="#F0F0F0" ItemStyle-BackColor="#FFFFFF"
																						AutoGenerateColumns="false" OnItemCommand="included_Command">
																						<Columns>
																							<asp:TemplateColumn HeaderText="Charting" HeaderStyle-Width="100px" HeaderStyle-ForeColor="white">
																								<ItemTemplate>
																									<asp:LinkButton runat="server" CommandName="Remove" Text="Remove" /><br>
																									<asp:LinkButton runat="server" CommandName="Chart" Text="Chart Vitals" />
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:BoundColumn HeaderText="Population" HeaderStyle-Width="100px" HeaderStyle-ForeColor="White"
																								DataField="Population" />
																							<asp:BoundColumn HeaderText="Species Name" HeaderStyle-Width="125px" HeaderStyle-ForeColor="White"
																								DataField="SpeciesName" />
																							<asp:BoundColumn HeaderText="Births" HeaderStyle-ForeColor="White" DataField="BirthCount" />
																							<asp:BoundColumn HeaderText="Starved" HeaderStyle-ForeColor="White" DataField="StarvedCount" />
																							<asp:BoundColumn HeaderText="Killed" HeaderStyle-ForeColor="White" DataField="KilledCount" />
																							<asp:BoundColumn HeaderText="Errors" HeaderStyle-ForeColor="White" DataField="ErrorCount" />
																							<asp:BoundColumn HeaderText="Timeouts" HeaderStyle-ForeColor="White" DataField="TimeoutCount" />
																							<asp:BoundColumn HeaderText="Sick" HeaderStyle-ForeColor="White" DataField="SickCount" />
																							<asp:BoundColumn HeaderText="Old" HeaderStyle-ForeColor="White" DataField="OldAgeCount" />
																							<asp:BoundColumn HeaderText="Security Violation" HeaderStyle-ForeColor="White" DataField="SecurityViolationCount" />
																						</Columns>
																					</ASP:DataGrid>
																				</td>
																			</tr>
																			<tr>
																				<td align="left" valign="middle">
																					<table border="0" cellpadding="0" cellspacing="0">
																						<tr>
																							<td align="center" valign="top">
																								<span class="ChartLabel">Start Time:</span>
																							</td>
																							<td>&nbsp;</td>
																							<td align="center" valign="middle">
																								<asp:DropDownList id="StartTime" runat="server" CssClass="ChartValue" AutoPostBack="true">
																									<asp:ListItem>Last 24 hours</asp:ListItem>
																									<asp:ListItem>Select Date</asp:ListItem>
																								</asp:DropDownList>
																								<asp:Calendar id="Calendar1" runat="server" BorderColor="Black">
																									<DayStyle CssClass="CalendarDay"></DayStyle>
																									<DayHeaderStyle CssClass="CalendarDayHeader"></DayHeaderStyle>
																									<SelectedDayStyle CssClass="CalendarSelectedDay"></SelectedDayStyle>
																									<TitleStyle CssClass="CalendarTitle"></TitleStyle>
																								</asp:Calendar>
																							</td>
																							<td>&nbsp;&nbsp;</td>
																							<td align="center" valign="middle">
																							</td>
																						</tr>
																					</table>
																				</td>
																			</tr>
																		</table>
																	</span>
																	<!-- The Chart -->
																	<table width="100%" border="0">
																		<tr>
																			<td align="center" valign="middle" height="32">
																				&nbsp;
																			</td>
																		</tr>
																		<tr>
																			<td align="center" valign="middle">
																				<table border="0" style="BORDER-RIGHT: black 1pt solid; BORDER-TOP: black 1pt solid; BORDER-LEFT: black 1pt solid; BORDER-BOTTOM: black 1pt solid"
																					cellpadding="0" cellspacing="0">
																					<tr>
																						<td><asp:Image id="Chart" runat="server"></asp:Image></td>
																					</tr>
																				</table>
																			</td>
																		</tr>
																	</table>
															</form>
															</P>
														</td>
													</tr>
												</table>
											</td>
											<!-- END CONTENT AREA -->
											<!-- BEGIN RIGHT MENU BAR -->
											<td class="MenuBar" align="center" valign="top">
												<controls:InfoBar id="InfoBar1" runat="server" />
											</td>
											<td width="24" height="100%">&nbsp;</td>
											<!-- END RIGHT MENU BAR -->
										</tr>
										<tr>
											<td colspan="4">
												<controls:FooterBar id="FooterBar1" runat="server" />
											</td>
										</tr>
									</table>
								</td>
								<td width="30"><asp:Image ImageUrl="~/images/border_right.png" Width="30px" Height="100%" Runat="server" /></td>
							</tr>
			</tr>
		</table>
		</TD></TR></TBODY></TABLE> 
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
