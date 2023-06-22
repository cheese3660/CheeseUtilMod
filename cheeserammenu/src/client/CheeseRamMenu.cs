using LogicAPI.Client;
using LogicLog;

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
