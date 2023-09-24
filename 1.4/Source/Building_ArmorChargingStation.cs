using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using Verse.AI;

namespace Grimforge
{
    public class Building_ArmorChargingStation : Building
    {
        //Again, using Mechanical Humanlikes as an example

        private IEnumerable<IntVec3> adjacencies;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            adjacencies = GenAdj.CellsAdjacent8Way(this);
        }

        public virtual FloatMenuOption CheckIfNotAllowed(Pawn pawn)
        {
            //Many of these are again copied from MH.  
            // Check if the pawn can reach the building safely.
            if (!pawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some))
            {
                return new FloatMenuOption("CannotUseNoPath".Translate(), null);
            }

            // Check if the building itself has power and is not broken down.
            if (GetComp<CompPowerTrader>()?.PowerOn != true)
            {
                return new FloatMenuOption("CannotUseNoPower".Translate(), null);
            }

            // Check if the building has all of its interaction spots used.
            if (GetOpenRechargeSpot(pawn) == IntVec3.Invalid)
            {
                return new FloatMenuOption("GFAA_NoAvailableChargingSpots".Translate(), null);
            }

            // Check if the pawn is actually wearing GF Power Armor
            Apparel_WarcasketGrimforge sample = new Apparel_WarcasketGrimforge();
            //if(!sample.GetType().IsAssignableFrom(pawn.apparel.GetType()))
            //Log.Message("First apparel: " + pawn.apparel.GetType().ToString());
            //List<Apparel> la = pawn.apparel.WornApparel;
            //for(int i = 0; i < la.Count; i++)
            //{
            //    Log.Message(i.ToString() + " apparel: " + la[i].GetType().ToString());
            //}
            
            if (!typeof(Apparel_WarcasketGrimforge).IsAssignableFrom(pawn.apparel.WornApparel[0].GetType()))
            {
                return new FloatMenuOption("GFAA_NotWearingGFAAArmor".Translate(), null);
            }


            // All checks passed, this pawn may be forced to charge. Return null.
            return null;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn pawn)
        {
            base.GetFloatMenuOptions(pawn);
            FloatMenuOption failureReason = CheckIfNotAllowed(pawn);
            if (failureReason != null)
            {
                yield return failureReason;
            }
            else
            {
                yield return new FloatMenuOption("GFAA_ForceCharge".Translate(), delegate ()
                {
                    Log.Message("ping1");
                    Job job = new Job(GF_JobDefOf.GFAA_GetRecharge, new LocalTargetInfo(GetOpenRechargeSpot(pawn)), new LocalTargetInfo(this));
                    Log.Message("ping2");
                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    Log.Message("ping3");
                });
            }
        }

        // If multiple pawns are selected, correctly identify pawns that can be told to charge and allow those pawns to do so as a group.
        // GF code will need to do this eventually, but I'm focusing on basic functionality first.  
        public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
        {
            base.GetMultiSelectFloatMenuOptions(selPawns);
            List<Pawn> pawnsCanReach = new List<Pawn>();
            FloatMenuOption failureReason = null;

            // Generate a list of pawns that can use the station.
            foreach (Pawn pawn in selPawns)
            {
                failureReason = CheckIfNotAllowed(pawn);
                if (failureReason == null)
                {
                    pawnsCanReach.Add(pawn);
                }
            }

            // If there are no pawns that can reach, give a reason why. Note: It will only display the last failure reason detected.
            if (pawnsCanReach.NullOrEmpty())
            {
                if (failureReason != null)
                    yield return failureReason;
                else
                    yield break;
            }
            else
            {
                yield return new FloatMenuOption("GFAA_ForceCharge".Translate(), delegate ()
                {
                    // Attempt to assign all pawns that can reach to the station a spot. If a pawn takes the last slot, then abort the process. Left-over pawns won't charge.
                    foreach (Pawn pawn in pawnsCanReach)
                    {
                        IntVec3 chargingSpot = GetOpenRechargeSpot(pawn);

                        if (chargingSpot == IntVec3.Invalid)
                            break;

                        Job job = new Job(GF_JobDefOf.GFAA_GetRecharge, new LocalTargetInfo(chargingSpot), new LocalTargetInfo(this));
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                });
            }
        }


        // Return the first available spot on this station. Return IntVec3.Invalid if there is none.
        public virtual IntVec3 GetOpenRechargeSpot(Pawn pawn)
        {
            foreach (IntVec3 adjPos in adjacencies)
            {
                if (pawn.CanReach(new LocalTargetInfo(adjPos), PathEndMode.OnCell, Danger.Deadly) && (pawn.Position == adjPos || !pawn.Map.pawnDestinationReservationManager.IsReserved(adjPos)))
                    return adjPos;
            }
            return IntVec3.Invalid;
        }

    }
}
