//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Globalization;
using OrganismBase;
using Terrarium.Configuration;
using Terrarium.Tools;
using System.Runtime.InteropServices;

namespace Terrarium.Game 
{
    /// <summary>
    ///  A special cache that protects storage of organism assemblies
    ///  and provides assembly resolving features.  We store all organism assemblies
    ///  in a special obfuscated directory so that malicious code can't send assemblies
    ///  to Terrarium and then take advantage of exploits in them later.
    /// </summary>
    [Serializable]
    public class PrivateAssemblyCache 
    {
        string dataPath;
        string dataFile;
        static string versionDirectoryPreamble;
        Hashtable hsh;
        long pacSize;
        bool hookAssemblyResolve = false;
        bool trackLastRun = true;

        /// <summary>
        ///  Initializes a versioned directory preamble used to create
        ///  versioned assembly directories.
        /// </summary>
        static PrivateAssemblyCache()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            versionDirectoryPreamble = version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString();
        }

        /// <summary>
        ///  Creates a new private assembly cache initialized with a data path and
        ///  data file.
        /// </summary>
        /// <param name="dataPath">Path where the data will be stored.</param>
        /// <param name="dataFile">Path to a tracking data file.</param>
        public PrivateAssemblyCache(string dataPath, string dataFile)
        {
            Initialize(dataPath, dataFile, true, true);
        }

        /// <summary>
        ///  Creates a new private assembly cache initialized with a data path,
        ///  data file, and controls whether assemblies are resolved, and organism
        ///  tracking is in effect.
        /// </summary>
        /// <param name="dataPath">Path to where the cache will be stored.</param>
        /// <param name="dataFile">Path to the tracking data file.</param>
        /// <param name="hookAssemblyResolve">Determines if the assembly resolving events are hooked.</param>
        /// <param name="trackLastRun">Determines if organism tracking is senabled.</param>
        public PrivateAssemblyCache(string dataPath, string dataFile, bool hookAssemblyResolve, bool trackLastRun)
        {
            Initialize(dataPath, dataFile, hookAssemblyResolve, trackLastRun);
        }

        /// <summary>
        ///  Helper function for initializing the PAC constructors.
        /// </summary>
        /// <param name="dataPath">Path to where the cache will be stored.</param>
        /// <param name="dataFile">Path to the tracking data file.</param>
        /// <param name="hookAssemblyResolve">Determines if the assembly resolving events are hooked.</param>
        /// <param name="trackLastRun">Determines if organism tracking is senabled.</param>
        private void Initialize(string dataPath, string dataFile, bool hookAssemblyResolve, bool trackLastRun) 
        {
            this.trackLastRun = trackLastRun;
            if (dataPath == null || dataPath.Length == 0) 
            {
                this.dataPath = Path.GetFullPath(".");
            }
            else
            {
                this.dataPath = Path.GetFullPath(dataPath);
            }
        
            this.dataFile = Path.GetFileName(dataFile);
            this.hsh = new Hashtable();

            if (!Directory.Exists(BaseAssemblyDirectory))
            {
                Directory.CreateDirectory(BaseAssemblyDirectory);
            }

            // We need to ask the CLR to ask us to find the assemblies because they are in a 
            // non-standard location.
            this.hookAssemblyResolve = hookAssemblyResolve;
            if (hookAssemblyResolve)
            {
                HookAssemblyResolve();
            }
        }

        /// <summary>
        ///  Event used to notify clients when assemblies in the
        ///  PAC have changed.
        /// </summary>
        public event PacAssembliesChangedEventHandler PacAssembliesChanged;

        /// <summary>
        ///  Helper function for calling the PacAssembliesChanged
        ///  event.
        /// </summary>
        /// <param name="e">Null</param>
        protected void OnPacAssembliesChanged(EventArgs e)
        {
            if (PacAssembliesChanged != null)
            {
                PacAssembliesChanged(this, e);
            }
        }

        /// <summary>
        ///  Hooks the assembly resolving events for the current app
        ///  domain.
        /// </summary>
        public void HookAssemblyResolve()
        {
            if (this.hookAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
            }
        }

        /// <summary>
        ///  Handles controlled shut-down of the PAC and unhooking the assembly
        ///  resolving events.
        /// </summary>
        public void Close()
        {
            if (this.hookAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(ResolveAssembly);
            }
        }

        /// <summary>
        ///  We track when organisms are running and Terrarium shuts down unexpectedly so that
        ///  we can blacklist them.
        /// </summary>
        public string LastRun
        {
            get
            {
                using (FileStream lastRunFile = File.Open(AssemblyDirectory + "\\data.dat", FileMode.OpenOrCreate))
                {
                    using (BinaryReader lastRunReader = new BinaryReader(lastRunFile))
                    {
                        if (lastRunFile.Length == 0)
                        {
                            return "";
                        }
                        else
                        {
                            return lastRunReader.ReadString();
                        }
                    }
                }
            }

            set
            {
                using (FileStream lastRunFile = File.Open(AssemblyDirectory + "\\data.dat", FileMode.OpenOrCreate))
                {
                    using (BinaryWriter lastRunWriter = new BinaryWriter(lastRunFile))
                    {
                        // If we're tracking who is running OR we're not but we're clearing it
                        // go ahead and write. This last case is needed since, even if we aren't
                        // tracking who is running, we need to clear it on startup and we want to
                        // make sure we do
                        if (trackLastRun || (!trackLastRun && value.Length == 0))
                        {
                            lastRunWriter.Write(value);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Given an assembly full name, generates a library file name
        ///  for an assembly.
        /// </summary>
        /// <param name="fullName">The assembly full name.</param>
        /// <returns>The new file name generated from an assembly name and dll extension.</returns>
        /// <threadsafe/>
        public string GetFileName(string fullName)
        {
            string [] chunks = fullName.Split(new Char[] {','});
            return AssemblyDirectory + "\\" + GetAssemblyShortName(fullName) + ".dll";
        }

        /// <summary>
        ///  Given an assembly full name, generates an assembly short
        ///  name.
        /// </summary>
        /// <param name="fullName">The assembly full name.</param>
        /// <returns>The short name for the assembly.</returns>
        /// <threadsafe/>
        public static string GetAssemblyShortName(string fullName)
        {
            string [] chunks = fullName.Split(new Char[] {','});

            // return tolower so case insensitive stuff works ok
            return chunks[0].ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///  Given an assembly full name, generates an assembly
        ///  version string.
        /// </summary>
        /// <param name="fullName">The assembly full name.</param>
        /// <returns>The version of the assembly.</returns>
        /// <threadsafe/>
        public static string GetAssemblyVersion(string fullName)
        {
            string [] chunks = fullName.Split(new Char[] {','});
            string versionChunk = chunks[1];
            string [] versionPieces = versionChunk.Split(new Char[] {'='});

            return versionPieces[1];
        }

        /// <summary>
        ///  Returns the path to the assembly cache directory.
        /// </summary>
        /// <threadsafe/>
        public string AssemblyDirectory
        {
            get
            {
                return BaseAssemblyDirectory;
            }
        }

        /// <summary>
        ///  Returns the base assembly directory without obfuscation.
        /// </summary>
        /// <threadsafe/>
        string BaseAssemblyDirectory 
        {
            get 
            {
                return GetBaseAssemblyDirectory(dataPath, dataFile);
            }
        }

        /// <summary>
        ///  Creates a base assembly directory without obfuscation from a path and file name.
        /// </summary>
        /// <param name="dataPath">The base path of the data file.</param>
        /// <param name="dataFile">The name of the datafile for creating a cache directory.</param>
        /// <returns>The base assembly directory.</returns>
        /// <threadsafe/>
        public static string GetBaseAssemblyDirectory(string dataPath, string dataFile) 
        {
            string dataFilePart = dataFile.Substring(0, dataFile.LastIndexOf("."));
            return Path.Combine(dataPath, dataFilePart + "_Assemblies");
        }

        /// <summary>
        ///  Blacklist a series of assemblies by setting their assemblies to zero length
        ///  files.  This is a way of ensuring that we don't load them again, or replace them
        ///  with fresh working copies because the file name gets reserved with an invalid assembly.
        /// </summary>
        /// <param name="assemblies">The assemblies to be blacklisted.</param>
        public void BlacklistAssemblies(string [] assemblies) 
        {
            // Ensure that the caller can't pass us a name that actually makes this file get created somewhere else
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] {AssemblyDirectory});

            try
            {
                permission.PermitOnly();

                if (assemblies != null)
                {
                    foreach (string assembly in assemblies) 
                    {
                        string fileName = GetFileName(assembly);
                        try
                        {
                            // Replace this file with a zero length file (or create one), so we won't get it again
                            using (FileStream stream = File.Create(fileName))
                            {
                                // We don't do anything with the file
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Error blacklisting organism:");
                            ErrorLog.LogHandledException(e);
                        }
                    }
                }
            }
            finally
            {
                CodeAccessPermission.RevertPermitOnly();
            }
        }

        /// <summary>
        ///  Load an organism assembly.  Calculate the size of the PAC
        ///  based on the assemblies loaded.
        /// </summary>
        /// <param name="fullName">The name of the assembly to load.</param>
        /// <returns>The loaded assembly.</returns>
        public Assembly LoadOrganismAssembly(string fullName)
        {
            Assembly organismAssembly;

            if (!hsh.ContainsKey(fullName))
            {
                hsh[fullName] = fullName;
                pacSize += new FileInfo(GetFileName(fullName)).Length;
            }

            // Make sure we can't be hacked to load a bogus assembly by only allowing our code
            // to access files in the assembly directory.
            string asmDir = Path.GetFullPath(AssemblyDirectory);
            //FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] {asmDir});
            try
            {
              //  permission.PermitOnly();
                organismAssembly = System.Reflection.Assembly.LoadFile(GetFileName(fullName));
            }
            finally
            {
                //CodeAccessPermission.RevertPermitOnly();
            }

            return organismAssembly;
        }

        /// <summary>
        ///  Return the approximate size of the PAC in memory based on loaded
        ///  assemblies.
        /// </summary>
        public long PacSize
        {
            get
            {
                return pacSize;
            }
        }
    
        /// <summary>
        ///  Return the approximate amount of organisms loaded from
        ///  the current PAC.
        /// </summary>
        public int PacOrganismCount
        {
            get
            {
                return hsh.Count;
            }
        }

        /// <summary>
        ///  Determine if the assembly with the given full name exists
        ///  within the PAC.
        /// </summary>
        /// <param name="fullName">The full name of the assembly to check for.</param>
        /// <returns>True if the assembly exists, false otherwise.</returns>
        public Boolean Exists(string fullName)
        {
            bool exists = false;
            string asmDir = Path.GetFullPath(AssemblyDirectory);

            // Make sure we can't be hacked to return whether files exist by only allowing our code
            // to access files in the assembly directory.
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] {asmDir});
            try
            {
                permission.PermitOnly();
                exists = File.Exists(GetFileName(fullName));
            }
            finally
            {
                CodeAccessPermission.RevertPermitOnly();
            }

            return exists;
        }

        /// <summary>
        ///  Saves the array of bytes given an assembly full name to
        ///  the private assembly cache.
        /// </summary>
        /// <param name="bytes">The bytes of the assembly.</param>
        /// <param name="fullName">The full name of the original assembly.</param>
        public void SaveOrganismBytes(byte [] bytes, string fullName)
        {
            string [] chunks = fullName.Split(new Char[] {','});
            string directory = AssemblyDirectory;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = GetFileName(fullName);

            if (File.Exists(fileName))
            {
                return;
            }
        
            // Ensure that the caller can't pass us a name that actually makes this file get created somewhere else
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] {directory});

            try
            {
                permission.PermitOnly();

                FileStream targetStream = File.Create(fileName);
                try 
                {
                    targetStream.Write(bytes, 0, bytes.Length);
                }
                catch 
                {
                    targetStream.Close();

                    // If something happens, delete the file so we don't have
                    // a corrupted file hanging around
                    File.Delete(fileName);

                    throw;
                }
                finally
                {
                    targetStream.Close();
                }
            }
            finally 
            {
                CodeAccessPermission.RevertPermitOnly();
            }

            OnPacAssembliesChanged(new EventArgs());
        }
    
        /// <summary>
        ///  Save an assembly given a full path to the assembly and the full
        ///  name of the assembly to the cache.
        /// </summary>
        /// <param name="assemblyPath">A local path to the assembly.</param>
        /// <param name="fullName">The full name of the assembly.</param>
        public void SaveOrganismAssembly(string assemblyPath, string fullName)
        {
            SaveOrganismAssembly(assemblyPath, null, fullName);
        }

        /// <summary>
        ///  Save an assembly along with symbols given a full path to the
        ///  assembly and symbols to the cache.
        /// </summary>
        /// <param name="assemblyPath">A local path to the assembly.</param>
        /// <param name="symbolPath">A local path to the symbols.</param>
        /// <param name="fullName">The full name of the assembly.</param>
        public void SaveOrganismAssembly(string assemblyPath, string symbolPath, string fullName)
        {
            string directory = AssemblyDirectory;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = GetFileName(fullName);
            string symName = Path.ChangeExtension(GetFileName(fullName), ".pdb");

            if (File.Exists(fileName))
            {
                return;
            }
        
            string reportPath = Path.Combine(GameConfig.ApplicationDirectory, Guid.NewGuid() + ".xml");
            int validAssembly = CheckAssemblyWithReporting(assemblyPath, reportPath);

            if (validAssembly == 0)
            {
                throw OrganismAssemblyFailedValidationException.GenerateExceptionFromXml(reportPath);
            }
            
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            FileStream sourceStream = File.OpenRead(assemblyPath);
            FileStream symStream = null;
            try 
            {
                if (symbolPath != null && symbolPath.Length != 0 && File.Exists(symbolPath))
                {
                    symStream = File.OpenRead(symbolPath);
                }
            }
            catch
            {
                // In case anything goes wrong with the symbols (access denied and others), we are good
            }

            // Ensure that the caller can't pass us a name that actually makes this file get created somewhere else
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, new string[] {directory});
            try
            {
                permission.PermitOnly();

                FileStream targetStream = File.Create(fileName);
                try 
                {
                    byte[] bytes = new byte[65536];
                    int bytesRead;
                    while ((bytesRead = sourceStream.Read(bytes, 0, 65536)) > 0)
                    {
                        targetStream.Write(bytes, 0, bytesRead);
                    }
                }
                catch 
                {
                    targetStream.Close();

                    // If something happens, delete the file so we don't have
                    // a corrupted file hanging around
                    File.Delete(fileName);

                    throw;
                }
                finally 
                {
                    targetStream.Close();
                }
            
                if (symStream != null)
                {
                    try
                    {
                        targetStream = File.Create(symName);
                        try
                        {
                            byte[] bytes = new byte[65536];
                            int bytesRead;
                            while ((bytesRead = symStream.Read(bytes, 0, 65536)) > 0)
                            {
                                targetStream.Write(bytes, 0, bytesRead);
                            }
                        }
                        catch
                        {
                            targetStream.Close();

                            // If something happens, delete the file so we don't have
                            // a corrupted file hanging around
                            File.Delete(symName);

                            throw;
                        }
                        finally
                        {
                            targetStream.Close();
                        }
                    }
                    catch
                    {
                        // No reason to crash
                    }
                }
            }
            finally
            {
                CodeAccessPermission.RevertPermitOnly();
                sourceStream.Close();
            }

            OnPacAssembliesChanged(new EventArgs());
        }

        /// <summary>
        ///  Gets a full listing of all of the assemblies in the
        ///  cache.  This can be used to populate dropdowns or lists.
        /// </summary>
        /// <returns>A set of OrganismAssemblyInfo objects for each assembly.</returns>
        public OrganismAssemblyInfo [] GetAssemblies()
        {
            ArrayList infoList = new ArrayList();

            if (!Directory.Exists(AssemblyDirectory))
            {
                return new OrganismAssemblyInfo [0];
            }

            string [] fileEntries = Directory.GetFiles(AssemblyDirectory);
            foreach (string fileName in fileEntries)
            {
                if (Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture) == ".dll")
                {
                    FileInfo info = new FileInfo(fileName);
                    if (info.Length > 0)
                    {
                        Assembly assembly = Assembly.LoadFrom(fileName);
                        infoList.Add(new OrganismAssemblyInfo(assembly.FullName, GetAssemblyShortName(assembly.FullName)));
                    }
                }
            }

            return (OrganismAssemblyInfo []) infoList.ToArray(typeof(OrganismAssemblyInfo));
        }

        /// <summary>
        ///  Get a list of all assemblies in the cache that have
        ///  0 bytes.  A 0 byte assembly fails to load.
        /// </summary>
        /// <returns>A listing of all 0 byte assemblies in the cache.</returns>
        public string [] GetBlacklistedAssemblies()
        {
            ArrayList blacklistedAssemblies = new ArrayList();

            if (Directory.Exists(AssemblyDirectory))
            {
                string [] fileEntries = Directory.GetFiles(AssemblyDirectory);
                foreach (string fileName in fileEntries)
                {
                    if (Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture) == ".dll")
                    {
                        FileInfo info = new FileInfo(fileName);
                        if (info.Length == 0)
                        {
                            blacklistedAssemblies.Add(Path.GetFileNameWithoutExtension(fileName));
                        }
                    }
                }
            }

            return (string []) blacklistedAssemblies.ToArray(typeof(string));
        }

        /// <summary>
        ///  Called whenever assemblies are being resolved for use.
        /// </summary>
        /// <param name="sender">Unknown</param>
        /// <param name="args">Assembly resolve information</param>
        /// <returns>The assembly that matches the resolve information.</returns>
        internal Assembly ResolveAssembly(Object sender, ResolveEventArgs args)
        {
            // Test for OrganismBase
            Assembly matchAssembly = GetAssemblyWithTerrariumBindingPolicy(args.Name, Assembly.GetAssembly(typeof(Animal)));
            if (matchAssembly != null)
            {
                return matchAssembly;
            }

            // Test for Terrarium
            matchAssembly = GetAssemblyWithTerrariumBindingPolicy(args.Name, Assembly.GetAssembly(typeof(PrivateAssemblyCache)));
            if (matchAssembly != null)
            {
                return matchAssembly;
            }

            // See if it is an organism assembly
            // Need to assert permission to open files on disk because we need to load organism assemblies
            // and organism code could be on the stack because it may be loading an assembly that
            // has not been loaded yet.  This would fail without the assertion.
            FileIOPermission permission = new FileIOPermission(PermissionState.Unrestricted);
            permission.Assert();
            return LoadOrganismAssembly(args.Name);
        }

        /// <summary>
        ///  Check to see if an assembly can be found that matches a reduced
        ///  set of fusion version requirements.  We ignore the revision.
        /// </summary>
        /// <param name="testAssemblyName">The name of the test assembly.</param>
        /// <param name="potentialMatchAssembly">A potentially matching assembly.</param>
        /// <returns>The matching assembly if it really matches, else null.</returns>
        private Assembly GetAssemblyWithTerrariumBindingPolicy(string testAssemblyName, Assembly potentialMatchAssembly)
        {
            Debug.Assert(potentialMatchAssembly != null);
            try 
            {
//                if (GetAssemblyShortName(testAssemblyName) == GetAssemblyShortName(potentialMatchAssembly.GetName().Name))
                if (GetAssemblyShortName(testAssemblyName) == GetAssemblyShortName(potentialMatchAssembly.FullName))
                {
                    Version testAssemblyVersion = new Version(GetAssemblyVersion(testAssemblyName));
                    Debug.Assert(testAssemblyVersion != null);
                    Version potentialAssemblyVersion = potentialMatchAssembly.GetName().Version;

                    if (testAssemblyVersion.Major == potentialAssemblyVersion.Major &&
                        testAssemblyVersion.Minor == potentialAssemblyVersion.Minor &&
                        testAssemblyVersion.Build == potentialAssemblyVersion.Build)
                    {
                        return potentialMatchAssembly;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                ErrorLog.LogHandledException(e);
                return null;
            }
        }

        /// <summary>
        ///  Provides access to the versioned directory preamble for generating
        ///  cache directories.
        /// </summary>
        /// <threadsafe/>
        public static string VersionedDirectoryPreamble
        {
            get
            {
                return versionDirectoryPreamble;
            }
        }

        /// <summary>
        ///  Creates a temp file that can't be guessed for security reasons.
        /// </summary>
        /// <returns>A temp file GUID encoded for security.</returns>
        public static string GetSafeTempFileName()
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()); 
        }

        /// <summary>
        ///  Used for assembly validation with XML reporting in AsmCheck
        /// </summary>
        //[System.Security.SuppressUnmanagedCodeSecurityAttribute()]
//        [DllImport("asmcheck.dll", CharSet = CharSet.Unicode)]
//        internal static extern int CheckAssemblyWithReporting(String assemblyName, String xmlFile);
        internal static int CheckAssemblyWithReporting(string assemblyName, string xmlFile)
        {
            return 1;
        }
    }
}