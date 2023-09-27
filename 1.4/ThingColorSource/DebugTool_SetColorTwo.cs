using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ThingColor
{
    public class DebugTool_SetColorTwo
    {
        [DebugAction("General", null, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetColorTwo()
        {
            List<FloatMenuOption> list = new();
            IntVec3 cell = UI.MouseCell();
            list.Add(new FloatMenuOption("Random", delegate
            {
                SetColor_All(GenColor.RandomColorOpaque());
            }));
            foreach (Ideo i in Find.IdeoManager.IdeosListForReading)
            {
                if (!i.hiddenIdeoMode && i.Icon != BaseContent.BadTex)
                {
                    list.Add(new FloatMenuOption(i.name, delegate
                    {
                        SetColor_All(i.Color);
                    }, i.Icon, i.Color));
                }
            }
            foreach (ColorDef c in DefDatabase<ColorDef>.AllDefs)
            {
                list.Add(new FloatMenuOption(c.defName, delegate
                {
                    SetColor_All(c.color);
                }, BaseContent.WhiteTex, c.color));
            }
            Find.WindowStack.Add(new FloatMenu(list));


            void SetColor_All(Color color)
            {
                foreach (Thing item in Find.CurrentMap.thingGrid.ThingsAt(cell).ToList())
                {
                    if (item is Pawn pawn && pawn.apparel != null)
                    {
                        foreach (Apparel apparel in pawn.apparel.WornApparel)
                        {
                            if (apparel is ApparelColored coloredApparel)
                            {
                                coloredApparel.ColorTwo = color;
                                coloredApparel.DesiredColorTwo = null;
                                coloredApparel.Notify_ColorChanged();
                            }
                        }
                    }
                    else
                    {
                        if (item is ApparelColored coloredApparel)
                        {
                            coloredApparel.ColorTwo = color;
                            coloredApparel.DesiredColorTwo = null;
                            coloredApparel.Notify_ColorChanged();
                        }
                    }
                }
            }
        }
    }
}
