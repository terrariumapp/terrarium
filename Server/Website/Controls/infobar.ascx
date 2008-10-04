<%@ Control Language="c#" %>
<%@ Register TagPrefix="controls" TagName="TopCritters" Src="~/Controls/TopCritters.ascx" %>
<%@ Register TagPrefix="controls" TagName="ServerStatus" Src="~/Controls/ServerStatus.ascx" %>
<%@ Register TagPrefix="controls" TagName="RandomTips" Src="~/Controls/RandomTips.ascx" %>
<table cellpadding="0" cellspacing="0" border="0" id="Table7">
    <tr>
        <td class="MenuPanelSpacer" align="center">
        </td>
    </tr>
    <tr>
        <td align="center">
            <table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" id="Table8">
                <tr>
                    <td>
                        <span class="MenuTitle">Server Status</span>
                        <controls:ServerStatus ID="ServerStatus1" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="MenuPanelSpacer" align="center">
        </td>
    </tr>
    <tr>
        <td align="center">
            <table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" id="Table9">
                <tr>
                    <td>
                        <span class="MenuTitle">Top Critters</span>
                        <controls:TopCritters ID="TopCritters1" runat="server" />
                        <asp:HyperLink ID="Hyperlink1" NavigateUrl="~/charts/default.aspx" CssClass="SmallLink" runat="server">more...</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="MenuPanelSpacer" align="center">
        </td>
    </tr>
    <tr>
        <td align="center">
            <table class="MenuPanel" border="0" cellpadding="0" cellspacing="0" id="Table10">
                <tr>
                    <td>
                        <span class="MenuTitle">Random Tip</span>
                        <controls:RandomTips ID="RandomTips1" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
