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
    public static class TextInput
    {

		public static GameObject constructTextInput()
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Text Field");

			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(0, 1);
				rectTransform.pivot = new Vector2(0, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(1000f, 0);
			}

			gameObject.AddComponent<CanvasRenderer>();

			Rectangle rectangle = gameObject.AddComponent<Rectangle>();
			rectangle.ShapeProperties = new GeoUtils.OutlineShapeProperties()
			{
				DrawFillShadow = false, //Never draw shadows.
				DrawFill = true,
				DrawOutline = true,
			};
			rectangle.RoundedProperties = new RoundedRects.RoundedProperties()
			{
				Type = RoundedRects.RoundedProperties.RoundedType.Uniform,
				ResolutionMode = RoundedRects.RoundedProperties.ResolutionType.Uniform,
				UniformRadius = 10f,
				UniformResolution = new GeoUtils.RoundingProperties()
				{
					ResolutionMaxDistance = 3f,
				},
			};

			gameObject.addPaletteGraphic(PaletteColor.InputField);
			gameObject.addPaletteRectangleOutline(PaletteColor.Tertiary);

			constructInputTextFieldTextArea(
				gameObject,
				out RectTransform textViewport,
				out TMP_Text textComponent,
				out Graphic placeholder
			);

			TMP_InputField inputField = gameObject.AddComponent<TMP_InputField>();
			inputField.textViewport = textViewport;
			inputField.textComponent = textComponent;
			inputField.placeholder = placeholder;

			gameObject.addInputFieldSettingsApplier();
			gameObject.addPaletteInputFieldSelection(PaletteColor.Accent, 165);
			gameObject.addMakeInputFieldTabbable();

			gameObject.SetActive(true);
			return gameObject;
		}

		private static void constructInputTextFieldTextArea(GameObject parent, out RectTransform textViewport, out TMP_Text textComponent, out Graphic placeholder)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Text Field Text Area");

			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			textViewport = rectTransform;
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(-20, 0);
			}

			gameObject.AddComponent<RectMask2D>();

			constructInputTextFieldTextAreaCaret(gameObject);
			constructInputTextFieldTextAreaPlaceholder(gameObject, out placeholder);
			constructInputTextFieldTextAreaText(gameObject, out textComponent);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}

		private static void constructInputTextFieldTextAreaCaret(GameObject parent)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Text Field Text Area Caret");

			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(0, 0);
			}

			gameObject.AddComponent<CanvasRenderer>();

			gameObject.AddComponent<TMP_SelectionCaret>();

			gameObject.AddComponent<LayoutElement>().ignoreLayout = true;

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}

		private static void constructInputTextFieldTextAreaPlaceholder(GameObject parent, out Graphic placeholder)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Text Field Field Text Area Placeholder");

			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(0, 0);
			}

			gameObject.AddComponent<CanvasRenderer>();

			TextMeshProUGUI text = WindowHelper.addTMP(gameObject);
			placeholder = text;

			gameObject.addPaletteGraphic(PaletteColor.InputFieldText, 127);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}

		private static void constructInputTextFieldTextAreaText(GameObject parent, out TMP_Text textComponent)
		{
			GameObject gameObject = WindowHelper.makeGameObject("CRM: Text Field Field Text Area Text");

			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			{
				rectTransform.anchorMin = new Vector2(0, 0);
				rectTransform.anchorMax = new Vector2(1, 1);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0, 0);
				rectTransform.sizeDelta = new Vector2(0, 0);
			}

			gameObject.AddComponent<CanvasRenderer>();

			TextMeshProUGUI text = WindowHelper.addTMP(gameObject);
			textComponent = text;
			text.horizontalAlignment = HorizontalAlignmentOptions.Center;
			text.enableWordWrapping = false;
			text.fontSizeMin = 18;
			text.fontSizeMax = 72;
			text.enableAutoSizing = true;
			text.richText = false;
			text.margin = new Vector4(0.0f, -5.0f, 0.0f, -5.0f);

			gameObject.addPaletteGraphic(PaletteColor.InputFieldText);

			gameObject.SetActive(true);
			gameObject.setParent(parent);
		}
	}
}
