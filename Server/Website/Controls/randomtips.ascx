<%@ Control Language="c#" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Terrarium.Server" %>
<%@ OutputCache Duration="10" VaryByParam="None" %>
<%--
//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------
--%>
<script language="C#" runat="server">
	
    void Page_Load(object sender, EventArgs e)
    {
		try
		{	
			SqlConnection connection = new SqlConnection( ServerSettings.SpeciesDsn );
			connection.Open();
			
			SqlCommand command = new SqlCommand( "SELECT Id, Tip FROM RandomTips", connection );
			
			SqlDataAdapter adapter = new SqlDataAdapter(command);
			
			DataSet tipsData = new DataSet();
			adapter.Fill( tipsData, "Tips" );
			
			connection.Close();
							
			Random random = new Random( Environment.TickCount );
			int index = random.Next(tipsData.Tables["Tips"].Rows.Count);
			
			DataRow tipRow = tipsData.Tables["Tips"].Rows[index];
			
			Tip.InnerHtml = Convert.ToString(tipRow["Tip"]);
		}
		catch( Exception ex )
		{
			Tip.InnerHtml = ex.Message;
		}
    }
	 
</script>

<table border="0" width="100%">
	<tr>
		<td valign="middle" align="left" class="Tip">
			<span id="Tip" runat="server"/>
		</td>
	</tr>
</table>
