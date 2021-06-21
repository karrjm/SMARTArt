using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     ButtonElement class.
    ///     based on Baum2.Editor.ButtonElement class.
    /// </summary>
    public class ButtonElement : GroupElement
    {
        protected readonly Dictionary<string, object> ButtonJson;

        public ButtonElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            ButtonJson = json.GetDic("button");
        }

        public override void Render(ref GameObject targetObject,
            RenderContext renderContext,
            GameObject parentObject)
        {
            GetOrCreateSelfObject(renderContext, ref targetObject, parentObject);

            var children = RenderChildren(renderContext, targetObject);
            var deleteObjects = new Dictionary<GameObject, bool>();

            var button = ElementUtil.GetOrAddComponent<Button>(targetObject);


            GameObject targetImageObject = null;
            if (ButtonJson != null)
            {
                var targetGraphics = ButtonJson.GetArray("target_graphic").Select(o => o.ToString()).ToList();
                var targetImage =
                    ElementUtil.FindComponentByNames<Image>(children, targetGraphics);
                if (targetImage != null)
                {
                    button.targetGraphic = targetImage;
                    targetImageObject = targetImage.gameObject;
                    targetImageObject.SetActive(true);
                }

                // すげ替え画像を探し、設定する
                // 見つかった場合は
                // その画像オブジェクトを削除し
                // SpriteSwapに設定する
                var spriteStateJson = ButtonJson.GetDic("sprite_state");
                if (spriteStateJson != null)
                {
                    var spriteState =
                        ElementUtil.CreateSpriteState(spriteStateJson, RenderedChildren, ref deleteObjects);
                    button.spriteState = spriteState;
                }

                // transitionの設定が明記してある場合は上書き設定する
                var type = ButtonJson.Get("transition");
                switch (type)
                {
                    case "sprite-swap":
                        button.transition = Selectable.Transition.SpriteSwap;
                        break;
                    case "color-tint":
                        button.transition = Selectable.Transition.ColorTint;
                        break;
                    case "animation":
                        button.transition = Selectable.Transition.Animation;
                        break;
                    case "none":
                        button.transition = Selectable.Transition.None;
                        break;
                }
            }

            foreach (var keyValuePair in deleteObjects)
                // 他の状態にtargetImageの画像が使われている可能性もあるためチェックする
                if (keyValuePair.Key != targetImageObject)
                    Object.DestroyImmediate(keyValuePair.Key);

            // TargetGraphicが設定されなかった場合
            if (button.targetGraphic == null)
            {
                // 子供からImage持ちを探す

                var image = targetObject.GetComponentInChildren<Image>();
                if (image != null)
                    // アクティブにする
                    image.gameObject.SetActive(true);
                else
                    // componentでないか探す
                    image = targetObject.GetComponent<Image>();

                button.targetGraphic = image;
            }

            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);
            ElementUtil.SetupLayoutElement(targetObject, LayoutElementJson);
            ElementUtil.SetupComponents(targetObject, ComponentsJson);
        }
    }
}