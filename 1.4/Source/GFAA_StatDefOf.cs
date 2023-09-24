using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;

namespace Grimforge
{
    [DefOf]
    public static class GFAA_StatDefOf
    {
        static GFAA_StatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GFAA_StatDefOf));
        }

        public static StatDef GFAA_ChargingSpeed;
    }
}
