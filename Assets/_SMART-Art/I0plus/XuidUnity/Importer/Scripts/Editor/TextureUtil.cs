using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniJSON;
using OnionRing;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     シリアライズできるDictionaryクラス
    /// </summary>
    public class Dict : Dictionary<string, string>, ISerializationCallbackReceiver
    {
        // ReadOnlyをつけるとシリアライズできなくなる
        // RiderでCodeCleanUpでついてしまう
        [SerializeField] private List<string> keys = new List<string>();
        [SerializeField] private List<string> vals = new List<string>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            vals.Clear();

            var e = GetEnumerator();

            while (e.MoveNext())
            {
                keys.Add(e.Current.Key);
                vals.Add(e.Current.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            var cnt = keys.Count <= vals.Count ? keys.Count : vals.Count;
            for (var i = 0; i < cnt; ++i)
                this[keys[i]] = vals[i];
        }
    }

    public class TextureUtil
    {
        public const string ImageHashMapCacheFileName = "ImageHashMap-cache.json";
        public const string ImagePathMapCacheFileName = "ImagePathMap-cache.json";

        /// <summary>
        ///     Layout.jsonのみ読み込んだときに、過去出力したテクスチャを読み込めるようにするための情報（シェアしていても）
        ///     <Hash, path> テクスチャハッシュHashは、pathテクスチャファイルがある、という情報
        /// </summary>
        private static Dict imageHashMap = new Dict(); // Rider Code Cleanupで Dictが変わってしまう

        /// <summary>
        ///     Layout.jsonのみ読み込んだときに、過去出力したテクスチャを読み込めるようにするための情報（シェアしていても）
        ///     <path1, path2> path1のテクスチャは、path2を利用する、という情報
        /// </summary>
        private static Dict imagePathMap = new Dict(); // Rider Code Cleanupで Dictが変わってしまう


        private static void SaveCache(string folderAssetPath)
        {
            var jsonImageHashMap = JsonUtility.ToJson(imageHashMap);
            var hashMapAssetPath = folderAssetPath + "/" + ImageHashMapCacheFileName;
            File.WriteAllText(hashMapAssetPath, jsonImageHashMap);
            var jsonImagePathMap = JsonUtility.ToJson(imagePathMap);
            File.WriteAllText(folderAssetPath + "/" + ImagePathMapCacheFileName, jsonImagePathMap);
        }

        private static void LoadCache(string folderAssetPath)
        {
            try
            {
                var imageHashMapCacheAssetPath = folderAssetPath + "/" + ImageHashMapCacheFileName;
                if (File.Exists(imageHashMapCacheAssetPath))
                {
                    var jsonImageHashMap = File.ReadAllText(imageHashMapCacheAssetPath);
                    imageHashMap = JsonUtility.FromJson<Dict>(jsonImageHashMap);
                }
                else
                {
                    imageHashMap = new Dict();
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"exception: Read ImageHashMap cache. {ex.Message}");
                imageHashMap = new Dict();
            }

            try
            {
                var assetPath = folderAssetPath + "/" + ImagePathMapCacheFileName;
                if (File.Exists(assetPath))
                {
                    var jsonImagePathMap = File.ReadAllText(assetPath);
                    imagePathMap = JsonUtility.FromJson<Dict>(jsonImagePathMap);
                }
                else
                {
                    imagePathMap = new Dict();
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"exception: Read ImagePathMap cache. {ex.Message}");
                imagePathMap = new Dict();
            }
        }

        /// <summary>
        ///     読み込み可能なTextureを作成する
        ///     Texture2DをC#ScriptでReadableに変更するには？ - Qiita
        ///     https://qiita.com/Katumadeyaruhiko/items/c2b9b4ccdfe51df4ad4a
        /// </summary>
        /// <param name="sourceTexture"></param>
        /// <param name="sourceMoveX">左上座標系</param>
        /// <param name="sourceMoveY">左上座標系</param>
        /// <param name="destWidth"></param>
        /// <param name="destHeight"></param>
        /// <returns></returns>
        private static Texture2D CreateReadableTexture2D(
            Texture sourceTexture,
            int? sourceMoveX, int? sourceMoveY, int? destWidth, int? destHeight)
        {
            // オプションをRenderTextureReadWrite.sRGBに変更した
            var sourceRenderTexture = RenderTexture.GetTemporary(
                sourceTexture.width,
                sourceTexture.height,
                0,
                RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.sRGB);

            // ソーステクスチャをRenderテクスチャにコピーする
            Graphics.Blit(sourceTexture, sourceRenderTexture);

            // 現在アクティブなレンダーテクスチャを退避
            var previous = RenderTexture.active;
            RenderTexture.active = sourceRenderTexture;
            // テクスチャを作成
            var destTexture = new Texture2D(destWidth ?? sourceTexture.width, destHeight ?? sourceTexture.height);
            // テクスチャをクリア
            var pixels = destTexture.GetPixels32();
            var clearColor = new Color32(0, 0, 0, 0);
            for (var i = 0; i < pixels.Length; i++) pixels[i] = clearColor;
            destTexture.SetPixels32(pixels);
            // コピー
            try
            {
                var moveX = sourceMoveX ?? 0;
                var moveY = sourceMoveY ?? 0;
                // 左下座標系に変換する
                // TODO:METALは変換しなくて良いらしい（未確認）
                moveY = destTexture.height - sourceTexture.height - moveY;
                var readHeight = sourceTexture.height;
                var readY = 0;
                if (moveY < 0)
                {
                    readY = -moveY;
                    readHeight -= readY;
                    moveY = 0;
                }

                destTexture.ReadPixels(new Rect(0, readY, sourceTexture.width, readHeight),
                    moveX,
                    moveY);
                destTexture.Apply();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Importer.NAME}] ReadPixels failed.:{ex.Message}");
            }

            // レンダーテクスチャをもとに戻す
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(sourceRenderTexture);
            return destTexture;
        }

        /// <summary>
        ///     バイナリデータを読み込む
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static byte[] ReadFileToBytes(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var bin = new BinaryReader(fileStream);
            var values = bin.ReadBytes((int) bin.BaseStream.Length);

            bin.Close();

            return values;
        }

        public static Texture2D CreateTextureFromPng(string path)
        {
            // Debug.Log($"CreateTextureFromPng {path}");
            var readBinary = ReadFileToBytes(path);

            var pos = 16; // 16バイトから開始

            var width = 0;
            for (var i = 0; i < 4; i++) width = width * 256 + readBinary[pos++];

            var height = 0;
            for (var i = 0; i < 4; i++) height = height * 256 + readBinary[pos++];

            var texture = new Texture2D(width, height);
            texture.LoadImage(readBinary);

            return texture;
        }

        /// <summary>
        ///     ハッシュをつかって
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetSameImagePath(string path)
        {
            var folderPath = Path.GetDirectoryName(path);
            LoadCache(folderPath);
            path = path.Replace("\\", "/");
            if (imagePathMap.ContainsKey(path))
            {
                var sameImagePath = imagePathMap[path];
                if (File.Exists(sameImagePath))
                {
                    return sameImagePath;
                }

                // ファイルが無かった
                imagePathMap.Remove(path);
                SaveCache(folderPath);
            }

            return path;
        }


        // Textureデータの書き出し
        // 同じファイル名の場合書き込みしない
        private static string CheckWrite(string writePngPath, byte[] pngData, Hash128 pngHash)
        {
            // HashMapキャッシュファイルを作成のために
            // - \を/にする
            // - 相対パスである
            writePngPath = writePngPath.Replace("\\", "/");
            LoadCache(Path.GetDirectoryName(writePngPath));

            var hashStr = pngHash.ToString();

            // ハッシュが同じテクスチャがある Shareする
            if (imageHashMap.ContainsKey(hashStr))
            {
                var path = imageHashMap[hashStr];
                if (File.Exists(path))
                {
                    // Debug.Log("shared texture " + Path.GetFileName(newPath) + "==" + Path.GetFileName(name));
                    imagePathMap[writePngPath] = path;
                    SaveCache(Path.GetDirectoryName(writePngPath));
                    return "Shared other path texture.";
                }

                imageHashMap.Remove(hashStr); // hashStrの登録を削除
                foreach (var keyValuePair in imagePathMap)
                {
                    if (keyValuePair.Key == path)
                    {
                        Debug.LogError("Key 存在しないファイルを参照している");
                    }

                    if (keyValuePair.Value == path)
                    {
                        Debug.LogError("Value 存在しないファイルを参照している");
                    }
                }
            }

            // ハッシュからのパスを登録
            imageHashMap[hashStr] = writePngPath;
            // 置き換え対象のパスを登録
            imagePathMap[writePngPath] = writePngPath;

            // 同じファイル名のテクスチャがある（前の変換時に生成されたテクスチャ）
            if (File.Exists(writePngPath))
            {
                var oldPngData = File.ReadAllBytes(writePngPath);
                // 中身をチェックする
                if (oldPngData.Length == pngData.Length && pngData.SequenceEqual(oldPngData))
                {
                    // 全く同じだった場合、書き込まないでそのまま利用する
                    // UnityのDB更新を防ぐ
                    SaveCache(Path.GetDirectoryName(writePngPath));
                    return "Same texture existed.";
                }
            }

            // 本来はフォルダを作成しなくても良いはず
            EditorUtil.CreateFolder(Path.GetDirectoryName(writePngPath));
            File.WriteAllBytes(writePngPath, pngData);
            SaveCache(Path.GetDirectoryName(writePngPath));
            return "Texture: Created";
        }

        private static string CheckWriteSpriteFromTexture(string writeSpritePath, Texture2D texture, Boarder boarder)
        {
            if (texture == null)
            {
                Debug.LogError($"textureがNullです CheckWriteTexture({writeSpritePath})");
                return $"error {writeSpritePath} is null";
            }

            var pngHash = texture.imageContentsHash;
            var writeFolder = Path.GetDirectoryName(writeSpritePath);

            // HashMapキャッシュファイルを作成のために
            // - \を/にする
            // - 相対パスである
            writeSpritePath = writeSpritePath.Replace("\\", "/");
            LoadCache(writeFolder);

            var hashStr = pngHash.ToString();

            // ハッシュが同じテクスチャがある Shareする
            if (imageHashMap.ContainsKey(hashStr))
            {
                var path = imageHashMap[hashStr];
                if (File.Exists(path))
                {
                    // Debug.Log("shared texture " + Path.GetFileName(newPath) + "==" + Path.GetFileName(name));
                    imagePathMap[writeSpritePath] = path;
                    SaveCache(writeFolder);
                    return "Texture: Shared";
                }

                // ファイルが存在しない
                imageHashMap.Remove(hashStr); // hashStrの登録を削除

                var deleteList = imageHashMap
                    .Where(kvp => kvp.Key == path || kvp.Value == path) // 存在しないファイルを参照している
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in deleteList)
                {
                    imagePathMap.Remove(key);
                }
            }

            // ハッシュからのパスを登録
            imageHashMap[hashStr] = writeSpritePath;
            // 置き換え対象のパスを登録
            imagePathMap[writeSpritePath] = writeSpritePath;

            // のちのUnity PreprocessTextureの処理への情報受け渡し
            // （PNGインポートでスプライトを作成するUnityのイベント）
            var slicedTexture = new SlicedTexture(texture, boarder);
            PreprocessTexture.SlicedTextures[writeSpritePath] = slicedTexture;

            try
            {
                // 本来はフォルダを作成しなくても良いはず
                EditorUtil.CreateFolder(Path.GetDirectoryName(writeSpritePath));

                // PNG での保存　（多プラットフォームに対応しやすい）
                var pngData = ImageConversion.EncodeToPNG(texture);
                File.WriteAllBytes(writeSpritePath, pngData);

                // spriteでの保存
                /*
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                AssetDatabase.CreateAsset(sprite, Path.ChangeExtension(writeSpritePath, "asset"));
                */

                SaveCache(writeFolder);
            }
            catch (Exception ex)
            {
                Debug.LogAssertion($"Textureの書き込み中に例外が発生しました:{ex.Message}");
            }

            return "Texture: Created";
        }

        /// <summary>
        ///     アセットのイメージをスライスする
        ///     戻り地は、変換リザルトメッセージ
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="sourceImagePath"></param>
        /// <returns></returns>
        public static string SliceSprite(string outputPath, string sourceImagePath)
        {
            // オプションJSONの読み込み
            Dictionary<string, object> json = null;
            var imageJsonPath = sourceImagePath + ".json";
            if (File.Exists(imageJsonPath))
            {
                var text = File.ReadAllText(imageJsonPath);
                json = Json.Deserialize(text) as Dictionary<string, object>;
            }

            // PNGを読み込み、同じサイズのTextureを作成する
            var sourceTexture = CreateTextureFromPng(sourceImagePath);
            var optionJson = json.GetDic("copy_rect");
            var readableTexture = CreateReadableTexture2D(sourceTexture,
                optionJson?.GetInt("offset_x"),
                optionJson?.GetInt("offset_y"),
                optionJson?.GetInt("width"),
                optionJson?.GetInt("height")
            );
            if (readableTexture == null)
            {
                Debug.LogError($"readableTextureがNULLです{sourceImagePath}");
            }

            // LoadAssetAtPathをつかったテクスチャ読み込み サイズが2のべき乗になる　JPGも読める
            // var texture = CreateReadableTexture2D(AssetDatabase.LoadAssetAtPath<Texture2D>(asset));
            if (PreprocessTexture.SlicedTextures == null)
                PreprocessTexture.SlicedTextures = new Dictionary<string, SlicedTexture>();

            var slice = json?.Get("slice").ToLower();
            switch (slice)
            {
                case null:
                case "auto":
                {
                    var slicedTexture = TextureSlicer.Slice(readableTexture);
                    return CheckWriteSpriteFromTexture(outputPath, slicedTexture.Texture, slicedTexture.Boarder);
                }
                case "none":
                {
                    return CheckWriteSpriteFromTexture(outputPath, readableTexture, new Boarder(0, 0, 0, 0));
                }
                case "border":
                {
                    var border = json.GetDic("slice_border");
                    if (border == null) break; // borderパラメータがなかった

                    // 上・右・下・左の端から内側へのオフセット量
                    var top = border.GetInt("top") ?? 0;
                    var right = border.GetInt("right") ?? 0;
                    var bottom = border.GetInt("bottom") ?? 0;
                    var left = border.GetInt("left") ?? 0;

                    return CheckWriteSpriteFromTexture(outputPath, readableTexture, new Boarder(left, bottom, right, top));
                }
            }
            Debug.LogError($"[{Importer.NAME}] SliceSpriteの処理ができませんでした");
            return null;
        }
    }
}