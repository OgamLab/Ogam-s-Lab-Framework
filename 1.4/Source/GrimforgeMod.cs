using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

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
}
