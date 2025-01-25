using System.Reflection;
using HarmonyLib;
using Verse;

namespace LyingFace;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        new Harmony("com.tammybee.lyingface").PatchAll(Assembly.GetExecutingAssembly());
    }
}