using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using VFEPirates;
using VFEPirates.Buildings;

namespace Grimforge
{
    [HotSwappable]
    [HarmonyPatch(typeof(Dialog_WarcasketCustomization), "DoPartsSelect")]
    public static class Dialog_WarcasketCustomization_DoPartsSelect_Patch
    {
        public static void Postfix(Dialog_WarcasketCustomization __instance, Rect inRect, WarcasketDef current)
        {
            var compProperties = current.GetCompProperties<CompProperties_Decals>();
            if (compProperties != null)
            {
                var buttonRect = new Rect(inRect.x + 15, 640, inRect.width - 30, 32);
                var data = __instance.project.GetAdditionalWarcasketProjectData();
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
            }
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
