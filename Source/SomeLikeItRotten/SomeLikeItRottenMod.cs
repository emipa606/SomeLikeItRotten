﻿using System.Collections.Generic;
using Mlie;
using UnityEngine;
using Verse;

namespace SomeLikeItRotten
{
    [StaticConstructorOnStartup]
    internal class SomeLikeItRottenMod : Mod
    {
        /// <summary>
        ///     The instance of the settings to be read by the mod
        /// </summary>
        public static SomeLikeItRottenMod instance;

        private static string currentVersion;

        private static Vector2 scrollPosition;

        private static bool[] rottenTempList;


        /// <summary>
        ///     The private settings
        /// </summary>
        private SomeLikeItRottenModSettings settings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="content"></param>
        public SomeLikeItRottenMod(ModContentPack content)
            : base(content)
        {
            instance = this;
            scrollPosition = Vector2.zero;
            currentVersion =
                VersionFromManifest.GetVersionFromModMetaData(
                    ModLister.GetActiveModWithIdentifier("Mlie.SomeLikeItRotten"));
        }


        /// <summary>
        ///     The instance-settings for the mod
        /// </summary>
        internal SomeLikeItRottenModSettings Settings
        {
            get
            {
                if (settings != null)
                {
                    return settings;
                }

                settings = GetSettings<SomeLikeItRottenModSettings>();
                if (settings.RottenAnimals == null)
                {
                    settings.RottenAnimals = new List<string>();
                }

                return settings;
            }

            set => settings = value;
        }

        /// <summary>
        ///     The settings-window
        /// </summary>
        /// <param name="rect"></param>
        public override void DoSettingsWindowContents(Rect rect)
        {
            if (rottenTempList == null)
            {
                rottenTempList = new bool[SomeLikeItRotten.AllAnimals.Count];
                for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
                {
                    var animal = SomeLikeItRotten.AllAnimals[index];
                    rottenTempList[index] = Settings.RottenAnimals.Contains(animal.defName);
                }
            }

            base.DoSettingsWindowContents(rect);
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Label("SLIR.info.label".Translate());
            listing_Standard.CheckboxLabeled("SLIR.logging.label".Translate(), ref Settings.VerboseLogging,
                "SLIR.logging.tooltip".Translate());
            if (currentVersion != null)
            {
                GUI.contentColor = Color.gray;
                listing_Standard.Label("SLIR.version.label".Translate(currentVersion));
                GUI.contentColor = Color.white;
            }
            else
            {
                listing_Standard.Gap();
            }

            var frameRect = rect;
            frameRect.y += 100;
            frameRect.height -= 100;
            var contentRect = frameRect;
            contentRect.height = SomeLikeItRotten.AllAnimals.Count * 25f / 3;
            contentRect.width -= 20;
            contentRect.x = 0;
            contentRect.y = 0;

            listing_Standard.GapLine();
            listing_Standard.End();

            var scrollListing = new Listing_Standard();
            scrollListing.BeginScrollView(frameRect, ref scrollPosition, ref contentRect);
            scrollListing.ColumnWidth = (contentRect.width - 40) / 3;
            for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
            {
                if (index == (SomeLikeItRotten.AllAnimals.Count / 3) + 1)
                {
                    scrollListing.NewColumn();
                }

                if (index == (SomeLikeItRotten.AllAnimals.Count / 3 * 2) + 1)
                {
                    scrollListing.NewColumn();
                }

                var animal = SomeLikeItRotten.AllAnimals[index];
                HighlightedCheckbox(animal.label.CapitalizeFirst(), ref rottenTempList[index], scrollListing);
            }

            scrollListing.EndScrollView(ref contentRect);
        }

        private static void HighlightedCheckbox(string label, ref bool checkOn, Listing_Standard listing)
        {
            var lineHeight = Text.LineHeight;
            var rect = listing.GetRect(lineHeight);
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }

            Widgets.CheckboxLabeled(rect, label, ref checkOn);
            listing.Gap(listing.verticalSpacing);
        }

        public override void WriteSettings()
        {
            Settings.RottenAnimals = new List<string>();
            for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
            {
                var animal = SomeLikeItRotten.AllAnimals[index];
                if (rottenTempList[index])
                {
                    Settings.RottenAnimals.Add(animal.defName);
                }
            }

            base.WriteSettings();
        }

        /// <summary>
        ///     The title for the mod-settings
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return "Some Like It Rotten";
        }
    }
}