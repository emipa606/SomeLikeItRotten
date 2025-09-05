using HarmonyLib;
using RimWorld;
using Verse;

namespace SomeLikeItRotten;

[HarmonyPatch(typeof(FoodUtility), nameof(FoodUtility.AddFoodPoisoningHediff), typeof(Pawn), typeof(Thing),
    typeof(FoodPoisonCause))]
public static class FoodUtility_AddFoodPoisoningHediff
{
    public static bool Prefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
    {
        if (!SomeLikeItRottenMod.Instance.Settings.RottenAnimals.Contains(pawn.def.defName) &&
            !SomeLikeItRottenMod.Instance.Settings.BoneAnimals.Contains(pawn.def.defName))
        {
            return true;
        }

        if (ingestible is not Corpse corpse)
        {
            return true;
        }

        if (corpse.InnerPawn == null)
        {
            return false;
        }

        return !SomeLikeItRotten.CanEat(corpse, pawn);
    }
}