using RimWorld;
using Verse;

namespace SomeLikeItRotten;

public class StatPart_IsCorpseFreshOrRotten : StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        if (tryGetIsFreshFactor(req, out var num))
        {
            val *= num;
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        if (tryGetIsFreshFactor(req, out var num) && num != 1f)
        {
            return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
        }

        return null;
    }

    private static bool tryGetIsFreshFactor(StatRequest req, out float factor)
    {
        if (!req.HasThing || req.Thing is not Corpse corpse)
        {
            factor = 1f;
            return false;
        }

        switch (corpse.GetRotStage())
        {
            case RotStage.Fresh:
                factor = 1f;
                break;
            case RotStage.Rotting:
                factor = 0.5f;
                break;
            default:
                factor = 0.25f;
                break;
        }

        return true;
    }
}