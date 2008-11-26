//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using Terrarium.Game;

namespace Terrarium.Hosting
{
    /// <summary>
    ///  Encapsulates a series of functions that can be used to
    ///  query and modify security.
    /// </summary>
    public class SecurityUtils
    {
        private static readonly byte[] s_ecmaPublicKey =
            {
                0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0
            };

        private static readonly byte[] s_microsoftPublicKey = {
                                                                  0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0,
                                                                  0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0,
                                                                  1, 0, 7, 209, 250, 87, 196, 174, 217, 240, 163, 46,
                                                                  132, 170, 15,
                                                                  174, 253, 13, 233, 232, 253, 106, 236, 143, 135, 251,
                                                                  3, 118, 108, 131,
                                                                  76, 153, 146, 30, 178, 59, 231, 154, 217, 213, 220,
                                                                  193, 221, 154, 210,
                                                                  54, 19, 33, 2, 144, 11, 114, 60, 249, 128, 149, 127,
                                                                  196, 225, 119,
                                                                  16, 143, 198, 7, 119, 79, 41, 232, 50, 14, 146, 234, 5
                                                                  , 236, 228,
                                                                  232, 33, 192, 165, 239, 232, 241, 100, 92, 76, 12, 147
                                                                  , 193, 171, 153,
                                                                  40, 93, 98, 44, 170, 101, 44, 29, 250, 214, 61, 116,
                                                                  93, 111, 45,
                                                                  229, 241, 126, 94, 175, 15, 196, 150, 61, 38, 28, 138,
                                                                  18, 67, 101,
                                                                  24, 32, 109, 192, 147, 52, 77, 90, 210, 147
                                                              };

        /// <summary>
        ///  Determines if security is enabled on the machine.
        /// </summary>
        public static bool SecurityEnabled
        {
            get
            {
#pragma warning disable 618 
                return SecurityManager.SecurityEnabled;
#pragma warning restore 618
            }
        }

        /// <summary>
        ///  Determine if verification has been disabled.
        /// </summary>
        public static bool VerificationDisabled
        {
            get
            {
                var st = new StrongNameVerificationState();
                return (st.GlobalSkip || st.TerrariumSkip);
            }
        }

        /// <summary>
        ///  Find out if a managed debugger is attached.
        /// </summary>
        public static bool IsDebuggerAttached
        {
            get { return Debugger.IsAttached; }
        }

        // Make an extremely restrictive permission set that allows only:
        // Execution Permission.
        private static PermissionSet MakeExecutionOnlyPermSet()
        {
            var pSet = new PermissionSet(PermissionState.None);
            var perm = new SecurityPermission(SecurityPermissionFlag.Execution);
            pSet.AddPermission(perm);
            return pSet;
        }

        // Make a permission set that allows unrestricted access
        private static PermissionSet MakeTrustedPermSet()
        {
            var pSet = new PermissionSet(PermissionState.Unrestricted);
            return pSet;
        }

        // Helper routine to pull the StrongName Public Key blob from the assembly
        // that a type resides in.
        private static StrongNamePublicKeyBlob AssemblyBlobFromType(Type t)
        {
            var asm = Assembly.GetAssembly(t);
            if (asm == null)
            {
                throw new Exception("Can't get assembly for " + t);
            }

            var asmName = asm.GetName();
            if (asmName == null)
            {
                throw new Exception("Can't get AssemblyName object");
            }

            var dynablob = asmName.GetPublicKey();
            if (dynablob == null)
            {
                throw new Exception("Can't retrieve assembly public key--is assembly signed?");
            }
            return new StrongNamePublicKeyBlob(dynablob);
        }

        // Helper routine that returns the StrongName public key blob from the 
        // assembly that this code is in
        private static StrongNamePublicKeyBlob MakeSelfRelativeBlob()
        {
            return AssemblyBlobFromType(typeof (AppMgr));
        }

        // Grant all code signed with the Microsoft private key full trust
        // (all of the the .Net Framework and CLR assemblies)
        private static UnionCodeGroup MakeMSCodeGroup()
        {
            var ms = new UnionCodeGroup(
                new StrongNameMembershipCondition(new StrongNamePublicKeyBlob(s_microsoftPublicKey), null, null),
                new PolicyStatement(new PermissionSet(PermissionState.Unrestricted)));
            return ms;
        }

        // Grant all code signed with the Ecma private key full trust
        // (all of the the .Net Framework and CLR assemblies)
        private static UnionCodeGroup MakeEcmaCodeGroup()
        {
            var ecma = new UnionCodeGroup(
                new StrongNameMembershipCondition(new StrongNamePublicKeyBlob(s_ecmaPublicKey), null, null),
                new PolicyStatement(new PermissionSet(PermissionState.Unrestricted)));
            return ecma;
        }

        // This routine sets up the Code Access Security policy that Terrarium runs under.  It is key
        // to ensuring that organisms can never do anything dangerous.
        // 
        // The policy tree looks like this:
        // All Code - Nothing
        //   My Computer - Nothing
        //      Terrarium Code Directory - Execute permission only
        //      Terrarium Key - Full Trust
        //      System.dll Code base - Full Trust (for XML serialization emitted assemblies)
        //      MS Name - Full Trust
        //      ECMA Name - Full Trust
        //      Terrarium.Exe Code Dir - Execute Only
        // 
        // Order is important since we're using a first match code group.  If an assembly lives in the cache directory,
        // it gets nothing.
        internal static PolicyLevel MakePolicyLevel(string cacheDir)
        {
            var noPerms = new PermissionSet(PermissionState.None);

            // All Code, Nothing
            var allCode = new FirstMatchCodeGroup(new AllMembershipCondition(),
                                                  new PolicyStatement(noPerms));

            // My Computer, Nothing
            var myComputer =
                new FirstMatchCodeGroup(new ZoneMembershipCondition(SecurityZone.MyComputer),
                                        new PolicyStatement(noPerms));

            // Terrarium code dir: if name is blank, skip it
            UnionCodeGroup cacheDirGroup = null;
            if (cacheDir != null)
            {
                var cacheDirFull = Path.GetFullPath(cacheDir);
                if (Directory.Exists(cacheDirFull))
                {
                    var fileCanon = cacheDirFull.Replace("\\", "/");
                    var fileUrl = String.Format("file://{0}/*", fileCanon);

                    cacheDirGroup = new UnionCodeGroup(new UrlMembershipCondition(fileUrl),
                                                       new PolicyStatement(MakeExecutionOnlyPermSet()));
                }
            }

            // When webservices creates a serialization dll dynamically, it loads it into memory
            // and it gets the evidence from System.Dll.  Thus, to make sure these assemblies get
            // full trust, we need to make sure that anything that has this same evidence is
            // added to policy
            var codeBase = typeof (Process).Assembly.CodeBase;
            var systemDll =
                new UnionCodeGroup(new UrlMembershipCondition(codeBase),
                                   new PolicyStatement(MakeTrustedPermSet()));

            var myCodeTrust =
                new UnionCodeGroup(new StrongNameMembershipCondition(
                                       MakeSelfRelativeBlob(), null, null),
                                   new PolicyStatement(MakeTrustedPermSet()));

            var myMSTrust = MakeMSCodeGroup();
            var ecmaTrust = MakeEcmaCodeGroup();

            // Terrarium does a Load(byte []) on assemblies to check them before it copies them to the PAC to 
            // truly load them.  However, since unsigned (by MS or Terrarium) assemblies outside of the PAC
            // don't get execute permissions, this load fails.  Since we are doing a Load(byte []), the evidence
            // will say that the assembly is coming from the same location as Terrarium.Exe, therefore
            // we need to add policy that gives unsigned assemblies in the same location as terrarium.exe
            // Execute permissions.  This is the exact code (and logic) we use for the systemDll code group above.
            var terrariumCodeBase = typeof (GameEngine).Assembly.CodeBase;
            var checkAssemblyTrust = new UnionCodeGroup(new UrlMembershipCondition(terrariumCodeBase),
                                                        new PolicyStatement(MakeExecutionOnlyPermSet()));

            // add children of MyComputer CG
            if (cacheDirGroup != null)
            {
                myComputer.AddChild(cacheDirGroup);
            }

            myComputer.AddChild(myCodeTrust);
            myComputer.AddChild(systemDll);
            myComputer.AddChild(myMSTrust);
            myComputer.AddChild(ecmaTrust);
            myComputer.AddChild(checkAssemblyTrust);

            // add MyComputer under All Code
            allCode.AddChild(myComputer);
            var level = PolicyLevel.CreateAppDomainLevel();
            level.RootCodeGroup = allCode;
            return level;
        }

        /// <summary>
        ///  Determine if the given assembly has a terrarium key.
        /// </summary>
        /// <param name="asmName">The assembly to check.</param>
        /// <returns>True if the assembly has the Terrarium key, false otherwise.</returns>
        public static bool AssemblyHasTerrariumKey(AssemblyName asmName)
        {
            var terrKey = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
            var checkKey = asmName.GetPublicKeyToken();

            if (terrKey == null)
            {
                throw new SecurityException("Terrarium must be signed");
            }

            // bugs don't have to be signed, so it's OK it is has no key
            if ((checkKey == null) || (checkKey.Length != terrKey.Length))
            {
                return false;
            }

            if (checkKey.Length == terrKey.Length)
            {
                for (var i = 0; i < terrKey.Length; i++)
                {
                    if (checkKey[i] != terrKey[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}