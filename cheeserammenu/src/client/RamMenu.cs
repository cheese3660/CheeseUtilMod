using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheeseMenu.Client;
using LogicAPI.Client;
using LogicLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LogicWorld;
using LogicWorld.UI;
using LICC;
using LogicAPI.Data;
using LogicAPI.Data.BuildingRequests;
using LogicWorld.Building.NewShit;
using LogicWorld.Interfaces;
using EccsWindowHelper.Client;
using EccsWindowHelper.Client.Prefabs;
using JimmysUnityUtilities;
using LogicLocalization;
using LogicUI.HoverTags;
using LogicUI.MenuParts.Toggles;
using LogicUI.MenuTypes;
using LogicUI.MenuTypes.ConfigurableMenus;
using HarmonyLib;
using System.Reflection;
using LogicWorld.GameStates;
using TMPro;
using LogicUI.MenuParts;
using CheeseUtilMod.Client;
using System.IO;

namespace CheeseRamMenu.Client
{
    //Based off of: https://github.com/Ecconia/Ecconia-LogicWorld-Mods
    public class RamMenuSingleton : ToggleableSingletonMenu<RamMenuSingleton>, ICheeseMenu
    {
        public static TMP_InputField inputField;
        public static HoverButton hbutton;

        public static void init()
        {
            GameObject contentPlane = constructContent();
            WindowBuilder wb = new WindowBuilder
            {
                x = 0,
                y = 100,
                w = 300,
                h = 200,
                isResizableX = true,
                isResizableY = true,
                rootName = "CheeseRamMenu",
                titleKey = "CheeseRamMenu.EditRamMenu",
                contentPlane = contentPlane,
                singletonClass = typeof(RamMenuSingleton),
            };
            var controller = wb.build();
            WindowBuilder.updateContentPlane(contentPlane);
            Instance.gameObject.AddComponent<RamMenu>().SetupListener();
            CheeseMenu.Client.CheeseMenu.RegisterMenu(Instance);
            OnMenuHidden += GameStateManager.TransitionBackToBuildingState;
        }
        private static GameObject constructContent()
        {
            GameObject gameObject = WindowHelper.makeGameObject("CRM: ContentPlane");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            {
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.sizeDelta = new Vector2(0, 0);
                rectTransform.anchoredPosition = new Vector2(0, 0);
            }

            ContentSizeFitter fitter = gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childForceExpandHeight = false;
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandWidth = false;
            verticalLayoutGroup.childControlWidth = false;
            verticalLayoutGroup.spacing = 20;
            //Now here is where we add the prefab children
            //Fixed height toggle thing
            GameObject heightThingy = WindowHelper.makeGameObject("CRM: Toggle Height");
            RectTransform rectTransform2 = heightThingy.AddComponent<RectTransform>();
            {
                rectTransform2.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform2.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform2.pivot = new Vector2(0.5f, 0.5f);
                rectTransform2.sizeDelta = new Vector2(100f, 51.9f);
                rectTransform2.anchoredPosition = new Vector2(0, 0);
            }
            var field = Prefabs.TextInput.constructTextInput();
            inputField = field.GetComponent<TMP_InputField>();
            heightThingy.addChild(field);
            heightThingy.SetActive(true);
            gameObject.addChild(heightThingy);
            var button = Prefabs.Button.generateButton();
            hbutton = button.GetComponent<HoverButton>();
            gameObject.addChild(button);
            gameObject.SetActive(true);
            return gameObject;
        }
    }
    public class RamMenu : EditComponentMenu
    {
        public void SetupListener()
        {
            RamMenuSingleton.hbutton.OnClickEnd += Hbutton_OnClickEnd;
        }

        private void Hbutton_OnClickEnd()
        {
            var loadable = (FileLoadable)(FirstComponentBeingEdited.ClientCode);
            var file = RamMenuSingleton.inputField.text;
            if (File.Exists(file))
            {
                var bs = File.ReadAllBytes(file);
                var lw = LConsole.BeginLine();
                loadable.Load(bs, lw, true);
                lw.End();
            } else
            {
                LConsole.WriteLine($"Unable to load file {file} as it does not exist");
            }
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return new string[] {
                "CheeseUtilMod.Ram4aX1b",
                "CheeseUtilMod.Ram8aX1b",
                "CheeseUtilMod.Ram16aX1b",
                "CheeseUtilMod.Ram4aX4b",
                "CheeseUtilMod.Ram8aX4b",
                "CheeseUtilMod.Ram16aX4b",
                "CheeseUtilMod.Ram4aX8b",
                "CheeseUtilMod.Ram8aX8b",
                "CheeseUtilMod.Ram16aX8b",
                "CheeseUtilMod.Ram4aX16b",
                "CheeseUtilMod.Ram8aX16b",
                "CheeseUtilMod.Ram16aX16b",
            };
        }
    }
}
