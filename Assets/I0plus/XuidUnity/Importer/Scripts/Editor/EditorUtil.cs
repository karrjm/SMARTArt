using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     EditorUtil class.
    ///     based on Baum2.Editor.EditorUtil class.
    /// </summary>
    public static class EditorUtil
    {
        public const string IMPORT_FOLDER_MARK_FILENAME = "_XuidUnity_Import";
        public const string SPRITES_FOLDER_MARK_FILENAME = "_XuidUnity_Sprites";
        public const string PREFABS_FOLDER_MARK_FILENAME = "_XuidUnity_Prefabs";
        public const string FONTS_FOLDER_MARK_FILENAME = "_XuidUnity_Fonts";

        /// <summary>
        ///     【C#】ドライブ直下からのファイルリスト取得について - Qiita
        ///     https://qiita.com/OneK/items/8b0d02817a9f2a2fbeb0#%E8%A7%A3%E8%AA%AC
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static List<string> GetAllFiles(string dirPath)
        {
            var lstStr = new List<string>();

            try
            {
                // ファイル取得
                var strBuff = Directory.GetFiles(dirPath);
                lstStr.AddRange(strBuff);

                // ディレクトリの取得
                strBuff = Directory.GetDirectories(dirPath);
                foreach (var s in strBuff)
                {
                    var lstBuff = GetAllFiles(s);
                    lstBuff.ForEach(delegate(string str) { lstStr.Add(str); });
                }
            }
            catch (UnauthorizedAccessException)
            {
                // アクセスできなかったので無視
            }

            return lstStr;
        }

        /// <summary>
        ///     Assets以下のパスにする
        ///     \を/におきかえる
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToAssetPath(string path)
        {
            path = path.Replace("\\", "/");
            var assetPath = path;
            if (path.StartsWith(Application.dataPath))
                assetPath = "Assets" + path.Substring(Application.dataPath.Length);
            return assetPath;
        }

        public static string FindFileAssetPath(string fileName, bool throwException = true)
        {
            var guids = AssetDatabase.FindAssets(fileName);
            if (guids.Length == 0)
            {
                if (throwException)
                    throw new ApplicationException($"{fileName}ファイルがプロジェクト内に存在しません。");
                return null;
            }

            if (guids.Length > 1) Debug.LogErrorFormat("{0}ファイルがプロジェクト内に複数個存在します。", fileName);

            var fileAssetPath
                = AssetDatabase.GUIDToAssetPath(guids[0]);

            return fileAssetPath;
        }

        private static string FindSomeFileAssetPath(IEnumerable<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                // var path = FindFolderAssetPath(markFile, false);
                var fileAssetPath = FindFileAssetPath(fileName, false);
                var path = fileAssetPath?.Replace("\\", "/");
                if (path != null) return path;
            }
            return null;
        }

        private static string FindSomeFolderAssetPath(IEnumerable<string> fileNames)
        {
            var filePath = FindSomeFileAssetPath(fileNames);
            return Path.GetDirectoryName(filePath)?.Replace("\\", "/");
        }


        /// <summary>
        ///     優先順位に基づき、みつかったマークファイル名を返す
        /// </summary>
        /// <returns></returns>
        public static string GetImportFolderAssetPath()
        {
            var markFiles = new[]
            {
                IMPORT_FOLDER_MARK_FILENAME + "1",
                IMPORT_FOLDER_MARK_FILENAME
            };
            var assetPath = FindSomeFolderAssetPath(markFiles);
            if (assetPath == null)
            {
                // Importフォルダは無くてもよいため、Log出力しない
                // Debug.LogAssertion($"[{Importer.Name}] {IMPORT_FOLDER_MARK_FILENAME} not found.");
                return null;
            }

            return assetPath;
        }
        
        public static string GetImportMarkFileAssetPath()
        {
            var markFiles = new[]
            {
                IMPORT_FOLDER_MARK_FILENAME + "1",
                IMPORT_FOLDER_MARK_FILENAME
            };
            var assetPath = FindSomeFileAssetPath(markFiles);
            return assetPath;
        }

        public static string GetOutputSpritesFolderAssetPath()
        {
            var markFiles = new[]
            {
                SPRITES_FOLDER_MARK_FILENAME + "1",
                SPRITES_FOLDER_MARK_FILENAME
            };
            var assetPath = FindSomeFolderAssetPath(markFiles);
            if (assetPath == null)
            {
                Debug.LogAssertion($"[{Importer.NAME}] ${SPRITES_FOLDER_MARK_FILENAME} not found.");
                return null;
            }

            return assetPath;
        }

        public static string GetOutputPrefabsFolderAssetPath()
        {
            var markFiles = new[]
            {
                PREFABS_FOLDER_MARK_FILENAME + "1",
                PREFABS_FOLDER_MARK_FILENAME
            };
            var assetPath = FindSomeFolderAssetPath(markFiles);
            if (assetPath == null)
            {
                Debug.LogAssertion($"[{Importer.NAME}] ${PREFABS_FOLDER_MARK_FILENAME} not found.");
                return null;
            }

            return assetPath;
        }

        public static string GetFontsFolderAssetPath()
        {
            var markFiles = new[]
            {
                FONTS_FOLDER_MARK_FILENAME + "1",
                FONTS_FOLDER_MARK_FILENAME
            };
            var assetPath = FindSomeFolderAssetPath(markFiles);
            if (assetPath == null)
            {
                Debug.LogAssertion($"[{Importer.NAME}] ${FONTS_FOLDER_MARK_FILENAME} not found.");
                return null;
            }

            return assetPath;
        }

        /**
         * /Assets/Top/Second/File.txt
         * return Second
         */
        public static string GetSubFolderName(string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);
            return Path.GetFileName(folderPath);
        }

        public static Color HexToColor(string hex)
        {
            if (hex[0] == '#') hex = hex.Substring(1);

            var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }

        public static RectTransform CopyTo(this RectTransform self, RectTransform to)
        {
            to.sizeDelta = self.sizeDelta;
            to.position = self.position;
            return self;
        }

        public static Image CopyTo(this Image self, Image to)
        {
            to.sprite = self.sprite;
            to.type = self.type;
            to.color = self.color;
            return self;
        }
    }
}