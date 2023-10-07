using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using VFEPirates;

namespace Grimforge
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }
    public class GrimforgeMod : Mod
    {
        public GrimforgeMod(ModContentPack pack) : base(pack)
        {
            new Harmony("GrimforgeMod").PatchAll();
        }
    }

    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            StaticStartup.colors.AddRange(new[]
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
            StaticStartup.colors.SortByColor((Color x) => x);
        }
    }
}
