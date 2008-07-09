<%@ Control Language="c#" %>
<%@ OutputCache Duration="120" VaryByParam="None" %>
<%--
//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------
--%>

<script language="C#" runat="server">
	
    void Page_Load(object sender, EventArgs e)
    {
		int peerCount = 0;
		
		try
		{
			peerCount = TestServices();
			
			servicesResults.InnerHtml = "<span class=\"SubTitle\" style=\"Color: Green;\">Up</span>";
		}
		catch
		{
			servicesResults.InnerHtml = "<span class=\"SubTitle\" style=\"Color: Maroon;\">Down</span>";
		}
		
		try
		{
			TestDatabase();
			
			databaseResults.InnerHtml = "<span class=\"SubTitle\" style=\"Color: Green;\">Up</span>";
		}
		catch
		{
			databaseResults.InnerHtml = "<span class=\"SubTitle\" style=\"Color: Maroon;\">Down</span>";
		}

		peerResults.InnerHtml = "<span class=\"SubTitle\">" + peerCount.ToString() + "</span>";
		
    }
    
    int TestServices()
    {
		int peerCount = 0;
		
		Terrarium.Server.PeerDiscoveryService peerService = new Terrarium.Server.PeerDiscoveryService();
		peerCount = peerService.GetNumPeers( "2.0.50522", "Ecosystem" );
		
		return peerCount;				
	}
	
	void TestDatabase()
	{
		System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection( Terrarium.Server.ServerSettings.SpeciesDsn );
		connection.Open();
		connection.Close();
	}
	 
</script>

<table border="0" width="100%">
	<tr>
		<td>
			<table border="0" cellpadding="0" cellspacing="2" width="100%">
				<tr>
					<td><span class="SubTitle">Web Services:</span></td>
					<td align="center"><span id="servicesResults" runat="Server"/></td>
				</tr>
				<tr>
					<td><span class="SubTitle">Database:</span></td>
					<td align="center"><span id="databaseResults" runat="Server"/></td>
				</tr>
				<tr>
					<td><span class="SubTitle">Peers:</span></td>
					<td align="center"><span id="peerResults" runat="Server"/></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
