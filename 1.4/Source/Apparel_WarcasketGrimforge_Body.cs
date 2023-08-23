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

        private float energy = 50;

        //private float maxEnergy = 100f;
        
        public float MaxEnergy { get { return def.maxEnergyAmount; } set { def.maxEnergyAmount = value; } }
        public float Energy
        {
            get
            {
                if (energy > def.maxEnergyAmount) { energy = def.maxEnergyAmount; }
                return energy;
            }
            set
            {
                energy = value;
                if (energy < 0) { energy = 0; }
                else if (energy > def.maxEnergyAmount) { energy = def.maxEnergyAmount; }
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
            //Log.Message("GetWornGizmos in Body firing");
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
                //Log.Message("ifDevMode within GetWornGizmos firing");
                yield return new Command_Toggle
                {
                    defaultLabel = "GF.TestPassiveLabel".Translate(),
                    defaultDesc = "GF.TestPassiveDesc".Translate(),
                    //hotkey
                    icon = ContentFinder<Texture2D>.Get("TEST/chest"),
                    //isActive = () => IsActive()
                    isActive = () => IsActive("TestPassiveName"),
                    toggleAction = delegate { SwitchPassive("TestPassiveName"); }
                };

                yield return new Command_Action
                {
                    defaultLabel = "GF.TestActiveLabel".Translate(),
                    defaultDesc = "GF.TestActiveDesc".Translate(),
                    //icon
                    action = delegate { TestRemoveEnergyAction(); }
                };
                yield return new Command_Action
                {
                    defaultLabel = "GF.TestActive2Label".Translate(),
                    defaultDesc = "GF.TestActive2Desc".Translate(),
                    //icon
                    action = delegate { TestAddEnergyAction(); }
                };
            }

        }

        public void TestRemoveEnergyAction()
        {
            energy = energy / 2;
        }

        public void TestAddEnergyAction()
        {
            energy = energy + (def.maxEnergyAmount - energy) / 2;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energy, "energy");
        }
        //public FortyKCasketDef def => base.def as FortyKCasketDef;

    }
}
