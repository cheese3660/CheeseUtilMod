using LogicUI.MenuParts.Toggles;
using LogicUI.Palettes;
using ThisOtherThing.UI;
using ThisOtherThing.UI.Shapes;
using ThisOtherThing.UI.ShapeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EccsWindowHelper.Client;

namespace CheeseRamMenu.Client.Prefabs
{
    public static class Button
    {
        public static GameObject generateButton()
        {
            var gameObject = WindowHelper.makeGameObject("CheeseWindowComponents: Button");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(0, 1);
				rectTransform.pivot = new Vector2(0, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(300f, 100f);
            }
			Rectangle rectangle = gameObject.AddComponent<Rectangle>();
			rectangle.ShapeProperties = new GeoUtils.OutlineShapeProperties()
			{
				DrawFillShadow = false,
				DrawOutline = true,
			};
			rectangle.RoundedProperties = new RoundedRects.RoundedProperties()
			{
				Type = RoundedRects.RoundedProperties.RoundedType.Uniform,
				ResolutionMode = RoundedRects.RoundedProperties.ResolutionType.Uniform,
			};

			gameObject.addPaletteRectangleOutline(PaletteColor.Tertiary);
			
			gameObject.addHoverButton().SetPaletteColor(PaletteColor.Accent);
			generateText(gameObject);
			gameObject.SetActive(true);
            return gameObject;
        }
		static void generateText(GameObject parent)
        {
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Button Text");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(0f, 0f);
			}
			gameObject.AddComponent<CanvasRenderer>();
			TextMeshProUGUI txt = WindowHelper.addTMP(gameObject);

			gameObject.addLocalizedTextMesh().SetLocalizationKey("cheeserammenu.FileLoad");
			gameObject.addPaletteGraphic(PaletteColor.Text_Primary);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}
    }
}
