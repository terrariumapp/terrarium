<%@ Register TagPrefix="controls" TagName="FooterBar" Src="~/Controls/FooterBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="HeaderBar" Src="~/Controls/HeaderBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="InfoBar" Src="~/Controls/InfoBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="MenuBar" Src="~/Controls/MenuBar.ascx" %>

<%@ Page Language="c#" %>

<html>
<head>
    <title></title>
    <link rel="stylesheet" type="text/css" href="theme.css" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
</head>
<body>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
        <tr>
            <td align="center" valign="top">
                <table width="80%" height="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <!-- LEFT SHADOW -->
                        <td width="30px">
                            <asp:Image ImageUrl="~/Images/border_left.png" Width="30px" Height="100%" runat="server"
                                ID="Image2" />
                        </td>
                        <td bgcolor="#FFFFFF" width="*" valign="top">
                            <!-- MAIN LAYOUT -->
                            <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                                <!-- BEGIN TITLE BAR AREA -->
                                <tr>
                                    <td colspan="4" class="TitleBar">
                                        <controls:HeaderBar ID="HeaderBar1" runat="server" height="64px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" height="16px">
                                        &nbsp;</td>
                                </tr>
                                <!-- END TITLE BAR AREA -->
                                <tr>
                                    <td width="24px" height="100%">
                                        &nbsp;</td>
                                    <!-- BEGIN CONTENT AREA -->
                                    <td width="*" valign="top">
                                        <table border="0" cellspacing="0" cellpadding="0" style="padding-left: 0px; padding-right: 16px;"
                                            width="00%">
                                            <tr>
                                                <td width="30000px" align="left" valign="top">
                                                    <table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <div align="left" class="SectionTitle">
                                                                    Welcome to .NET Terrarium Version 2.0</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td width="30000px" align="left" valign="top">
                                                    <table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <div align="left" class="SectionTitle">
                                                                    Usage Statistics</div>
                                                                <br />
                                                                <div>
                                                                    Available now is a page that you can use to track your usage statistics. Check it
                                                                    out <a href="Usage/default.aspx"><b>now</b></a>!<br />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td width="30000px" align="left" valign="top">
                                                    <table class="SectionPanel" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <div align="left" class="SectionTitle">
                                                                    Known Issues</div>
                                                                <br />
                                                                <div>
                                                                    This is a list of known issues. This list will be updated as existing issues are
                                                                    resolved and new issues are discovered.<br />
                                                                </div>
                                                                <ul type="disc" style="margin: 4 auto 4 auto;">
                                                                    <li style="margin-bottom: 6px;"><b>Server Configuration and Setup</b><br />
                                                                        The current server is undergoing a bit of a fixup so peers and connections from
                                                                        time to time may experience outages.</li>
                                                                    <li style="margin-bottom: 6px"><b>Rendering Issues under Remote Desktop Connection</b><br />
                                                                        There are several artifacts and drawing problems that occur when running Terrarium
                                                                        via Remote Desktop Connection. Things like tool tips in the wrong spot or clipped,
                                                                        the world view being rendered incorrectly are a couple of known problems. </li>
                                                                    <li style="margin-bottom: 6;"><b>64-bit is not supported.</b><br />
                                                                        Unfortunately running on 64-bit machines is not supported at this time. This has
                                                                        to do with the COM-Interop that Terrarium uses for interfacing with DirectDraw.</li>
                                                                </ul>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <!-- END CONTENT AREA -->
                                    <!-- BEGIN RIGHT MENU BAR -->
                                    <td class="MenuBar" align="center" valign="top" width="32px">
                                        <controls:InfoBar ID="InfoBar1" runat="server" />
                                    </td>
                                    <td width="24px" height="100%">
                                        &nbsp;</td>
                                    <!-- END RIGHT MENU BAR -->
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <controls:FooterBar ID="FooterBar1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="30px">
                            <asp:Image ID="Image10" ImageUrl="~/Images/border_right.png" Width="30px" Height="100%" runat="server" /></td>
                    </tr>
                    <!-- END CENTER ALIGNMENT TABLE -->
        </tr>
    </table>
</body>
</html>
