using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class TestPassive : Ability_Passive
    {
        public TestPassive()
        {
            Name = "TestPassiveName";
            Active = true;
            Drain = 1f;
        }
    }
}
