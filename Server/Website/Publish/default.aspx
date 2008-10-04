<%@ Register TagPrefix="controls" TagName="FooterBar" Src="~/Controls/FooterBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="HeaderBar" Src="~/Controls/HeaderBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="InfoBar" Src="~/Controls/InfoBar.ascx" %>
<%@ Register TagPrefix="controls" TagName="MenuBar" Src="~/Controls/MenuBar.ascx" %>

<%@ Page Language="c#" %>

<%--
//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------
--%>
<html>
<head>
    <title>Terrarium Whidbey</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="../theme.css">
    <style type="text/css">
<!--
TD
{
    font-size: 1.0em;
}


.divTag {border:1px; border-style:solid; background-color:#FFFFFF; text-decoration:none; height:auto; width:auto; background-color:#cecece}
.BannerColumn {background-color:#000000}
.Banner {border:0; padding:0; height:8px; margin-top:0px; color:#ffffff; filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=1,StartColorStr='#1c5280',EndColorStr='#FFFFFF');}
.BannerTextCompany {font:bold; font-size:18pt; color:#cecece; font-family:Tahoma; height:0px; margin-top:0; margin-left:8px; margin-bottom:0; padding:0px; white-space:nowrap; filter:progid:DXImageTransform.Microsoft.dropshadow(OffX=2,OffY=2,Color='black',Positive='true');}
.BannerTextApplication {font:bold; font-size:18pt; font-family:Tahoma; height:0px; margin-top:0; margin-left:8px; margin-bottom:0; padding:0px; white-space:nowrap; filter:progid:DXImageTransform.Microsoft.dropshadow(OffX=2,OffY=2,Color='black',Positive='true');}
.BannerText {font:bold; font-size:18pt; font-family:Tahoma; height:0px; margin-top:0; margin-left:8px; margin-bottom:0; padding:0px; filter:progid:DXImageTransform.Microsoft.dropshadow(OffX=2,OffY=2,Color='black',Positive='true');}
.BannerSubhead {border:0; padding:0; height:16px; margin-top:0px; margin-left:10px; color:#ffffff; filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=1,StartColorStr='#4B3E1A',EndColorStr='#FFFFFF');}
.BannerSubheadText {font:bold; height:11px; font-size:11px; font-family:Tahoma; margin-top:1; margin-left:10; filter:progid:DXImageTransform.Microsoft.dropshadow(OffX=2,OffY=2,Color='black',Positive='true');}
.FooterRule {border:0; padding:0; height:1px; margin:0px; color:#ffffff; filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=1,StartColorStr='#4B3E1A',EndColorStr='#FFFFFF');}
.FooterText {font-size:11px; font-weight:normal; text-decoration:none; font-family:Verdana; margin-top:10; margin-left:0px; margin-bottom:2; padding:0px; color:#999999; white-space:nowrap}
.FooterText A:link {font-weight:normal; color:#999999; text-decoration:underline}
.FooterText A:visited {font-weight:normal; color:#999999; text-decoration:underline}
.FooterText A:active {font-weight:normal; color:#999999; text-decoration:underline}
.FooterText A:hover {font-weight:normal; color:#FF6600; text-decoration:underline}
.ClickOnceInfoText {font-size:11px; font-weight:normal; text-decoration:none; font-family:Tahoma; margin-top:0; margin-right:2px; margin-bottom:0; padding:0px; color:#000000}
.InstallTextStyle {font:bold; font-size:14pt; font-family:Tahoma; a:#FF0000; text-decoration:None}
.DetailsStyle {margin-left:30px}
.ItemStyle {margin-left:-15px; font-weight:bold}
.StartColorStr {background-color:#4B3E1A}
.JustThisApp A:link {font-weight:normal; color:#000066; text-decoration:underline}
.JustThisApp A:visited {font-weight:normal; color:#000066; text-decoration:underline}
.JustThisApp A:active {font-weight:normal; text-decoration:underline}
.JustThisApp A:hover {font-weight:normal; color:#FF6600; text-decoration:underline}
-->

</style>

    <script language="JavaScript">
<!--
runtimeVersion = "2.0.0";
directLink = "Terrarium.application";


function window::onload()
{
  if (HasRuntimeVersion(runtimeVersion))
  {
    InstallButton.href = directLink;
    BootstrapperSection.style.display = "none";
  }
}
function HasRuntimeVersion(v)
{
  var va = GetVersion(v);
  var i;
  var a = navigator.userAgent.match(/\.NET CLR [0-9.]+/g);
  if (a != null)
    for (i = 0; i < a.length; ++i)
      if (CompareVersions(va, GetVersion(a[i])) <= 0)
		return true;
  return false;
}
function GetVersion(v)
{
  var a = v.match(/([0-9]+)\.([0-9]+)\.([0-9]+)/i);
    return a.slice(1);
}
function CompareVersions(v1, v2)
{
  for (i = 0; i < v1.length; ++i)
  {
    var n1 = new Number(v1[i]);
    var n2 = new Number(v2[i]);
    if (n1 < n2)
      return -1;
    if (n1 > n2)
      return 1;
  }
  return 0;
}

-->
    </script>

</head>
<body>
    <!-- BEGIN CENTER ALIGNMENT TABLE -->
    <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
        <tr>
            <td align="center" valign="top">
                <table width="80%" height="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <!-- LEFT SHADOW -->
                        <td width="30px">
                            <asp:Image ImageUrl="~/images/border_left.png" Width="30px" Height="100%" runat="server"
                                ID="Image2" /></td>
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
                                                                    Install Terrarium</div>
                                                                <br />
                                                                <div align="left">
                                                                    From this page you can install Terrarium by using Click-Once. Once deployed,
                                                                    the application will keep itself updated automatically.</div>
                                                                <br />
                                                                <table width="100%" cellpadding="0" cellspacing="2" border="0">
                                                                    <!-- Begin Dialog -->
                                                                    <tr>
                                                                        <td align="LEFT">
                                                                            <table cellpadding="2" cellspacing="0" border="0" width="540">
                                                                                <tr>
                                                                                    <td width="496">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    <b>Name:</b></td>
                                                                                                <td width="5">
                                                                                                    <spacer type="block" width="10" />
                                                                                                </td>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    Terrarium Whidbey</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    <b>Version:</b></td>
                                                                                                <td width="5">
                                                                                                    <spacer type="block" width="10" />
                                                                                                </td>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    2.0.50522.8</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    <b>Publisher:</b></td>
                                                                                                <td width="5">
                                                                                                    <spacer type="block" width="10" />
                                                                                                </td>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    Terrarium Whidbey</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <!-- End AppInfo -->
                                                                                        <br />
                                                                                        <!-- Begin Prerequisites -->
                                                                                        <table id="BootstrapperSection" border="0">
                                                                                            <tr>
                                                                                                <td style="font-size: 0.75em;" colspan="2">
                                                                                                    The following prerequisites are required:</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="10">
                                                                                                    &nbsp;</td>
                                                                                                <td style="font-size: 0.75em;">
                                                                                                    <ul>
                                                                                                        <li>.NET Framework 2.0 Beta</li>
                                                                                                    </ul>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="font-size: 0.75em;" colspan="2">
                                                                                                    If these components are already installed, you can <span class="JustThisApp"><a href="Terrarium.application">
                                                                                                        launch</a></span> the application now. Otherwise, click the button below to
                                                                                                    install the prerequisites and run the application.
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    &nbsp;</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <!-- End Prerequisites -->
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <!-- Begin Buttons -->
                                                                        <tr>
                                                                            <td align="LEFT">
                                                                                <table cellpadding="2" cellspacing="0" border="0" width="540" style="cursor: hand"
                                                                                    onclick="window.navigate(InstallButton.href)">
                                                                                    <tr>
                                                                                        <td width="75" align="LEFT">
                                                                                            <table cellpadding="1" bgcolor="#333333" cellspacing="0" border="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table cellpadding="1" bgcolor="#cecece" cellspacing="0" border="0">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <table cellpadding="1" bgcolor="#efefef" cellspacing="0" border="0">
                                                                                                                        <tr>
                                                                                                                            <td width="20">
                                                                                                                                <spacer type="block" width="20" height="1" />
                                                                                                                            </td>
                                                                                                                            <td style="font-size: 0.70em; height: 24px;">
                                                                                                                                <a id="InstallButton" href="setup.exe"><b>Install</b></a>
                                                                                                                            </td>
                                                                                                                            <td width="20">
                                                                                                                                <spacer type="block" width="20" height="1" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td width="15%" align="right" />
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <!-- End Buttons -->
                                                        <!-- End Dialog -->
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
                <asp:Image ID="imgRightBorder" ImageUrl="~/images/border_right.png" Width="30px" Height="100%" runat="server" />
            </td>
                    </tr>
                </table>
    <!-- END CENTER ALIGNMENT TABLE -->
            </td>
        </tr>
    </table>
</body>
</html>
