using HarmonyLib;
using RimWorld;
using Verse;

namespace LyingFace;

[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.LayingFacing))]
internal class PawnRenderer_LayingFacing
{
    [HarmonyPrefix]
    public static bool UpdateLayingFacing(ref Rot4 __result, Pawn ___pawn)
    {
        var comp = ___pawn?.GetComp<CompLyingFace>();
        if (comp == null || ___pawn.GetPosture() != PawnPosture.LayingInBed)
        {
            return true;
        }

        __result = comp.rotation;
        return false;
    }
}