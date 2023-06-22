using LogicAPI.Client;
using LogicLog;
using LogicWorld.UI;
using System.Collections.Generic;
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
