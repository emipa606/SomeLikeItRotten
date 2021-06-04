using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SomeLikeItRotten
{
    [StaticConstructorOnStartup]
    public static class SomeLikeItRotten
    {
        public static readonly List<ThingDef> AllAnimals;

        static SomeLikeItRotten()
        {
            AllAnimals = (from animal in DefDatabase<ThingDef>.AllDefsListForReading
                where animal.race?.Animal == true
                orderby animal.label
                select animal).ToList();

            LogMessage($"Found {AllAnimals.Count} animals", true);
            var harmony = new Harmony("Mlie.SomeLikeItRotten");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void LogMessage(string message, bool forced = false)
        {
            if (!forced && !SomeLikeItRottenMod.instance.Settings.VerboseLogging)
            {
                return;
            }

            Log.Message($"[SomeLikeItRotten]: {message}");
        }

        public static bool CanEat(Thing thing, Pawn pawn)
        {
            if (thing is not Corpse corpse)
            {
                if (thing.IngestibleNow)
                {
                    return true;
                }

                return false;
            }

            if (!corpse.InnerPawn.RaceProps.IsFlesh)
            {
                return false;
            }

            switch (corpse.GetRotStage())
            {
                case RotStage.Fresh:
                    return true;
                case RotStage.Rotting:
                    return SomeLikeItRottenMod.instance.Settings.RottenAnimals.Contains(pawn.def.defName);
                case RotStage.Dessicated:
                    return SomeLikeItRottenMod.instance.Settings.BoneAnimals.Contains(pawn.def.defName);
                default:
                    return false;
            }
        }
    }
}