using LogicAPI.Client;
using LogicLog;
using LogicWorld.UI;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using LogicWorld.BuildingManagement;
using LogicAPI.Data.BuildingRequests;
using System;
using System.Reflection.Emit;
using HarmonyLib;

namespace CheeseUtilMod {
    public class CheeseUtilClient : ClientMod {
        private static Harmony harmony = null;
        static CheeseUtilClient()
        {
            harmony = new Harmony("CheeseUtilMod.patch");
            PropertyInfo property = typeof(EditAndGateMenu).GetProperty("MaxPegs");
            property = property.DeclaringType.GetProperty("MaxPegs");
            property.SetValue(null, 32, BindingFlags.NonPublic | BindingFlags.Static, null, null, null);
            MethodInfo getTextIDsInject = typeof(InjectedMenuCode).GetMethod("GetTextIDsOfComponentTypesThatCanBeEdited");
            MethodInfo getTextIDsRepl = typeof(EditAndGateMenu).GetMethod("GetTextIDsOfComponentTypesThatCanBeEdited");
            Inject(typeof(EditAndGateMenu), getTextIDsInject, getTextIDsRepl);
            MethodInfo valueChangedInject = typeof(InjectedMenuCode).GetMethod("InputCountSlider_OnValueChangedInt");
            MethodInfo valueChangedRepl = typeof(EditAndGateMenu).GetMethod("InputCountSlider_OnValueChangedInt");
            Inject(typeof(EditAndGateMenu), valueChangedInject, valueChangedRepl);
        }

        private static void Inject(Type cls, MethodInfo toInject, MethodInfo toReplace)
        {
            harmony.Patch(toInject, null, null, new HarmonyMethod(toReplace));
        }

        protected override void Initialize() {
            Logger.Info("Cheese Util Mod - Client Loaded");
        }
    }
    class InjectedMenuCode : EditComponentMenu
    {
        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            Debug.Log("Harmony patch for GetTextIDsOfComponentTypesThatCanBeEdited successful");
            yield return "MHG.AndGate";
            yield break;
        }
        private void InputCountSlider_OnValueChangedInt(int value)
        {
            Debug.Log("Harmony patch for OnValueChangedInt successful");
            foreach (EditingComponentInfo editingComponentInfo in base.ComponentsBeingEdited)
            {
                BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(editingComponentInfo.Address, value, editingComponentInfo.Component.Data.OutputCount), null);
            }
        }
    }
}