<%@ Control Language="c#" %>
<%@ register tagprefix="controls" tagname="TopCritters" src="~/Controls/TopCritters.ascx" %>
<%@ register tagprefix="controls" tagname="ServerStatus" src="~/Controls/ServerStatus.ascx" %>
<%@ register tagprefix="controls" tagname="RandomTips" src="~/Controls/RandomTips.ascx" %>
<%--
//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------
--%>
<table cellpadding="0" cellspacing="0" border="0" ID="Table7">
	<tr>
		<td class="MenuPanelSpacer" align="center"></td>
	</tr>
	<tr>
		<td align="center">
			<table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" ID="Table8">
				<tr>
					<td>
						<span class="MenuTitle">Server Status</span>
						<controls:ServerStatus id="ServerStatus1" RunAt="server"/>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="MenuPanelSpacer" align="center"></td>
	</tr>
	<tr>
		<td align="center">
			<table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" ID="Table9">
				<tr>
					<td>
						<span class="MenuTitle">Top Critters</span>
						<controls:TopCritters id="TopCritters1" RunAt="server"/>
						<asp:HyperLink NavigateUrl="~/charts/default.aspx" CssClass="SmallLink" Runat="server">more...</asp:HyperLink>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td class="MenuPanelSpacer" align="center"></td>
	</tr>
	<tr>
		<td align="center">
			<table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" ID="Table10">
				<tr>
					<td>
						<span class="MenuTitle">Random Tip</span>
						<controls:RandomTips id="RandomTips1" RunAt="server"/>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
