using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     Element class.
    ///     based on Baum2.Editor.Element class.
    /// </summary>
    public abstract class Element
    {
        protected readonly string Layer;
        protected readonly Dictionary<string, object> LayoutElementJson;
        protected readonly List<object> ParsedNames;

        protected readonly Dictionary<string, object> RectTransformJson;
        protected bool? Active;
        protected string Guid;

        protected string name;
        protected Element Parent;

        protected Element(Dictionary<string, object> json, Element parent)
        {
            Parent = parent;
            Guid = json.Get("guid");
            name = json.Get("name");
            //Debug.Log($"parsing {name}");
            Active = json.GetBool("active");
            Layer = json.Get("layer");
            ParsedNames = json.Get<List<object>>("parsed_names");

            RectTransformJson = json.GetDic("rect_transform");
            LayoutElementJson = json.GetDic("layout_element");
        }

        public string Name => name;

        public abstract void Render(ref GameObject targetObject, RenderContext renderContext,
            GameObject parentObject);

        public virtual void RenderPass2(List<Tuple<GameObject, Element>> selfAndSiblings)
        {
        }

        public bool HasParsedName(string parsedName)
        {
            if (ParsedNames == null || ParsedNames.Count == 0) return false;
            var found = ParsedNames.Find(s => (string) s == parsedName);
            return found != null;
        }

        /*
        protected void CreateUiGameObject(RenderContext renderContext, [CanBeNull] ref GameObject selfObject, GameObject parentObject)
        {
            selfObject = new GameObject(name);
            selfObject.AddComponent<RectTransform>();
            ElementUtil.SetLayer(selfObject, Layer);
            if (Active != null) selfObject.SetActive(Active.Value);
        }
        */

        /// <summary>
        ///     Objectの生成か再利用　親子関係の設定、Layer、Activeの設定
        /// </summary>
        /// <param name="renderContext"></param>
        /// <param name="selfObject"></param>
        /// <param name="parentObject"></param>
        /// <returns></returns>
        protected void GetOrCreateSelfObject(RenderContext renderContext, ref GameObject selfObject,
            GameObject parentObject)
        {
            // 指定のオブジェクトがある場合は生成・取得せずそのまま使用する
            if (selfObject == null)
                selfObject = ElementUtil.GetOrCreateGameObject(renderContext, Guid, name, parentObject);

            var rect = ElementUtil.GetOrAddComponent<RectTransform>(selfObject);
            if (parentObject)
            {
                var nearestPrefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(selfObject);
                if (nearestPrefabInstanceRoot == null)
                {
                    // 子供の並び順を整えるため、親からはずしまたくっつける
                    rect.SetParent(null);
                    rect.SetParent(parentObject.transform);
                }

                if (rect.parent == null)
                {
                    Debug.LogError("使用されないオブジェクトが作成された");
                }
            }

            if (renderContext.OptionAddXdGuidComponent) ElementUtil.SetGuid(selfObject, Guid);
            ElementUtil.SetActive(selfObject, Active);
            ElementUtil.SetLayer(selfObject, Layer);
        }
    }
}