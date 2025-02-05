﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using Verse;
using UnityEngine;
using RimWorld;

namespace Rim73
{
    [StaticConstructorOnStartup]
    public static class Rim73_Loader
    {
        // Loader
        static Rim73_Loader()
        {
            // HediffComps Caching
            Rim73_Hediff.InitHediffCompsDB();
            Rim73_Hediff.InitFieldInfos();

            // Jobs init
            Rim73_Jobs.InitFieldInfos();
        }

        // On Loading new game
        [HarmonyPatch(typeof(Game), "FinalizeInit", new Type[] { })]
        static class OnLoadedGame
        {
            static void Postfix(ref Game __instance)
            {
                // Inits the Dictionary for a fixed size in memory.
                Log.Warning("Rim73 > Loaded new game, resetting caching variables");
                Rim73_MindState.InitCache();
                Rim73_Pather.InitRegionCache();

                // Setup WarpSpeed
                if (Rim73_Settings.warpSpeed)
                {
                    Rim73.TickManager_UltraSpeedBoost.SetValue(__instance.tickManager, Rim73_Settings.warpSpeed);
                    Log.Warning("WarpSpeed is enabled, you can disable it from the Rim73 settings and reload the game to take effect.");
                }
            }
        }
    }

    public class Rim73 : Mod
    {

        public static Rim73_Settings Settings;
        public static string Version = "1.5a";

        // Immunity
        public static MethodInfo ImmunityHandler;
        public static FastInvokeHandler ImmunityFastInvoke;

        // Job Tracker
        public static MethodInfo JobTracker_TryFindAndStartJob;
        public static FastInvokeHandler JobTracker_TryFindAndStartJob_FastInvoke;

        public static MethodInfo TickList_Tick;
        public static FastInvokeHandler TickList_TickFastInvoke;

        // Job Driver
        public static MethodInfo JobDriver_CheckCurrentToilEndOrFail;
        public static FastInvokeHandler JobDriver_CheckCurrentToilEndOrFail_FastInvoke;
        public static MethodInfo JobDriver_TryActuallyStartNextToil;
        public static FastInvokeHandler JobDriver_TryActuallyStartNextToil_FastInvoke;
        public static MethodInfo JobDriver_CanStartNextToilInBusyStance;
        public static FastInvokeHandler JobDriver_CanStartNextToilInBusyStance_FastInvoke;

        // Hediff comp
        public static MethodInfo HediffComp_HealPermanentWounds_CompPostTick;
        public static FastInvokeHandler HediffComp_HealPermanentWounds_CompPostTick_FastInvoke;

        // Tick Manager
        public static FieldInfo TickManager_UltraSpeedBoost;

        // Ticker
        public static int Ticks = 0;
        public static int RealTicks = 0;


        public Rim73(ModContentPack content) : base(content)
        {
            Harmony harmony = new Harmony("ghost.rolly.rim73");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Get MethodInfo for Immunity Handler
            MethodInfo[] ImmunityHandler_Methods = typeof(ImmunityHandler).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            ImmunityHandler = ImmunityHandler_Methods[ImmunityHandler_Methods.FirstIndexOf((MethodInfo x) => (x.Name == "ImmunityHandlerTick"))];
            ImmunityFastInvoke = MethodInvoker.GetHandler(ImmunityHandler);

            // Methods for JobDriver
            MethodInfo[] JobDriver_Methods = typeof(Verse.AI.JobDriver).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            JobDriver_CheckCurrentToilEndOrFail = JobDriver_Methods[JobDriver_Methods.FirstIndexOf((MethodInfo x) => (x.Name == "CheckCurrentToilEndOrFail"))];
            JobDriver_CheckCurrentToilEndOrFail_FastInvoke = MethodInvoker.GetHandler(JobDriver_CheckCurrentToilEndOrFail);
            JobDriver_TryActuallyStartNextToil = JobDriver_Methods[JobDriver_Methods.FirstIndexOf((MethodInfo x) => (x.Name == "TryActuallyStartNextToil"))];
            JobDriver_TryActuallyStartNextToil_FastInvoke = MethodInvoker.GetHandler(JobDriver_TryActuallyStartNextToil);

            // Methods for JobTracker
            MethodInfo[] JobTracker_Methods = typeof(Verse.AI.Pawn_JobTracker).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            JobTracker_TryFindAndStartJob = JobTracker_Methods[JobTracker_Methods.FirstIndexOf((MethodInfo x) => (x.Name == "TryFindAndStartJob"))];
            JobTracker_TryFindAndStartJob_FastInvoke = MethodInvoker.GetHandler(JobTracker_TryFindAndStartJob);

            MethodInfo[] Tick_Methods = typeof(Verse.TickList).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            TickList_Tick = Tick_Methods[Tick_Methods.FirstIndexOf((MethodInfo x) => (x.Name == "Tick"))];
            TickList_TickFastInvoke = MethodInvoker.GetHandler(TickList_Tick);


            // Methods for HediffComp_HealPermanentWounds
            HediffComp_HealPermanentWounds_CompPostTick = typeof(Verse.HediffComp_HealPermanentWounds).GetMethod("CompPostTick");
            HediffComp_HealPermanentWounds_CompPostTick_FastInvoke = MethodInvoker.GetHandler(HediffComp_HealPermanentWounds_CompPostTick);

            // WarpSpeed
            Rim73.TickManager_UltraSpeedBoost = typeof(TickManager).GetField("UltraSpeedBoost", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Static);

            // Compatibilities
            Rim73_Compatibility.DoAllCompatiblities(harmony);

            Verse.Log.Message("Rim73 " + Version + " Initialized :)");
            base.GetSettings<Rim73_Settings>();
        }

        public override string SettingsCategory()
        {
            return "Rim73 - " + Version;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rim73_Settings.DoSettingsWindowContents(inRect);
        }

    }
}
