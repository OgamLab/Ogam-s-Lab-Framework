using HarmonyLib;
using Verse;
using VFEPirates;

namespace Grimforge
{
    [HarmonyPatch(typeof(WarcasketProject), "ApplyOn")]
    public static class WarcasketProject_ApplyOn_Patch
    {
        public static void Postfix(WarcasketProject __instance, Pawn pawn)
        {
            var data = __instance.GetAdditionalWarcasketProjectData();
            if (!data.shoulderDecals.NullOrEmpty())
            {
                var shoulder = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.shoulderPadsDef);
                var comp = shoulder.GetComp<CompDecals>();
                comp.SetGraphic(data.shoulderDecals);
            }
            if (!data.armorDecals.NullOrEmpty())
            {
                var armor = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.armorDef);
                var comp = armor.GetComp<CompDecals>();
                comp.SetGraphic(data.armorDecals);
            }
            if (!data.helmetDecals.NullOrEmpty())
            {
                var helmet = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.helmetDef);
                var comp = helmet.GetComp<CompDecals>();
                comp.SetGraphic(data.helmetDecals);
            }
        }
    }
}
