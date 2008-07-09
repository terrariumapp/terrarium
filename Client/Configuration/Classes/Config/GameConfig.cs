//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                         
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Terrarium.Configuration
{
	/// <summary>
	///  Centralized class file giving strongly typed
	///  access to all user configuration file values.
	///  This class primarily encapsulates a series
	///  of string and boolean values, but can be extended
	///  for more complex values, like the State and
	///  Country classes.
	/// </summary>
	public class GameConfig 
	{
		#region "Boolean Configuration Fields"
		/// <summary>
		///  Field representing the background grid rendering state
		/// </summary>
		private static CachedBooleanConfig backgroundGrid = new CachedBooleanConfig("backgroundGrid", false);
		/// <summary>
		///  Field representing the bounding box rendering state
		/// </summary>
		private static CachedBooleanConfig boundingBoxes = new CachedBooleanConfig("boundingBoxes", false);
		/// <summary>
		///  Field representing the Demo Mode state.
		/// </summary>
		private static CachedBooleanConfig demoMode = new CachedBooleanConfig("demoMode", false);
		/// <summary>
		///  Field representing the destination lines rendering state
		/// </summary>
		private static CachedBooleanConfig destinationLines = new CachedBooleanConfig("destinationLines", false);
		/// <summary>
		///  Field representing the use of the discovery override for the network engine
		/// </summary>
		private static CachedBooleanConfig discoveryOverride = new CachedBooleanConfig("discoveryOverride", false);
		/// <summary>
		///  Field representing the screen drawing rendering state
		/// </summary>
		private static CachedBooleanConfig drawScreen = new CachedBooleanConfig("drawScreen", true);
		/// <summary>
		///  Field representing the use of a NAT device for the network engine
		/// </summary>
		private static CachedBooleanConfig enableNat = new CachedBooleanConfig("enableNat", false);
		/// <summary>
		///  Field representing the large graphics modes rendering state
		/// </summary>
		private static CachedBooleanConfig largeGraphicsMode = new CachedBooleanConfig("largeGraphicsMode", false);

		/// <summary>
		/// Field representing whether to use the simple screen saver
		/// </summary>
		private static CachedBooleanConfig useSimpleScreenSaver = new CachedBooleanConfig("useSimpleScreenSaver",false);

		/// <summary>
		/// Field that represents whether to start in fullscreen (maximized) mode.
		/// </summary>
		private static CachedBooleanConfig startFullscreen = new CachedBooleanConfig( "startFullscreen", false );

		/// <summary>
		/// Field that represents whether to start in fullscreen (maximized) mode.
		/// </summary>
		private static CachedBooleanConfig screenSaverSpanMonitors = new CachedBooleanConfig( "screenSaverSpanMonitors", false );

		/// <summary>
		/// Allows a client to skip checking the server for disabled versions
		/// </summary>
		private static CachedBooleanConfig skipVersionCheck = new CachedBooleanConfig("skipVersionCheck", false );

		#endregion
		#region "String Configuration Fields"
		/// <summary>
		///  Field representing the version string of a blocked terrarium
		/// </summary>
		static string blockedVersionString = "";
		/// <summary>
		///  Field representing the local IP address to use for the network engine
		/// </summary>
		static string localIPAddress = "";  
		/// <summary>
		///  Field representing the current trace logging mode for the Hosting code
		/// </summary>
		static string loggingMode = "";
		/// <summary>
		///  Field representing the location of media files for the graphics engine
		/// </summary>
		static string mediaDirectory="";

        /// <summary>
        /// Field representing the location of application files.
        /// </summary>
        static string applicationDirectory="";

		/// <summary>
		///  Field representing a comma separated list of peers for use with
		///  discovery override.
		/// </summary>
		static string peerList = "";
		/// <summary>
		///  Field representing the user's country/region as set by the peer properties
		///  dialogue.  This value is sent during teleportation.
		/// </summary>
		static string userCountry = "";
		/// <summary>
		///  Field representing the user's email address.  This value is used
		///  when sending error reports to the Terrarium server and is useful
		///  in contacting the user for additional details or a repro case.
		/// </summary>
		static string userEmail= "";
		/// <summary>
		///  Field representing the user's state as set by the peer properties
		///  dialogue.  This value is sent during teleportation.
		/// </summary>
		static string userState = "";
		/// <summary>
		///  Field representing the Terrarium server to connect to for peer
		///  discovery and reporting.  This value is also used to create the
		///  bin directory for storing game state.
		/// </summary>
		static string webRoot="";

		/// <summary>
		/// Field representing the Metal Style that Terrarium should use
		/// for the custom controls.
		/// </summary>
		static string styleName = "";

		#endregion
		#region "Demo Mode Configuration Properties"
		/// <summary>
		///  Determines if the Terrarium Client should give a visible
		///  cue when errors occur.  By default errors are always shown
		///  except when in Demo Mode.  Demo Mode is useful for presentations
		///  where an error dialogue might interrupt the user experience.
		/// </summary>
		public static bool ShowErrors
		{
			get
			{
				return !DemoMode;
			}
		}

		/// <summary>
		///  Determines whether the Terrarium Client should accept updates
		///  from the update server.  By default updates are allowed unless
		///  the client is in Demo Mode.
		/// </summary>
		public static bool AllowUpdates
		{
			get
			{
				return !DemoMode;
			}
		}

		/// <summary>
		///  Determines whether the Terrarium Client graphics engine should
		///  use large graphics for the rendering engine.  Large graphics are
		///  full size 48x48 sprites in most skins, or the largest available
		///  sprite size if 48x48 sprites don't exist.
		/// </summary>
		public static bool UseLargeGraphics
		{
			get
			{
				return largeGraphicsMode.Getter();
			}
        
			set
			{
				largeGraphicsMode.Setter(value);
			}
		}

		/// <summary>
		///  Determines whether or not the Terrarium Client is in demonstration
		///  mode.  Certain features are disabled when the client is in demonstration
		///  mode while other features are enabled.
		/// </summary>
		public static bool DemoMode
		{
			get
			{
				return demoMode.Getter();
			}
        
			set 
			{
				demoMode.Setter(value);
			}
		}
		#endregion
		#region "Configuration Properties"
		/// <summary>
		///  Retrieves the current logging mode.  Logging modes are used to
		///  determine how creature trace output is handled.  The default
		///  trace logging allows for trace output to be placed on the
		///  debugger output stream.
		/// </summary>
		public static string LoggingMode
		{
			get
			{
				if (loggingMode == null)
				{
					loggingMode = GetSetting("loggingMode");
				}
            
				return loggingMode;
			}
        
			set 
			{
				SetSetting("loggingMode", value);
				loggingMode = value;
			}
		}

		/// <summary>
		///  Retrieves the current graphics drawing mode.  Graphics can be
		///  turned on and off to limit processor usage by the graphics
		///  engine.
		/// </summary>
		public static bool DrawScreen
		{
			get
			{
				return drawScreen.Getter();
			}
        
			set
			{
				drawScreen.Setter(value);
			}
		}

		/// <summary>
		///  Determines if creature destination lines should be drawn
		///  through the graphics engine.  Destination lines use more
		///  CPU but are useful for movement debugging.
		/// </summary>
		public static bool DestinationLines
		{
			get
			{
				return destinationLines.Getter();
			}
        
			set
			{
				destinationLines.Setter(value);
			}
		}

		/// <summary>
		///  Determines if a special background is painted using the
		///  rendering engine that contains a grid overlay identical
		///  to the Terrarium Engine's cell grid.
		/// </summary>
		public static bool BackgroundGrid
		{
			get 
			{
				return backgroundGrid.Getter();
			}
        
			set
			{
				backgroundGrid.Setter(value);
			}
		}

		/// <summary>
		///  Determines if cell boundary bounding boxes are drawn
		///  for creatures.  Bounding boxes use more CPU but are
		///  useful for movement debugging.
		/// </summary>
		public static bool BoundingBoxes
		{
			get
			{
				return boundingBoxes.Getter();
			}
        
			set
			{
				boundingBoxes.Setter(value);
			}
		}

		/// <summary>
		///  Determines if features of the Network Engine are bypassed
		///  in order to enable NAT devices.  This turns off several
		///  checks that would normally stop the Terrarium Client from
		///  running.
		/// </summary>
		public static bool EnableNat
		{
			get
			{
				return enableNat.Getter();
			}
        
			set
			{
				enableNat.Setter(value);
			}
		}
    
		/// <summary>
		///  Specifies whether the config file or the Terrarium
		///  Server web service should be used for peer discovery.
		///  This option is used in conjunction with PeerList.
		/// </summary>
		public static bool UseConfigForDiscovery
		{
			get
			{
				return discoveryOverride.Getter();
			}       

			set
			{
				discoveryOverride.Setter(value);
			}
		}

		/// <summary>
		///  Specifies a blocked version number.  A Terrarium
		///  Deployment Server can be used to shut-down potentially
		///  dangerous clients.  The user will have to remove this
		///  option from the config file or upgrade their client to
		///  continue playing.
		/// </summary>
		public static string BlockedVersion
		{
			get
			{
				if (blockedVersionString.Length == 0)
				{
					blockedVersionString = GetSetting("blockedVersion");
				}

				return blockedVersionString;
			}
        
			set
			{
				SetSetting("blockedVersion", value);
				blockedVersionString = value;
			}
		}

		/// <summary>
		///  Specifies the Local IP Address of this host rather than retrieving
		///  the value from the network adapter list.
		/// </summary>
		public static string LocalIPAddress 
		{
			get
			{
				if (localIPAddress.Length == 0)
				{
					localIPAddress = GetSetting("localIPAddress");
				}

				return localIPAddress;
			}
        
			set
			{
				SetSetting("localIPAddress", value);
				localIPAddress = value;
			}
		}

		/// <summary>
		///  Specifies a comma separated value listing of peer IP
		///  addresses to connect to in the absence of a peer
		///  discovery service.  This is enabled whenever the
		///  discovery override option is enabled.
		/// </summary>
		public static string PeerList
		{
			get
			{
				if (peerList.Length == 0)
				{
					peerList = GetSetting("peerList");
				}

				return peerList;
			}
        
			set 
			{
				SetSetting("peerList", value);
				peerList = value;
			}
		}

		/// <summary>
		///  Used by the Terrarium Client to build the Web Service
		///  url's for connecting with a Terrarium server for discovery
		///  and reporting.
		/// </summary>
		public static string WebRoot
		{
			get
			{
				if (webRoot.Length == 0)
				{
					webRoot = GetSetting("webRoot");
				}

				return webRoot;
			}
        
			set
			{
				SetSetting("webRoot", value);
				webRoot = value;
			}
		}

		/// <summary>
		/// Contains the name of the Metal Style file to load and set
		/// as the active style
		/// </summary>
		public static string StyleName
		{
			get
			{
				if ( styleName.Length == 0 )
				{
					styleName = GetSetting( "styleName" );
				}

				return styleName;
			}

			set
			{
				SetSetting( "styleName", value );
				styleName = value;
			}
		}

		/// <summary>
		///  Creates the location used to retrieve the media files for
		///  the graphics engine.  The value is generated using the
		///  location of the currently executing assembly.
		/// </summary>
		public static string MediaDirectory
		{
			get
			{
				if (mediaDirectory == null || mediaDirectory.Length == 0)
				{
					mediaDirectory = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
				}

				return mediaDirectory;
			}
		}
    
        /// <summary>
        /// Returns the full path to the application.
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                if (applicationDirectory == null || applicationDirectory.Length == 0)
                {
                    applicationDirectory = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;
                }

                return applicationDirectory;
            }
        }

		/// <summary>
		///  Retrieves a special CPU Throttle value that controls how many
		///  creatures can run inside of a Terrarium  Values less than 100
		///  will operate the Terrarium with less creatures than normal,
		///  while values greater than 100 will cause more creatures than
		///  normal to operate.
		/// </summary>
		public static int CpuThrottle
		{
			get
			{
				int i = 100;
				try
				{
					i = int.Parse(GetSetting("CPUThrottle"));
				}
				catch
				{
					CpuThrottle = i;
				}
            
				if (i < 50)
				{
					i = 50;
				}
				if (i > 200)
				{
					i = 200;
				}
            
				return i;
			}
        
			set
			{
				int i = value;

				if (i < 50)
				{
					i = 50;
				}
				if (i > 200)
				{
					i = 200;
				}
            
				SetSetting("CPUThrottle", i.ToString());
			}
		}
    
		/// <summary>
		///  Retrieves the current country/region of the user.  This value is
		///  set by the user by using the Peer Properties dialogue.
		/// </summary>
		public static string UserCountry
		{
			get
			{
				if (userCountry.Length == 0)
				{
					userCountry = GetSetting("country");
				}
            
				if (userCountry.Length == 0 || !CountrySettings.Validate(userCountry))
				{
					userCountry = "<Unknown>";
					SetSetting("country", "<Unknown>");
				}
            
				return userCountry;
			}
        
			set
			{
				if (!CountrySettings.Validate(value)) 
				{
					userCountry = "<Unknown>";
					SetSetting("country", "<Unknown>");
					return;
				}
            
				SetSetting("country", value);
				userCountry = value;
			}
		}
    
		/// <summary>
		///  Retrieves the current state of the user if the user is located
		///  within the United States.  This value is set by the Peer Properties
		///  dialogue.
		/// </summary>
		public static string UserState
		{
			get
			{
				if (userState.Length == 0)
				{
					userState = GetSetting("state");
				}
            
				if (userState.Length == 0 || !StateSettings.Validate(userState))
				{
					userState = "<Unknown>";
					SetSetting("state", "<Unknown>");
				}
            
				return userState;
			}
        
			set
			{
				if (!StateSettings.Validate(value))
				{
					userState = "<Unknown>";
					SetSetting("state", "<Unknown>");
					return;
				}
        
				SetSetting("state", value);
				userState = value;
			}
		}

		/// <summary>
		///  Retrieves the current user's email address as set in the
		///  Peer Properties dialogue.  This value is sent in debug reports
		///  to the server so that the user can be contacted.
		/// </summary>
		public static string UserEmail
		{
			get
			{
				if (userEmail.Length == 0)
				{
					userEmail = GetSetting("email");
				}

				return userEmail;
			}
        
			set
			{
				SetSetting("email", value);
				userEmail = value;
			}
		}

		/// <summary>
		///  A special value used to relaunch the terrarium with a given
		///  command line.  This is used by the auto-update service and
		///  when the user switches between Terrarium Mode/EcoSystem Mode.
		/// </summary>
		public static string RelaunchCommandLine
		{
			get
			{
				return GetSetting("relaunchCommandLine");
			}
        
			set
			{
				SetSetting("relaunchCommandLine", value);
			}
		}

		/// <summary>
		/// Allows the user to specify that when running in screen saver
		/// mode, to just use the "simple" screen saver rather than
		/// actually running the Terrarium client.  No UI to access this,
		/// user would hand edit the userconfig.xml
		/// </summary>
		public static bool UseSimpleScreenSaver
		{
			get
			{
				return useSimpleScreenSaver.Getter();
			}

			set
			{
				useSimpleScreenSaver.Setter(value);
			}
		}

		/// <summary>
		/// Allows the user to specify whether Terrarium should start
		/// in fullscreen mode.
		/// </summary>
		public static bool StartFullscreen
		{
			get
			{
				return startFullscreen.Getter();
			}
			set
			{
				startFullscreen.Setter(value);
			}
		}

		/// <summary>
		/// Allows the screen saver to span multiple monitors.  Should be considered
		/// experimental as we make no guarantees it will work depending on video
		/// memory, etc.
		/// </summary>
		public static bool ScreenSaverSpanMonitors
		{
			get
			{
				return screenSaverSpanMonitors.Getter();
			}
			set
			{
				screenSaverSpanMonitors.Setter(value);
			}
		}

		/// <summary>
		/// Allows the client to skip checking the server for disabled versions.
		/// </summary>
		public static bool SkipVersionCheck
		{
			get
			{
				return skipVersionCheck.Getter();
			}
			set
			{
				skipVersionCheck.Setter(value);
			}
		}

		#endregion
		#region "XML Configuration File Members"
		/// <summary>
		///  Retrieves a setting from the user configuration
		///  file based on a setting name.  This method only
		///  retrieves the text of the element in string format.
		/// </summary>
		/// <param name="settingName">Name of the setting to retrieve.</param>
		/// <returns>Returns the value of the setting on success or an empty string if it doesn't.</returns>
		internal static string GetSetting(string settingName)
		{
			XmlDocument myXmlDocument = GetUserSettings();
			XmlNode myXmlSetting = null;

			if ( myXmlDocument != null )
			{
				myXmlSetting = myXmlDocument.SelectSingleNode("/settings/" + settingName);
			}

			if ( myXmlSetting == null )
			{
				return "";
			}
			else
			{
				return myXmlSetting.InnerText;
			}
		}

		/// <summary>
		///  Sets a user configuration file value given the setting name
		///  and the new value in string format.  This method will write
		///  new values, or update existing values.
		/// </summary>
		/// <param name="settingName">Name of the setting to create or update.</param>
		/// <param name="settingValue">Value for the setting.</param>
		internal static void SetSetting(string settingName, string settingValue)
		{
			bool foundElement = false;

			XmlDocument myXmlDocument = GetUserSettings();
			if (myXmlDocument == null)
			{
				myXmlDocument = new XmlDocument();
				XmlNode newRootNode = myXmlDocument.AppendChild(myXmlDocument.CreateNode(XmlNodeType.Element, "settings", ""));
				XmlNode newNode = newRootNode.AppendChild(myXmlDocument.CreateNode(XmlNodeType.Element, settingName, ""));
				newNode.InnerText = settingValue;
			}
			else
			{
				XmlNode root = myXmlDocument.DocumentElement;
				foreach (XmlNode node in root.ChildNodes)
				{
					if (node.Name == settingName)
					{
						node.InnerText = settingValue;
						foundElement = true;
					}
				}

				if (!foundElement)
				{
					XmlNode newNode = root.AppendChild(myXmlDocument.CreateNode(XmlNodeType.Element, settingName, ""));
					newNode.InnerText = settingValue;
				}
			}

			myXmlDocument.Save(UserSettingsLocation);
		}

		/// <summary>
		///  This property is used to retrieve the location of the
		///  game settings file to make it more visible to the user.
		///
		///  Note, this is used to store user config and skin settings.
		/// </summary>
		public static string GameConfigDirectory
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Terrarium");
			}
		}

		/// <summary>
		///  This property is used to retrieve the location of the user
		///  settings file.  The value is based on the location of the
		///  currently executing assembly, generally the Terrarium client.
		/// </summary>
		public static string UserSettingsLocation 
		{
			get
			{
				return Path.Combine(GameConfigDirectory, "userconfig.xml");
			}
		}

		/// <summary>
		///  This property is used to find if the User Settings file
		///  exists.  If it doesn't, then write it out of our cached
		///  resource.
		/// </summary>
		private static bool UserSettingsExists
		{
			get
			{
				return (new FileInfo(UserSettingsLocation)).Exists;
			}
		}

		/// <summary>
		///  This is a *safe* method for writing out a static configuration
		///  file from the embedded configuration resource file.  We have to
		///  have a template to build from.  We could use the GameConfig to
		///  build one, but this allows us to pick and choose which options
		///  will be in the stock config without complicated code.
		/// </summary>
		/// <param name="configFile">This should be a pointer to UserSettingsLocation</param>
		private static void WriteConfigFileFromResources(string configFile) 
		{
			try 
			{
				// Make sure the directory exists
				FileInfo configFileInfo = new FileInfo(configFile);
				DirectoryInfo configDirectory = configFileInfo.Directory;
                
				if ( !configDirectory.Exists ) 
				{
					configDirectory.Create();
				}
            
				// Using hard-coded name here.  We aren't enumerating since that might be dangerous
				// and we wouldn't want to load the wrong default configuration.
				using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Terrarium.Configuration.userconfig.xml")))
				{
					using(StreamWriter sw = new StreamWriter(configFile))
					{
						sw.Write(sr.ReadToEnd());
					}
				}
			} 
			catch {}
		}

		/// <summary>
		///  Gets the current user configuration settings in the form
		///  of an XmlDocument file.
		/// </summary>
		/// <returns>An XmlDocument initalized with settings on success, null otherwise.</returns>
		private static XmlDocument GetUserSettings()
		{
			if ( !UserSettingsExists ) 
			{
				WriteConfigFileFromResources(UserSettingsLocation);
			}

			XmlDocument myXmlDocument = null;
			XmlTextReader reader = null;
			try
			{
				// Load the XML from file
				reader = new XmlTextReader(UserSettingsLocation);
				reader.WhitespaceHandling = WhitespaceHandling.None;

				// Create an XmlDocument from the XmlTextReader
				myXmlDocument = new XmlDocument();
				myXmlDocument.Load(reader);
			}
			catch (FileNotFoundException)
			{
				myXmlDocument = null;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
			return myXmlDocument;
		}

		/// <summary>
		///  Checks the user configuration file for accessibility by
		///  attempting to retrive the user settings.  If the file doesn't
		///  exist then a general exception is found and caught by GetUserSettings.
		///  Other exceptions will get passed back to the callee.  This is
		///  primarily used to detect permissions errors with respect to the
		///  config file.
		/// </summary>
		public static void CheckConfigFile()
		{
			GetUserSettings();
		}
		#endregion
	}
}