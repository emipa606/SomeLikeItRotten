using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SomeLikeItRotten;

[StaticConstructorOnStartup]
public static class SomeLikeItRotten
{
    public static readonly List<ThingDef> AllAnimals;

    static SomeLikeItRotten()
    {
        AllAnimals = (from creature in DefDatabase<ThingDef>.AllDefsListForReading
            where creature.race != null && !creature.IsCorpse
            orderby creature.label
            select creature).ToList();

        logMessage($"Found {AllAnimals.Count} creatures", true);
        var harmony = new Harmony("Mlie.SomeLikeItRotten");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private static void logMessage(string message, bool forced = false)
    {
        if (!forced && !SomeLikeItRottenMod.Instance.Settings.VerboseLogging)
        {
            return;
        }

        Log.Message($"[SomeLikeItRotten]: {message}");
    }

    public static bool CanEat(Thing thing, Pawn pawn)
    {
        if (thing == null)
        {
            return false;
        }

        if (thing is not Corpse corpse)
        {
            return thing.IngestibleNow;
        }

        if (corpse.InnerPawn?.RaceProps?.IsFlesh != true)
        {
            return false;
        }

        switch (corpse.GetRotStage())
        {
            case RotStage.Fresh:
                return true;
            case RotStage.Rotting:
                return SomeLikeItRottenMod.Instance.Settings.RottenAnimals.Contains(pawn.def.defName);
            case RotStage.Dessicated:
                return SomeLikeItRottenMod.Instance.Settings.BoneAnimals.Contains(pawn.def.defName);
            default:
                return false;
        }
    }
}