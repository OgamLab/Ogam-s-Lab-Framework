using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Grimforge
{
    public class CompProperties_Decals : CompProperties
    {
        public List<string> texPaths;
        public CompProperties_Decals()
        {
            this.compClass = typeof(CompDecals);
        }
    }
    public class CompDecals : ThingComp
    {
        public CompProperties_Decals Props => base.props as CompProperties_Decals;
        public string texPath;

        private Graphic graphic;
        public Graphic Graphic
        {
            get
            {
                if (graphic == null)
                {
                    graphic = GraphicDatabase.Get(typeof(Graphic_Multi), texPath, this.parent.Graphic.Shader, this.parent.Graphic.drawSize, Color.white, Color.white);
                }
                return graphic;
            }
        }

        public void SetGraphic(string texPath)
        {
            this.texPath = texPath;
            this.graphic = null;
            (this.parent.ParentHolder as Pawn_ApparelTracker).Notify_ApparelChanged();
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref texPath, "texPath");
        }
    }
}
