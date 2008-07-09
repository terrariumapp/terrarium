//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using Microsoft.Win32;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace Terrarium.Hosting 
{
    // Determines if this machine has shut off strong name verification in some way.  If they have
    // we don't want the user to run, because they could get hacked by a malicious animal.
    internal class StrongNameVerificationState
    {
        private const string StrongNameKey = @"Software\Microsoft\StrongName\Verification"; // HKLM
        private const string _globalSkip = "*,*";
        private RegistryKey _key = Registry.LocalMachine;
        private StringCollection _skipEntries;

        public StrongNameVerificationState()
        {
            _skipEntries = new StringCollection();
            AcquireState();
        }

        // Open up the reg key that shuts off strong name verification and see
        // what state it is in.
        private void AcquireState()
        {
            RegistryKey snKey = _key.OpenSubKey(StrongNameKey,false);
            if (null != snKey)
            {
                string[] entries = snKey.GetSubKeyNames();
                if (null != entries)
                {
                    _skipEntries.AddRange(entries);
                }
            }
        }

        // Returns true if strong name verification is completely off
        internal bool GlobalSkip
        {
            get
            {
                return(_skipEntries != null && _skipEntries.Contains(_globalSkip));
            }
        }

        internal static string BytesToHexString(byte[] input)
        {
            StringBuilder sb = new StringBuilder(64);
            if ( input != null )
            {
                int i;
                for (i = 0; i < input.Length; i++)
                {
                    sb.Append(String.Format("{0:x2}",input[i]));
                }
            }
            return sb.ToString();
        }

        internal string GetTerrariumPubKeyToken()
        {
            byte[] key = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
            return BytesToHexString(key);
        }

        // Returns true if the user has shut off strong name verification for the Terrarium assembly
        // There are lots of complexities in how users can shut off verification.
        // You can skip a given public key token (e.g. the terrarium one)
        // or skip only for given users
        // we just check for "terrarium" and global skips
        internal bool TerrariumSkip
        {
            get
            {
                bool result = false;
                if (_skipEntries != null)
                {
                    string token = GetTerrariumPubKeyToken();
                    string key;

                    foreach (string s in _skipEntries)
                    {
                        // e.g. , this looks like terrarium,0271xxxxxxx
                        // or just 0271974B642E7A95 if using anything with the terrarium 
                        // public key token
                        // or *,0271974B642E7A95
                        // if our token is there, we'll fail it
                        // users are in sub keys, but we'll err on the side of enforcement
                        // rather then checking them
                        key = s.ToLower(CultureInfo.InvariantCulture);

                        if ((-1 != key.IndexOf("terrarium")) || (-1 != key.IndexOf(token)))
                        {
                            result = true;
                            break;
                        }
                    }
                }

                return result;
            }
        }

    }
}