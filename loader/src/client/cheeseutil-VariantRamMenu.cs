using System;
using System.Collections.Generic;
using System.Linq;
using LogicAPI.Data.BuildingRequests;
using LogicSettings;
using LogicUI.MenuParts;
using LogicWorld.BuildingManagement;
using UnityEngine;
using LogicWorld.UI;
using LogicLog;

namespace CheeseUtilMod.UI {
    public class VariantRamMenu : EditComponentMenu {
        protected override void OnStartEditing() {
            this.InputCountSlider.SetValueWithoutNotify((float)base.ComponentsBeingEdited.First<EditingComponentInfo>().Component.Data.InputCount - 10f);
        }

        public override void Initialize()
        {
            base.Initialize();
            Logger.Info("HEY!!!!!!!");
            this.InputCountSlider.SliderInterval = 1f;
            this.InputCountSlider.Min = 1f;
            this.InputCountSlider.Max = 24f; //Up to 24 pegs for addressing
            this.InputCountSlider.OnValueChangedInt += this.InputCountSlider_OnValueChangedInt;
        }

        private void InputCountSlider_OnValueChangedInt(int value) {
            foreach (var component in base.ComponentsBeingEdited)
            {
                BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(component.Address, value + 10, 1), null);
            }
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited() {
            yield return "CheeseUtilMod.VariantRam";
            yield break;
        }

        [SerializeField]
        private InputSlider InputCountSlider;

        private static ILogicLogger Logger = LogicLogger.For("Cheese Util");
    }
}