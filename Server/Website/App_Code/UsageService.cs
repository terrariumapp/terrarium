using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace Terrarium.Server
{
    /// <summary>
    /// Summary description for UsageService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UsageService : WebService
    {
        [WebMethod]
        public void ReportUsage(UsageData data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ServerSettings.SpeciesDsn))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("TerrariumReportUsage", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Alias", data.Alias);
                    command.Parameters.AddWithValue("@Domain", data.Domain);
                    command.Parameters.AddWithValue("@IPAddress", Context.Request.ServerVariables["REMOTE_ADDR"]);
                    command.Parameters.AddWithValue("@GameVersion", data.GameVersion);
                    command.Parameters.AddWithValue("@PeerChannel", data.PeerChannel);
                    command.Parameters.AddWithValue("@PeerCount", data.PeerCount);
                    command.Parameters.AddWithValue("@AnimalCount", data.AnimalCount);
                    command.Parameters.AddWithValue("@MaxAnimalCount", data.MaxAnimalCount);
                    command.Parameters.AddWithValue("@WorldWidth", data.WorldWidth);
                    command.Parameters.AddWithValue("@WorldHeight", data.WorldHeight);
                    command.Parameters.AddWithValue("@MachineName", data.MachineName);
                    command.Parameters.AddWithValue("@OSVersion", data.OSVersion);
                    command.Parameters.AddWithValue("@ProcessorCount", data.ProcessorCount);
                    command.Parameters.AddWithValue("@ClrVersion", data.ClrVersion);
                    command.Parameters.AddWithValue("@WorkingSet", data.WorkingSet);
                    command.Parameters.AddWithValue("@MaxWorkingSet", data.MaxWorkingSet);
                    command.Parameters.AddWithValue("@MinWorkingSet", data.MinWorkingSet);
                    command.Parameters.AddWithValue("@ProcessorTime", data.ProcessorTimeInSeconds);
                    command.Parameters.AddWithValue("@ProcessStartTime", data.ProcessStartTime);

                    command.ExecuteNonQuery();

                    command.Dispose();
                }
            }
            catch (Exception e)
            {
                InstallerInfo.WriteEventLog("ReportUsage", e.ToString());
            }
        }
    }
}