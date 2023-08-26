using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Grimforge
{
    //public enum Ability_Enum
    //{
    //    Test
    //}
    public abstract class Ability_Passive
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public float Drain { get; set; }

        public string Label { get; set; }
        public string Description { get; set; }
        //public Def
        //public Texture2D Texture { get; set; }
        public Action toggle_Action { get; set; }

        public void Flip()
        {
            Active = !Active;
        }
        public void Set(bool state)
        {
            Active = state;
        }

    }

    public class TestPassive : Ability_Passive
    {
        public TestPassive()
        {
            Name = "TestPassiveName";
            Active = false;
            Drain = 0.1f;
        }
    }
}
