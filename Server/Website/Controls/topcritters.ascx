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
		try
		{
		System.Text.StringBuilder buffer = new System.Text.StringBuilder( 8192 );
		AddTopOrganisms( Terrarium.Server.OrganismType.Plant, buffer );
		AddTopOrganisms( Terrarium.Server.OrganismType.Herbivore, buffer );
		AddTopOrganisms( Terrarium.Server.OrganismType.Carnivore, buffer );
				
		ContentSpan.InnerHtml = buffer.ToString();
		}
		catch
		{
		}
    }
    
    void AddTopOrganisms( Terrarium.Server.OrganismType organismType, System.Text.StringBuilder buffer )
    {		
		System.Data.DataSet topPlants = Terrarium.Server.ChartBuilder.GetTopAnimals( "2.0.50522", organismType, 10 );

		buffer.Append( "<span class=\"SubTitle\">" + organismType.ToString() + "</span>" );
		buffer.Append( "<ol type=\"1\" class=\"CritterList\">" );
		
		if( topPlants != null )
		{
			if ( topPlants.Tables.Contains( "Species" ) == true )
			{
				System.Data.DataTable speciesTable = topPlants.Tables["Species"];
				foreach( System.Data.DataRow row in speciesTable.Rows )
				{
					buffer.Append( "<li>" );
					buffer.Append( Convert.ToString( row["Name"] ) + " (" + Convert.ToString( row["Population"] ) + ")" );
					buffer.Append( "</li>" );
				}
			}
		}
		
		buffer.Append( "</ol>" );
		buffer.Append( "</span>" );
    }
    
</script>

<table border="0" width="100%">
	<tr>
		<td>
			<span id="ContentSpan" runat="server"/>
		</td>
	</tr>
</table>
