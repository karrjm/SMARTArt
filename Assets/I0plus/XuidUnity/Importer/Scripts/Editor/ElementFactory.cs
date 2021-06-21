using System;
using System.Collections.Generic;
using UnityEngine;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     ElementFactory class.
    ///     based on Baum2.Editor.ElementFactory class.
    /// </summary>
    public static class ElementFactory
    {
        private static readonly Dictionary<string, Func<Dictionary<string, object>, Element, Element>> Generator =
            new Dictionary<string, Func<Dictionary<string, object>, Element, Element>>
            {
                {"Image", (d, p) => new ImageElement(d, p)},
                {"Mask", (d, p) => new MaskElement(d, p)},
                {"Group", (d, p) => new GroupElement(d, p)},
                {"Text", (d, p) => new TextElement(d, p)},
                {"Button", (d, p) => new ButtonElement(d, p)},
                {"Slider", (d, p) => new SliderElement(d, p)},
                {"Scrollbar", (d, p) => new ScrollbarElement(d, p)},
                {"Toggle", (d, p) => new ToggleElement(d, p)},
                {"Input", (d, p) => new InputElement(d, p)},
#if TMP_PRESENT
                {"TextMeshPro", (d, p) => new TextMeshProElement(d, p)},
#else
                {"TextMeshPro", (d, p) => new TextElement(d, p)},
#endif
                {"Rect", (d, p) => new RectElement(d, p)},
                {"Instance", (d, p) => new InstanceElement(d, p)}
            };

        public static Element Generate(Dictionary<string, object> json, Element parent)
        {
            var type = json.Get("type");
            if (type == null || !Generator.ContainsKey(type))
            {
                Debug.LogError("[{Importer.Name}] Unknown type: " + type);
                return null;
            }


            // Debug.Log($"generate {type}");

            return Generator[type](json, parent);
        }
    }
}