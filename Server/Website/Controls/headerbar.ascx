<%@ Control Language="VB" ClassName="Header" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <h5 style="margin-top: 16px; margin-bottom: 5px;">
                &nbsp;.NET Terrarium Version 2.0</h5>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Image ID="Image1" ImageUrl="~/images/banner_bottom_middle.png" Height="1px"
                Width="100%" runat="server" /></td>
        <td>
            <asp:Image ImageUrl="~/images/banner_bottom_right.png" runat="server" ID="Image3" /></td>
    </tr>
    <tr>
        <td bgcolor="#989898" height="24px" colspan="2" valign="middle">
            <table border="0" cellpadding="0" cellspacing="0" height="100%">
                <tr valign="middle">
                    <td>
                        <asp:HyperLink NavigateUrl="~/default.aspx" runat="server" CssClass="MenuItem" ID="Hyperlink3">Home</asp:HyperLink>
                    </td>
                    <td>
                        <span class="MenuItemSeperator">|</span>
                    </td>
                    <td>
                        <asp:HyperLink NavigateUrl="~/Usage/default.aspx" runat="server" CssClass="MenuItem"
                            ID="Hyperlink6">Usage&nbsp;Stats</asp:HyperLink>
                    </td>
                    <td>
                        <span class="MenuItemSeperator">|</span>
                    </td>
                    <td>
                        <asp:HyperLink NavigateUrl="~/documentation/default.aspx" runat="server" CssClass="MenuItem"
                            ID="Hyperlink1">Documentation</asp:HyperLink>
                    </td>
                    <td>
                        <span class="MenuItemSeperator">|</span>
                    </td>
                    <td>
                        <asp:HyperLink NavigateUrl="~/charts/default.aspx" runat="server" CssClass="MenuItem"
                            ID="Hyperlink2">Charts</asp:HyperLink>
                    </td>
                    <td>
                        <span class="MenuItemSeperator">|</span>
                    </td>
<%--
                    <td>
                        <asp:HyperLink NavigateUrl="~/publish/default.aspx" runat="server" CssClass="MenuItem"
                            ID="Hyperlink4">Install</asp:HyperLink>
                    </td>
--%>
                </tr>
            </table>
        </td>
        <td width="128px">
            <asp:Image ID="Image6" ImageUrl="~/Images/banner_center_right.png" runat="server" Width="128px"
                Height="100%" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Image ID="Image5" ImageUrl="~/images/banner_bottom_middle.png" Height="1px" Width="100%"
                runat="server" /></td>
        <td>
            <asp:Image ImageUrl="~/images/banner_bottom_right.png" runat="server" ID="Image2" /></td>
    </tr>
</table>
