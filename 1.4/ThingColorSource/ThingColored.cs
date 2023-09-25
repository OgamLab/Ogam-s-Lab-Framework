using UnityEngine;
using Verse;

namespace ThingColor
{
    public class ThingColored : ThingWithComps
    {
        public override Color DrawColor
        {
            get
            {
                var extension = this.def.GetModExtension<ThingExtension>();
                var color = this.GetColorFor(extension.colorOneStuff);
                if (color != null)
                {
                    return color.Value;
                }
                return base.DrawColor;
            }
        }

        public override Color DrawColorTwo
        {
            get
            {
                var extension = this.def.GetModExtension<ThingExtension>();
                var color = this.GetColorFor(extension.colorTwoStuff);
                if (color != null)
                {
                    return color.Value;
                }
                return base.DrawColorTwo;
            }
        }
    }
}
