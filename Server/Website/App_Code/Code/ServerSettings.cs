//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Configuration;

namespace Terrarium.Server
{
    public sealed class ServerSettings
    {
        private static string _assemblyPath;
        private static string _chartPath;
        private static string _chartUrl;
        private static string _installRoot;
        private static int _introductionDailyLimit = -1;
        private static int _introductionWait = -1;
        private static string _latestVersion;
        private static int _millisecondsToRollupData = -1;
        private static string _motd;
        private static string _speciesDsn;
        private static string _welcomeMessage;
        private static string _wordListFile;

        public static string WordListFile
        {
            get
            {
                if (_wordListFile == null)
                {
                    string temp = ConfigurationManager.AppSettings["WordListFile"];
                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = InstallRoot;
                        if (temp != string.Empty)
                        {
                            temp += "invalidwordlist.txt";
                        }
                    }
                    _wordListFile = temp;
                }

                return _wordListFile;
            }
        }

        public static string ChartUrl
        {
            get
            {
                if (_chartUrl == null)
                {
                    string temp = ConfigurationManager.AppSettings["ChartUrl"];
                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = "~/chartdata";
                    }
                    _chartUrl = temp;
                }

                return _chartUrl;
            }
        }

        public static string ChartPath
        {
            get
            {
                if (_chartPath == null)
                {
                    string temp = ConfigurationManager.AppSettings["ChartPath"];
                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = InstallRoot;
                        if (temp != string.Empty)
                        {
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
                if (_welcomeMessage == null)
                {
                    string temp = ConfigurationManager.AppSettings["WelcomeMessage"];
                    if (string.IsNullOrEmpty(temp))
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
                if (_motd == null)
                {
                    string temp = ConfigurationManager.AppSettings["MOTD"];
                    if (string.IsNullOrEmpty(temp))
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
                if (_latestVersion == null)
                {
                    string temp = ConfigurationManager.AppSettings["LatestVersion"];
                    if (string.IsNullOrEmpty(temp))
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
            get
            {
                if (_assemblyPath == null)
                {
                    string temp = ConfigurationManager.AppSettings["AssemblyPath"];
                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = InstallRoot;
                        if (temp != string.Empty)
                        {
                            temp += "\\species\\assemblies";
                        }
                    }

                    _assemblyPath = temp;
                }

                return _assemblyPath;
            }
        }

        public static string InstallRoot
        {
            get
            {
                if (_installRoot == null)
                {
                    string temp = ConfigurationManager.AppSettings["InstallRoot"];
                    _installRoot = string.IsNullOrEmpty(temp) ? string.Empty : temp;
                }

                return _installRoot;
            }
        }

        public static string SpeciesDsn
        {
            get
            {
                if (_speciesDsn == null)
                {
                    string temp = ConfigurationManager.AppSettings["SpeciesDsn"];
                    _speciesDsn = string.IsNullOrEmpty(temp) ? string.Empty : temp;
                }

                return _speciesDsn;
            }
        }

        public static int MillisecondsToRollupData
        {
            get
            {
                if (_millisecondsToRollupData == -1)
                {
                    try
                    {
                        int temp = Int32.Parse(ConfigurationManager.AppSettings["MillisecondsToRollupData"]);
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
                if (_introductionWait == -1)
                {
                    try
                    {
                        int temp =
                            Int32.Parse(ConfigurationManager.AppSettings["IntroductionWait"]);
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
                if (_introductionDailyLimit == -1)
                {
                    try
                    {
                        int temp =
                            Int32.Parse(ConfigurationManager.AppSettings["IntroductionDailyLimit"]);
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
}