using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     GroupElement class.
    ///     based on Baum2.Editor.GroupElement class.
    /// </summary>
    public class GroupElement : Element
    {
        protected readonly Dictionary<string, object> CanvasGroup;
        protected readonly List<object> ComponentsJson;
        protected readonly Dictionary<string, object> ContentSizeFitterJson;

        protected readonly List<Element> Elements;
        protected readonly string FillColorJson;
        protected readonly Dictionary<string, object> LayoutGroupJson;
        protected readonly Dictionary<string, object> MaskJson;
        protected readonly Dictionary<string, object> ScrollRectJson;
        protected bool? RectMask2D;

        public GroupElement(Dictionary<string, object> json, Element parent, bool resetStretch = false) : base(json,
            parent)
        {
            Elements = new List<Element>();
            var jsonElements = json.Get<List<object>>("elements");
            if (jsonElements != null)
                foreach (var jsonElement in jsonElements)
                {
                    var elem = ElementFactory.Generate(jsonElement as Dictionary<string, object>, this);
                    if (elem != null)
                        Elements.Add(elem);
                }

            Elements.Reverse();
            CanvasGroup = json.GetDic("canvas_group");
            LayoutGroupJson = json.GetDic("layout_group");
            ContentSizeFitterJson = json.GetDic("content_size_fitter");
            MaskJson = json.GetDic("mask");
            RectMask2D = json.GetBool("rect_mask_2d");
            ScrollRectJson = json.GetDic("scroll_rect");
            FillColorJson = json.Get("fill_color");
            ComponentsJson = json.Get<List<object>>("components");
        }

        public List<Tuple<GameObject, Element>> RenderedChildren { get; private set; }

        public override void Render( ref GameObject targetObject,
            RenderContext renderContext,
            GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);
            
            var graphic = targetObject.GetComponent<Graphic>() as Component;
            if (graphic != null)
            {
                // Groupには、描画コンポーネントは必要ない　Graphicコンポーネントがある 削除する
                // オフにする手もあるかもだが、のちのSetupComponentsとぶつかる可能性あり
                // Debug.LogWarning($"[{Importer.NAME}] {graphic.gameObject.name}: Graphic Component change to {typeof(T)}.", go);
                Object.DestroyImmediate(graphic);
            }
            var canvasRenderer = targetObject.GetComponent<CanvasRenderer>();
            if (canvasRenderer != null)
            {
                Object.DestroyImmediate(canvasRenderer);
            }

            RenderedChildren = RenderChildren(renderContext, targetObject);
            ElementUtil.SetupCanvasGroup(targetObject, CanvasGroup);
            ElementUtil.SetupChildImageComponent(targetObject, RenderedChildren);
            ElementUtil.SetupFillColor(targetObject, FillColorJson);
            ElementUtil.SetupContentSizeFitter(targetObject, ContentSizeFitterJson);
            ElementUtil.SetupLayoutGroup(targetObject, LayoutGroupJson);
            ElementUtil.SetupLayoutElement(targetObject, LayoutElementJson);
            ElementUtil.SetupComponents(targetObject, ComponentsJson);
            ElementUtil.SetupMask(targetObject, MaskJson);
            ElementUtil.SetupRectMask2D(targetObject, RectMask2D);
            // ScrollRectを設定した時点で、はみでたContentがアジャストされる　PivotがViewport内に入っていればOK
            GameObject goContent = null;
            if (RenderedChildren.Count > 0) goContent = RenderedChildren[0].Item1;
            ElementUtil.SetupScrollRect(targetObject, goContent, ScrollRectJson);
            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);

        }

        public override void RenderPass2(List<Tuple<GameObject, Element>> selfAndSiblings)
        {
            var self = selfAndSiblings.Find(tuple => tuple.Item2 == this);
            var scrollRect = self.Item1.GetComponent<ScrollRect>();
            if (scrollRect)
            {
                // scrollRectをもっているなら、ScrollBarを探してみる
                // TODO: 探すスクロールバーの名前は設定している
                var scrollbars = selfAndSiblings
                    .Where(goElem => goElem.Item2 is ScrollbarElement) // 兄弟の中からScrollbarを探す
                    .Select(goElem => goElem.Item1.GetComponent<Scrollbar>()) // ScrollbarコンポーネントをSelect
                    .ToList();
                scrollbars.ForEach(scrollbar =>
                {
                    switch (scrollbar.direction)
                    {
                        case Scrollbar.Direction.LeftToRight:
                            scrollRect.horizontalScrollbar = scrollbar;
                            break;
                        case Scrollbar.Direction.RightToLeft:
                            scrollRect.horizontalScrollbar = scrollbar;
                            break;
                        case Scrollbar.Direction.BottomToTop:
                            scrollRect.verticalScrollbar = scrollbar;
                            break;
                        case Scrollbar.Direction.TopToBottom:
                            scrollRect.verticalScrollbar = scrollbar;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            }
        }

        protected List<Tuple<GameObject, Element>> RenderChildren(RenderContext renderContext, GameObject parent,
            Action<GameObject, Element> callback = null)
        {
            var list = new List<Tuple<GameObject, Element>>();
            foreach (var element in Elements)
            {
                GameObject go = null;
                element.Render(ref go, renderContext, parent);
                // if (go.transform.parent != parent.transform) Debug.Log("生成されたObjectの親は設定した親になっていない:" + go.name);
                list.Add(new Tuple<GameObject, Element>(go, element));
                if (callback != null) callback.Invoke(go, element);
            }

            foreach (var element in Elements) element.RenderPass2(list);

            RenderedChildren = list;
            return list;
        }
    }
}