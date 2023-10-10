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
            var shoulder = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.shoulderPadsDef) as Apparel_WarcasketGrimforge_Pads;
            var armor = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.armorDef) as Apparel_WarcasketGrimforge_Body;
            var helmet = pawn.apparel.WornApparel.FirstOrDefault(x => x.def == __instance.helmetDef) as Apparel_WarcasketGrimforge_Helm;
            if (shoulder != null)
            {
                shoulder.colorApparelTwo = data.shoulderColorTwo;
                //if (!data.shoulderDecals.NullOrEmpty())
                //{
                //    var comp = shoulder.GetComp<CompDecals>();
                //    comp.SetGraphic(data.shoulderDecals);
                //}
            }
            if (armor != null)
            {
                armor.colorApparelTwo = data.armorColorTwo;
                //if (!data.armorDecals.NullOrEmpty())
                //{
                //    var comp = armor.GetComp<CompDecals>();
                //    comp.SetGraphic(data.armorDecals);
                //}
            }
            if (helmet != null)
            {
                helmet.colorApparelTwo = data.helmetColorTwo;
                //if (!data.helmetDecals.NullOrEmpty())
                //{
                //    var comp = helmet.GetComp<CompDecals>();
                //    comp.SetGraphic(data.helmetDecals);
                //}
            }
        }
    }
}
