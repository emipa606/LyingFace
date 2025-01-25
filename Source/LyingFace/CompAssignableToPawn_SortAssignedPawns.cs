using HarmonyLib;
using RimWorld;

namespace LyingFace;

[HarmonyPatch(typeof(CompAssignableToPawn), "SortAssignedPawns")]
internal class CompAssignableToPawn_SortAssignedPawns
{
    public static bool Prefix(CompAssignableToPawn __instance)
    {
        return __instance.parent.def.thingClass != typeof(Building_Bed);
    }
}