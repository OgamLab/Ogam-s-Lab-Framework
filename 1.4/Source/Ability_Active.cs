using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;

namespace Grimforge
{
    /// <summary>
    /// "Frag Grenade" Simple damage ability with small/medium size AOE
    /// Krak Grenade Simple anti armor grenade, single target
    /// "Fire on my mark" Aura which gives less aim time and higher accuracy
    /// "Charge!"     Aura which gives melee attack cooldown reduction, dodge chance, bonus movement speed, automatic switch weapons to melee (if possible) (if its possible to code)
    /// "Grappling hook"     Dash which need to be "connect" to wall
    /// </summary>
    public abstract class Ability_Active
    {
        public Pawn pawn;
        public string Name { get; set; }
        public float Cost { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public Action Action_ { get; set; }

        public Ability_Active(Pawn wearer)
        {
            pawn = wearer;
        }
    }

    public class TestActive : Ability_Active
    {
        public TestActive(Pawn wearer) : base(wearer)
        {
            Name = "TestName";

            Cost = 10f;

            
        }
    }
    
}
