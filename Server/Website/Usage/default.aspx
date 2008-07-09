<%@ Page MaintainScrollPositionOnPostback="true" Language="C#" Inherits="Terrarium.Server.Reporting.Default" CodeFile="Default.aspx.cs" Debug="True" %>
<%@ register tagprefix="controls" tagname="FooterBar" src="~/Controls/FooterBar.ascx" %>
<%@ register tagprefix="controls" tagname="HeaderBar" src="~/Controls/HeaderBar.ascx" %>
<%@ register tagprefix="controls" tagname="InfoBar" src="~/Controls/InfoBar.ascx" %>
<%@ register tagprefix="controls" tagname="MenuBar" src="~/Controls/MenuBar.ascx" %>

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
										    <form runat="server">
											<table border="0" cellspacing="0" cellpadding="0" style="padding-left: 0px; padding-right: 16px;" width="00%">
												<tr>
													<td width="30000px" align="left" valign="top">
														<table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%" >
															<tr>
																<td colspan="6">
																	<div align="left" class="SectionTitle">Usage Summary</div>																	
																</td>
															</tr>
															<tr>
															    <td colspan="6">
															    <div style="height: 12px">Below are the statistics for you.</div>
															    </td>
															</tr>
															<tr>
															    <td colspan="6"  class="ReportHeader"><b>Your Statistics (<asp:Label ID="UserAliasLabel" runat="server" />)</b></td>
															</tr>
															<tr style="background-color: White">
															    <td>Today's Hours</td>
															    <td><asp:Label CssClass="NormalValue" ID="UserTodayLabel" runat="server" /></td>
															    <td>This Week's Hours</td>
															    <td><asp:Label CssClass="NormalValue" ID="UserWeekLabel" runat="server" /></td>
															    <td>Total Hours</td>
															    <td><asp:Label CssClass="NormalValue" ID="UserTotalLabel" runat="server" /></td>
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
																	<div align="left" class="SectionTitle">Top 10 Participants</div>
                                                                </td>
                                                                <td align="right" valign="middle">
                                                                    <b>Period</b>
																	<asp:DropDownList Font-Size="1.0em" ID="IndividualPeriodDropDown" runat="server" AutoPostBack="true">
																	    <asp:ListItem>Today</asp:ListItem>
																	    <asp:ListItem Selected="true">Week</asp:ListItem>
																	    <asp:ListItem>Total</asp:ListItem>
																	</asp:DropDownList>
                                                                </td>
		                                                     </tr>
															<tr>
															    <td colspan="2">
															    <div style="height: 12px">These are the top 10 participants based on total hours that they have run Terrarium.  Use the dropdown list to choose which period to view.</div>
															    </td>
															</tr>		                                                     
		                                                     <tr>
		                                                        <td colspan="2">
                                                                    <asp:DataGrid CellPadding="4" GridLines="None" Width="100%" ID="individualDataGrid" runat="server" AutoGenerateColumns="False" EnableViewState="False">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="0.75em" BackColor="White" ForeColor="Black"/>
                                                                        <AlternatingItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="0.75em" BackColor="#F0F0F0" ForeColor="Black"/>
																	    <Columns>                                            
                                                                            <asp:TemplateColumn HeaderText="Rank">
                                                                                <HeaderStyle Width="64px" />
                                                                                <ItemTemplate>
                                                                                    <span><%# (Container.DataSetIndex + 1).ToString() %></span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>                                                                                                      
                                                                                <HeaderTemplate>
                                                                                Alias
                                                                                </HeaderTemplate>
                                                                                <HeaderStyle Font-Bold="True"/>
                                                                                <ItemTemplate>
                                                                                    <span><%# GetUserUsageSummary(Container.DataItem).Alias%></span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn>                                                                                                      
                                                                                <HeaderTemplate>
                                                                                Hours
                                                                                </HeaderTemplate>
                                                                                <HeaderStyle CssClass="TypeGridNameHeader" Width="96px" />
                                                                                <ItemTemplate>
                                                                                    <span><%# GetUserUsageSummary(Container.DataItem).TotalHours%></span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#808080" Font-Bold="True" Font-Size="0.75em" ForeColor="White"
                                                                            HorizontalAlign="Left" VerticalAlign="Middle" />
																	</asp:DataGrid>
																</td>
															</tr>
														</table>
													</td>
												</tr>												
												<tr><td>&nbsp;</td></tr>
											</table>
											</form>
										</td>
										<!-- END CONTENT AREA -->

										<!-- BEGIN RIGHT MENU BAR -->
										<td valign="top">
											<table border="0" cellpadding="0" cellspacing="0">
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
							
							<td width="30px"><asp:Image ID="Image1" ImageUrl="~/images/border_right.png" Width="30px" Height="100%" Runat="server"/></td>

						</tr>
					</table>
				</td>
			</tr>
		</table>
		<!-- END CENTER ALIGNMENT TABLE -->
	</body>
</HTML>
