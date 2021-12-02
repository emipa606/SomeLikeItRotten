using System;
using System.Collections.Generic;
using Mlie;
using UnityEngine;
using Verse;

namespace SomeLikeItRotten;

[StaticConstructorOnStartup]
internal class SomeLikeItRottenMod : Mod
{
    private const float checkboxSpacer = 100;

    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SomeLikeItRottenMod instance;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static bool[] rottenTempList;

    private static bool[] boneTempList;


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

            if (settings.BoneAnimals == null)
            {
                settings.BoneAnimals = new List<string>();
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

        if (boneTempList == null)
        {
            boneTempList = new bool[SomeLikeItRotten.AllAnimals.Count];
            for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
            {
                var animal = SomeLikeItRotten.AllAnimals[index];
                boneTempList[index] = Settings.BoneAnimals.Contains(animal.defName);
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
            listing_Standard.Label(string.Empty);
        }

        listing_Standard.ColumnWidth = (rect.width - 60) / 3;
        var labelSpot = listing_Standard.Label("SLIR.animal.label".Translate());
        Widgets.Label(
            new Rect(labelSpot.x + labelSpot.width - (checkboxSpacer * 1.25f), labelSpot.y, checkboxSpacer / 2,
                labelSpot.height), "SLIR.rotten.label".Translate());
        Widgets.Label(
            new Rect(labelSpot.x + labelSpot.width - (checkboxSpacer * 0.5f), labelSpot.y, checkboxSpacer / 2,
                labelSpot.height), "SLIR.bone.label".Translate());
        listing_Standard.GapLine();
        for (var i = 0; i < 2; i++)
        {
            listing_Standard.NewColumn();
            listing_Standard.Gap(labelSpot.y);
            labelSpot = listing_Standard.Label("SLIR.animal.label".Translate());
            Widgets.Label(
                new Rect(labelSpot.x + labelSpot.width - (checkboxSpacer * 1.25f), labelSpot.y, checkboxSpacer / 2,
                    labelSpot.height), "SLIR.rotten.label".Translate());
            Widgets.Label(
                new Rect(labelSpot.x + labelSpot.width - (checkboxSpacer * 0.5f), labelSpot.y, checkboxSpacer / 2,
                    labelSpot.height), "SLIR.bone.label".Translate());
            listing_Standard.GapLine();
        }

        listing_Standard.End();

        var frameRect = rect;
        frameRect.y += labelSpot.y + 40;
        frameRect.height -= labelSpot.y + 40;
        var contentRect = frameRect;
        contentRect.height = SomeLikeItRotten.AllAnimals.Count * 25f / 3;
        contentRect.width -= 20;
        contentRect.x = 0;
        contentRect.y = 0;


        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect);
        scrollListing.Begin(contentRect);
        scrollListing.ColumnWidth = (contentRect.width - 40) / 3;

        for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
        {
            if (index == (int)Math.Floor(SomeLikeItRotten.AllAnimals.Count / (decimal)3))
            {
                scrollListing.NewColumn();
            }

            if (index == (int)Math.Floor(SomeLikeItRotten.AllAnimals.Count / (decimal)3 * 2))
            {
                scrollListing.NewColumn();
            }

            var animal = SomeLikeItRotten.AllAnimals[index];
            HighlightedCheckbox(animal.label.CapitalizeFirst(), ref rottenTempList[index], ref boneTempList[index],
                scrollListing);
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private static void HighlightedCheckbox(string label, ref bool checkOn, ref bool alsoCheckOn,
        Listing_Standard listing)
    {
        var lineHeight = Text.LineHeight;
        var rect = listing.GetRect(lineHeight);
        if (Mouse.IsOver(rect))
        {
            Widgets.DrawHighlight(rect);
        }

        var leftRect = rect;
        leftRect.width -= checkboxSpacer;
        var rightRect = rect;
        rightRect.width = checkboxSpacer / 2;
        rightRect.x = rect.x + leftRect.width + (checkboxSpacer / 2);
        Widgets.CheckboxLabeled(leftRect, label, ref checkOn);
        Widgets.Checkbox(rightRect.position, ref alsoCheckOn);
        listing.Gap(listing.verticalSpacing);
    }

    public override void WriteSettings()
    {
        Settings.RottenAnimals = new List<string>();
        Settings.BoneAnimals = new List<string>();
        for (var index = 0; index < SomeLikeItRotten.AllAnimals.Count; index++)
        {
            var animal = SomeLikeItRotten.AllAnimals[index];
            if (rottenTempList[index])
            {
                Settings.RottenAnimals.Add(animal.defName);
            }

            if (boneTempList[index])
            {
                Settings.BoneAnimals.Add(animal.defName);
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