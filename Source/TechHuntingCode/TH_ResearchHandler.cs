using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace TechHunting
{
    public class CompProperties_ProgressTechProject : CompProperties_UseEffect
    {
        public int researchProgress;
        public CompProperties_ProgressTechProject()
        {
            this.compClass = typeof(CompUseEffect_ProgressTechProject);
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
        {
            foreach (StatDrawEntry item in base.SpecialDisplayStats(req))
            {
                yield return item;
            }
            yield return new StatDrawEntry(StatCategoryDefOf.Ability, "Research progress", researchProgress.ToString(), "The amount of research progress gained for the current project when using this item.", 5000);
        }
    }
    public class CompUseEffect_ProgressTechProject : CompUseEffect
    {
        public CompProperties_ProgressTechProject Props => (CompProperties_ProgressTechProject)this.props;

        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            ResearchProjectDef project = Find.ResearchManager.GetProject();
            if (project != null)
            {
                AddProgress(project, usedBy, Props.researchProgress);
            }
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            if (Find.ResearchManager.GetProject() == null)
            {
                return "NoActiveResearchProjectToFinish".Translate();
            }
            return true;
        }

        private void AddProgress(ResearchProjectDef proj, Pawn usedBy, int amount)
        {
            Find.ResearchManager.AddProgress(proj, amount);
            Messages.Message($"The {proj.label} project was advanced by {amount} points!", MessageTypeDefOf.PositiveEvent );
        }
    }
}

