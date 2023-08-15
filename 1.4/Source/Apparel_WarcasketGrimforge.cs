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

        public float MaxEnergy { get { return maxEnergy; } set { maxEnergy = value; } }
        public float Energy
        {
            get
            {
                if (energy > maxEnergy) { energy = maxEnergy; }
                return energy;
            }
            set
            {
                energy = value;
                if (energy < 0) { energy = 0; }
                else if (energy > maxEnergy) { energy = maxEnergy; }
            }
        }
        public override void Tick()
        {
            base.Tick();
            //energy -= drainTotal;
            float dTot = 0;
            for (int i = 0; i < abilities_Passives.Count; i++)
            {
                if (abilities_Passives[i].Active) { dTot += abilities_Passives[i].Drain; }
            }

            energy = dTot > energy ? 0 : energy - dTot;
        }
    }
}
