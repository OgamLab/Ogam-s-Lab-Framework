using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Verse;
using VFEPirates;

namespace Grimforge
{
    public class Apparel_WarcasketGrimforge_Body : Apparel_WarcasketGrimforge
    {

        private float energy;

        private float maxEnergy = 100f;
        
        public float MaxEnergy { get { return maxEnergy; } set { maxEnergy = value; } }
        public float Energy
        {
            get
            {
                if (energy > maxEnergy) { energy = maxEnergy; }
                return energy;
            }
            set
            {
                energy = value;
                if (energy < 0) { energy = 0; }
                else if (energy > maxEnergy) { energy = maxEnergy; }
            }
        }
        public override void Tick()
        {
            base.Tick();
            //energy -= drainTotal;
            float dTot = 0;
            for (int i = 0; i < abilities_Passives.Count; i++)
            {
                if (abilities_Passives[i].Active) { dTot += abilities_Passives[i].Drain; }
            }

            energy = dTot > energy ? 0 : energy - dTot;
        }

        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            Log.Message("GetWornGizmos in Body firing");
            foreach (var gizmo in base.GetWornGizmos())
            {
                yield return gizmo;
            }

            if (Find.Selector.SingleSelectedThing == Wearer && Wearer.IsColonistPlayerControlled)
            {
                var gizmo_ArmorEnergyStatus = new Gizmo_ArmorEnergyStatus
                {
                    casket = this
                };
                yield return gizmo_ArmorEnergyStatus;
            }

            if (Prefs.DevMode)
            {
                yield return new Command_Toggle
                {
                    defaultLabel = "GF.TestPassiveLabel".Translate(),
                    defaultDesc = "GF.TestPassiveDesc".Translate(),
                    //hotkey
                    icon = ContentFinder<Texture2D>.Get("TEST/chest"),
                    //isActive = () => IsActive()
                    isActive = () => IsActive("Test"),
                    toggleAction = delegate { SwitchPassive("Test"); }
                };
            }

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energy, "energy");
        }

        //public FortyKCasketDef def => base.def as FortyKCasketDef;

    }
}
