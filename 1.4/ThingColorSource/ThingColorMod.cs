using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ThingColor
{
    public class ThingExtension : DefModExtension
    {
        public List<StuffCategoryDef> colorOneStuff;
        public List<StuffCategoryDef> colorTwoStuff;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }

    [HotSwappable]
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "TryCreateRecolorJob")]
    public static class JobGiver_OptimizeApparel_TryCreateRecolorJob_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            foreach (var instruction in codeInstructions)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Stloc_2)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(
                        typeof(JobGiver_OptimizeApparel_TryCreateRecolorJob_Patch), "FillApparelToRecolor"));
                }
            }
        }

        public static void FillApparelToRecolor(Pawn pawn)
        {
            foreach (Apparel item in pawn.apparel.WornApparel)
            {
                if (item is ApparelColored colored && colored.DesiredColorTwo.HasValue)
                {
                    JobGiver_OptimizeApparel.tmpApparelToRecolor.Add(item);
                }
            }
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(CompColorable), "Recolor")]
    public static class CompColorable_Recolor_Patch
    {
        public static void Prefix(CompColorable __instance)
        {
            var apparel = __instance.parent as ApparelColored;
            if (apparel?.DesiredColorTwo != null)
            {
                apparel.ColorTwo = apparel.DesiredColorTwo.Value;
                apparel.DesiredColorTwo = null;
                if (!__instance.desiredColor.HasValue)
                {
                    __instance.desiredColor = __instance.Color;
                }
            }
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "AnyApparelNeedsRecoloring", MethodType.Getter)]
    public static class Pawn_ApparelTracker_AnyApparelNeedsRecoloring_Patch
    {
        public static void Postfix(Pawn_ApparelTracker __instance, ref bool __result)
        {
            foreach (Apparel item in __instance.WornApparel)
            {
                if (item is ApparelColored colored && colored.DesiredColorTwo.HasValue)
                {
                    __result = true;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Dialog_StylingStation),  MethodType.Constructor, new Type[] { typeof(Pawn), typeof(Thing) })]
    public static class Dialog_StylingStation_Constructor_Patch
    {
        public static void Postfix(Dialog_StylingStation __instance)
        {
            Reset(__instance);
            _ = __instance.AllColors;
            __instance.allColors.AddRange(new[]
            {
                new ColorInt(255, 202, 68).ToColor,
                new ColorInt(221, 161, 47).ToColor,
                new ColorInt(100, 100, 100).ToColor,
                new ColorInt(48, 48, 48).ToColor,
                new ColorInt(0, 0, 0).ToColor,
                new ColorInt(107, 122, 92).ToColor,
                new ColorInt(73, 103, 64).ToColor,
                new ColorInt(95, 102, 112).ToColor,
                new ColorInt(90, 97, 115).ToColor,
                new ColorInt(68, 126, 210).ToColor,
                new ColorInt(178, 64, 127).ToColor,
                new ColorInt(235, 244, 243).ToColor,
                new ColorInt(53, 79, 131).ToColor,
                new ColorInt(198, 51, 46).ToColor,
                new ColorInt(240, 210, 2).ToColor,
                new ColorInt(0, 118, 23).ToColor,
                new ColorInt(42, 141, 152).ToColor
            });
            __instance.allColors.SortByColor((Color x) => x);
        }

        public static void Reset(Dialog_StylingStation __instance)
        {
            Dialog_StylingStation_DrawApparelColor_Patch.apparelColorsTwo.Clear();
            foreach (Apparel item in __instance.pawn.apparel.WornApparel)
            {
                if (item is ApparelColored)
                {
                    Dialog_StylingStation_DrawApparelColor_Patch.apparelColorsTwo.Add(item, item.DrawColorTwo);
                }
            }
            Dialog_StylingStation_DrawApparelColor_Patch.colorTwoMode = false;
            Dialog_StylingStation_DrawPawn_Patch.tmpOriginalColors.Clear();
        }
    }
    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_StylingStation), "ApplyApparelColors")]
    public static class Dialog_StylingStation_ApplyApparelColors_Patch
    {
        public static void Postfix(Dialog_StylingStation __instance)
        {
            ApplyApparelColors(__instance);
        }

        private static void ApplyApparelColors(Dialog_StylingStation __instance)
        {
            foreach (KeyValuePair<Apparel, Color> apparelColor in Dialog_StylingStation_DrawApparelColor_Patch.apparelColorsTwo)
            {
                if (apparelColor.Key.DrawColorTwo != apparelColor.Value)
                {
                    (apparelColor.Key as ApparelColored).DesiredColorTwo = apparelColor.Value;
                }
            }
            Dialog_StylingStation_DrawPawn_Patch.tmpOriginalColors.Clear();
            __instance.pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            PortraitsCache.SetDirty(__instance.pawn);
            PortraitsCache.PortraitsCacheUpdate();
            GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(__instance.pawn);
            if (Find.World != null)
            {
                Find.ColonistBar.MarkColonistsDirty();
            }
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_StylingStation), "Reset")]
    public static class Dialog_StylingStation_Reset_Patch
    {
        public static void Prefix(Dialog_StylingStation __instance, bool resetColors = true)
        {
            Reset(__instance, resetColors);
        }

        private static void Reset(Dialog_StylingStation __instance, bool resetColors = true)
        {
            if (resetColors)
            {
                Dialog_StylingStation_Constructor_Patch.Reset(__instance);
            }
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_StylingStation), "DrawPawn")]
    public static class Dialog_StylingStation_DrawPawn_Patch
    {
        public static Dictionary<Apparel, Color?> tmpOriginalColors = new Dictionary<Apparel, Color?>();
        public static bool Prefix(Dialog_StylingStation __instance, Rect rect)
        {
            DrawPawn(__instance, rect);
            return false;
        }

        private static void DrawPawn(Dialog_StylingStation __instance, Rect rect)
        {
            Rect rect2 = rect;
            rect2.yMin = rect.yMax - Text.LineHeight * 2f;
            Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f), "ShowHeadgear".Translate(), 
                ref __instance.showHeadgear);
            Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y + rect2.height / 2f, rect2.width, rect2.height / 2f), 
                "ShowApparel".Translate(), ref __instance.showClothes);
            rect.yMax = rect2.yMin - 4f;
            Widgets.BeginGroup(rect);
            tmpOriginalColors.Clear();
            foreach (KeyValuePair<Apparel, Color> item in Dialog_StylingStation_DrawApparelColor_Patch.apparelColorsTwo)
            {
                ApparelColored key = item.Key as ApparelColored;
                if (key != null)
                {
                    tmpOriginalColors.Add(key, key.ColorTwo);
                    key.ColorTwo = item.Value;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                Rect position = new Rect(0f, rect.height / 3f * (float)i, rect.width, rect.height / 3f).ContractedBy(4f);
                RenderTexture image = PortraitsCache.Get(__instance.pawn, new Vector2(position.width, position.height), 
                    new Rot4(2 - i), Dialog_StylingStation.PortraitOffset, 1.1f, supersample: true, compensateForUIScale: true, 
                    __instance.showHeadgear, __instance.showClothes, __instance.apparelColors,
                    __instance.desiredHairColor, stylingStation: true);
                GUI.DrawTexture(position, image);
            }

            foreach (var tmpOriginalColor in tmpOriginalColors)
            {
                (tmpOriginalColor.Key as ApparelColored).ColorTwo = tmpOriginalColor.Value;
            }
            Widgets.EndGroup();
            tmpOriginalColors.Clear();
            if (__instance.pawn.style.HasAnyUnwantedStyleItem)
            {
                string text = "PawnUnhappyWithStyleItems".Translate(__instance.pawn.Named("PAWN")) + ": ";
                Dialog_StylingStation.tmpUnwantedStyleNames.Clear();
                if (__instance.pawn.style.HasUnwantedHairStyle)
                {
                    Dialog_StylingStation.tmpUnwantedStyleNames.Add("Hair".Translate());
                }
                if (__instance.pawn.style.HasUnwantedBeard)
                {
                    Dialog_StylingStation.tmpUnwantedStyleNames.Add("Beard".Translate());
                }
                if (__instance.pawn.style.HasUnwantedFaceTattoo)
                {
                    Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooFace".Translate());
                }
                if (__instance.pawn.style.HasUnwantedBodyTattoo)
                {
                    Dialog_StylingStation.tmpUnwantedStyleNames.Add("TattooBody".Translate());
                }
                GUI.color = ColorLibrary.RedReadable;
                Widgets.Label(new Rect(rect.x, rect.yMin - 30f, rect.width, Text.LineHeight * 2f + 10f), "Warning".Translate()
                    + ": " + text + Dialog_StylingStation.tmpUnwantedStyleNames.ToCommaList().CapitalizeFirst());
                GUI.color = Color.white;
            }
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_StylingStation), "DrawApparelColor")]
    public static class Dialog_StylingStation_DrawApparelColor_Patch
    {
        public static Dictionary<Apparel, Color> apparelColorsTwo = new Dictionary<Apparel, Color>();
        public static bool colorTwoMode;

        public static bool Prefix(Dialog_StylingStation __instance, Rect rect)
        {
            DrawApparelColor(__instance, rect, colorTwoMode ? apparelColorsTwo : __instance.apparelColors);
            return false;
        }

        private static void DrawApparelColor(Dialog_StylingStation __instance, Rect rect, Dictionary<Apparel, Color> apparelColors)
        {
            bool flag = false;
            Rect viewRect = new Rect(rect.x, rect.y, rect.width - 16f, __instance.viewRectHeight);
            Widgets.BeginScrollView(rect, ref __instance.apparelColorScrollPosition, viewRect);
            int num = 0;
            float curY = rect.y;
            foreach (Apparel item in __instance.pawn.apparel.WornApparel)
            {
                Rect rect2 = new Rect(rect.x, curY, viewRect.width, 110);
                if (apparelColors.TryGetValue(item, out var color) is false)
                {
                    apparelColors[item] = color = Color.white;
                }
                curY += rect2.height + 10f;
                if (!__instance.pawn.apparel.IsLocked(item))
                {
                    flag |= Widgets.ColorSelector(rect2, ref color, __instance.AllColors, out var _, item.def.uiIcon);
                    float num2 = rect2.x;
                    float buttonWidth = 175;
                    if (__instance.pawn.Ideo != null && !Find.IdeoManager.classicMode)
                    {
                        rect2 = new Rect(num2, curY, buttonWidth, 24f);
                        if (Widgets.ButtonText(rect2, "SetIdeoColor".Translate()))
                        {
                            flag = true;
                            color = __instance.pawn.Ideo.ApparelColor;
                            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                        }
                        num2 += buttonWidth + 10f;
                    }
                    if (__instance.pawn.story?.favoriteColor.HasValue ?? false)
                    {
                        rect2 = new Rect(num2, curY, buttonWidth, 24f);
                        if (Widgets.ButtonText(rect2, "SetFavoriteColor".Translate()))
                        {
                            flag = true;
                            color = __instance.pawn.story.favoriteColor.Value;
                            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                        }
                        num2 += buttonWidth + 10f;
                    }
                    if (item is ApparelColored)
                    {
                        rect2 = new Rect(num2, curY, 100, 24f);
                        if (Widgets.RadioButtonLabeled(rect2, "Color one", colorTwoMode is false))
                        {
                            colorTwoMode = false;
                        }
                        num2 += rect2.width + 10;
                        rect2 = new Rect(num2, curY, 100, 24f);
                        if (Widgets.RadioButtonLabeled(rect2, "Color two", colorTwoMode))
                        {
                            colorTwoMode = true;
                        }
                    }

                    if (!color.IndistinguishableFrom(item.DrawColor))
                    {
                        num++;
                    }
                    apparelColors[item] = color;
                }
                else
                {
                    Widgets.ColorSelectorIcon(new Rect(rect2.x, rect2.y, 88f, 88f), item.def.uiIcon, color);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Rect rect3 = rect2;
                    rect3.x += 100f;
                    Widgets.Label(rect3, ((string)"ApparelLockedCannotRecolor".Translate(__instance.pawn.Named("PAWN"), 
                        item.Named("APPAREL"))).Colorize(ColorLibrary.RedReadable));
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                curY += 34f;
            }
            if (num > 0)
            {
                __instance.DrawDyeRequirement(rect, ref curY, num);
            }
            if (__instance.pawn.Map.resourceCounter.GetCount(ThingDefOf.Dye) < num)
            {
                Rect rect4 = new Rect(rect.x, curY, rect.width - 16f - 10f, 60f);
                Color color2 = GUI.color;
                GUI.color = ColorLibrary.RedReadable;
                Widgets.Label(rect4, "NotEnoughDye".Translate() + " " + "NotEnoughDyeWillRecolorApparel".Translate());
                GUI.color = color2;
                curY += rect4.height;
            }
            if (Event.current.type == EventType.Layout)
            {
                __instance.viewRectHeight = curY - rect.y;
            }
            Widgets.EndScrollView();
        }
    }
    [StaticConstructorOnStartup]
    public static class ThingColorMod
    {
        static ThingColorMod()
        {
            new Harmony("ThingColorMod").PatchAll();
        }

        public static Color? GetColorFor(this Thing thing, List<StuffCategoryDef> colorStuff)
        {
            if (colorStuff is null) return null;
            var stuff = thing.Stuff;
            if (stuff.stuffProps?.categories != null)
            {
                foreach (var cat in stuff.stuffProps.categories)
                {
                    if (colorStuff.Contains(cat))
                    {
                        return stuff.stuffProps.color;
                    }
                }
            }

            if (thing.def.CostList != null)
            {
                foreach (var cost in thing.def.CostList)
                {
                    if (cost.thingDef.stuffProps?.categories != null)
                    {
                        foreach (var cat in cost.thingDef.stuffProps.categories)
                        {
                            if (colorStuff.Contains(cat))
                            {
                                return cost.thingDef.stuffProps.color;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }

    [HarmonyPatch(typeof(ApparelGraphicRecordGetter), "TryGetGraphicApparel")]
    public static class ApparelGraphicRecordGetter_TryGetGraphicApparel_Patch
    {
        public static bool Prefix(ref bool __result, Apparel apparel, BodyTypeDef bodyType, ref ApparelGraphicRecord rec)
        {
            if (apparel.DrawColorTwo != Color.white)
            {
                __result = TryGetGraphicApparel(apparel, bodyType, ref rec);
                return false;
            }
            return true;
        }

        public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, ref ApparelGraphicRecord rec)
        {
            if (bodyType == null)
            {
                Log.Error("Getting apparel graphic with undefined body type.");
                bodyType = BodyTypeDefOf.Male;
            }
            if (apparel.WornGraphicPath.NullOrEmpty())
            {
                rec = new ApparelGraphicRecord(null, null);
                return false;
            }
            string path = ((apparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead && apparel.def.apparel.LastLayer != ApparelLayerDefOf.EyeCover && !PawnRenderer.RenderAsPack(apparel) && !(apparel.WornGraphicPath == BaseContent.PlaceholderImagePath) && !(apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath)) ? (apparel.WornGraphicPath + "_" + bodyType.defName) : apparel.WornGraphicPath);
            Shader shader = ShaderDatabase.Cutout;
            if (apparel is ApparelColored)
            {
                if (apparel.def.apparel.useWornGraphicMask is false)
                {
                    apparel.def.apparel.useWornGraphicMask = true;
                }
            }
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
