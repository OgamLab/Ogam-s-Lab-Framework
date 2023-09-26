using RimWorld;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace ThingColor
{
    public class ApparelColored : Apparel
    {
        private Color? colorTwo;
        public Color? ColorTwo
        {
            get => colorTwo; 
            set => colorTwo = value;
        }

        private Color? desiredColorTwo;
        public Color? DesiredColorTwo
        {
            get => desiredColorTwo;
            set => desiredColorTwo = value; 
        }

        public override Color DrawColor
        {
            get
            {
                if (!def.apparel.useWornGraphicMask)
                {
                    def.apparel.useWornGraphicMask = true;
                }
                CompColorable comp = GetComp<CompColorable>();
                if (comp != null && comp.Active)
                {
                    return comp.Color;
                }
                var extension = def.GetModExtension<ThingExtension>();
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
                if (!def.apparel.useWornGraphicMask)
                {
                    def.apparel.useWornGraphicMask = true;
                }
                if (colorTwo.HasValue)
                {
                    return colorTwo.Value;
                }
                var extension = def.GetModExtension<ThingExtension>();
                var color = this.GetColorFor(extension.colorTwoStuff);
                if (color != null)
                {
                    return color.Value;
                }

                return base.DrawColorTwo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref colorTwo, "colorTwo");
            Scribe_Values.Look(ref desiredColorTwo, "desiredColorTwo");
        }
    }
}
