using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;


///Borrowing HEAVILY from Nanosuit
namespace Grimforge
{
    [StaticConstructorOnStartup]
    public class Gizmo_ArmorEnergyStatus : Gizmo
    {
        private static readonly Texture2D FullEnergyBarTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        private static readonly Texture2D EmptyEnergyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public Apparel_WarcasketGrimforge casket;

        public Gizmo_ArmorEnergyStatus()
        {
            order = -100f;
        }

        public override float GetWidth(float maxWidth)
        {
            //throw new NotImplementedException();
            //NOTE: Not desirable, won't scale to user's screen in odd circumstances, possibly such as too many psycasts.
            return 140f;
        }


        //Getting an "Object reference not set to an instance of an object" on this function.  This disables 
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            //throw new NotImplementedException();
            var rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            var rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);

            var rect3 = rect2;
            rect3.height = rect.height / 2f;

            Text.Font = GameFont.Tiny;
            //TODO: 1 Label text.  In Nanosuit, this is "nanosuit.LabelShortCap"
            Widgets.Label(rect3, "TestLabelTODO1");

            var rect4 = rect2;
            rect4.yMin = rect2.y + (rect2.height / 2f);

            //TODO: Implement
            //var fillPercent = casket.Energy / casket.def.maxEnergyAmount;
            var fillPercent = 0.5f;
            Widgets.FillableBar(rect4, fillPercent, FullEnergyBarTex, EmptyEnergyBarTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            //Widgets.Label(rect4, $"{casket.Energy:F0} / {casket.def.maxEnergyAmount}");
            Widgets.Label(rect4, "TestLabelPercentage");
            Text.Anchor = TextAnchor.UpperLeft;
            return new GizmoResult(GizmoState.Clear);
        }

        
    }
}
