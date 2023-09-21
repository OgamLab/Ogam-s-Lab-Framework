using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;

namespace Grimforge
{
    public class GF_JobDefOf
    {
        static GF_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GF_JobDefOf));
        }

        public static JobDef GF_GetRecharge;



    }
}
