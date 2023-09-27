using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ThingColor
{
    public class ModExt_PawnKindApparelColorTwo : DefModExtension
    {
        public Color colorTwo = Color.white;
    }


    [HarmonyPatch(typeof(PawnApparelGenerator), "PostProcessApparel")]
    public static class PawnApparelGenerator_PostProcessApparel_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(ref Apparel apparel, ref Pawn pawn)
        {
            if (apparel is ApparelColored coloredApparel)
            {
                Log.Message("Got colored apparel");
                var ext = pawn.kindDef.GetModExtension<ModExt_PawnKindApparelColorTwo>();
                if (ext != null)
                {
                    Log.Message("recolored " + apparel.def.defName);
                    coloredApparel.ColorTwo = ext.colorTwo;
                    coloredApparel.DesiredColorTwo = null;
                    Log.Message("and the color is... " + apparel.DrawColorTwo);
                }
            }
        }
    }
}
