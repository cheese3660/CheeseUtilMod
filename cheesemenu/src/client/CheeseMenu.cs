using LogicAPI.Client;
using LogicLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LogicWorld;
using LogicWorld.UI;
using LICC;
using System;
using System.Collections.Generic;
using LogicAPI.Data;
using LogicAPI.Data.BuildingRequests;
using LogicWorld.Building.Overhaul;
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
namespace CheeseMenu.Client
{
    public class CheeseMenu : ClientMod
    {
		public static ILogicLogger logger;
        public static List<ICheeseMenu> allmenus = new List<ICheeseMenu>();

        public static void RegisterMenu(ICheeseMenu menu)
        {
            allmenus.Add(menu);
        }

		protected override void Initialize()
		{
			logger = Logger;
            var type = typeof(ChairMenu).Assembly.GetType("LogicWorld.UI.ComponentMenusManager");
            var meth = type.GetMethod("Reset", BindingFlags.Static | BindingFlags.NonPublic);
            var postfix = new HarmonyMethod(typeof(CheeseMenu).GetMethod("ReinitializeMenus", BindingFlags.Static | BindingFlags.Public));
            new Harmony("CheeseMenuPatcher").Patch(meth, null, postfix);
        }

        public static void ReinitializeMenus()
        {
            logger.Info("REINITIALIZING MENUS");
            var type = typeof(ICheeseMenu);
            foreach (var menu in allmenus)
            {
                menu.gameObject.GetComponent<EditComponentMenu>().Initialize();
            }
        }

	}
}
