using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace LyingFace;

public class CompLyingFace : ThingComp
{
    public Rot4 rotation = Rot4.Invalid;

    public override void Initialize(CompProperties props)
    {
        base.Initialize(props);
        if (rotation == Rot4.Invalid && parent is Pawn pawn)
        {
            rotation = DefaultRotation(pawn);
        }
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (parent is not Pawn p || p.GetPosture() != PawnPosture.LayingInBed)
        {
            yield break;
        }

        yield return new Command_Action
        {
            defaultLabel = "LyingFace.SetLyingFace".Translate(),
            icon = getIcon(rotation),
            action = delegate
            {
                var list = new List<FloatMenuOption>();
                for (var i = 0; i < 4; i++)
                {
                    var rot = new Rot4(i);
                    var label = rot.ToStringHuman();

                    list.Add(new FloatMenuOption(label, Action));
                    continue;

                    void Action()
                    {
                        rotation = rot;
                    }
                }

                var window = new FloatMenu(list);
                Find.WindowStack.Add(window);
            }
        };
    }

    public override void PostExposeData()
    {
        var invalid = Rot4.Invalid;
        Scribe_Values.Look(ref rotation, "lyingFace", invalid);
        if (Scribe.mode == LoadSaveMode.LoadingVars && rotation == Rot4.Invalid && parent is Pawn pawn)
        {
            rotation = DefaultRotation(pawn);
        }
    }

    private static Rot4 DefaultRotation(Pawn pawn)
    {
        if (pawn.RaceProps.Humanlike)
        {
            switch (pawn.thingIDNumber % 4)
            {
                case 0:
                    return Rot4.South;
                case 1:
                    return Rot4.South;
                case 2:
                    return Rot4.East;
                case 3:
                    return Rot4.West;
            }
        }
        else
        {
            switch (pawn.thingIDNumber % 4)
            {
                case 0:
                    return Rot4.South;
                case 1:
                    return Rot4.East;
                case 2:
                    return Rot4.West;
                case 3:
                    return Rot4.West;
            }
        }

        return Rot4.Random;
    }

    public static Texture2D getIcon(Rot4 rot)
    {
        return rot.AsInt switch
        {
            0 => MyTex.LyingFaceNorth,
            1 => MyTex.LyingFaceEast,
            2 => MyTex.LyingFaceSouth,
            3 => MyTex.LyingFaceWest,
            _ => throw new Exception("rotInt's value cannot be >3 but it is:" + rot.AsInt)
        };
    }
}