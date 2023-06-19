using HarmonyLib;
using System.Collections.Generic;
using Verse;
using VFEPirates;

namespace Grimforge
{
    [HarmonyPatch(typeof(WarcasketProject), "ExposeData")]
    public static class WarcasketProject_ExposeData_Patch
    {
        public static void Postfix(WarcasketProject __instance)
        {
            var data = __instance.GetAdditionalWarcasketProjectData();
            Scribe_Deep.Look(ref data, "warcasketProjectAdditionalData");
            pawnWarcasketProjectAdditionalData[__instance] = data;
        }

        public static Dictionary<WarcasketProject, WarcasketProjectAdditionalParts> pawnWarcasketProjectAdditionalData = new();
        public static WarcasketProjectAdditionalParts GetAdditionalWarcasketProjectData(this WarcasketProject project)
        {
            if (!pawnWarcasketProjectAdditionalData.TryGetValue(project, out var data))
            {
                pawnWarcasketProjectAdditionalData[project] = data = new WarcasketProjectAdditionalParts();
            }
            return data;
        }
    }
}
