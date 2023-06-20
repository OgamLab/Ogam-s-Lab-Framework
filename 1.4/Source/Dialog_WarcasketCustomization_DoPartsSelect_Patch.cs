using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using VFECore.UItils;
using VFEPirates;
using VFEPirates.Buildings;

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

    public class Apparel_WarcasketGrimforge : Apparel_Warcasket
    {
        public Color? colorApparelTwo;
        public override Color DrawColorTwo => colorApparelTwo ??= this.def.colorGenerator.NewRandomizedColor();
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref colorApparelTwo, "colorApparelTwo");
        }
    }

    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_WarcasketCustomization), "DoPartsSelect")]
    public static class Dialog_WarcasketCustomization_DoPartsSelect_Patch
    {
        public static Dictionary<WarcasketDef, bool> colors = new Dictionary<WarcasketDef, bool>();
        public static bool Prefix(Dialog_WarcasketCustomization __instance, Rect inRect, string label, 
            List<WarcasketDef> options, WarcasketDef current, Action<WarcasketDef> setCurrent, 
            ref Color currentColor,
            bool doResearchText = false)
        {
            Text.Font = GameFont.Small;
            var compProperties = current.GetCompProperties<CompProperties_Decals>();
            if (compProperties != null)
            {
                var data = __instance.project.GetAdditionalWarcasketProjectData();
                var checkboxOne = new Rect(inRect.x + 15, 615, 100, 24);
                var checkboxTwo = new Rect(checkboxOne.xMax + 15, checkboxOne.y, checkboxOne.width, checkboxOne.height);
                if (!colors.TryGetValue(current, out var colorOne))
                {
                    colors[current] = colorOne = true;
                }

                if (Widgets.RadioButtonLabeled(checkboxOne, "Grimforge.First".Translate(), colorOne == true))
                {
                    colors[current] = colorOne = true;
                }
                else if (Widgets.RadioButtonLabeled(checkboxTwo, "Grimforge.Second".Translate(), colorOne == false))
                {
                    colors[current] = colorOne = false;
                }

                var buttonRect = new Rect(inRect.x + 15, 645, inRect.width - 30, 32);
                if (__instance.helmets.Contains(current))
                {
                    DoSelection(__instance, compProperties, buttonRect, ref data.helmetDecals, delegate(string x)
                    {
                        data.helmetDecals = x;
                    });
                }
                if (__instance.armors.Contains(current))
                {
                    DoSelection(__instance, compProperties, buttonRect, ref data.armorDecals, delegate (string x)
                    {
                        data.armorDecals = x;
                    });
                }
                if (__instance.shoulders.Contains(current))
                {
                    DoSelection(__instance, compProperties, buttonRect, ref data.shoulderDecals, delegate (string x)
                    {
                        data.shoulderDecals = x;
                    });
                }

                inRect = inRect.ContractedBy(3f);
                Text.Font = GameFont.Small;
                Widgets.Label(inRect.TakeTopPart(20f), label);
                inRect.y += 2f;
                __instance.DoSelection(inRect.TakeTopPart(24f), options, current, setCurrent);
                if (doResearchText)
                {
                    Text.Font = GameFont.Tiny;
                    Text.Anchor = TextAnchor.MiddleLeft;
                    var researchTextRect = inRect.TakeTopPart(25f);
                    researchTextRect.width *= 3;
                    Widgets.Label(researchTextRect, "VFEP.ResearchText".Translate().Colorize(ColoredText.SubtleGrayColor));
                    Text.Font = GameFont.Small;
                }
                else
                    inRect.y += 25f;

                var infoRect = inRect.TakeTopPart(30f);
                Widgets.InfoCardButton(infoRect.x, infoRect.y, current);
                infoRect.x += 30f;
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(infoRect, current.LabelCap);
                Text.Anchor = TextAnchor.UpperLeft;
                inRect.y += 5f;
                Widgets.Label(inRect.TakeTopPart(100f), current.shortDescription);
                inRect.y += 5f;
                Widgets.Label(inRect.TakeTopPart(40f), "VFEP.ResourceCost".Translate() + " " + current.costList.Join(cost => cost.LabelCap));
                inRect.y += 5f;

                if (colorOne)
                {
                    if (Widgets.ColorSelector(inRect.TakeTopPart(150f), ref currentColor, StaticStartup.colors, out _))
                    {
                        __instance.Notify_SettingsChanged();
                    }
                }
                else
                {
                    if (__instance.helmets.Contains(current))
                    {
                        if (Widgets.ColorSelector(inRect.TakeTopPart(150f), ref data.helmetColorTwo, StaticStartup.colors, out _))
                        {
                            __instance.Notify_SettingsChanged();
                        }
                    }
                    if (__instance.armors.Contains(current))
                    {
                        if (Widgets.ColorSelector(inRect.TakeTopPart(150f), ref data.armorColorTwo, StaticStartup.colors, out _))
                        {
                            __instance.Notify_SettingsChanged();
                        }
                    }
                    if (__instance.shoulders.Contains(current))
                    {
                        if (Widgets.ColorSelector(inRect.TakeTopPart(150f), ref data.shoulderColorTwo, StaticStartup.colors, out _))
                        {
                            __instance.Notify_SettingsChanged();
                        }
                    }
                }
                return false;
            }
            return true;
        }

        private static void DoSelection(Dialog_WarcasketCustomization __instance, CompProperties_Decals compProperties, 
            Rect buttonRect, ref string field, Action<string> action)
        {
            if (field.NullOrEmpty())
            {
                field = compProperties.texPaths[0];
            }
            if (Widgets.ButtonText(buttonRect, field))
            {
                var list = new List<FloatMenuOption>();
                foreach (var option in compProperties.texPaths)
                {
                    list.Add(new FloatMenuOption(option, delegate
                    {
                        action(option);
                        __instance.Notify_SettingsChanged();
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
        }
    }
}
