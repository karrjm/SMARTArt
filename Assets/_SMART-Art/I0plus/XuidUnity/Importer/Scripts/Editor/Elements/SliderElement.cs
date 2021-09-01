using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     SliderElement class.
    ///     based on Baum2.Editor.SliderElement class.
    /// </summary>
    public sealed class SliderElement : GroupElement
    {
        private readonly Dictionary<string, object> _sliderJson;

        public SliderElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            _sliderJson = json.GetDic("slider");
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            var slider = ElementUtil.GetOrAddComponent<Slider>(targetObject);

            var children = RenderChildren(renderContext, targetObject);

            var direction = _sliderJson.Get("direction");
            if (direction != null)
            {
                direction = direction.ToLower();
                switch (direction)
                {
                    case "left-to-right":
                    case "ltr":
                    case "x":
                        slider.direction = Slider.Direction.LeftToRight;
                        break;
                    case "right-to-left":
                    case "rtl":
                        slider.direction = Slider.Direction.RightToLeft;
                        break;
                    case "bottom-to-top":
                    case "btt":
                    case "y":
                        slider.direction = Slider.Direction.BottomToTop;
                        break;
                    case "top-to-bottom":
                    case "ttb":
                        slider.direction = Slider.Direction.TopToBottom;
                        break;
                }
            }

            slider.transition = Selectable.Transition.None;
            //slider.interactable = false;

            var fillRect =
                ElementUtil.FindComponentByClassName<RectTransform>(children, _sliderJson.Get("fill_rect_name"));
            if (fillRect != null)
            {
                slider.value = slider.maxValue;
                slider.fillRect = fillRect;
            }

            var handleRect =
                ElementUtil.FindComponentByClassName<RectTransform>(children, _sliderJson.Get("handle_rect_name"));
            if (handleRect != null)
            {
                slider.handleRect = handleRect;
                slider.interactable = true;
            }

            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);
        }
    }
}