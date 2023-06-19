using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Grimforge
{
    [HarmonyPatch(typeof(PawnRenderer), "DrawBodyApparel")]
    public static class PawnRenderer_DrawBodyApparel_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var drawMeshNowOrLaterInfo = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawMeshNowOrLater), new Type[]
            {
                typeof(Mesh), typeof(Vector3), typeof(Quaternion), typeof(Material), typeof(bool)
            });
            var codes = codeInstructions.ToList();
            for (var i =  0; i < codes.Count; i++)
            {
                var codeInstruction = codes[i];
                if (codeInstruction.opcode == OpCodes.Ldloc_S && codeInstruction.operand is LocalBuilder lb && lb.LocalIndex == 5)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_3);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 5);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PawnRenderer_DrawBodyApparel_Patch), "ModifyDrawLoc"));
                }
                yield return codeInstruction;
            }
        }

        public static void ModifyDrawLoc(ApparelGraphicRecord record, ref Vector3 loc)
        {
            if (PawnGraphicSet_ResolveApparelGraphics_Patch.decalGraphics.Contains(record))
            {
                loc.y += 0.1f;
            }
        }
    }
}
