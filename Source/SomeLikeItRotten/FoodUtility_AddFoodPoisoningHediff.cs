using HarmonyLib;
using RimWorld;
using Verse;

namespace SomeLikeItRotten;

[HarmonyPatch(typeof(FoodUtility), "AddFoodPoisoningHediff", typeof(Pawn), typeof(Thing), typeof(FoodPoisonCause))]
public static class FoodUtility_AddFoodPoisoningHediff
{
    public static bool Prefix(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
    {
        if (!SomeLikeItRottenMod.instance.Settings.RottenAnimals.Contains(pawn.def.defName))
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

        if (!SomeLikeItRotten.CanEat(corpse, pawn))
        {
            return true;
        }

        return false;
    }
}