using System;
using EccsLogicWorldAPI.Client.Hooks;
using LogicAPI.Client;
using LogicWorld;

namespace CheeseRamMenu.Client
{
    public class CheeseRamMenu : ClientMod
    {
        protected override void Initialize()
        {
            WorldHook.worldLoading += () => {
                try
                {
                    RamMenu.init();
                }
                catch(Exception e)
                {
                    Logger.Error("Could not initialize CheeseRamMenu");
                    SceneAndNetworkManager.TriggerErrorScreen(e);
                }
            };
        }
    }
}
