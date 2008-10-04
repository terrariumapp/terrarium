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