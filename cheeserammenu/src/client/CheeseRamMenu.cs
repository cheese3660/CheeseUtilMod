using LogicAPI.Client;
using LogicLog;
using HarmonyLib;
using System.Reflection;
namespace CheeseRamMenu.Client
{
    public class CheeseRamMenu : ClientMod
    {
        public static ILogicLogger logger;

        protected override void Initialize()
        {
            RamMenuSingleton.init();
        }

    }
}
