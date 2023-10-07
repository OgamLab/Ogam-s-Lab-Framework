using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using VFEPirates.Buildings;

namespace Grimforge
{
    [HarmonyPatch(typeof(Dialog_WarcasketCustomization), "DoWindowContents")]
    public static class Dialog_WarcasketCustomization_DoWindowContents_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            foreach (var code in codeInstructions)
            {
                if (code.OperandIs(650f))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 720f);
                }
                else
                {
                    yield return code;
                }
            }
        }
    }
}
