using LogicAPI.Client;

namespace CheeseRamMenu.Client
{
    public class CheeseRamMenu : ClientMod
    {
        protected override void Initialize()
        {
            RamMenuSingleton.init();
        }
    }
}
