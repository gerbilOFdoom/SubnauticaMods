﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using HarmonyLib;
using Nautilus.Options;
using Nautilus.Handlers;
using Nautilus.Utility;
using LitJson;
using System.Net.NetworkInformation;
using Nautilus.Options.Attributes;
using Nautilus.Json;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;

namespace FreeLook
{
    public static class Logger
    {
        internal static ManualLogSource MyLog { get; set; }
        public static void Log(string message)
        {
            MyLog.LogInfo("[FreeLook] " + message);
        }
    }
    [Menu("FreeLook Options")]
    public class MyConfig : ConfigFile
    {
        [Toggle("Enable Hinting")]
        public bool isHintingEnabled = true;

        [Keybind("FreeLook Key")]
        public KeyCode FreeLookKey = KeyCode.LeftAlt;
    }

    [BepInPlugin("com.mikjaw.subnautica.freelook.mod", "FreeLook", "2.0.1")]
    [BepInDependency("com.snmodding.nautilus")]
    public class FreeLookPatcher : BaseUnityPlugin
    {
        internal static MyConfig config { get; private set; }
        //public static Options Options = new Options();
        public void Awake()
        {
            FreeLook.Logger.MyLog = base.Logger;
        }
        public void Start()
        {
            config = OptionsPanelHandler.RegisterModOptions<MyConfig>();
            //OptionsPanelHandler.RegisterModOptions(Options);
            var harmony = new Harmony("com.mikjaw.subnautica.freelook.mod");
            harmony.PatchAll();

            // here we coerce tweaks and fixes into compatibility
            // in other words, we neuter one of its patches.
            var type2 = Type.GetType("Tweaks_Fixes.SeaMoth_patch, Tweaks and Fixes", false, false);
            if (type2 != null)
            {
                var TweaksFixesSeamothUpdatePrefix = AccessTools.Method(type2, "UpdatePrefix");
                harmony.Patch(TweaksFixesSeamothUpdatePrefix, prefix: new HarmonyMethod(typeof(FreeLookPatcher), nameof(TweaksFixesSeamothUpdatePrefixPrefix)));
            }
        }

        public static bool TweaksFixesSeamothUpdatePrefixPrefix(SeaMoth __instance, ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
