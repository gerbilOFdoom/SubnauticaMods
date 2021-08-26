﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace RollControl
{
    [HarmonyPatch(typeof(Vehicle))]
    public class VehiclePatcher
    {
        /*
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void StartPostfix(Vehicle __instance)
        {
            var src = __instance.gameObject.EnsureComponent<SeamothRollController>();
            src.mySeamoth = __instance;
        }
        */

        [HarmonyPrefix]
        [HarmonyPatch("EnterVehicle")]
        public static bool EnterVehiclePrefix()
        {
            // ensure we enter vehicles correctly
            Player.main.GetComponent<ScubaRollController>().GetReadyToStopRolling();
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("FixedUpdate")]
        public static void FixedUpdatePostfix(Vehicle __instance)
        {
            // ensure we're rotated correctly
            if (Player.main.currentMountedVehicle == __instance)
            {
                Player.main.transform.localRotation = Quaternion.identity;
            }
        }
    }
}