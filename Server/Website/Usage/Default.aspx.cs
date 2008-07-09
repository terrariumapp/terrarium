//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using Terrarium.Server;
using System.IO;
using System.Text;
using System.Xml;

namespace Terrarium.Server.Reporting
{
    public partial class Default : System.Web.UI.Page
    {
        private bool sortAscending = false;
        private string lastSortExpression = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string alias = null;

                if (Request.QueryString["Alias"] != null)
                {
                    alias = Request.QueryString["Alias"];
                }
                else
                {
                    alias = this.Context.User.Identity.Name;
                    alias = alias.Substring(alias.IndexOf('\\') + 1);
                }

                this.UserAliasLabel.Text = alias;

                this.UserTodayLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Today).TotalHours.ToString() + " hours";
                this.UserWeekLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Week).TotalHours.ToString() + " hours";
                this.UserTotalLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Total).TotalHours.ToString() + " hours";

                int inCount = 0, outCount = 0, percentage = 0;

                TeamUsageSummary teamSummary = UsageReporting.GetTeamUsageForUser(alias, UsagePeriod.Today);

                if (teamSummary != null)
                {
                    //this.PumAliasLabel.Text = teamSummary.PumAlias;

                    //this.TeamTodayAverageLabel.Text = teamSummary.AverageHours.ToString("#.0");
                    //this.TeamTodayHoursLabel.Text = teamSummary.TotalHours.ToString();
                    //this.TeamTodayParticipationLabel.Text = teamSummary.ParticipationRate.ToString() + "% (" + teamSummary.InCount.ToString() + " / " + teamSummary.TeamCount.ToString() + ")";

                    //teamSummary = UsageReporting.GetTeamUsageForUser(alias, UsagePeriod.Week);
                    //this.TeamWeekAverageLabel.Text = teamSummary.AverageHours.ToString("#.0");
                    //this.TeamWeekHoursLabel.Text = teamSummary.TotalHours.ToString();
                    //this.TeamWeekParticipationLabel.Text = teamSummary.ParticipationRate.ToString() + "% (" + teamSummary.InCount.ToString() + " / " + teamSummary.TeamCount.ToString() + ")";

                    //teamSummary = UsageReporting.GetTeamUsageForUser(alias, UsagePeriod.Total);
                    //this.TeamTotalAverageLabel.Text = teamSummary.AverageHours.ToString("#.0");
                    //this.TeamTotalHoursLabel.Text = teamSummary.TotalHours.ToString();
                    //this.TeamTotalParticipationLabel.Text = teamSummary.ParticipationRate.ToString() + "% (" + teamSummary.InCount.ToString() + " / " + teamSummary.TeamCount.ToString() + ")";
                }

                this.individualDataGrid.DataSource = UsageReporting.GetUserSummaries((UsagePeriod)Enum.Parse(typeof(UsagePeriod), this.IndividualPeriodDropDown.SelectedItem.Text));

                this.DataBind();

                if (this.IsPostBack == false)
                {
                    this.ViewState["SortExpression"] = "ParticipationRate";
                    this.ViewState["SortAscending"] = false;
                }

                this.SortTeams();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void SortTeams_Click(object sender, DataGridSortCommandEventArgs e)
        {
            if (this.ViewState["SortExpression"] != null && this.ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                this.ViewState["SortAscending"] = !(bool)this.ViewState["SortAscending"];
            }
            else
            {
                this.ViewState["SortAscending"] = true;
            }

            this.ViewState["SortExpression"] = e.SortExpression;

            this.SortTeams();
        }

        protected void SortTeams()
        {
            /*
            List<TeamUsageSummary> summaryList = (List<TeamUsageSummary>)this.teamDataGrid.DataSource;

            if (this.ViewState["SortExpression"].ToString() == "ParticipationRate")
            {
                summaryList.Sort(this.SortByParticipationRate);
            }
            else if (this.ViewState["SortExpression"].ToString() == "InCount")
            {
                summaryList.Sort(this.SortByInCount);
            }
            else if (this.ViewState["SortExpression"].ToString() == "OutCount")
            {
                summaryList.Sort(this.SortByOutCount);
            }
            else if (this.ViewState["SortExpression"].ToString() == "TotalHours")
            {
                summaryList.Sort(this.SortByTotalHours);
            }
            else if (this.ViewState["SortExpression"].ToString() == "AverageHours")
            {
                summaryList.Sort(this.SortByAverageHours);
            }
            else if (this.ViewState["SortExpression"].ToString() == "PumAlias")
            {
                summaryList.Sort(this.SortByPumAlias);
            }

            this.teamDataGrid.DataSource = summaryList;
            this.teamDataGrid.DataBind();
             */
        }

        private int SortByParticipationRate(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.ParticipationRate.CompareTo(right.ParticipationRate);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return value;
            }
            else
            {
                return -1 * value;
            }
        }

        private int SortByInCount(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.InCount.CompareTo(right.InCount);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return -1 * value;
            }
            else
            {
                return value;
            }
        }

        private int SortByOutCount(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.OutCount.CompareTo(right.OutCount);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return -1 * value;
            }
            else
            {
                return value;
            }
        }

        private int SortByTotalHours(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.TotalHours.CompareTo(right.TotalHours);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return -1 * value;
            }
            else
            {
                return value;
            }
        }

        private int SortByAverageHours(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.AverageHours.CompareTo(right.AverageHours);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return -1 * value;
            }
            else
            {
                return value;
            }
        }

        private int SortByPumAlias(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.PumAlias.CompareTo(right.PumAlias);

            if (true == (bool)this.ViewState["SortAscending"])
            {
                return -1 * value;
            }
            else
            {
                return value;
            }
        }
        protected TeamUsageSummary GetTeamUsageSummary(object target)
        {
            return (TeamUsageSummary)target;
        }

        protected UserUsageSummary GetUserUsageSummary(object target)
        {
            return (UserUsageSummary)target;
        }
    }
}