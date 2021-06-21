using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if TMP_PRESENT
using TMPro;
#endif

namespace I0plus.XduiUnity.Importer.Editor
{
    public class GameObjectIdentifier
    {
        public GameObjectIdentifier(string name, string xdGuid)
        {
            Name = name;
            XdGuid = xdGuid;
        }

        public string Name { get; }
        public string XdGuid { get; }
    }

    public class RenderContext
    {
        private readonly string spriteOutputFolderAssetPath;

        public string SpriteOutputFolderAssetPath => spriteOutputFolderAssetPath;

        private readonly string fontFolderAssetPath;

        public string FontFolderAssetPath => fontFolderAssetPath;

        private readonly GameObject rootObject;
        public Stack<GameObject> NewPrefabs { get; } = new Stack<GameObject>();
        public Dictionary<string, GameObject> ToggleGroupMap { get; } = new Dictionary<string, GameObject>();

        public Dictionary<GameObject, GameObjectIdentifier> FreeChildObjects { get; }

        /// <summary>
        ///     XdGuid compornentをAddする
        /// </summary>
        public bool OptionAddXdGuidComponent { get; set; }

        /// <summary>
        ///     Overwrite時、再利用されなかったオブジェクトを移動する
        /// </summary>
        public bool OptionMoveNotUsedObjectOnOverwrite { get; set; } = true;

        /// <summary>
        ///     Overwrite時、再利用されなかったXdオブジェクトを移動する
        /// </summary>
        public bool OptionMoveNotUsedXdObject { get; set; } = true;

        public RenderContext(string spriteOutputFolderAssetPath, string fontFolderAssetPath, GameObject rootObject)
        {
            this.spriteOutputFolderAssetPath = spriteOutputFolderAssetPath;
            this.fontFolderAssetPath = fontFolderAssetPath;
            this.rootObject = rootObject;
            OptionAddXdGuidComponent = false;
            FreeChildObjects = new Dictionary<GameObject, GameObjectIdentifier>();
            if (rootObject != null)
            {
                // 全ての子供を取得する
                var allChildren = new List<GameObject>();
                ElementUtil.GetChildRecursive(rootObject, ref allChildren);
                foreach (var obj in allChildren)
                {
                    if (obj == rootObject)
                        // 自分自身はスキップする
                        continue;

                    AddFreeObject(obj);
                }
            }
        }

        private Transform RecursiveFindChild(Transform parent, string childName)
        {
            var foundChild = parent.Find(childName);
            if (foundChild) return foundChild;
            foreach (Transform child in parent)
            {
                var found = RecursiveFindChild(child, childName);
                if (found != null) return found;
            }

            return null;
        }

        public GameObject FindObject(string name)
        {
            if (rootObject == null || rootObject.transform == null) return null;
            var findTransform = RecursiveFindChild(rootObject.transform, name);
            if (findTransform == null) return null;
            return findTransform.gameObject;
        }

        /// <summary>
        ///     親の名前も使用して検索する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentObject"></param>
        /// <returns></returns>
        public GameObject FindObject(string name, GameObject parentObject)
        {
            // 出来るだけユニークな名前になるように、Rootからの名前を作成する
            var findNames = new List<string> {name};
            var fullName = name;
            while (parentObject != null)
            {
                fullName = parentObject.name + "/" + fullName;
                findNames.Add(fullName);
                var parent = parentObject.transform.parent;
                parentObject = parent ? parent.gameObject : null;
            }

            findNames.Reverse();

            // Rootから親のパス付名 → 単体の名前の順に検索する
            foreach (var findName in findNames)
            {
                var selfObject = FindObject(findName);
                if (selfObject != null)
                    // Debug.Log($"GetSelfObject({findName})");
                    return selfObject;
            }

            return null;
        }

        public List<GameObject> FindFreeObjectsByName(string name, GameObject parentObject)
        {
            // 出来るだけユニークな名前になるように、Rootからの名前を作成する
            var findNames = new List<string> {name};
            var fullName = "/" + name;
            while (parentObject != null)
            {
                fullName = "/" + parentObject.name + fullName;
                findNames.Add(fullName);
                var parent = parentObject.transform.parent;
                parentObject = parent ? parent.gameObject : null;
            }

            // Rootから親のパス付名 → 単体の名前の順に検索する
            findNames.Reverse();

            var founds = new List<GameObject>();
            foreach (var findName in findNames)
            {
                foreach (var keyValuePair in FreeChildObjects)
                {
                    var pathName = keyValuePair.Value.Name;
                    var xdGuid = keyValuePair.Value.XdGuid;
                    if (pathName != null && pathName.EndsWith(findName))
                        founds.Add(keyValuePair.Key);
                }

                if (founds.Count > 0) break;
            }

            return founds;
        }

        public GameObject FindFreeObjectFromXdGuid(string guid)
        {
            foreach (var freeChildObject in FreeChildObjects)
            {
                var xdGuid = freeChildObject.Value.XdGuid;
                if (xdGuid == guid) return freeChildObject.Key;
            }

            return null;
        }

        public GameObject OccupyObject(string guid, string name, GameObject parentObject)
        {
            GameObject found = null;
            if (guid != null) found = FindFreeObjectFromXdGuid(guid);

            if (found == null && name != null)
            {
                // Debug.Log($"guidで見つからなかった:{name}");
                var founds = FindFreeObjectsByName(name, parentObject);
                if (founds == null || founds.Count == 0) return null;
                found = founds[0];
            }

            if (found != null) FreeChildObjects.Remove(found);

            return found;
        }

        public void AddFreeObject(GameObject obj)
        {
            // 後の名前検索で正確にできるように/を前にいれる
            var name = "/" + obj.name;
            var parent = obj.transform.parent;
            while (parent)
            {
                name = "/" + parent.name + name;
                parent = parent.parent;
            }

            // XdGuidコンポーネントがある場合、Guidも情報にいれる
            string xdGuid = null;
            var xdGuidComponent = obj.GetComponent<XdGuid>();
            if (xdGuidComponent != null) xdGuid = xdGuidComponent.guid;

            FreeChildObjects.Add(obj, new GameObjectIdentifier(name, xdGuid));
        }

        public Sprite GetSprite(string spriteName)
        {
            var spriteAssetPath = Path.Combine(spriteOutputFolderAssetPath, spriteName) + ".png";
            // 相対パスの記述に対応した
            // var fileInfo = new FileInfo(path); なぜフルパスにしたか忘れた
            spriteAssetPath = TextureUtil.GetSameImagePath(spriteAssetPath);
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);
            if (sprite == null)
            {
                Debug.LogWarning($"pngで見つからなかった{spriteAssetPath}");
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Path.ChangeExtension(spriteAssetPath,"asset"));
            }
            if (sprite == null)
            {
                var folderPath = Path.GetDirectoryName(spriteAssetPath);
                var obj = AssetDatabase.LoadAssetAtPath<Object>(folderPath);
                Debug.LogError($"[{Importer.NAME}] sprite not found:\"{spriteAssetPath}\"", obj);
            }

            return sprite;
        }

        public Font GetFont(string fontName)
        {
            Font font = null;
            
            var ttfAssetPath = Path.Combine(fontFolderAssetPath, fontName) + ".ttf";
            if (File.Exists(ttfAssetPath))
            {
                font = AssetDatabase.LoadAssetAtPath<Font>(ttfAssetPath);
                if (font != null) return font;
            }

            var otfAssetPath = Path.Combine(fontFolderAssetPath, fontName) + ".otf";
            if (File.Exists(otfAssetPath))
            {
                font = AssetDatabase.LoadAssetAtPath<Font>(otfAssetPath);
                if (font != null) return font;
            }
            
            Debug.LogError($"[{Importer.NAME}] font {fontName}.ttf (or .otf) is not found");
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            return font;
        }

#if TMP_PRESENT
        public TMP_FontAsset GetTMPFontAsset(string fontName, string style)
        {
            var fontFileName = Path.Combine(fontFolderAssetPath, fontName) + "-" + style + " SDF.asset";
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontFileName);
            if (font == null)
            {
                Debug.LogError(string.Format($"[{Importer.NAME}] TMP_FontAsset {fontFileName} is not found"));
            }

            return font;
        }
#endif
    }
}