using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SomeLikeItRotten;

[HarmonyPatch(typeof(FoodUtility), "BestFoodSourceOnMap")]
public static class FoodUtility_TryFindBestFoodSourceFor
{
    public static bool Prefix(Pawn getter, out ThingDef foodDef, ref Thing __result)
    {
        foodDef = null;
        if (!SomeLikeItRottenMod.instance.Settings.RottenAnimals.Contains(getter.def.defName) &&
            !SomeLikeItRottenMod.instance.Settings.BoneAnimals.Contains(getter.def.defName))
        {
            return true;
        }

        bool Validator(Thing t)
        {
            return t.def.category == ThingCategory.Item && getter.CanReserve(t) && !t.IsForbidden(getter) &&
                   SomeLikeItRotten.CanEat(t, getter) && getter.RaceProps.CanEverEat(t);
        }

        //SomeLikeItRotten.LogMessage($"Looking for food for {getter}");
        var thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map,
            ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(getter),
            10f, Validator);
        if (thing == null)
        {
            //SomeLikeItRotten.LogMessage($"no food found for {getter}");
            return true;
        }

        __result = thing;
        foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
        return false;
    }
}