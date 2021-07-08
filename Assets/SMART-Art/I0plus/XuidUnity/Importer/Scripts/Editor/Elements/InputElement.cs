using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     InputElement class.
    ///     based on Baum2.Editor.InputElement class.
    /// </summary>
    public class InputElement : GroupElement
    {
        protected readonly Dictionary<string, object> InputJson;

        public InputElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            InputJson = json.GetDic("input");
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            var children = RenderChildren(renderContext, targetObject);

            var inputField = ElementUtil.GetOrAddComponent<InputField>(targetObject);
            inputField.transition = Selectable.Transition.None;
            if (InputJson != null)
            {
                var textComponent =
                    ElementUtil.FindComponentByClassName<Text>(children, InputJson.Get("text_component_class"));
                if (textComponent != null)
                {
                    inputField.textComponent = textComponent;
                    if (!textComponent.gameObject.activeSelf)
                    {
                        textComponent.gameObject.SetActive(true);
                    }
                }

                var placeholderText =
                    ElementUtil.FindComponentByClassName<Graphic>(children, InputJson.Get("placeholder_class"));
                if (placeholderText != null) inputField.placeholder = placeholderText;

                var targetGraphic =
                    ElementUtil.FindComponentByClassName<Text>(children, InputJson.Get("target_graphic_class"));
                if (targetGraphic != null) inputField.targetGraphic = targetGraphic;
            }

            ElementUtil.SetLayer(targetObject, Layer);
            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);
        }
    }
}