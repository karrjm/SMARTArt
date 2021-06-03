using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            await Import(forImportAssetPaths, true, true);
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

            var folders = new List<string> {path};
            await ImportFolders(folders, false, true, false);
        }

        [MenuItem("Assets/XuidUnity/(experimental)Overwrite Import...")]
        public static async void MenuOverwriteImportSpecifiedFolder()
        {
            var path = EditorUtility.OpenFolderPanel("Overwrite Import:Specify Folder", "", "");
            if (string.IsNullOrWhiteSpace(path)) return;

            var folders = new List<string> {path};
            await ImportFolders(folders, true, true, false);
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


        private static async Task<int> ImportFolders(IEnumerable<string> importFolderPaths, bool overwriteImportFlag,
            bool convertImageFlag,
            bool deleteAssetsFlag)
        {
            var importedAssets = new List<string>();

            foreach (var importFolderPath in importFolderPaths)
            {
                // トップディレクトリの追加
                // importedAssets.Add(importFolderPath);

                // var folders = Directory.EnumerateDirectories(importFolderPath);
                // importedAssets.AddRange(folders);

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
            }

            if (importedAssets.Count > 100)
            {
                var result = EditorUtility.DisplayDialog("Import",
                    $"Importing {importedAssets.Count} files.\n Continue?", "Continue", "Cancel");
                if (!result) return -1;
            }

            await Import(importedAssets, overwriteImportFlag, deleteAssetsFlag);

            // インポートしたアセットのソース削除が必要ならここでするべきかも
            EditorUtility.DisplayDialog("Import", "Done.", "Ok");
            return 0;
        }

        private static bool? IsFolder(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path)) return null;
            try
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            }
            catch (Exception exception)
            {
                // ignored
                Debug.LogAssertion(exception.Message);
            }

            return false;
        }

        /// <summary>
        ///     Assetディレクトリに追加されたファイルを確認、インポート処理を行う
        /// </summary>
        /// <param name="importedAssetPaths"></param>
        /// <param name="optionOverwriteImport"></param>
        /// <param name="optionDeleteImportedAssets"></param>
        private static async Task<int> Import(IEnumerable<string> importedAssetPaths, bool optionOverwriteImport,
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
                if (IsFolder(importedAssetPath) == true)
                    // すでにフォルダパスはスルー
                    continue;

                var folderPath = Path.GetDirectoryName(importedAssetPath);
                var extension = Path.GetExtension(importedAssetPath);
                importedFolderAssetPaths.Add(folderPath, extension);
            }

            // 出力フォルダの作成
            foreach (var importedFolderInfo in importedFolderAssetPaths)
            {
                if (IsFolder(importedFolderInfo.Key) != true) continue;

                // フォルダであった場合
                var importedFullPath = Path.GetFullPath(importedFolderInfo.Key);
                var subFolderName = Path.GetFileName(importedFolderInfo.Key);

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
                        AssetDatabase.CreateFolder(EditorUtil.GetOutputSpritesFolderAssetPath(),
                            subFolderName);
                    }
                }

                var prefabsOutputPath = Path.Combine(EditorUtil.GetOutputPrefabsFolderAssetPath(), subFolderName);
                if (!Directory.Exists(prefabsOutputPath))
                {
                    AssetDatabase.CreateFolder(EditorUtil.GetOutputPrefabsFolderAssetPath(),
                        subFolderName);
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
                foreach (var importedAsset in importedPaths)
                {
                    // Debug.Log($"Slice: {importedAsset}");
                    if (!importedAsset.EndsWith(".png", StringComparison.Ordinal)) continue;
                    //
                    if (!clearedImageMap) clearedImageMap = true;

                    // スライス処理
                    var message = TextureUtil.SliceSprite(importedAsset);

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
                var subFolderName = EditorUtil.GetSubFolderName(layoutFilePath);
                var saveAssetPath =
                    Path.Combine(Path.Combine(EditorUtil.GetOutputPrefabsFolderAssetPath(),
                        subFolderName), prefabFileName);
                return saveAssetPath;
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

            // Create Prefab
            foreach (var layoutFilePath in importLayoutFilePaths)
            {
                var subFolderName = EditorUtil.GetSubFolderName(layoutFilePath);
                UpdateDisplayProgressBar($"Layout: {subFolderName}");
                _progressCount += 1;
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

                    // Create Prefab
                    var prefabCreator = new PrefabCreator(layoutFilePath, prefabs);
                    prefabCreator.Create(ref go, renderContext);
                    CreateFolder(Path.GetDirectoryName(saveAssetPath));
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

                await Task.Delay(100);
            }


            // インポートしたファイルを削除し、そのフォルダが空になったらフォルダも削除する
            if (optionDeleteImportedAssets)
            {
                foreach (var forImportAssetPath in importedAssetPaths)
                {
                    // フォルダの場合はスルー
                    if (IsFolder(forImportAssetPath) == true) continue;

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

        private static void CreateSpritesFolder(string asset)
        {
            var directoryName = Path.GetFileName(Path.GetFileName(asset));
            var directoryPath = EditorUtil.GetOutputSpritesFolderAssetPath();
            var directoryFullPath = Path.Combine(directoryPath, directoryName);
            if (Directory.Exists(directoryFullPath))
                // 画像出力用フォルダに画像がのこっていればすべて削除
                // Debug.LogFormat($"[{Importer.Name}] Delete Exist Sprites: {0}", EditorUtil.ToUnityPath(directoryFullPath));
                foreach (var filePath in Directory.GetFiles(directoryFullPath, "*.png",
                    SearchOption.TopDirectoryOnly))
                    File.Delete(filePath);
            else
                // Debug.LogFormat($"[{Importer.Name}] Create Directory: {0}", EditorUtil.ToUnityPath(directoryPath) + "/" + directoryName);
                AssetDatabase.CreateFolder(EditorUtil.ToAssetPath(directoryPath),
                    Path.GetFileName(directoryFullPath));
        }

        /**
        * SliceSpriteではつかなくなったが､CreateAtlasでは使用する
        */
        private static string ImportSpritePathToOutputPath(string asset)
        {
            var folderName = Path.GetFileName(Path.GetDirectoryName(asset));
            var folderPath = Path.Combine(EditorUtil.GetOutputSpritesFolderAssetPath(), folderName);
            var fileName = Path.GetFileName(asset);
            return Path.Combine(folderPath, fileName);
        }

        /// <summary>
        ///     複数階層のフォルダを作成する
        /// </summary>
        /// <param name="folderAssetPath">一番子供のフォルダまでのパスe.g.)Assets/Resources/Sound/</param>
        /// <remarks>パスは"Assets/"で始まっている必要があります。Splitなので最後のスラッシュ(/)は不要です</remarks>
        public static void CreateFolder(string folderAssetPath)
        {
            folderAssetPath = folderAssetPath.Replace("\\", "/");
            Debug.Assert(folderAssetPath.StartsWith("Assets/"),
                "arg `path` of CreateFolderRecursively doesn't starts with `Assets/`");

            // もう存在すれば処理は不要
            // if (AssetDatabase.IsValidFolder(folderAssetPath)) return; // AssetDatabaseでのチェックの場合、Refresh等してDBの更新が必要
            if (IsFolder(folderAssetPath) == true) return;

            // スラッシュで終わっていたら除去
            if (folderAssetPath.EndsWith("/"))
                folderAssetPath = folderAssetPath.Substring(0, folderAssetPath.Length - 1);

            var names = folderAssetPath.Split('/');
            for (var i = 1; i < names.Length; i++)
            {
                var parent = string.Join("/", names.Take(i).ToArray());
                var target = string.Join("/", names.Take(i + 1).ToArray());
                var subFolderName = names[i];
                if (IsFolder(folderAssetPath) != true)
                {
                    // Debug.Log($"[{Importer.Name}] CreateFolder: {subFolderName}");
                    AssetDatabase.CreateFolder(parent, subFolderName);
                }
            }
        }

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