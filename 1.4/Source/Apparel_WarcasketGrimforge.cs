using UnityEngine;
using Verse;
using VFEPirates;

using System.Collections.Generic;

namespace Grimforge
{
    public class Apparel_WarcasketGrimforge : Apparel_Warcasket
    {
        public Color? colorApparelTwo;

        private float energy;

        private float maxEnergy = 100f;
        private List<Ability_Passive> abilities_Passives = new List<Ability_Passive>();
        private List<Ability_Active> abilities_Active = new List<Ability_Active>();

        public override Color DrawColor
        {
            set 
            { 
                colorApparel = value; 
            }
        }
        public override Color DrawColorTwo
        {
            get
            {
                return colorApparelTwo ??= this.def.colorGenerator.NewRandomizedColor();
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref colorApparelTwo, "colorApparelTwo");
        }
    }
}
