using UnityEngine;
using Verse;

namespace Grimforge
{
    public class WarcasketProjectAdditionalParts : IExposable
    {
        public string armorDecals;
        public string helmetDecals;
        public string shoulderDecals;

        public Color armorColorTwo;
        public Color helmetColorTwo;
        public Color shoulderColorTwo;
        public void ExposeData()
        {
            Scribe_Values.Look(ref armorDecals, "armorDecals");
            Scribe_Values.Look(ref helmetDecals, "helmetDecals");
            Scribe_Values.Look(ref shoulderDecals, "shoulderDecals");

            Scribe_Values.Look(ref armorColorTwo, "armorColorTwo");
            Scribe_Values.Look(ref helmetColorTwo, "helmetColorTwo");
            Scribe_Values.Look(ref shoulderColorTwo, "shoulderColorTwo");
        }
    }
}
