using LogicAPI.Client;
using LogicLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CheeseUtilMod {
    public class CheeseUtilClient : ClientMod {
        static CheeseUtilClient() {
        }
        protected override void Initialize() {
            Logger.Info("Cheese Util Mod - Client Loaded");
        }
    }
}