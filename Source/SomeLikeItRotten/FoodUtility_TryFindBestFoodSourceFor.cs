using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SomeLikeItRotten;

[HarmonyPatch(typeof(FoodUtility), nameof(FoodUtility.BestFoodSourceOnMap))]
public static class FoodUtility_TryFindBestFoodSourceFor
{
    public static bool Prefix(Pawn getter, out ThingDef foodDef, ref Thing __result)
    {
        foodDef = null;
        if (!SomeLikeItRottenMod.Instance.Settings.RottenAnimals.Contains(getter.def.defName) &&
            !SomeLikeItRottenMod.Instance.Settings.BoneAnimals.Contains(getter.def.defName))
        {
            return true;
        }

        var thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map,
            ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(getter),
            10f, validator);
        if (thing == null)
        {
            return true;
        }

        __result = thing;
        foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
        return false;

        bool validator(Thing t)
        {
            return t.def.category == ThingCategory.Item && getter.CanReserve(t) && !t.IsForbidden(getter) &&
                   SomeLikeItRotten.CanEat(t, getter) && getter.RaceProps.CanEverEat(t);
        }
    }
}