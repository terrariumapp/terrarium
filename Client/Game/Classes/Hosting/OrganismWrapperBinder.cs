//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                              
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Terrarium.Game;

namespace Terrarium.Hosting
{
    // Use a binder to ensure that only a certain set of objects
    // can be deserialized that we know to be safe from a
    // Code Access Security perspective.
    internal class OrganismWrapperBinder : OrganismBaseBinder
    {
        public override Type BindToType(string asm, string type)
        {
            if (!IsTerrarium(asm) && !IsMscorlib(asm))
            {
                Trace.WriteLine(string.Format("Unhandled Type - {0}|{1}", asm, type));
                throw new ApplicationException(string.Format("Invalid Type in OrganismWrapperBinder {0}|{1}", asm, type));
            }

            switch (type)
            {
                case "System.UnitySerializationHolder":
                case "Terrarium.Hosting.OrganismWrapper":
                    return null;

                default:
                    Trace.WriteLine(string.Format("Unhandled Type - {0}|{1}", asm, type));
                    throw new ApplicationException(string.Format("Invalid Type in OrganismWrapperBinder {0}|{1}", asm,
                                                                 type));
            }
        }
    }
}