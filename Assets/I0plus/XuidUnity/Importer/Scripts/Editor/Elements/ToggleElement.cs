using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     TextMeshProElement class.
    /// </summary>
    public sealed class ToggleElement : GroupElement
    {
        private readonly Dictionary<string, object> _toggleJson;

        public ToggleElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            _toggleJson = json.GetDic("toggle");
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            var children = RenderChildren(renderContext, targetObject);

            var toggle = ElementUtil.GetOrAddComponent<Toggle>(targetObject);

            // トグルグループ名
            var group = _toggleJson.Get("group");
            if (group != null)
            {
                var toggleToRadio = ElementUtil.GetOrAddComponent<ToggleToRadio>(targetObject);
                toggleToRadio.GroupName = group;
            }

            GameObject targetImageObject = null;
            var targetGraphics = _toggleJson.GetArray("target_graphic").Select(o => o.ToString()).ToList();
            var targetImage =
                ElementUtil.FindComponentByNames<Image>(children, targetGraphics);
            if (targetImage != null)
            {
                toggle.targetGraphic = targetImage;
                targetImageObject = targetImage.gameObject;
                //TODO: 強制的にActiveにする
                targetImageObject.SetActive(true);
            }

            // ON graphic
            var onGraphics = _toggleJson.GetArray("on_graphic").Select(o => o.ToString()).ToList();
            var graphicImage = ElementUtil.FindComponentByNames<Image>(children, onGraphics);
            if (graphicImage != null)
            {
                toggle.graphic = graphicImage;
                if (graphicImage.gameObject.activeSelf)
                    toggle.isOn = true;
                else
                    //TODO: 強制的にActiveにする
                    graphicImage.gameObject.SetActive(true);
            }

            // ON/OFF が画像の入れ替えとして動作するコンポーネント
            var graphicSwap = _toggleJson.GetBool("graphic_swap");
            if (graphicSwap != null && graphicSwap.Value)
                ElementUtil.GetOrAddComponent<ToggleGraphicSwap>(targetObject);

            var deleteObjects = new Dictionary<GameObject, bool>();
            var spriteStateJson = _toggleJson.GetDic("sprite_state");
            if (spriteStateJson != null)
            {
                var spriteState = ElementUtil.CreateSpriteState(spriteStateJson, RenderedChildren, ref deleteObjects);
                toggle.spriteState = spriteState;
            }

            foreach (var keyValuePair in deleteObjects)
                // 他の状態にtargetImageの画像が使われている可能性もあるためチェックする
                if (keyValuePair.Key != targetImageObject)
                    Object.DestroyImmediate(keyValuePair.Key);


            ElementUtil.SetupLayoutElement(targetObject, LayoutElementJson);
            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);
        }
    }
}