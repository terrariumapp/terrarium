//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Configuration;
using System.Web;

namespace Terrarium.Server 
{
    public sealed class ServerSettings
	{
        /* <settings>
            <add key="InstallRoot" value="c:\fxdev\samples\terrarium" />
            <add key="ChartPath" value="c:\fxdev\samples\terrarium\website\chartdata\" />
            <add key="ChartUrl" value="/terrarium/chartdata/" />
            <add key="AssemblyPath" value="c:\fxdev\samples\terrarium\website\species\assemblies" />
            <add key="WordListFile" value="c:\terrarium_list.txt" />
        */

        private static string	_installRoot;
        private static string	_chartPath;
        private static string	_chartUrl;
        private static string	_assemblyPath;
        private static string	_wordListFile;
		private static string	_speciesDsn;
		private static int		_millisecondsToRollupData = -1;
		private static string	_welcomeMessage;
		private static string	_motd;
		private static string	_latestVersion;
		private static int		_introductionWait = -1;
		private static int		_introductionDailyLimit = -1;
		
        public static string WordListFile {
            get {
                if ( _wordListFile == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["WordListFile"];
                    if ( temp == null || temp == string.Empty ) {
                        temp = ServerSettings.InstallRoot;
                        if ( temp != string.Empty ) {
                            temp += "invalidwordlist.txt";
                        }
                    }

                    _wordListFile = temp;
                }

                return _wordListFile;
            }
        }

        public static string ChartUrl {
            get {
                if ( _chartUrl == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["ChartUrl"];
                    if ( temp == null || temp == string.Empty ) 
					{
                        temp = "~/chartdata";
                    }

                    _chartUrl = temp;
                }

                return _chartUrl;
            }
        }

        public static string ChartPath {
            get {
                if ( _chartPath == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["ChartPath"];
                    if ( temp == null || temp == string.Empty ) {
                        temp = ServerSettings.InstallRoot;
                        if ( temp != string.Empty ) {
                            temp += "chartdata";
                        }
                    }

                    _chartPath = temp;
                }

                return _chartPath;
            }
        }

		public static string WelcomeMessage 
		{
			get 
			{
				if ( _welcomeMessage == null ) 
				{
					string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["WelcomeMessage"];
					if ( temp == null || temp == string.Empty ) 
					{
							temp = "";
					}

					_welcomeMessage = temp;
				}

				return _welcomeMessage;
			}
		}

		public static string MOTD 
		{
			get 
			{
				if ( _motd == null ) 
				{
					string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["MOTD"];
					if ( temp == null || temp == string.Empty ) 
					{
						temp = "";
					}

					_motd = temp;
				}

				return _motd;
			}
		}

		public static string LatestVersion 
		{
			get 
			{
				if ( _latestVersion == null ) 
				{
					string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["LatestVersion"];
					if ( temp == null || temp == string.Empty ) 
					{
						temp = "1.0.0.0";
					}

					_latestVersion = temp;
				}

				return _latestVersion;
			}
		}

        public static string AssemblyPath 
		{
            get {
                if ( _assemblyPath == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["AssemblyPath"];
                    if ( temp == null || temp == string.Empty ) {
                        temp = ServerSettings.InstallRoot;
                        if ( temp != string.Empty ) {
                            temp += "\\species\\assemblies";
                        }
                    }

                    _assemblyPath = temp;
                }

                return _assemblyPath;
            }
        }

        public static string InstallRoot {
            get {
                if ( _installRoot == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["InstallRoot"];
                    if ( temp == null || temp == string.Empty ) {
                        _installRoot = string.Empty;
                    } else {
                        _installRoot = temp;
                    }
                }

                return _installRoot;
            }
        }

		public static string SpeciesDsn 
		{
			get 
			{
				if ( _speciesDsn == null ) 
				{
					string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["SpeciesDsn"];
					if ( temp == null || temp == string.Empty ) 
					{
						_speciesDsn = string.Empty;
					} 
					else 
					{
						_speciesDsn = temp;
					}
				}

				return _speciesDsn;
			}
		}

		public static int MillisecondsToRollupData 
		{
			get 
			{
				if ( _millisecondsToRollupData == -1 ) 
				{
					try 
					{
						int temp = Int32.Parse((String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["MillisecondsToRollupData"]);
						_millisecondsToRollupData = temp;
					} 
					catch 
					{
						_millisecondsToRollupData = 450000;
					}
				}

				return _millisecondsToRollupData;
			}
		}

		public static int IntroductionWait 
		{
			get 
			{
				if ( _introductionWait == -1 ) 
				{
					try 
					{
						int temp = Int32.Parse((String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["IntroductionWait"]);
						_introductionWait = temp;
					} 
					catch 
					{
						_introductionWait = 5;
					}
				}

				return _introductionWait;
			}
		}

		public static int IntroductionDailyLimit 
		{
			get 
			{
				if ( _introductionDailyLimit == -1 ) 
				{
					try 
					{
						int temp = Int32.Parse((String) ((Hashtable) HttpContext.Current.GetConfig("serverSettings"))["IntroductionDailyLimit"]);
						_introductionDailyLimit = temp;
					} 
					catch 
					{
						_introductionDailyLimit = 30;
					}
				}

				return _introductionDailyLimit;
			}
		}

    }

    public sealed class RemoteSettings {
        /* <remoteSettings>
            <add key="Terrarium" value="server=severname;uid=TerrariumUser;password=b33tl3;database=Terrarium;" />
            <add key="RemoteStatsServer" value="http://fwfarm2/terrarium" />
            <add key="RemoteStatsServerRoot" value="http://fwfarm2" />
            <add key="InternalInstallPath" value="\\ndpdist\terrarium" />
            <add key="Internal" value="True" />
            <add key="Admins" value="alias@domain.com" />
            <add key="StatsTimeout" value="1000" />
        */

        private static string _terrarium;
        private static string _remoteStatsServer;
        private static string _remoteStatsServerRoot;
        private static string _internalInstallPath;
        private static int _internal = -1;
        private static string _admins;
        private static int _statsTimeout = -1;

        public static int StatsTimeout {
            get {
                if ( _statsTimeout == -1 ) {
                    try {
                        int temp = Int32.Parse((String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["StatsTimeout"]);
                        _statsTimeout = temp;
                    } catch {
                        _statsTimeout = 1000;
                    }
                }

                return _statsTimeout;
            }
        }

        public static string InternalInstallPath {
            get {
                if ( _internalInstallPath == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["InternalInstallPath"];
                    if ( temp == null || temp == string.Empty ) {
                        _internalInstallPath = string.Empty;
                    } else {
                        _internalInstallPath = temp;
                    }
                }

                return _internalInstallPath;
            }
        }

        public static string Admins {
            get {
                if ( _admins == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["Admins"];
                    if ( temp == null || temp == string.Empty ) {
                        _admins = string.Empty;
                    } else {
                        _admins = temp;
                    }
                }

                return _admins;
            }
        }

        public static bool Internal {
            get {
                if ( _internal == -1 ) {
                    try {
                        bool temp = Boolean.Parse((String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["Internal"]);
                        if ( temp ) {
                            _internal = 1;
                        } else {
                            _internal = 0;
                        }
                    } catch {
                        _internal = 0;
                    }
                }

                if ( _internal == 1 ) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public static string Terrarium {
            get {
                if ( _terrarium == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["Terrarium"];
                    if ( temp == null || temp == string.Empty ) {
                        _terrarium = string.Empty; // We should find a way to do a default here
                    } else {
                        _terrarium = temp;
                    }
                }

                return _terrarium;
            }
        }

        public static string RemoteStatsServer {
            get {
                if ( _remoteStatsServer == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["RemoteStatsServer"];
                    if ( temp == null || temp == string.Empty ) {
                        _remoteStatsServer = RemoteStatsServerRoot;
                        if ( _remoteStatsServer != string.Empty ) {
                            _remoteStatsServer += "/terrarium";
                        }
                    } else {
                        _remoteStatsServer = temp;
                    }
                }

                return _remoteStatsServer;
            }
        }

        public static string RemoteStatsServerRoot {
            get {
                if ( _remoteStatsServerRoot == null ) {
                    string temp = (String) ((Hashtable) HttpContext.Current.GetConfig("remoteSettings"))["RemoteStatsServerRoot"];
                    if ( temp == null || temp == string.Empty ) {
                        _remoteStatsServerRoot = string.Empty;
                    } else {
                        _remoteStatsServerRoot = temp;
                    }
                }

                return _remoteStatsServerRoot;
            }
        }
    }
}