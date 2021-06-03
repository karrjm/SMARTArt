using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MiniJSON;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if TMP_PRESENT
using TMPro;

#endif

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     PrefabCreator class.
    ///     based on Baum2.Editor.PrefabCreator class.
    /// </summary>
    public sealed class PrefabCreator
    {
        private static readonly string[] Versions = {"0.6.0", "0.6.1"};
        private readonly List<GameObject> nestedPrefabs;

        /// <summary>
        /// </summary>
        /// <param name="spriteRootPath"></param>
        /// <param name="fontRootPath"></param>
        /// <param name="assetPath">フルパスでの指定 Unity Assetフォルダ外もよみこめる</param>
        public PrefabCreator(List<GameObject> prefabs)
        {
            nestedPrefabs = prefabs;
        }

        public void Create(ref GameObject targetObject, RenderContext renderContext,
            Dictionary<string, object> rootJson)
        {
            if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;

            var rootElement = ElementFactory.Generate(rootJson, null);
            rootElement.Render(ref targetObject, renderContext, null);

            if (renderContext.ToggleGroupMap.Count > 0)
            {
                // ToggleGroupが作成された場合
                var go = new GameObject("ToggleGroup");
                go.transform.SetParent(targetObject.transform);
                foreach (var keyValuePair in renderContext.ToggleGroupMap)
                {
                    var gameObject = keyValuePair.Value;
                    gameObject.transform.SetParent(go.transform);
                }
            }

            // 使われなかったオブジェクトの退避グループ
            var notUsedGroupObject =
                ElementUtil.GetOrCreateGameObject(renderContext, null, $"[{Importer.NAME}] Unused", null);
            var notUsedchilds = renderContext.FreeChildObjects;
            if (notUsedchilds.Count > 0)
            {
                var notUsedGroupRect = ElementUtil.GetOrAddComponent<RectTransform>(notUsedGroupObject);
                foreach (var keyValuePair in notUsedchilds)
                {
                    var go = keyValuePair.Key;
                    if (go != null)
                    {
                        if (PrefabUtility.GetNearestPrefabInstanceRoot(go) == null)
                        {
                            // NestedPrefabの子供でなければNotUsedグループに移動できる
                            var goRect = go.GetComponent<RectTransform>();
                            if (goRect != null)
                            {
                                goRect.SetParent(notUsedGroupRect);
                            }
                        }
                        else
                        {
                            // NestedPrefabの子供を転送しようとするとエラーになるため、
                            // 非アクティブにする
                            //TODO: unusedといった名前をつけるか検討
                            go.SetActive(false);
                        }
                    }
                    else
                    {
                        // Debug.Log("既に廃棄されています");
                    }
                }

                // not used グループに子供があった場合のみ移動
                if (notUsedGroupRect.childCount > 0)
                {
                    notUsedGroupRect.SetParent(targetObject.transform);
                    notUsedGroupObject.SetActive(false);
                }
            }
            else
            {
                Object.DestroyImmediate(notUsedGroupObject);
            }

            foreach (var prefab in renderContext.NewPrefabs.ToList())
                //if we haven't created a prefab out of the referenced GO we do so now
                if (PrefabUtility.GetPrefabAssetType(prefab) == PrefabAssetType.NotAPrefab)
                {
                    //TODO: Ugly path generation
                    var nestedPrefabDirectory = Path.Combine(Application.dataPath.Replace("Assets", ""),
                        Path.Combine(Path.Combine(EditorUtil.GetOutputPrefabsFolderAssetPath()), "Components"));

                    if (!Directory.Exists(nestedPrefabDirectory))
                        Directory.CreateDirectory(nestedPrefabDirectory);

                    nestedPrefabs.Add(PrefabUtility.SaveAsPrefabAssetAndConnect(prefab,
                        Path.Combine(nestedPrefabDirectory, prefab.name + ".prefab"), InteractionMode.AutomatedAction));
                }
        }

        private void Postprocess(GameObject go)
        {
            var methods = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(BaumPostprocessor)))
                .Select(x => x.GetMethod("OnPostprocessPrefab"));
            foreach (var method in methods) method.Invoke(null, new object[] {go});
        }

        public void Validation(Dictionary<string, object> info)
        {
            var version = info.Get("version");
            if (!Versions.Contains(version))
                throw new Exception(string.Format("version {0} is not supported", version));
        }
    }
}