﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using HarmonyLib;
using SMLHelper.V2.Options;
using SMLHelper.V2.Handlers;
using LitJson;
using System.Runtime.CompilerServices;
using System.Collections;

namespace RollControlZero
{
    [HarmonyPatch(typeof(SeaTruckMotor))]
    [HarmonyPatch("StabilizeRoll")]
    class SeaTruckMotorPatcher
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (RollControlPatcher.Config.isSeatruckRollOn)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
