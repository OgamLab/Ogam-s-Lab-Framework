using UnityEngine;
using Verse;
using VFEPirates;

namespace Grimforge
{
    public class Apparel_WarcasketGrimforge : Apparel_Warcasket
    {
        public Color? colorApparelTwo;

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
