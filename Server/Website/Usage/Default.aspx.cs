//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Terrarium.Server.Reporting
{
    public partial class Default : Page
    {
        private string lastSortExpression = "";
        private bool sortAscending;

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
                    alias = Context.User.Identity.Name;
                    alias = alias.Substring(alias.IndexOf('\\') + 1);
                }

                UserAliasLabel.Text = alias;

                UserTodayLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Today).TotalHours + " hours";
                UserWeekLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Week).TotalHours + " hours";
                UserTotalLabel.Text = UsageReporting.GetUserUsage(alias, UsagePeriod.Total).TotalHours + " hours";

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

                individualDataGrid.DataSource =
                    UsageReporting.GetUserSummaries(
                        (UsagePeriod) Enum.Parse(typeof (UsagePeriod), IndividualPeriodDropDown.SelectedItem.Text));

                DataBind();

                if (IsPostBack == false)
                {
                    ViewState["SortExpression"] = "ParticipationRate";
                    ViewState["SortAscending"] = false;
                }

                SortTeams();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        protected void SortTeams_Click(object sender, DataGridSortCommandEventArgs e)
        {
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                ViewState["SortAscending"] = !(bool) ViewState["SortAscending"];
            }
            else
            {
                ViewState["SortAscending"] = true;
            }

            ViewState["SortExpression"] = e.SortExpression;

            SortTeams();
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

            if ((bool) ViewState["SortAscending"])
            {
                return value;
            }
            else
            {
                return -1*value;
            }
        }

        private int SortByInCount(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.InCount.CompareTo(right.InCount);

            if ((bool) ViewState["SortAscending"])
            {
                return -1*value;
            }
            else
            {
                return value;
            }
        }

        private int SortByOutCount(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.OutCount.CompareTo(right.OutCount);

            if ((bool) ViewState["SortAscending"])
            {
                return -1*value;
            }
            else
            {
                return value;
            }
        }

        private int SortByTotalHours(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.TotalHours.CompareTo(right.TotalHours);

            if ((bool) ViewState["SortAscending"])
            {
                return -1*value;
            }
            else
            {
                return value;
            }
        }

        private int SortByAverageHours(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.AverageHours.CompareTo(right.AverageHours);

            if ((bool) ViewState["SortAscending"])
            {
                return -1*value;
            }
            else
            {
                return value;
            }
        }

        private int SortByPumAlias(TeamUsageSummary left, TeamUsageSummary right)
        {
            int value = left.PumAlias.CompareTo(right.PumAlias);

            if ((bool) ViewState["SortAscending"])
            {
                return -1*value;
            }
            else
            {
                return value;
            }
        }

        protected TeamUsageSummary GetTeamUsageSummary(object target)
        {
            return (TeamUsageSummary) target;
        }

        protected UserUsageSummary GetUserUsageSummary(object target)
        {
            return (UserUsageSummary) target;
        }
    }
}