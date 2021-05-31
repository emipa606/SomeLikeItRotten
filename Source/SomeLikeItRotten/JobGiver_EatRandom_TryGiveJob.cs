using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace SomeLikeItRotten
{
    [HarmonyPatch(typeof(JobGiver_EatRandom), "TryGiveJob", typeof(Pawn))]
    public static class JobGiver_EatRandom_TryGiveJob
    {
        public static bool Prefix(Pawn pawn, ref Job __result)
        {
            if (SomeLikeItRottenMod.instance.Settings.RottenAnimals.Contains(pawn.def.defName))
            {
                return true;
            }

            if (pawn.Downed)
            {
                __result = null;
                return false;
            }

            bool Validator(Thing t)
            {
                return t.def.category == ThingCategory.Item && SomeLikeItRotten.CanEat(t, pawn, true) &&
                       pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t);
            }

            var thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map,
                ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn),
                10f, Validator);
            if (thing == null)
            {
                __result = null;
                return false;
            }

            __result = JobMaker.MakeJob(JobDefOf.Ingest, thing);

            return false;
        }
    }
}