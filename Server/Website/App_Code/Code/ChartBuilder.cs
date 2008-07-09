//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Web;
using System.Web.Util;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using WebChart;

namespace Terrarium.Server 
{
    /*
        Enum:       OrganismType
        Purpose:    Used to filter organisms by type when returning
        top organism information.
    */
    public enum OrganismType {
        Carnivore,
        Herbivore,
        Plant
    }

    /*
        Class:      ChartBuilder
        Purpose:    This class encapsulates the code which merges creature
        data with the NewChart graphing class in order to generate user
        defined graphs of creature vitals and population.
    */
    public sealed class ChartBuilder {
        private static DataSet cachedSpeciesList;
        private static Color[] colors = new Color[] {Color.Red, Color.Blue, Color.Green, Color.Gray, Color.Pink, Color.Plum, Color.Lavender, Color.LightSkyBlue, Color.Orange, Color.Beige};

        
        // This is only a class of static members and shouldn't be instantiated
        private ChartBuilder() {
        }

        /*
            Property:   SpeciesList
            Purpose:    Exports the functionality for getting a cached species
            list for use when displaying species in a datagrid for selection
            during charting.
        */
        public static DataSet SpeciesList {
            get {
                if ( cachedSpeciesList == null ) {
                    RefreshSpeciesList();
                }

                return cachedSpeciesList;
            }
        }
    
        /*
            Member:     GetTopAnimals
            Purpose:    This class get the n top animal of a specific organism
            classification from the database and returns their information as
            a DataSet
        */
        public static DataSet GetTopAnimals(string version, OrganismType organismType, int count) {

            version = new Version(version).ToString(3);

            using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) {
                myConnection.Open();

                SqlCommand command = new SqlCommand( "TerrariumTopAnimals", myConnection );
				command.CommandType = CommandType.StoredProcedure;
				
				command.Parameters.Add( "@Count", count );
				command.Parameters.Add( "@Version", version );
				command.Parameters.Add( "@SpeciesType", organismType.ToString() );

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                DataSet tempData = new DataSet();
                adapter.Fill(tempData, "Species");
				
                myConnection.Close();
                return tempData;
            }
        }
    
        /*
            Member:     GrabLatestSpeciesData
            Purpose:    This function grabs the latest data point of information for a given
            species.  This is used to grab and display vitals in a grid format once a creature
            has been selected for charting.
        */
        public static DataSet GrabLatestSpeciesData(string species) {
            using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) {
                myConnection.Open();

                SqlCommand command = new SqlCommand("TerrariumGrabLatestSpeciesData", myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parmSpecies = command.Parameters.Add("@SpeciesName", SqlDbType.VarChar, 50);
                parmSpecies.Value = species;

                DataSet tempData = new DataSet();
                adapter.Fill(tempData, "Species");

                myConnection.Close();
                return tempData;
            }
        }
    
        /*
            Member:     ChartPopulation
            Purpose:    This function charts a population graph given a start date and an end
            data along with a list of species to chart side by side.  This graph is useful for
            comparing success between different species.
        */
        public static string ChartPopulation(DateTime beginTime, DateTime endTime, DataSet included) {
            DataSet includedData = included;
            DateTime begin = beginTime;
            DateTime end = endTime;
            ChartControl chartControl = new ChartControl ();
            ChartControl.PhysicalPath = ServerSettings.ChartPath + "\\";
			ChartControl.VirtualPath = "/Terrarium/ChartData";
            
            if ( includedData != null ) 
            {
                SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn);
                
                myConnection.Open();
            
                int index=0;
                ChartPointCollection[] data=new ChartPointCollection[12];
                Chart[] charts=new Chart[12];
                chartControl.Charts.Clear();
                
                long beginTicks = beginTime.Ticks;
                long totalTicks = endTime.Ticks - beginTicks;
                float[] buckets = new float[24];
    
                foreach(DataRow item in includedData.Tables[0].Rows) 
                {
                    SqlCommand command = new SqlCommand("TerrariumGrabSpeciesDataInDateRange", myConnection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@SpeciesName", SqlDbType.VarChar, 50).Value = item["SpeciesName"];
                    command.Parameters.Add("@BeginDate", SqlDbType.DateTime, 8).Value = begin;
                    command.Parameters.Add("@EndDate", SqlDbType.DateTime, 8).Value = end;

                    SqlDataReader dr = command.ExecuteReader();
                    data[index] = new ChartPointCollection();

                    while(dr.Read()) 
                    {
                        TimeSpan offset = endTime - ((DateTime) dr["SampleDateTime"]);
                        int bucket = 24 - (int) Math.Round(offset.TotalHours);
                        
                        if ( bucket > -1 && bucket < buckets.Length ) {
                            buckets[bucket] = (float) (int) dr["Population"];
                        }
                    }
    
                    dr.Close();
                    
                    for(int i = 0; i < buckets.Length; i++)
                    {
                        data[index].Add(new ChartPoint(i.ToString(), buckets[i]));
                        buckets[i] = 0;
                    }
                        
                    charts[index] = new SmoothLineChart(data[index], Color.Black );
                    charts[index].LineMarker = new CircleLineMarker(0,charts[index].Fill.Color ,charts[index].Fill.Color );
                    charts[index].ShowLineMarkers = false;
                    charts[index].DataLabels.ShowValue = false;
                    charts[index].Legend = item["SpeciesName"].ToString();
                    charts[index].Fill.Color = colors[index];
                    charts[index].Line.Color = colors[index];
                    charts[index].Line.Width = 2; 
                                                
                    chartControl.Charts.Add(charts[index]);
                    index++;
                }
            }

			chartControl.Width = 640;
			chartControl.Height = 480;
			chartControl.ChartPadding = 40;
			chartControl.TopPadding = 32;
			chartControl.Border = new ChartLine( Color.Black );
			
			chartControl.Background = new ChartInterior( Color.FromArgb( 216, 216, 216 ) );

			chartControl.GridLines = GridLines.Horizontal;
			chartControl.Legend.Position = LegendPosition.Right;
			
			chartControl.ChartTitle = new ChartText();
			chartControl.ChartTitle.Text = "Population (24 hours starting " + begin.ToString() + ")";
			chartControl.ChartTitle.Font = new Font( "Verdana", 10.0f, FontStyle.Bold );
			chartControl.ShowXValues = false;
			chartControl.XTicksInterval = 4;
			chartControl.YCustomStart = 1;
			chartControl.RedrawChart();
            
			return "~/ChartData/" + chartControl.ImageID + ".png";
            
        }

        /*
            Member:     ChartVitals
            Purpose:    This function charts all of the vital statistics between a given
            start and end data for a single species.  This is useful for determining the
            effectiveness of your creatures code.
        */
        public static string ChartVitals(DateTime beginTime, DateTime endTime, string species) {
            ChartControl chartControl = new ChartControl ();
            ChartControl.PhysicalPath = ServerSettings.ChartPath + "\\";
			ChartControl.VirtualPath = "/Terrarium/ChartData";

            ChartPointCollection[] data=new ChartPointCollection[8];
            Chart[] charts=new Chart[8];
    
            DateTime begin = beginTime;
            DateTime end = endTime;

            SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn); 
            
            myConnection.Open();

            long bottomTicks = begin.Ticks;
            long totalTicks = end.Ticks - begin.Ticks;
            
            SqlCommand command = new SqlCommand("TerrariumGrabSpeciesDataInDateRange", myConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@SpeciesName", SqlDbType.VarChar, 50).Value = species;
            command.Parameters.Add("@BeginDate", SqlDbType.DateTime, 8).Value = begin;
            command.Parameters.Add("@EndDate", SqlDbType.DateTime, 8).Value = end;

            for(int i = 0; i < data.Length; i++)
            {
                data[i] = new ChartPointCollection();
            }
            
            float[,] buckets = new float[8,24];
            for(int i = 0; i < 8; i++) {
                for(int j = 0; j < 24; j++) {
                    buckets[i, j] = -1;
                }
            }

            using(SqlDataReader dr = command.ExecuteReader())
            {
                while(dr.Read()) 
                {
                    TimeSpan offset = endTime - ((DateTime) dr["SampleDateTime"]);
                    int bucket = 24 - (int) Math.Round(offset.TotalHours);

                    if ( bucket > -1 && bucket <= buckets.GetUpperBound(1) ) {
                        buckets[0, bucket] = (float) (int) dr["BirthCount"];
                        buckets[1, bucket] = (float) (int) dr["StarvedCount"];
                        buckets[2, bucket] = (float) (int) dr["KilledCount"];
                        buckets[3, bucket] = (float) (int) dr["ErrorCount"];
                        buckets[4, bucket] = (float) (int) dr["TimeoutCount"];
                        buckets[5, bucket] = (float) (int) dr["SickCount"];
                        buckets[6, bucket] = (float) (int) dr["OldAgeCount"];
                        buckets[7, bucket] = (float) (int) dr["SecurityViolationCount"];
                    }
                    
                    /*
                    long recordTicks = ((DateTime) dr["SampleDateTime"]).Ticks;
                    float normalizedSampleTime = (float) ((double) (recordTicks - bottomTicks) / (double) (totalTicks / 24));
                    int ibirth = (int) dr["BirthCount"];
                    int istarve = (int) dr["StarvedCount"];
                    int ikilled = (int) dr["KilledCount"];
                    int ierror = (int) dr["ErrorCount"];
                    int itimeout = (int) dr["TimeoutCount"];
                    int isick = (int) dr["SickCount"];
                    int iold = (int) dr["OldAgeCount"];
                    int isecurity = (int) dr["SecurityViolationCount"];

                    data[0].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) ibirth));
                    data[1].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) istarve));
                    data[2].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) ikilled));
                    data[3].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) ierror));
                    data[4].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) itimeout));
                    data[5].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) isick));
                    data[6].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) iold));
                    data[7].Add(new ChartPoint(normalizedSampleTime.ToString(), (float) isecurity));
                    */
                }
                dr.Close();
            }

            string[] names = new string[] { "Birth", "Starved", "Killed", "Errors", "Timed Out", "Sick", "Old Age", "Security Exception" };
            
            for(int i = 0; i < 24; i++)
            {
                data[0].Add(new ChartPoint(i.ToString(), buckets[0, i]));
                data[1].Add(new ChartPoint(i.ToString(), buckets[1, i]));
                data[2].Add(new ChartPoint(i.ToString(), buckets[2, i]));
                data[3].Add(new ChartPoint(i.ToString(), buckets[3, i]));
                data[4].Add(new ChartPoint(i.ToString(), buckets[4, i]));
                data[5].Add(new ChartPoint(i.ToString(), buckets[5, i]));
                data[6].Add(new ChartPoint(i.ToString(), buckets[6, i]));
                data[7].Add(new ChartPoint(i.ToString(), buckets[7, i]));
            }

            for(int i = 0; i < charts.Length; i++) {
                charts[i] = new SmoothLineChart(data[i], colors[i]);
                charts[i].Legend = names[i];
                charts[i].Fill.Color = colors[i];
                charts[i].Line.Color = colors[i];
                charts[i].ShowLineMarkers = false;
                charts[i].DataLabels.ShowValue = false;
                charts[i].Line.Width = 2;
                chartControl.Charts.Add(charts[i]);
            }

			chartControl.Width = 640;
			chartControl.Height = 480;
			chartControl.ChartPadding = 40;
			chartControl.TopPadding = 32;
			chartControl.Border = new ChartLine( Color.Black );
			chartControl.Background = new ChartInterior( Color.FromArgb( 216, 216, 216 ) );
			chartControl.GridLines = GridLines.Horizontal;
			chartControl.Legend.Position = LegendPosition.Right;
			
			chartControl.ChartTitle = new ChartText();
			chartControl.ChartTitle.Text = "Vitals (24 hours starting " + begin.ToString() + ")";
			chartControl.ChartTitle.Font = new Font( "Verdana", 10.0f, FontStyle.Bold );
			chartControl.ShowXValues = false;
			chartControl.XTicksInterval = 4;
			chartControl.RedrawChart();

			return "~/ChartData/" + chartControl.ImageID + ".png";
		}
    
        /*
            Member:     RefreshSpeciesList
            Purpose:    Used by the SpeciesList property to get a DataSet with the latest
            species information for use in a datagrid.  The information returned is most
            useful when trying to select creatures by name, population, and author.
        */
        public static void RefreshSpeciesList() {
            using(SqlConnection myConnection = new SqlConnection(ServerSettings.SpeciesDsn)) {
                myConnection.Open();

                DataSet speciesList = new DataSet();
                SqlCommand command = new SqlCommand("TerrariumGrabSpeciesInfo", myConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandType = CommandType.StoredProcedure;
                adapter.Fill(speciesList, "Species");
            
                command = new SqlCommand("SELECT DISTINCT VERSION FROM SPECIES ORDER BY VERSION DESC", myConnection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(speciesList, "Versions");
            
                cachedSpeciesList = speciesList;
            }
        }
    }
}