using UnityEngine;
using Verse;
using VFEPirates;
using RimWorld.IO;

using System.Collections.Generic;
using System.Linq;

namespace Grimforge
{
    public class Apparel_WarcasketGrimforge : Apparel_Warcasket
    {
        public Color? colorApparelTwo;

        public List<Ability_Passive> abilities_Passives = new List<Ability_Passive>();
        public List<Ability_Active> abilities_Active = new List<Ability_Active>();

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

        public bool IsActive(string name)
        {
            if(abilities_Passives.Where(x=>x.Name == name && x.Active == true).Count() > 0)
            {
                return true;
            }
            return false;
        }

        public void SwitchPassive(string name)
        {
            List<Ability_Passive> res = abilities_Passives.Where(x => x.Name == name).ToList();
            if(res.Count() > 0)
            {
                res[0].Active = !res[0].Active;
            }
        }
    }
}
