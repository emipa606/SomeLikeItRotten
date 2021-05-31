using System.Collections.Generic;
using Verse;

namespace SomeLikeItRotten
{
    /// <summary>
    ///     Definition of the settings for the mod
    /// </summary>
    internal class SomeLikeItRottenModSettings : ModSettings
    {
        public List<string> RottenAnimals;
        public bool VerboseLogging;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
            Scribe_Collections.Look(ref RottenAnimals, "RottenAnimals", LookMode.Value);
        }
    }
}