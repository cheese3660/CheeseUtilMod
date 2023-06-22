using TMPro;
using UnityEngine;
using EccsWindowHelper.Client;
using LogicUI.MenuParts;
using EccsWindowHelper.Client.Prefabs;

namespace CheeseRamMenu.Client.Prefabs
{
    public static class NamedSliderPrefab
    {
		public static GameObject generateNamedSlider(string nameKey, int width, int titleWidth, int interactableWidth, int heightModifier, out InputSlider slider)
        {
            GameObject otherContent = InputSliderPrefab.generateInputSlider();
			slider = otherContent.GetComponent<InputSlider>();
			GameObject gameObject = WindowHelper.makeGameObject("CRM Named Sldier");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(0, 0);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(width, 51.9f * heightModifier);
			}
			gameObject.AddComponent<CanvasRenderer>();

			//Content:
			constructSettingsTitle(gameObject, titleWidth,nameKey);
			constructSettingsArea(gameObject, otherContent, interactableWidth, heightModifier);

			gameObject.SetActive(true);
			return gameObject;
		}
		private static void constructSettingsTitle(GameObject parent, int width, string key)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Named Slider Title");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(0, 1);
				rectTransform.pivot = new Vector2(0.0f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(width, 0f);
			}
			gameObject.AddComponent<CanvasRenderer>();

			TextMeshProUGUI text = WindowHelper.addTMP(gameObject);
			text.horizontalAlignment = HorizontalAlignmentOptions.Right;

			gameObject.addLocalizedTextMesh().SetLocalizationKey(key);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}

		private static void constructSettingsArea(GameObject parent, GameObject otherContent, int width, int heightMultiplier)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Named Slider Area");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(1, 0.5f * (heightMultiplier - 1) / heightMultiplier);
				rectTransform.anchorMax = new Vector2(1, 0.5f * (heightMultiplier + 1) / heightMultiplier);
				rectTransform.pivot = new Vector2(1.0f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(width, 0f);
			}

			gameObject.addChild(otherContent);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}

	}
}
