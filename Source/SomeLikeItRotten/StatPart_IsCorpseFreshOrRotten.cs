using RimWorld;
using Verse;

namespace SomeLikeItRotten
{
    public class StatPart_IsCorpseFreshOrRotten : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (TryGetIsFreshFactor(req, out var num))
            {
                val *= num;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (TryGetIsFreshFactor(req, out var num) && num != 1f)
            {
                return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
            }

            return null;
        }

        private bool TryGetIsFreshFactor(StatRequest req, out float factor)
        {
            if (!req.HasThing)
            {
                factor = 1f;
                return false;
            }

            if (!(req.Thing is Corpse corpse))
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
                    factor = 0;
                    break;
            }

            return true;
        }
    }
}