//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Terrarium.Server.Charts
{
    /// <summary>
    /// Summary description for ChartNew.
    /// </summary>
    public partial class ChartNew : Page
    {
        protected Panel ChartControls;
        protected Image Image1;
        protected string SortDirection;
        protected string SortExpression;

        /*
            Method:     Page_Load
            Purpose:    This method runs whenever the page is visited.
    
            This method controls the visibility of the calendar control.
    
            If the visit to the page is not a form postback then the Version
            drop down list is populated with items, the Calendar's date is
            set to the current time, the species datagrid page index is set
            to 0 and a default filter is applied which selects all creatures
            that are present in the most current version.
        */
        protected void Page_Load(Object Src, EventArgs E)
        {
            if (!IsPostBack)
            {
                ViewState["SortExpression"] = "Population";
                ViewState["SortDirection"] = "DESC";
                Chart.Visible = false;
            }

            Calendar1.Visible = StartTime.SelectedItem.Text != "Last 24 hours";

            SortExpression = (string) ViewState["SortExpression"];
            SortDirection = (string) ViewState["SortDirection"];

            if (!IsPostBack)
            {
                Trace.Write("Adding Version Items");
                Version.DataSource = ChartBuilder.SpeciesList.Tables["Versions"].DefaultView;
                Version.DataTextField = "VERSION";
                Version.DataValueField = "VERSION";
                Version.DataBind();
                Version.Items.Add("All Versions");
                Version.SelectedIndex = Version.Items.Count - 1;

                included.DataSource = new ArrayList();
                included.DataBind();

                Calendar1.SelectedDate = DateTime.UtcNow;
                notIncluded.CurrentPageIndex = 0;
                ApplyFilter();
            }
        }


        /*
            Method:     ApplyFilter
            Purpose:    This method contains all of the logic to generate a filter
            for the creatures dataset to allow for easy searching in the instance
            the dataset becomes quite large.  This function is responsible for querying
            all filtering controls and building a working row filter based on the
            contents of those controls.
        */

        protected void ApplyFilter()
        {
            DataView dv = ChartBuilder.SpeciesList.Tables["Species"].DefaultView;

            if (dv == null)
            {
                return;
            }

            string rowFilter = "";

            if (!string.IsNullOrEmpty(FilterValue.Text))
            {
                if (FilterColumn.SelectedItem.Text == "Population")
                {
                    int population = 0;
                    try
                    {
                        population = Int32.Parse(FilterValue.Text);
                    }
                    catch
                    {
                    }

                    rowFilter = FilterColumn.SelectedItem.Text + " >= " + population + "";
                }
                else
                {
                    rowFilter = FilterColumn.SelectedItem.Text + " Like '" + FilterValue.Text + "%'";
                }
            }

            if (Version.SelectedItem != null)
            {
                if (Version.SelectedItem.Text != "All Versions")
                {
                    if (rowFilter != "")
                    {
                        rowFilter += " AND ";
                    }
                    rowFilter += " Version = '" + Version.SelectedItem.Value + "'";
                }
            }
            else
            {
                if (Version.Items.Count > 0)
                {
                    if (rowFilter != "")
                    {
                        rowFilter += " AND ";
                    }
                    rowFilter += " Version = '" + Version.Items[0].Value + "'";
                }
            }

            dv.RowFilter = rowFilter;
            dv.Sort = SortExpression + " " + SortDirection;

            notIncluded.DataSource = dv;
            notIncluded.DataBind();
        }


        /*
            Method:     Page_PreRender
            Purpose:    This method runs before the page is rendered.
    
            In order to support separate graphs from the same user
            viewstate is used to hold the selected creatures rather than
            session state.  This means multiple browser instances won't
            share the same data, and that a user can more easily construct
            different graphs to display side by side.
        */

        protected void Page_PreRender(Object Src, EventArgs E)
        {
            DataSet includedData = LoadXmlDataFromViewState();

            if (includedData != null)
            {
                included.DataSource = includedData.Tables["Species"].DefaultView;
                included.DataBind();
            }
        }

        /*
            Method:     MakeHeader
            Purpose:    This method generates a UI for a sorted column header.
        */

        protected string MakeHeader(string headerText)
        {
            if (String.Join("", headerText.Split(new char[] {' '})) == SortExpression)
            {
                return "<B>" + headerText + " <font style='font-family: Webdings'>" +
                       ((SortDirection == "DESC") ? "6" : "5") + "</font></B>";
            }
            return headerText;
        }

        /*
            Method:     LoadXmlDataFromViewState
            Purpose:    This method loads a dataset out of the pages
            viewstate information by first reading the schema and finally
            the xml data.
        */

        protected DataSet LoadXmlDataFromViewState()
        {
            DataSet d = new DataSet();
            if (ViewState["SelectedSpeciesSchema"] != null && ViewState["SelectedSpecies"] != null)
            {
                d.ReadXmlSchema(new StringReader((string) ViewState["SelectedSpeciesSchema"]));
                d.ReadXml(new StringReader((string) ViewState["SelectedSpecies"]));
            }
            else
            {
                d = null;
            }

            return d;
        }

        /*
            Method:     applyFilterButton_Click
            Purpose:    This method is called whenever the apply filter
            button is clicked.  The current page index for the datagrid
            is set to 0 since a filter can modify the number of pages
            shown.
        */

        protected void applyFilterButton_Click(Object sender, EventArgs e)
        {
            notIncluded.CurrentPageIndex = 0;
            ApplyFilter();
        }

        /*
            Method:     notIncluded_Page
            Purpose:    This method allows for paging of the creatures datagrid.
        */

        protected void notIncluded_Page(Object sender, DataGridPageChangedEventArgs e)
        {
            notIncluded.CurrentPageIndex = e.NewPageIndex;
            ApplyFilter();
        }

        /*
            Method:     notIncluded_Command
            Purpose:    The creatures dataset has various commands that can
            be performed by clicking on a hyperlink within a given row.  This
            particular function looks for the Command of "Add" and then adds
            the creature to the list of selected creatures.
        */

        protected void notIncluded_Command(Object sender, DataGridCommandEventArgs e)
        {
            if (((LinkButton) e.CommandSource).CommandName == "Add")
            {
                DataSet includedData = LoadXmlDataFromViewState();
                if (includedData != null)
                {
                    // First check to see how many we have
                    // If 10 then exit
                    if (includedData.Tables["Species"].Rows.Count >= 10)
                    {
                        return;
                    }

                    // Ensure primary key
                    includedData.Tables["Species"].DefaultView.Sort = "SpeciesName DESC";
                    includedData.Tables["Species"].PrimaryKey = new DataColumn[]
                                                                    {
                                                                        includedData.Tables["Species"].Columns[
                                                                            "SpeciesName"]
                                                                    };
                    // Check to see if we already have this one
                    if (
                        includedData.Tables["Species"].Rows.Find(
                            HttpUtility.HtmlDecode(((DataBoundLiteralControl) e.Item.Cells[2].Controls[0]).Text)) !=
                        null)
                    {
                        return;
                    }
                }

                SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn);

                myConnection.Open();

                SqlCommand command = new SqlCommand("TerrariumGrabLatestSpeciesData", myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parmSpecies = command.Parameters.Add("@SpeciesName", SqlDbType.VarChar, 50);
                parmSpecies.Value =
                    HttpUtility.HtmlDecode(((DataBoundLiteralControl) e.Item.Cells[2].Controls[0]).Text.Trim());

                DataSet tempData = new DataSet();
                adapter.Fill(tempData, "Species");

                if (includedData != null)
                {
                    includedData.Merge(tempData);
                }
                else
                {
                    includedData = tempData;
                }

                myConnection.Close();

                included.DataSource = includedData;
                included.DataBind();

                ViewState["SelectedSpecies"] = includedData.GetXml();
                ViewState["SelectedSpeciesSchema"] = includedData.GetXmlSchema();
            }
        }

        /*
            Method:     included_Command
            Purpose:    The dataset of selected creatures has various commands
            that can be performed by clicking on the hyperlinks provided in
            each row.  This particular function looks for the commands "Remove"
            and "Chart".  The "Remove" command is responsible for removing the
            creature from the selected dataset.  The "Chart" command is responsible
            for charting the particular creatures vital statistics graph.
        */

        protected void included_Command(Object sender, DataGridCommandEventArgs e)
        {
            DataSet includedData;

            switch (((LinkButton) e.CommandSource).CommandName)
            {
                case "Remove":
                    includedData = LoadXmlDataFromViewState();
                    if (includedData != null)
                    {
                        // Ensure primary key
                        includedData.Tables["Species"].DefaultView.Sort = "SpeciesName DESC";
                        includedData.Tables["Species"].PrimaryKey = new DataColumn[]
                                                                        {
                                                                            includedData.Tables["Species"].Columns[
                                                                                "SpeciesName"]
                                                                        };

                        DataRow drow = includedData.Tables["Species"].Rows.Find(HttpUtility.HtmlDecode(e.Item.Cells[2].Text));
                        if (drow != null)
                        {
                            includedData.Tables["Species"].Rows.Remove(drow);
                        }

                        included.DataSource = includedData;
                        included.DataBind();

                        ViewState["SelectedSpecies"] = includedData.GetXml();
                        ViewState["SelectedSpeciesSchema"] = includedData.GetXmlSchema();
                    }
                    break;

                default:
                    break;
            }
        }

        /*
            Method:     notIncluded_Sort
            Purpose:    The dataset of selected creatures is capable of applying
            a sort based on a single column.  This function enables the setting
            of a SortExpression, and decides which direction to sort the data.
        */

        protected void notIncluded_Sort(Object sender, DataGridSortCommandEventArgs e)
        {
            if (SortExpression == e.SortExpression)
            {
                SortDirection = (SortDirection == "DESC") ? "ASC" : "DESC";
            }
            else
            {
                SortDirection = "DESC";
            }
            SortExpression = e.SortExpression;

            ViewState["SortExpression"] = SortExpression;
            ViewState["SortDirection"] = SortDirection;


            ApplyFilter();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}