using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace Terrarium.Server
{
    public enum UsagePeriod
    {
        Today,
        Week,
        Total
    }

    public class UserUsageSummary
    {
        private string alias;
        private float averageHours;
        private UsagePeriod period;
        private int totalHours;

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }


        public UsagePeriod Period
        {
            get { return period; }
            set { period = value; }
        }

        public int TotalHours
        {
            get { return totalHours; }
            set { totalHours = value; }
        }

        public float AverageHours
        {
            get { return averageHours; }
            set { averageHours = value; }
        }
    }

    public class TeamUsageSummary
    {
        private float averageHours;
        private int inCount;
        private int outCount;
        private int participationRate;
        private UsagePeriod period;
        private string pumAlias;
        private int teamCount;
        private int totalHours;

        public string PumAlias
        {
            get { return pumAlias; }
            set { pumAlias = value; }
        }

        public UsagePeriod Period
        {
            get { return period; }
            set { period = value; }
        }

        public int InCount
        {
            get { return inCount; }
            set { inCount = value; }
        }

        public int OutCount
        {
            get { return outCount; }
            set { outCount = value; }
        }

        public int TeamCount
        {
            get { return teamCount; }
            set { teamCount = value; }
        }

        public int ParticipationRate
        {
            get { return participationRate; }
            set { participationRate = value; }
        }

        public int TotalHours
        {
            get { return totalHours; }
            set { totalHours = value; }
        }

        public float AverageHours
        {
            get { return averageHours; }
            set { averageHours = value; }
        }
    }

    public static class UsageReporting
    {
        public static UserUsageSummary GetUserUsage(string alias, UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(ServerSettings.SpeciesDsn);
                connection.Open();

                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT SUM(UsageMinutes) AS UsageMinutes FROM Usage WHERE Alias = @Alias AND (TickTime >= @StartDate AND TickTime <= @EndDate)";
                command.Parameters.AddWithValue("@Alias", alias);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                GetPeriodDates(period, ref startDate, ref endDate);

                command.Parameters.AddWithValue("@StartDate", startDate.ToString());
                command.Parameters.AddWithValue("@EndDate", endDate.ToString());

                reader = command.ExecuteReader();

                UserUsageSummary summary = new UserUsageSummary();
                summary.Alias = alias;
                summary.Period = period;
                if (reader.Read())
                {
                    if (!((reader["UsageMinutes"]) is DBNull))
                    {
                        summary.TotalHours = Convert.ToInt32(reader["UsageMinutes"])/60;
                    }
                }


                TimeSpan span = endDate - startDate;

                if (span.Days == 0)
                {
                    span = new TimeSpan(1, 0, 0, 0);
                }

                summary.AverageHours = summary.TotalHours/(float) span.Days;

                return summary;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public static ReadOnlyCollection<UsageData> GetUserDetails(string alias, UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(ServerSettings.SpeciesDsn);
                connection.Open();

                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT * FROM Usage WHERE Alias = @Alias AND (TickTime >= @StartDate AND TickTime <= @EndDate)";
                command.Parameters.AddWithValue("@Alias", alias);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                GetPeriodDates(period, ref startDate, ref endDate);

                command.Parameters.AddWithValue("@StartDate", startDate.ToString());
                command.Parameters.AddWithValue("@EndDate", endDate.ToString());

                reader = command.ExecuteReader();

                UserUsageSummary summary = new UserUsageSummary();
                summary.Alias = alias;
                summary.Period = period;

                List<UsageData> usageList = new List<UsageData>();

                while (reader.Read())
                {
                    UsageData data = new UsageData();
                    data.Alias = Convert.ToString(reader["Alias"]);
                    data.Domain = Convert.ToString(reader["Domain"]);
                    data.TickTime = Convert.ToDateTime(reader["TickTime"]);
                    data.UsageMinutes = Convert.ToInt32(reader["UsageMinutes"]);
                    data.IPAddress = Convert.ToString(reader["IPAddress"]);
                    data.GameVersion = Convert.ToString(reader["GameVersion"]);
                    data.PeerChannel = Convert.ToString(reader["PeerChannel"]);
                    data.PeerCount = Convert.ToInt32(reader["PeerCount"]);
                    data.AnimalCount = Convert.ToInt32(reader["AnimalCount"]);
                    data.MaxAnimalCount = Convert.ToInt32(reader["MaxAnimalCount"]);
                    data.WorldHeight = Convert.ToInt32(reader["WorldHeight"]);
                    data.WorldWidth = Convert.ToInt32(reader["WorldWidth"]);
                    data.MachineName = Convert.ToString(reader["MachineName"]);
                    data.OSVersion = Convert.ToString(reader["OSVersion"]);
                    data.ProcessorCount = Convert.ToInt32(reader["ProcessorCount"]);
                    data.ClrVersion = Convert.ToString(reader["ClrVersion"]);
                    data.WorkingSet = Convert.ToInt32(reader["WorkingSet"]);
                    data.MaxWorkingSet = Convert.ToInt32(reader["MaxWorkingSet"]);
                    data.MinWorkingSet = Convert.ToInt32(reader["MinWorkingSet"]);
                    data.ProcessorTimeInSeconds = Convert.ToInt32(reader["ProcessorTime"]);
                    data.ProcessStartTime = Convert.ToDateTime(reader["ProcessStartTime"]);

                    usageList.Add(data);
                }

                return new ReadOnlyCollection<UsageData>(usageList);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public static TeamUsageSummary GetTeamUsageForUser(string alias, UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(ServerSettings.SpeciesDsn);
                connection.Open();

                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT P.Alias FROM Pum P, PumTeam PT WHERE P.Id = PT.PumId AND PT.Alias = @Alias";
                command.Parameters.AddWithValue("@Alias", alias);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return GetTeamUsage(Convert.ToString(reader["Alias"]), period);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public static List<TeamUsageSummary> GetTeamSummaries(UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                string cacheKey = "GetTeamSummaries(" + period + ")";
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    return (List<TeamUsageSummary>) HttpContext.Current.Cache[cacheKey];
                }
                else
                {
                    List<TeamUsageSummary> teamList = new List<TeamUsageSummary>();

                    connection = new SqlConnection(ServerSettings.SpeciesDsn);
                    connection.Open();

                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Alias FROM Pum ORDER BY Alias";

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string pumAlias = Convert.ToString(reader["Alias"]);
                        teamList.Add(GetTeamUsage(pumAlias, period));
                    }

                    HttpContext.Current.Cache.Add(cacheKey, teamList, null, DateTime.Now.AddMinutes(59), TimeSpan.Zero,
                                                  CacheItemPriority.Normal, null);

                    return teamList;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public static ReadOnlyCollection<UserUsageSummary> GetUserSummaries(UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                string cacheKey = "GetUserSummaries(" + period + ")";
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    return
                        new ReadOnlyCollection<UserUsageSummary>(
                            (List<UserUsageSummary>) HttpContext.Current.Cache[cacheKey]);
                }
                else
                {
                    List<UserUsageSummary> userList = new List<UserUsageSummary>();

                    connection = new SqlConnection(ServerSettings.SpeciesDsn);
                    connection.Open();

                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT TOP 10 Alias,	SUM(UsageMinutes) AS UsageTotal FROM Usage WHERE TickTime >= @StartDate AND TickTime <= @EndDate GROUP BY Alias ORDER BY UsageTotal DESC";

                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = DateTime.MinValue;

                    GetPeriodDates(period, ref startDate, ref endDate);

                    command.Parameters.AddWithValue("@StartDate", startDate.ToString());
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString());

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UserUsageSummary summary = new UserUsageSummary();
                        summary.Alias = Convert.ToString(reader["Alias"]);
                        summary.Period = period;
                        summary.TotalHours = Convert.ToInt32(reader["UsageTotal"])/60;

                        userList.Add(summary);
                    }

                    HttpContext.Current.Cache.Add(cacheKey, userList, null, DateTime.Now.AddMinutes(59), TimeSpan.Zero,
                                                  CacheItemPriority.Normal, null);

                    return new ReadOnlyCollection<UserUsageSummary>(userList);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public static TeamUsageSummary GetTeamUsage(string pumAlias, UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                TeamUsageSummary teamSummary = new TeamUsageSummary();
                teamSummary.PumAlias = pumAlias;
                teamSummary.Period = period;

                connection = new SqlConnection(ServerSettings.SpeciesDsn);
                connection.Open();

                command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT	PT.Alias, (SELECT 	SUM(U.UsageMinutes) FROM Usage U WHERE U.Alias = PT.Alias AND U.TickTime >= @StartDate AND U.TickTime <= @EndDate) AS UsageMinutes FROM Pum P, PumTeam PT WHERE 	PT.PumId = P.Id AND	P.Alias = @Alias ORDER BY UsageMinutes DESC";

                command.Parameters.AddWithValue("@Alias", pumAlias);

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                GetPeriodDates(period, ref startDate, ref endDate);

                command.Parameters.AddWithValue("@StartDate", startDate.ToString());
                command.Parameters.AddWithValue("@EndDate", endDate.ToString());

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["UsageMinutes"] is DBNull)
                    {
                        teamSummary.OutCount++;
                    }
                    else
                    {
                        teamSummary.TotalHours += Convert.ToInt32(reader["UsageMinutes"]);
                        teamSummary.InCount++;
                    }
                    teamSummary.TeamCount++;
                }

                teamSummary.TotalHours /= 60;
                teamSummary.AverageHours = teamSummary.TotalHours/(float) teamSummary.TeamCount;
                teamSummary.ParticipationRate = (int) (100.0f*(teamSummary.InCount/(float) teamSummary.TeamCount));

                return teamSummary;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        /// <summary>
        /// Returns an array of UserUsageSummary items for a team
        /// </summary>
        /// <param name="pumAlias">The Pum for the team</param>
        /// <param name="period">Which period we care about</param>
        /// <returns></returns>
        public static List<UserUsageSummary> GetTeamDetails(string pumAlias, UsagePeriod period)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                string cacheKey = "GetTeamDetails(" + pumAlias + "," + period + ")";
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    return (List<UserUsageSummary>) HttpContext.Current.Cache[cacheKey];
                }
                else
                {
                    TeamUsageSummary teamSummary = new TeamUsageSummary();
                    teamSummary.PumAlias = pumAlias;
                    teamSummary.Period = period;

                    connection = new SqlConnection(ServerSettings.SpeciesDsn);
                    connection.Open();

                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT PT.* FROM PumTeam PT, Pum P WHERE PT.PumID = P.Id AND P.Alias = @Alias";
                    command.Parameters.AddWithValue("@Alias", pumAlias);

                    reader = command.ExecuteReader();

                    List<UserUsageSummary> userList = new List<UserUsageSummary>();

                    while (reader.Read())
                    {
                        string userAlias = Convert.ToString(reader["Alias"]);
                        userList.Add(GetUserUsage(userAlias, period));
                    }

                    HttpContext.Current.Cache.Add(cacheKey, userList, null, DateTime.Now.AddMinutes(59), TimeSpan.Zero,
                                                  CacheItemPriority.Normal, null);

                    return userList;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        /// <summary>
        /// This is a helper function to give proper dates given the UsagePeriod
        /// </summary>
        /// <param name="period">The period we care about</param>
        /// <param name="startDate">The startDate, to be filled in by this function</param>
        /// <param name="endDate">The endDate, to be filled in by this function</param>
        private static void GetPeriodDates(UsagePeriod period, ref DateTime startDate, ref DateTime endDate)
        {
            switch (period)
            {
                case UsagePeriod.Today:
                    {
                        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                        break;
                    }
                case UsagePeriod.Week:
                    {
                        startDate = DateTime.Now.AddDays(-((int) DateTime.Now.DayOfWeek));
                        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                        endDate = DateTime.Now.AddDays(((int) DayOfWeek.Saturday) - ((int) DateTime.Now.DayOfWeek));
                        endDate = DateTime.Now.AddDays(1);
                        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                        break;
                    }
                case UsagePeriod.Total:
                    {
                        startDate = new DateTime(2005, 5, 24);
                        endDate = DateTime.Now.AddDays(1);
                        break;
                    }
            }
        }
    }
}