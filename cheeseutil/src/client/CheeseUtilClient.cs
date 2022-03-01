using LogicAPI.Client;
using LogicLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LogicWorld;
using LICC;
namespace CheeseUtilMod {
    public class CheeseUtilClient : ClientMod {
        static CheeseUtilClient() {
        }
        protected override void Initialize() {
            Logger.Info("Cheese Util Mod - Client Loaded");
        }
        [Command("loadram", Description = "Loads a file into any RAM components with the L pin active, does not clear out memory after the end of the file")]
        public static void loadram(string file)
        {
            SceneAndNetworkManager.RunCommandOnServer($"loadram {file}");
        }
    }
}