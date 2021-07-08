using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     ViewportElement class.
    ///     GroupElementに統合した
    ///     削除予定
    /// </summary>
    public sealed class ViewportElement : GroupElement
    {
        private readonly Dictionary<string, object> _contentJson;
        private readonly Dictionary<string, object> _scrollRectJson;
        private Element _parentElement;

        public ViewportElement(Dictionary<string, object> json, Element parent) : base(json, parent, true)
        {
            _scrollRectJson = json.GetDic("scroll_rect");
            _contentJson = json.GetDic("content");
            _parentElement = parent;
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            ElementUtil.SetLayer(targetObject, Layer);
            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);

            // タッチイベントを取得するイメージコンポーネントになる
            ElementUtil.SetupFillColor(targetObject, FillColorJson);

            // コンテンツ部分を入れるコンテナ
            var goContent = new GameObject("$Content");
            ElementUtil.SetLayer(goContent, Layer); // Viewportと同じレイヤー
            var contentRect = ElementUtil.GetOrAddComponent<RectTransform>(goContent);
            goContent.transform.SetParent(targetObject.transform);

            if (_contentJson != null)
            {
                goContent.name = _contentJson.Get("name") ?? "";
                var rectJson = _contentJson.GetDic("rect_transform");
                if (rectJson != null) ElementUtil.SetupRectTransform(goContent, rectJson);

                var contentLayout = _contentJson.GetDic("layout");
                ElementUtil.SetupLayoutGroup(goContent, contentLayout);

                var contentSizeFitter = _contentJson.GetDic("content_size_fitter");
                ElementUtil.SetupContentSizeFitter(goContent, contentSizeFitter);
            }

            //Viewportのチャイルドはもとより、content向けのAnchor・Offsetを持っている
            RenderChildren(renderContext, goContent);

            ElementUtil.SetupRectMask2D(targetObject, RectMask2D);
            // ScrollRectを設定した時点ではみでたContentがアジャストされる　PivotがViewport内に入っていればOK
            ElementUtil.SetupScrollRect(targetObject, goContent, _scrollRectJson);
        }


        public override void RenderPass2(List<Tuple<GameObject, Element>> selfAndSiblings)
        {
            var self = selfAndSiblings.Find(tuple => tuple.Item2 == this);
            var scrollRect = self.Item1.GetComponent<ScrollRect>();
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
}