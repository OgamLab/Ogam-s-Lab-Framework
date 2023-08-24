using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void PostPawn
    }
}
