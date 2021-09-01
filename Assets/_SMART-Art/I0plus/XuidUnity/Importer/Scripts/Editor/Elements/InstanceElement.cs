using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace I0plus.XduiUnity.Importer.Editor
{
    public class InstanceElement : Element
    {
        private readonly string master;
        private Dictionary<string, object> instanceRootJson;

        public InstanceElement(Dictionary<string, object> json, Element parent) : base(json, parent)
        {
            master = json.Get("master");
            instanceRootJson = json;
        }

        public override void Render(ref GameObject targetObject, RenderContext renderContext, GameObject parentObject)
        {
            targetObject = renderContext.OccupyObject(Guid, name, parentObject);
            if (targetObject != null)
                if (!PrefabUtility.IsAnyPrefabInstanceRoot(targetObject))
                {
                    Debug.Log($"CreateInstanceだが、プレハブのインスタンスではない:{targetObject.name}");
                    // 使われないように xd guidを書き換え
                    var xdGuidComponent = targetObject.GetComponent<XdGuid>();
                    if (xdGuidComponent != null) xdGuidComponent.guid = "old:" + xdGuidComponent.guid;
                    // Freeに戻す
                    renderContext.AddFreeObject(targetObject);
                    // 見つからなかったことにする
                    targetObject = null;
                }

            if (targetObject == null)
            {
                //　見つからなかった場合は プレハブの生成をする
                var path = EditorUtil.GetOutputPrefabsFolderAssetPath() + "/" + master + ".prefab";
                var prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefabObject == null)
                {
                    // 読み込むPrefabが存在しなかった
                    // ダミーのPrefabを作成する
                    Debug.LogWarning($"[{Importer.NAME}] {master}.prefab not found.　Create a temporary prefab. path:{path}");
                    var tempObject = new GameObject("temporary object");
                    tempObject.AddComponent<RectTransform>();
                    // ダミーとわかるようにmagentaイメージを置く -> non-destructiive importで、このイメージを採用してしまうためコメントアウト
                    // var image = tempObject.AddComponent<Image>();
                    // image.color = Color.magenta;
                    // フォルダの用意
                    EditorUtil.CreateFolder(path.Substring(0, path.LastIndexOf('/')));
                    // prefabの作成
                    var savedAsset = PrefabUtility.SaveAsPrefabAsset(tempObject, path);
                    AssetDatabase.Refresh();
                    // Debug.Log($"[{Importer.Name}] Created temporary prefab. {path}", savedAsset);
                    Object.DestroyImmediate(tempObject);
                    prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }

                // 仮のプレハブをセットする
                targetObject = PrefabUtility.InstantiatePrefab(prefabObject) as GameObject;
            }

            var rect = ElementUtil.GetOrAddComponent<RectTransform>(targetObject);
            if (parentObject != null)
            {
                /*
                if (PrefabUtility.IsPartOfPrefabInstance(parentObject))
                {
                    Debug.Log($"parent part of prefab:{parentObject.name}");
                }
                */
                rect.SetParent(null);
                rect.SetParent(parentObject.transform);
            }

            if (renderContext.OptionAddXdGuidComponent)
            {
                // PrehabのXdGuidにインスタンスGuidを書き込む
                // 注意点：
                // 仮プレハブに書き込んだ場合、正規のプレハブが生成されたとき、
                // インスタンスGUIDがPrefabGuidにもどってしまうかもしれない
                var xdGuid = ElementUtil.GetOrAddComponent<XdGuid>(targetObject);
                xdGuid.guid = Guid;
            }

            targetObject.name = Name;
            /*
            ElementUtil.SetLayer(targetObject, Layer);
            ElementUtil.SetupRectTransform(targetObject, RectTransformJson);
            if (Active != null) targetObject.SetActive(Active.Value);
            ElementUtil.SetupLayoutElement(targetObject, LayoutElementJson);
            */

            // Nested Prefab Creator
            var prefabCreator = new PrefabCreator(null);

            // Groupに変更しないと再帰が延々と繰り返される
            instanceRootJson["type"] = "Group";

            var subRenderContext = new RenderContext(renderContext.SpriteOutputFolderAssetPath,
                renderContext.FontFolderAssetPath, targetObject);
            prefabCreator.Create(ref targetObject, subRenderContext, instanceRootJson);
        }
    }
}