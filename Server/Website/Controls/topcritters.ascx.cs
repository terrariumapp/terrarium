using System;
using System.Data;
using System.Text;
using System.Web.UI;
using Terrarium.Server;

public partial class topcritters : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            StringBuilder buffer = new StringBuilder(8192);
            addTopOrganisms(OrganismType.Plant, buffer);
            addTopOrganisms(OrganismType.Herbivore, buffer);
            addTopOrganisms(OrganismType.Carnivore, buffer);
            ContentSpan.InnerHtml = buffer.ToString();
        }
        catch(Exception ex)
        {
            ContentSpan.InnerHtml = string.Format("<span class=\"Error\">{0}</span>", ex);
        }
    }

    private static void addTopOrganisms(OrganismType organismType, StringBuilder buffer)
    {
        DataSet dataSet = ChartBuilder.GetTopAnimals(ServerSettings.LatestVersion, organismType, 10);

        buffer.Append("<span class=\"SubTitle\">" + organismType + "</span>");
        buffer.Append("<ol type=\"1\" class=\"CritterList\">");

        if (dataSet != null)
        {
            if (dataSet.Tables.Contains("Species"))
            {
                DataTable speciesTable = dataSet.Tables["Species"];
                foreach (DataRow row in speciesTable.Rows)
                {
                    buffer.Append("<li>");
                    buffer.Append(Convert.ToString(row["Name"]) + " (" + Convert.ToString(row["Population"]) + ")");
                    buffer.Append("</li>");
                }
            }
        }

        buffer.Append("</ol>");
        buffer.Append("</span>");
    }
}