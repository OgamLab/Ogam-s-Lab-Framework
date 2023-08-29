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
    /// <summary>
    /// "Void shield"   Simple shield with massive HP pool but with slow regeneration (entire shield need 1/2 to 1 hours to recharge)
    /// "Invisibility"
    /// "Healing specialist"     Aura which speedup tending wounds even tending wounds itself
    ///+50% speed/quality tend
    ///+50% success surgery rate
    ///"Psyker" 
    ///He can cast spells from mamy magic schools
    ///100% Meditation psyfocus gain 
    ///+ 50 % Neural heat recovery rate
    ///+ 25 % Psychic Sensitivity
    ///"Techmarine"
    ///+ 5 to craft
    ///+ 50 % to craft/smith/trailor speed
    ///1+ to craft quality
    ///Chaplain
    ///+5 to social
    ///+100% suppression
    ///+75% to social impact


    /// </summary>
    public abstract class Ability_Passive
    {
        //public Pawn pawn;
        public Apparel_WarcasketGrimforge_Body attachedCasketBody;
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

        //public Ability_Passive(Pawn wearer)
        //{
        //    pawn = wearer;
        //}
        public Ability_Passive(Apparel_WarcasketGrimforge_Body _attachedCasketBody)
        {
            attachedCasketBody = _attachedCasketBody;
        }

    }

    public class TestPassive : Ability_Passive
    {
        //public TestPassive(Pawn wearer) : base(wearer)
        public TestPassive(Apparel_WarcasketGrimforge_Body _attachedCasketBody) : base(_attachedCasketBody)
        {
            Name = "TestPassiveName";
            Active = false;
            Drain = 0.1f;
            
            //pawn.GetStatValue()
            //StatDef.
        }
    }
}
