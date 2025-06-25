using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace LyingFace;

public class CompBedEx : ThingComp
{
    private List<Pawn> owners = [];

    private Building_Bed Bed => parent as Building_Bed;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if (owners.NullOrEmpty())
        {
            return;
        }

        var comp = Bed.GetComp<CompAssignableToPawn>();
        if (comp == null)
        {
            return;
        }

        Traverse.Create(comp).Field("assignedPawns").SetValue(owners);
        foreach (var owner in owners)
        {
            owner.ownership?.ClaimBedIfNonMedical((Building_Bed)parent);
        }
    }

    public override void PostExposeData()
    {
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            owners = Bed.OwnersForReading;
        }

        Scribe_Collections.Look(ref owners, "owners", LookMode.Reference);
        if (Scribe.mode != LoadSaveMode.LoadingVars || owners.NullOrEmpty())
        {
            return;
        }

        foreach (var owner in owners)
        {
            owner.ownership?.ClaimBedIfNonMedical((Building_Bed)parent);
        }
    }
}