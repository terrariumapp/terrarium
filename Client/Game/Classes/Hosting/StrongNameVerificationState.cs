//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace Terrarium.Hosting
{
    /// <summary>
    /// Determines if this machine has shut off strong name verification in some way.  
    /// If they have we don't want the user to run, because they could get hacked by 
    /// a malicious animal.
    /// </summary>
    internal class StrongNameVerificationState
    {
        private const string _globalSkip = "*,*";
        private const string StrongNameKey = @"Software\Microsoft\StrongName\Verification"; // HKLM
        private readonly RegistryKey _key = Registry.LocalMachine;
        private readonly StringCollection _skipEntries;

        public StrongNameVerificationState()
        {
            _skipEntries = new StringCollection();
            AcquireState();
        }

        // Returns true if strong name verification is completely off
        internal bool GlobalSkip
        {
            get { return (_skipEntries != null && _skipEntries.Contains(_globalSkip)); }
        }

        internal bool TerrariumSkip
        {
            get
            {
                var result = false;
                if (_skipEntries != null)
                {
                    var token = GetTerrariumPubKeyToken();
                    string key;

                    foreach (var s in _skipEntries)
                    {
                        // e.g. , this looks like terrarium,0271xxxxxxx
                        // or just 0271974B642E7A95 if using anything with the terrarium 
                        // public key token
                        // or *,0271974B642E7A95
                        // if our token is there, we'll fail it
                        // users are in sub keys, but we'll err on the side of enforcement
                        // rather then checking them
                        key = s.ToLower(CultureInfo.InvariantCulture);
                        if ((-1 == key.IndexOf("terrarium")) && (-1 == key.IndexOf(token))) continue;
                        result = true;
                        break;
                    }
                }

                return result;
            }
        }

        private void AcquireState()
        {
            var snKey = _key.OpenSubKey(StrongNameKey, false);
            if (null == snKey) return;
            _skipEntries.AddRange(snKey.GetSubKeyNames());
        }

        internal static string BytesToHexString(byte[] input)
        {
            var sb = new StringBuilder(64);
            if (input != null)
            {
                int i;
                for (i = 0; i < input.Length; i++)
                {
                    sb.Append(String.Format("{0:x2}", input[i]));
                }
            }
            return sb.ToString();
        }

        internal static string GetTerrariumPubKeyToken()
        {
            var key = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
            return BytesToHexString(key);
        }
    }
}