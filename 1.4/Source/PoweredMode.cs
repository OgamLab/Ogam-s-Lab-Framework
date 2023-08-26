using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

using RimWorld;

namespace Grimforge
{
    /// <summary>
    /// For example for all caskets more carry capacity, moving speed, self healing factor.
    /// For specialised caskets like Techmarine add bonus to production quality, work speed, research speed
    /// And all of this shoud be as simple and low ticks as possible
    /// </summary>
    public class PoweredMode : Ability_Passive
    {
        //Consider combining?
        public float carryCapacityBonus; // = 100f;
        public float moveSpeedBonus;// = 0.5f;
        public float selfHealFactor;

        public PoweredMode()
        {
            Name = "PoweredMode".Translate();
            Label = "PoweredMode".Translate();
            Description = "PoweredModeDesc".Translate();
            //Drain
        }
    }
}
