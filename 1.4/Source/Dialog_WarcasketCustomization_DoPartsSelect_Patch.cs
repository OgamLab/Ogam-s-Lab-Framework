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
    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_WarcasketCustomization), "DoPartsSelect")]
    public static class Dialog_WarcasketCustomization_DoPartsSelect_Patch
    {
        public static Dictionary<WarcasketDef, bool> colors = new Dictionary<WarcasketDef, bool>();
        public static bool Prefix(Dialog_WarcasketCustomization __instance, Rect inRect, string label, 
            List<WarcasketDef> options, WarcasketDef current, Action<WarcasketDef> setCurrent, 
            ref Color currentColor, bool doResearchText = false)
        {
            Text.Font = GameFont.Small;
            //var compProperties = current.GetCompProperties<CompProperties_Decals>();
            bool colorOne = true;
            var data = __instance.project.GetAdditionalWarcasketProjectData();
            if (typeof(Apparel_WarcasketGrimforge).IsAssignableFrom(current.thingClass))
            {
                var checkboxOne = new Rect(inRect.x + 15, 655, 100, 24);
                var checkboxTwo = new Rect(checkboxOne.xMax + 15, checkboxOne.y, checkboxOne.width, checkboxOne.height);
                if (!colors.TryGetValue(current, out colorOne))
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
            }

            //if (compProperties != null)
            //{
            //
            //    var buttonRect = new Rect(inRect.x + 15, checkboxOne.yMax, inRect.width - 30, 32);
            //    if (__instance.helmets.Contains(current))
            //    {
            //        DoSelection(__instance, compProperties, buttonRect, ref data.helmetDecals, delegate (string x)
            //        {
            //            data.helmetDecals = x;
            //        });
            //    }
            //    if (__instance.armors.Contains(current))
            //    {
            //        DoSelection(__instance, compProperties, buttonRect, ref data.armorDecals, delegate (string x)
            //        {
            //            data.armorDecals = x;
            //        });
            //    }
            //    if (__instance.shoulders.Contains(current))
            //    {
            //        DoSelection(__instance, compProperties, buttonRect, ref data.shoulderDecals, delegate (string x)
            //        {
            //            data.shoulderDecals = x;
            //        });
            //    } 
            //}

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
                if (Widgets.ColorSelector(inRect.TakeTopPart(250f), ref currentColor, StaticStartup.colors, out _))
                {
                    __instance.Notify_SettingsChanged();
                }
            }
            else
            {
                if (__instance.helmets.Contains(current))
                {
                    if (Widgets.ColorSelector(inRect.TakeTopPart(250f), ref data.helmetColorTwo, StaticStartup.colors, out _))
                    {
                        __instance.Notify_SettingsChanged();
                    }
                }
                if (__instance.armors.Contains(current))
                {
                    if (Widgets.ColorSelector(inRect.TakeTopPart(250f), ref data.armorColorTwo, StaticStartup.colors, out _))
                    {
                        __instance.Notify_SettingsChanged();
                    }
                }
                if (__instance.shoulders.Contains(current))
                {
                    if (Widgets.ColorSelector(inRect.TakeTopPart(250f), ref data.shoulderColorTwo, StaticStartup.colors, out _))
                    {
                        __instance.Notify_SettingsChanged();
                    }
                }
            }
            
            return false;
        }

        //private static void DoSelection(Dialog_WarcasketCustomization __instance, CompProperties_Decals compProperties, 
        //    Rect buttonRect, ref string field, Action<string> action)
        //{
        //    if (field.NullOrEmpty())
        //    {
        //        field = compProperties.texPaths[0];
        //    }
        //    if (Widgets.ButtonText(buttonRect, field))
        //    {
        //        var list = new List<FloatMenuOption>();
        //        foreach (var option in compProperties.texPaths)
        //        {
        //            list.Add(new FloatMenuOption(option, delegate
        //            {
        //                action(option);
        //                __instance.Notify_SettingsChanged();
        //            }));
        //        }
        //        Find.WindowStack.Add(new FloatMenu(list));
        //    }
        //}
    }
}
