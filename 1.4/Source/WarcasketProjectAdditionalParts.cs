using Verse;

namespace Grimforge
{
    public class WarcasketProjectAdditionalParts : IExposable
    {
        public string armorDecals;
        public string helmetDecals;
        public string shoulderDecals;
        public void ExposeData()
        {
            Scribe_Values.Look(ref armorDecals, "armorDecals");
            Scribe_Values.Look(ref helmetDecals, "helmetDecals");
            Scribe_Values.Look(ref shoulderDecals, "shoulderDecals");
        }
    }
}
