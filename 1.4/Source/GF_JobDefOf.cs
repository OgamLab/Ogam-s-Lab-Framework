using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;

namespace Grimforge
{
    [DefOf]
    public class GF_JobDefOf
    {
        static GF_JobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GF_JobDefOf));
        }

        //public static JobDef temp = new JobDef() { driverClass = typeof(JobDriver_RechargeArmor) };

        //public static JobDef GFAA_GetRecharge { get { return temp; } }
        public static JobDef GFAA_GetRecharge;



    }
}
