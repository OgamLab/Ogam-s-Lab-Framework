using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Grimforge
{
    [HarmonyPatch(typeof(ApparelGraphicRecordGetter), "TryGetGraphicApparel")]
    public static class ApparelGraphicRecordGetter_TryGetGraphicApparel_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ref bool __result, Apparel apparel, BodyTypeDef bodyType, ref ApparelGraphicRecord rec)
        {
            if (apparel is Apparel_WarcasketGrimforge)
            {
                __result = TryGetGraphicApparel(apparel, ref rec);
                return false;
            }
            return true;
        }

        public static bool TryGetGraphicApparel(Apparel apparel, ref ApparelGraphicRecord rec)
        {
            if (apparel.WornGraphicPath.NullOrEmpty())
            {
                rec = new ApparelGraphicRecord(null, null);
                return false;
            }
            string path = apparel.WornGraphicPath;
            Shader shader = ShaderDatabase.Cutout;
            if (apparel.def.apparel.useWornGraphicMask)
            {
                shader = ShaderDatabase.CutoutComplex;
            }
            Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader, apparel.def.graphicData.drawSize, apparel.DrawColor, apparel.DrawColorTwo);
            rec = new ApparelGraphicRecord(graphic, apparel);
            return true;
        }
    }
}
