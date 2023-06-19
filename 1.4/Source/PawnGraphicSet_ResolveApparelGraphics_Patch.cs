using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Grimforge
{
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveApparelGraphics")]
    public static class PawnGraphicSet_ResolveApparelGraphics_Patch
    {
        public static HashSet<ApparelGraphicRecord> decalGraphics = new HashSet<ApparelGraphicRecord>();
        public static void Postfix(PawnGraphicSet __instance)
        {
            foreach (var item in __instance.pawn.apparel.WornApparel)
            {
                TryAddSecondaryGraphics(item, __instance);
            }
        }

        public static void TryAddSecondaryGraphics(Apparel apparel, PawnGraphicSet __instance)
        {
            var comp = apparel.GetComp<CompDecals>();
            if (comp != null)
            {
                if (comp.texPath is null)
                {
                    comp.SetGraphic(comp.Props.texPaths.RandomElement());
                }
                var record = new ApparelGraphicRecord
                {
                    graphic = comp.Graphic,
                    sourceApparel = apparel
                }; 
                decalGraphics.Add(record);
                __instance.apparelGraphics.Add(record);
            }
        }
    }
}
