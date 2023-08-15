using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grimforge
{
    public abstract class Ability_Active
    {
        public string Name { get; set; }
        public float Chunk { get; set; }
    }

    public class TestActive : Ability_Active
    {
        public TestActive()
        {
            Name = "TestName";
            Chunk = 10f;
        }
    }
}
