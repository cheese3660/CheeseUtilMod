using LogicAPI.Client;
using LogicLog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CheeseUtilMod {
    public class CheeseUtilClient : ClientMod {
        static CheeseUtilClient() {
            SceneManager.sceneLoaded += generateMenus;
        }

        static void generateMenus(Scene scene, LoadSceneMode mode) {
            GameObject baseUIComponent = new GameObject("UI Component Made From Mod");
            baseUIComponent.AddComponent<Canvas>();
            Canvas canvas = baseUIComponent.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            baseUIComponent.AddComponent<CanvasScaler>();
            baseUIComponent.AddComponent<GraphicRaycaster>();
            
            GameObject textObject = new GameObject();
            textObject.transform.parent = baseUIComponent.transform;
            textObject.name = "TEST!";

            Text text = textObject.AddComponent<Text>();
            text.text = "TEST!!!";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 100;

            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(400, 200);
        }
        protected override void Initialize() {
            Logger.Info("Cheese Util Mod Loaded - Client");
        }
    }
}