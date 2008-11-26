//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Terrarium.Game;

namespace Terrarium.PeerToPeer
{
    // This is a serialization binder that is used to make sure Terrarium never deserializes an
    // object that shouldn't be able to be used by an organism. A malicious person could send
    // a stream that has objects in it that Code Access Security would prevent the organism from
    // creating.  However, When Terrarium deserializes the stream, there is no organism code on
    // the stack, so Terrarium could inadvertantly create these objects and hand them off.  This
    // binder ensures that this won't happen, because it prevents the serializer from creating objects
    // that aren't in the "safe" list of objects we know to be OK.
    internal class TeleportStateBinder : OrganismBaseBinder
    {
        public override Type BindToType(string asm, string type)
        {
            // If the object isn't from one of these assemblies, then it might not be safe
            if (!IsOrganismBase(asm) && !IsTerrarium(asm) && !IsMscorlib(asm) && !IsSystemDrawing(asm))
            {
                Trace.WriteLine(string.Format("Unhandled Type - {0}|{1}", asm, type));
                throw new ApplicationException(string.Format("Invalid Type in TeleportStateBinder {0}|{1}", asm, type));
            }

            switch (type)
            {
                case "System.UnitySerializationHolder":
                case "Terrarium.PeerToPeer.TeleportState":

                    // TeleportState
                case "System.Guid":
                case "OrganismBase.PlantState":
                case "OrganismBase.AnimalState":
                case "Terrarium.Hosting.OrganismWrapper":

                    // OrganismState
                case "OrganismBase.DefendAction":
                case "OrganismBase.AttackAction":
                case "OrganismBase.EatAction":
                case "OrganismBase.MoveToAction":
                case "OrganismBase.ReproduceAction":
                case "System.Drawing.Point":
                case "Terrarium.Game.PlantSpecies":
                case "Terrarium.Game.AnimalSpecies":
                case "OrganismBase.OrganismEventResults":
                case "OrganismBase.PopulationChangeReason":
                case "OrganismBase.AntennaState":
                case "OrganismBase.AntennaPosition":

                    // AnimalSpecies
                case "OrganismBase.AnimalSkinFamily":

                    // PlantSpecies
                case "OrganismBase.PlantSkinFamily":

                    // OrganismSpecies
                case "System.Drawing.KnownColor":

                    // MoveToAction
                case "OrganismBase.MovementVector":
                case "OrganismBase.ReasonForStop":

                    // OrganismEventResults
                case "OrganismBase.LoadEventArgs":
                case "OrganismBase.IdleEventArgs":
                case "OrganismBase.TeleportedEventArgs":
                case "OrganismBase.AttackedEventArgsCollection":
                case "OrganismBase.AttackedEventArgs":
                case "OrganismBase.OrganismRemovedEventArgs":
                case "OrganismBase.BornEventArgs":
                case "OrganismBase.MoveCompletedEventArgs":
                case "OrganismBase.EatCompletedEventArgs":
                case "OrganismBase.AttackCompletedEventArgs":
                case "OrganismBase.ReproduceCompletedEventArgs":
                case "OrganismBase.DefendCompletedEventArgs":
                case "System.Collections.ArrayList":
                    // for AttackedEventArgsCollection because it has an arraylist in ReadOnlyCollectionBase
                    return null;

                default:
                    Trace.WriteLine(string.Format("Unhandled Type - {0}|{1}", asm, type));
                    throw new ApplicationException(string.Format("Invalid Type in TeleportStateBinder {0}|{1}", asm,
                                                                 type));
            }
        }
    }
}