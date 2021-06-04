using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     RootElement class.
    ///     based on Baum2.Editor.ScrollbarElement class.
    /// </summary>
    public sealed class ScrollbarElement : GroupElement
    {
        private readonly Dictionary<string, object> _scrollbarJson;

        public ScrollbarElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            _scrollbarJson = json.GetDic("scrollbar");
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);

            var children = RenderChildren(renderContext, targetObject);
            ElementUtil.SetupChildImageComponent(targetObject, children);

            // DotsScrollbarかどうかの判定に、Toggleがあるかどうかを確認する
            var toggleChild = children.Find(child => child.Item2 is ToggleElement);
            Scrollbar scrollbar;
            if (toggleChild == null)
            {
                scrollbar = ElementUtil.GetOrAddComponent<Scrollbar>(targetObject);
            }
            else
            {
                // DotScrollbarとなる
                var dotScrollbar = targetObject.AddComponent<DotsScrollbar>();
                dotScrollbar.isAutoLayoutEnableOnEditMode = false;
                dotScrollbar.DotContainer = targetObject.GetComponent<RectTransform>();
                dotScrollbar.DotPrefab = toggleChild.Item1.GetComponent<Toggle>();
                // Toggleボタンの並びレイアウト
                ElementUtil.SetupLayoutGroup(targetObject, LayoutGroupJson);
                dotScrollbar.size = 1; // sizeを1にすることで、Toggleが複数Cloneされることをふせぐ
                scrollbar = dotScrollbar;
            }

            var direction = _scrollbarJson.Get("direction");
            if (direction != null)
                switch (direction)
                {
                    case "left-to-right":
                    case "ltr":
                    case "x":
                        scrollbar.direction = Scrollbar.Direction.LeftToRight;
                        break;
                    case "right-to-left":
                    case "rtl":
                        scrollbar.direction = Scrollbar.Direction.RightToLeft;
                        break;
                    case "bottom-to-top":
                    case "btt":
                    case "y":
                        scrollbar.direction = Scrollbar.Direction.BottomToTop;
                        break;
                    case "top-to-bottom":
                    case "ttb":
                        scrollbar.direction = Scrollbar.Direction.TopToBottom;
                        break;
                }

            var handleClassName = _scrollbarJson.Get("handle_class");
            if (handleClassName != null)
            {
                var found = children.Find(child => child.Item2.HasParsedName(handleClassName));
                if (found != null) scrollbar.handleRect = found.Item1.GetComponent<RectTransform>();
            }

            ElementUtil.SetupContentSizeFitter(targetObject, ContentSizeFitterJson);
        }
    }
}