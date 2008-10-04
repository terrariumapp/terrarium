//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Xml;
using Terrarium.Server;

namespace Terrarium.Server
{
	/*
		Class:      ChartService
		Purpose:    The chart service is capable of providing remote clients
		with charting data by calling on the local charting components to
		generate the chart and then returning any required information to the
		remote client to display the chart.  This service is also capable of
		returning all the data required to produce a remote interface to
		graph generation and the information required to generate top-n reports.
	*/
	public class ChartService : WebService 
	{
		/*
			Method:     GetSpeciesList
			Purpose:    Returns a graph of ALL species along with their latest
			point of population data.
		*/
		[WebMethod]
		public DataSet GetSpeciesList() 
		{
			return ChartBuilder.SpeciesList;
		}
	    
		/*
			Method:     GrabLatestSpeciesData
			Purpose:    Returns the latest datapoint of statistics information
			for a given species in the form of a dataset.
		*/
		[WebMethod]
		public DataSet GrabLatestSpeciesData(string species) 
		{
			return ChartBuilder.GrabLatestSpeciesData(species);
		}
	    
		/*
			Method:     GetTopAnimals
			Purpose:    Returns various information about the top-num animals
			of a given organism type for use in top-n reporting.
		*/
		[WebMethod]
		public DataSet GetTopAnimals(string version, OrganismType tat, int num) 
		{
			if ( num == 0 ) 
			{
				num = 3;
			}
	        
			return ChartBuilder.GetTopAnimals(version, tat, num);
		}
	}
}
