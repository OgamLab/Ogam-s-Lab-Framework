using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using RimWorld;
using Verse.AI;

namespace Grimforge
{
    public class JobDriver_RechargeArmor : JobDriver
    {
        public Building_Bed Bed => job.GetTarget(TargetIndex.A).Thing as Building_Bed;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (Bed != null && !pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed))
            {
                return false;
            }
            pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
            return true;
        }

        [DebuggerHidden]
        public override IEnumerable<Toil> MakeNewToils()
        {
            if (TargetThingA is Building_Bed)
            {
                yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A);
                yield return Toils_Bed.GotoBed(TargetIndex.A);
                yield return Toils_LayDownForPower.LayDown(TargetIndex.A, true);
            }
            else
            {
                yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
                yield return Toils_LayDownForPower.LayDown(TargetIndex.B, false);
            }
        }
    }
}
