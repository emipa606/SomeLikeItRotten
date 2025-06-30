using HarmonyLib;
using RimWorld;
using Verse;

namespace SomeLikeItRotten;

[HarmonyPatch(typeof(Corpse), nameof(Corpse.IngestibleNow), MethodType.Getter)]
public static class Corpse_IngestibleNow
{
    public static void Postfix(Corpse __instance, ref bool __result)
    {
        if (!__result)
        {
            __result = !__instance.IsBurning() && __instance.def.IsIngestible &&
                       __instance.InnerPawn.RaceProps.IsFlesh;
        }
    }
}