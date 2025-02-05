﻿using Verse;
using UnityEngine;

namespace Rim73
{
    public class Rim73_Settings : ModSettings
    {
        public static bool hediff = true;
        public static bool jobs = true;
        public static bool mood = true;
        public static bool needs = true;
        public static bool pather = true;
        public static bool mindstate = true;
        public static bool regionCache = true;
        public static bool enemiesNearbyCache = true;
        public static bool enemies = false;
        public static bool warpSpeed = false;

        private static Vector2 ScrollPos = Vector2.zero;

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            Rect viewRect = new Rect(0f, 0f, inRect.width, 36f * 26f);
            viewRect.xMax *= 0.9f;

            //listing_Standard.BeginScrollView(inRect, ref ScrollPos, ref viewRect);
            listing_Standard.Begin(viewRect);
            GUI.EndGroup();
            Widgets.BeginScrollView(inRect, ref ScrollPos, viewRect);
            listing_Standard.Label("rim73_settings".Translate());
            listing_Standard.GapLine();

            listing_Standard.CheckboxLabeled("rim73_hediff".Translate(), ref hediff, "rim73_hediff_note".Translate());
            //listing_Standard.CheckboxLabeled("rim73_mood".Translate(), ref mood, "rim73_mood_note".Translate());
            //listing_Standard.CheckboxLabeled("rim73_needs".Translate(), ref needs, "rim73_needs_note".Translate());
            listing_Standard.CheckboxLabeled("rim73_pather".Translate(), ref pather, "rim73_pather_note".Translate());
            listing_Standard.CheckboxLabeled("rim73_jobs".Translate(), ref jobs, "rim73_jobs_note".Translate());
            listing_Standard.CheckboxLabeled("rim73_mindstate".Translate(), ref mindstate, "rim73_mindstate_note".Translate());

            listing_Standard.Gap(36);
            listing_Standard.Label("rim73_settings_experimental".Translate());
            listing_Standard.GapLine();
            listing_Standard.CheckboxLabeled("rim73_regionCache".Translate(), ref regionCache, "rim73_regionCache_note".Translate());
            listing_Standard.CheckboxLabeled("rim73_enemiesNearbyCache".Translate(), ref enemiesNearbyCache, "rim73_enemiesNearbyCache_note".Translate());
            listing_Standard.CheckboxLabeled("rim73_enemies".Translate(), ref enemies, "rim73_enemies_note".Translate());

            listing_Standard.Gap(36);
            listing_Standard.Label("rim73_settings_misc".Translate());
            listing_Standard.GapLine();
            listing_Standard.CheckboxLabeled("rim73_superSpeed".Translate(), ref warpSpeed, "rim73_superSpeed_note".Translate());

            //listing_Standard.CheckboxLabeled("rim73_regionCache".Translate(), ref regionCache, "rim73_regionCache_note".Translate());

            /*
            listing_Standard.GapLine();
            listing_Standard.Label("temp".Translate());
            */

            //listing_Standard.EndScrollView(ref viewRect);
            Widgets.EndScrollView();
            //listing_Standard.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hediff, "rim73_hediff", true, false);
            Scribe_Values.Look(ref jobs, "rim73_jobs", true, false);
            Scribe_Values.Look(ref mood, "rim73_mood", false, false);
            Scribe_Values.Look(ref needs, "rim73_needs", true, false);
            Scribe_Values.Look(ref pather, "rim73_pather", true, false);
            Scribe_Values.Look(ref mindstate, "rim73_mindstate", true, false);
            Scribe_Values.Look(ref regionCache, "rim73_regionCache", true, false);
            Scribe_Values.Look(ref enemiesNearbyCache, "rim73_enemiesNearbyCache", true, false);
            Scribe_Values.Look(ref warpSpeed, "rim73_superSpeed", false, false);
            Scribe_Values.Look(ref enemies, "rim73_enemies", false, false);
        }
    }
}
