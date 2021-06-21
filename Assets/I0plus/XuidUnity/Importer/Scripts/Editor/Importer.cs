using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiniJSON;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_2019_1_OR_NEWER
using UnityEditor.U2D;
using UnityEngine.U2D;

#endif

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     based on Baum2/Editor/Scripts/BaumImporter file.
    /// </summary>
    public sealed class Importer : AssetPostprocessor
    {
        public const string NAME = "XuidUnity";
        private static int _progressTotal = 1;
        private static int _progressCount;
        private static bool _autoEnableFlag; // デフォルトがチェック済みの時には true にする

        /// <summary>
        ///     自動インポート 自動削除
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        public static async void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var importFolderAssetPath = EditorUtil.GetImportFolderAssetPath();
            // 自動インポート用フォルダが無い場合は終了
            if (importFolderAssetPath == null) return;
            var importMarkFileAssetPath = EditorUtil.GetImportMarkFileAssetPath();

            var forImportAssetPaths = importedAssets
                .Where(importedAsset => importedAsset.StartsWith(importFolderAssetPath) &&
                                        // IMPORT_FOLDER_MARK　ファイルをはじく
                                        importedAsset != importMarkFileAssetPath)
                .ToList();
            if (forImportAssetPaths.Count <= 0) return;

            await Import(importFolderAssetPath, forImportAssetPaths, true, true);
        }

        public override int GetPostprocessOrder()
        {
            return 1000;
        }

        private static void UpdateDisplayProgressBar(string message = "")
        {
            if (_progressTotal > 1)
                EditorUtility.DisplayProgressBar("XdUnitUI Import",
                    $"{_progressCount}/{_progressTotal} {message}",
                    (float) _progressCount / _progressTotal);
        }

        /*
        [MenuItem("Assets/XuidUnity/Import Selected Folders")]
        public static async Task MenuImportFromSelectFolder()
        {
            var folderPaths = ProjectHighlightedFolders();
            await ImportFolders(folderPaths, true, false);
        }
        */

        /*
        [MenuItem("Assets/XuidUnity/Import Selected Folders", true)]
        public static bool MenuImportSelectedFolderCheck()
        {
            var folderPaths = ProjectHighlightedFolders();
            return folderPaths.Any();
        }
        */

        /*
        [MenuItem("Assets/XuidUnity/Import Selected Folders(Layout Only)")]
        public static async Task MenuImportSelectedFolderLayoutOnly()
        {
            var folderPaths = ProjectHighlightedFolders();
            await ImportFolders(folderPaths, false, false);
        }
        */

        /*
        [MenuItem("Assets/XuidUnity/Import Selected Folders(Layout Only)", true)]
        public static bool MenuImportSelectedFolderLayoutOnlyCheck()
        {
            var folderPaths = ProjectHighlightedFolders();
            return folderPaths.Any();
        }
        */

        [MenuItem("Assets/XuidUnity/Clean Import...")]
        public static async void MenuImportSpecifiedFolder()
        {
            var path = EditorUtility.OpenFolderPanel("Clean import:Specify Folder", "", "");
            if (string.IsNullOrWhiteSpace(path)) return;

            await ImportFolder(path, false, true, false);
        }

        [MenuItem("Assets/XuidUnity/(experimental)Overwrite Import...")]
        public static async void MenuOverwriteImportSpecifiedFolder()
        {
            var path = EditorUtility.OpenFolderPanel("Overwrite Import:Specify Folder", "", "");
            if (string.IsNullOrWhiteSpace(path)) return;

            await ImportFolder(path, true, true, false);
        }


        /*
        [MenuItem("Assets/XuidUnity/Specify Folder Import(layout only)...")]
        public static async Task MenuImportSpecifiedFolderLayoutOnly()
        {
            var path = EditorUtility.OpenFolderPanel("Specify Exported Folder", "", "");
            if (string.IsNullOrWhiteSpace(path)) return;

            var folders = new List<string> {path};
            await ImportFolders(folders, false, false);
        }
        */

        /// <summary>
        ///     Project ウィンドウで、ハイライトされているディレクトリを取得する
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> ProjectHighlightedFolders()
        {
            var folders = new List<string>();

            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path)) folders.Add(path);
            }

            return folders;
        }


        private static async Task<int> ImportFolder(string importFolderPath, bool overwriteImportFlag,
            bool convertImageFlag,
            bool deleteAssetsFlag)
        {
            if (!File.Exists(importFolderPath + "/xuid-export.json"))
            {
                var result = EditorUtility.DisplayDialog("Import",
                    $"Please specify exported root folder.", "Quit", "Force import");
                if (result) return -1;
            }
            
            var importedAssets = new List<string>();

            // ファイルのリストアップ
            var files = Directory.EnumerateFiles(
                importFolderPath, "*", SearchOption.AllDirectories);

            // 関係あるファイルのみ追加
            foreach (var file in files)
            {
                if (!convertImageFlag && !file.EndsWith(".layout.json", StringComparison.OrdinalIgnoreCase))
                    continue;

                var extension = Path.GetExtension(file).ToLower();
                if (extension == ".meta") continue;
                importedAssets.Add(file);
            }

            if (importedAssets.Count > 100)
            {
                var result = EditorUtility.DisplayDialog("Import",
                    $"Importing {importedAssets.Count} files.\n Continue?", "Continue", "Cancel");
                if (!result) return -1;
            }

            await Import(importFolderPath, importedAssets, overwriteImportFlag, deleteAssetsFlag);

            // インポートしたアセットのソース削除が必要ならここでするべきかも
            EditorUtility.DisplayDialog("Import", "Done.", "Ok");
            return 0;
        }

        /// <summary>
        ///     Assetディレクトリに追加されたファイルを確認、インポート処理を行う
        /// </summary>
        /// <param name="importedAssetPaths"></param>
        /// <param name="optionOverwriteImport"></param>
        /// <param name="optionDeleteImportedAssets"></param>
        private static async Task<int> Import(string baseFolderPath, IEnumerable<string> importedAssetPaths,
            bool optionOverwriteImport,
            bool optionDeleteImportedAssets)
        {
            var importedPaths = importedAssetPaths.ToList();
            _progressTotal = importedPaths.Count();
            if (_progressTotal == 0) return 0;
            _progressCount = 0;

            // Png->Spriteに変換するリストをクリアする
            PreprocessTexture.SlicedTextures = null;

            // インポートされたファイルからフォルダパスリストを作成する
            // Key: AssetPath
            // Value: ディレクトリにあるファイルの拡張子
            var importedFolderAssetPaths = new FolderInfos();
            foreach (var importedAssetPath in importedAssetPaths)
            {
                if (EditorUtil.IsFolder(importedAssetPath) == true)
                    // すでにフォルダパスはスルー
                    continue;

                var folderPath = Path.GetDirectoryName(importedAssetPath);
                var extension = Path.GetExtension(importedAssetPath);
                importedFolderAssetPaths.Add(folderPath, extension);
            }

            // 出力フォルダの作成
            foreach (var importedFolderInfo in importedFolderAssetPaths)
            {
                if (EditorUtil.IsFolder(importedFolderInfo.Key) != true) continue;

                // フォルダであった場合
                var importedFullPath = Path.GetFullPath(importedFolderInfo.Key);
                var subFolderName = EditorUtil.GetSubFolderName(baseFolderPath, importedFolderInfo.Key + "/file.tmp");

                // このフォルダには.pngファイルがあるか
                var isSpriteFolder = importedFolderInfo.Value.Contains(".png");

                // スプライト出力フォルダの準備
                if (isSpriteFolder)
                {
                    var outputSpritesFolderAssetPath = Path.Combine(
                        EditorUtil.GetOutputSpritesFolderAssetPath(), subFolderName);
                    if (Directory.Exists(outputSpritesFolderAssetPath))
                    {
                        // フォルダがすでにある　インポートファイルと比較して、出力先にある必要のないファイルを削除する
                        // ダブっている分は比較し、異なっている場合に上書きするようにする
                        var outputFolderInfo = new DirectoryInfo(outputSpritesFolderAssetPath);
                        var importFolderInfo = new DirectoryInfo(importedFullPath);

                        var existSpritePaths = outputFolderInfo.GetFiles("*.png", SearchOption.AllDirectories);
                        var importSpritePaths = importFolderInfo.GetFiles("*.png", SearchOption.AllDirectories);

                        // outputフォルダにある importにはないファイルをリストアップする
                        // そしてそれを削除するという処理であったが不具合が発生する
                        // 別のLayout.Jsonから参照されているテクスチャも消えてしまう
                        // テクスチャについて未使用を削除するには
                        // - フォルダを消して再インポート
                        // - imageHashMapキャッシュファイルからつかわれていないものを削除する　という処理をいれなければならない
                        // var deleteEntries = existSpritePaths.Except(importSpritePaths, new FileInfoComparer()).ToList();
                        var deleteEntries = new List<FileInfo>();

                        // スプライト出力フォルダがすでにある場合はTextureハッシュキャッシュを削除する
                        deleteEntries.Add(new FileInfo(outputSpritesFolderAssetPath + "/" +
                                                       TextureUtil.ImageHashMapCacheFileName));
                        deleteEntries.Add(new FileInfo(outputSpritesFolderAssetPath + "/" +
                                                       TextureUtil.ImagePathMapCacheFileName));
                        if (!optionOverwriteImport)
                        {
                            // 削除する
                            foreach (var fileInfo in deleteEntries)
                            {
                                AssetDatabase.DeleteAsset(EditorUtil.ToAssetPath(fileInfo.FullName));
                            }
                        }
                    }
                    else
                    {
                        // Debug.Log($"[{Importer.Name}] Create Folder: {subFolderName}");
                        EditorUtil.CreateFolder( outputSpritesFolderAssetPath);
                    }
                }

                var outputPrefabsFolderAssetPath = Path.Combine(EditorUtil.GetOutputPrefabsFolderAssetPath(), subFolderName);
                if (!Directory.Exists(outputPrefabsFolderAssetPath))
                {
                    EditorUtil.CreateFolder( outputPrefabsFolderAssetPath);
                }

                UpdateDisplayProgressBar($"Import Folder Preparation: {subFolderName}");
            }

            await Task.Delay(1000);
            // ディレクトリが作成されたり、画像が削除されるためRefresh
            AssetDatabase.Refresh();

            // フォルダが作成され、そこに画像を出力する場合
            // Refresh後、DelayCallで画像生成することで、処理が安定した
            // await Task.Delay(1000);

            // SpriteイメージのハッシュMapをクリアしたかどうかのフラグ
            // importedAssetsに一気に全部の新規ファイルが入ってくる前提の処理
            // 全スライス処理が走る前、最初にClearImageMapをする
            var clearedImageMap = false;
            // 画像コンバート　スライス処理
            var messageCounter = new Dictionary<string, int>();
            var total = 0;
            try
            {
                foreach (var pngAssetPath in importedPaths)
                {
                    // Debug.Log($"Slice: {importedAsset}");
                    if (!pngAssetPath.EndsWith(".png", StringComparison.Ordinal)) continue;
                    //
                    if (!clearedImageMap) clearedImageMap = true;

                    var subFolderName = EditorUtil.GetSubFolderName(baseFolderPath, pngAssetPath);
                    var outputFolderPath = Path.Combine(EditorUtil.GetOutputSpritesFolderAssetPath(), subFolderName);
                    var outputFilePath = Path.Combine(outputFolderPath, Path.GetFileName(pngAssetPath));
                    // スライス処理
                    var message = TextureUtil.SliceSprite(outputFilePath, pngAssetPath);

                    total++;
                    _progressCount += 2; // pngファイル と png.jsonファイル
                    UpdateDisplayProgressBar(message);

                    // 出力されたログをカウントする
                    if (messageCounter.ContainsKey(message))
                        messageCounter[message] = messageCounter[message] + 1;
                    else
                        messageCounter.Add(message, 1);
                }
            }
            catch (Exception ex)
            {
                Debug.LogAssertion(ex.Message);
                Debug.LogAssertion(ex.StackTrace);
            }

            foreach (var keyValuePair in messageCounter)
                Debug.Log($"[{Importer.NAME}] {keyValuePair.Key} {keyValuePair.Value}/{total}");


            var importLayoutFilePaths = new List<string>();
            foreach (var layoutFilePath in importedPaths)
            {
                if (!layoutFilePath.EndsWith(".layout.json", StringComparison.OrdinalIgnoreCase)) continue;
                importLayoutFilePaths.Add(layoutFilePath);
            }

            string GetPrefabPath(string layoutFilePath)
            {
                var prefabFileName = Path.GetFileName(layoutFilePath).Replace(".layout.json", "") + ".prefab";
                var subFolderName = EditorUtil.GetSubFolderName(baseFolderPath, layoutFilePath);
                var saveAssetPath =
                    Path.Combine(Path.Combine(EditorUtil.GetOutputPrefabsFolderAssetPath(),
                        subFolderName), prefabFileName);
                return saveAssetPath;
            }

            string GetPrefabName(string layoutFilePath)
            {
                var prefabFileName = Path.GetFileName(layoutFilePath).Replace(".layout.json", "");
                var subFolderName = EditorUtil.GetSubFolderName(baseFolderPath, layoutFilePath);
                return subFolderName + "/" + prefabFileName;
            }

            // 出力されたスライスPNGをSpriteに変換する処理を走らせるために必須
            // ここでPreprocessTextureが実行されなければいけない
            AssetDatabase.Refresh();
            await Task.Delay(1000);
            if (PreprocessTexture.SlicedTextures != null && PreprocessTexture.SlicedTextures.Count != 0)
            {
                // Debug.LogWarning($"[{Importer.Name}] SlicedTextures is still available.");
            }

            var prefabs = new List<GameObject>();

            // .layout.jsonを全て読み込んで、コンバート順をソートする
            // Item1: prefab name dependensyチェック用
            // Item2: file path
            // Item3: json data
            var layoutJsons = new List<Tuple<string, string, Dictionary<string, object>>>();
            foreach (var layoutFilePath in importLayoutFilePaths)
            {
                var prefabName = GetPrefabName(layoutFilePath);
                // Load JSON
                var jsonText = File.ReadAllText(layoutFilePath);
                var json = Json.Deserialize(jsonText) as Dictionary<string, object>;
                layoutJsons.Add(
                    new Tuple<string, string, Dictionary<string, object>>(prefabName, layoutFilePath, json));
            }

            // コンバートする順番を決める
            layoutJsons.Sort((a, b) =>
            {
                List<object> GetDependency(Dictionary<string, object> json)
                {
                    return json.GetDic("info")?.GetArray("dependency");
                }

                int GetDependencyCount(Dictionary<string, object> json)
                {
                    var dr = GetDependency(json);
                    return dr?.Count ?? 0;
                }

                bool Check(string name, Dictionary<string, object> json)
                {
                    var nameList = GetDependency(json);
                    if (nameList == null) return false;
                    return nameList.Any(o => name == o as string);
                }

                // aはbより先に処理すべきか
                if (Check(a.Item1, b.Item3)) return -1;
                // bはaより先に処理すべきか
                if (Check(b.Item1, a.Item3)) return 1;

                // 依存ファイル数で決着をつける
                return GetDependencyCount(a.Item3) - GetDependencyCount(b.Item3);
            });

            // Create Prefab
            foreach (var layoutJson in layoutJsons)
            {
                UpdateDisplayProgressBar($"Layout: {layoutJson.Item1}");
                _progressCount += 1;

                var layoutFilePath = layoutJson.Item2;
                var subFolderName = EditorUtil.GetSubFolderName(baseFolderPath, layoutFilePath);

                GameObject go = null;
                try
                {
                    // Debug.Log($"[{Importer.Name}] in process...{Path.GetFileName(layoutFilePath)}");
                    var saveAssetPath = GetPrefabPath(layoutFilePath);
                    var spriteOutputFolderAssetPath =
                        Path.Combine(EditorUtil.GetOutputSpritesFolderAssetPath(), subFolderName);
                    var fontAssetPath = EditorUtil.GetFontsFolderAssetPath();

                    // overwriteImportFlagがTrueなら、ベースとなるPrefab上に生成していく
                    // 利用できるオブジェクトは利用していく
                    if (optionOverwriteImport)
                    {
                        // すでにあるプレハブを読み込む
                        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(saveAssetPath);
                        if (prefab != null)
                        {
                            go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.OutermostRoot,
                                InteractionMode.AutomatedAction);
                        }
                    }

                    // Render Context
                    var renderContext = new RenderContext(spriteOutputFolderAssetPath, fontAssetPath, go);
                    if (optionOverwriteImport) renderContext.OptionAddXdGuidComponent = true;

                    // Load JSON
                    var jsonText = File.ReadAllText(layoutFilePath);
                    var json = Json.Deserialize(jsonText) as Dictionary<string, object>;
                    //var info = json.GetDic("info");
                    //Validation(info);
                    var rootJson = json.GetDic("root");

                    // Create Prefab
                    var prefabCreator = new PrefabCreator(prefabs);
                    prefabCreator.Create(ref go, renderContext, rootJson);

                    // Save Prefab
                    EditorUtil.CreateFolder(Path.GetDirectoryName(saveAssetPath));
                    var savedAsset = PrefabUtility.SaveAsPrefabAsset(go, saveAssetPath);
                    Debug.Log($"[{Importer.NAME}] Created: <color=#7FD6FC>{Path.GetFileName(saveAssetPath)}</color>",
                        savedAsset);
                }
                catch (Exception ex)
                {
                    Debug.LogAssertion($"[{Importer.NAME}] " + ex.Message + "\n" + ex.StackTrace);
                    // 変換中例外が起きた場合もテンポラリGameObjectを削除する
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("Import Failed", ex.Message, "Close");
                    throw;
                }
                finally
                {
                    Object.DestroyImmediate(go);
                }

                AssetDatabase.Refresh();
                await Task.Delay(100);
            }


            // インポートしたファイルを削除し、そのフォルダが空になったらフォルダも削除する
            if (optionDeleteImportedAssets)
            {
                foreach (var forImportAssetPath in importedAssetPaths)
                {
                    // フォルダの場合はスルー
                    if (EditorUtil.IsFolder(forImportAssetPath) == true) continue;

                    // インポートするファイルを削除
                    AssetDatabase.DeleteAsset(forImportAssetPath);
                    // ファイルがあったフォルダが空になったかチェック
                    var folderName = Path.GetDirectoryName(forImportAssetPath);
                    var files = Directory.GetFiles(folderName);
                    if (files.Length == 0)
                    {
                        // フォルダの削除
                        // Debug.Log($"ディレクトリ削除{folderName}");
                        AssetDatabase.DeleteAsset(folderName);
                    }
                }
            }

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            return 0;
        }

        /// <summary>
        /// フォルダーにあるファイルの種類の情報
        /// </summary>
        private class FolderInfos : Dictionary<string, HashSet<string>>
        {
            public void Add(string path, string fileExtension)
            {
                if (!Keys.Contains(path))
                {
                    var extentions = new HashSet<string> {fileExtension};
                    this[path] = extentions;
                }

                this[path].Add(fileExtension);
            }
        }

        private class FileInfoComparer : IEqualityComparer<FileInfo>
        {
            public bool Equals(FileInfo iLhs, FileInfo iRhs)
            {
                if (iLhs.Name == iRhs.Name) return true;

                return false;
            }

            public int GetHashCode(FileInfo fi)
            {
                var s = fi.Name;
                return s.GetHashCode();
            }
        }
    }
}