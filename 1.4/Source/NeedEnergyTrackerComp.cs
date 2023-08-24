using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using UnityEngine;
using Verse;

namespace Grimforge
{
    /// <summary>
    /// Tracks stored energy for use in case of death.  Also uses a lot from RW-Androids1.4
    /// </summary>
    public class NeedEnergyTrackerComp : ThingComp
    {
        public float energy;

        Pawn pawn;

        Need_Energy energyNeed;

        public CompProperties_NeedEnergyTracker EnergyProperties
        {
            get
            {
                return props as CompProperties_NeedEnergyTracker;
            }
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            pawn = parent as Pawn;

            if (pawn != null)
            {
                energyNeed = pawn.needs.TryGetNeed<Need_Energy>();
            }
        }

        public override void CompTick()
        {
            //Original:
            //if (energyNeed != null)
            //    energy = energyNeed.CurLevel;

            if(energyNeed != null)
            {
                //TODO
                //energy = pawn.
            }

        }

        public override void PostExposeData()
        {
            //base.PostExposeData();
            Scribe_Values.Look(ref energy, "energy");
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            //Test buttons
            if (Prefs.DevMode)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEBUG: Set Energy to 100%",
                    action = () => energyNeed.CurLevelPercentage = 1.0f
                };
                yield return new Command_Action
                {
                    defaultLabel = "DEBUG: Set Energy to 0%",
                    action = () => energyNeed.CurLevelPercentage = 0
                };
            }
        }
    }
}
