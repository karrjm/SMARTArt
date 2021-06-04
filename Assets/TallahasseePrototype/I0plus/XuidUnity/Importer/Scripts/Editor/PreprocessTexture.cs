using System.Collections.Generic;
using System.IO;
using OnionRing;
using UnityEditor;
using UnityEngine;

namespace I0plus.XduiUnity.Importer.Editor
{
    /// <summary>
    ///     PreprocessTexture class.
    ///     based on Baum2.Editor.PreprocessTexture class.
    /// </summary>
    public sealed class PreprocessTexture : AssetPostprocessor
    {
        public static Dictionary<string, SlicedTexture> SlicedTextures;

        public void OnPreprocessTexture()
        {
            /*
            var importDirectoryPath = EditorUtil.GetImportDirectoryPath();
            if (assetPath.Contains(importDirectoryPath))
            {
                var importer = assetImporter as TextureImporter;
                if (importer == null) return;

                importer.textureType = TextureImporterType.Sprite;
                importer.isReadable = true;
            }
            else 
            */
            var spriteFolderAssetPath = EditorUtil.ToAssetPath(EditorUtil.GetOutputSpritesFolderAssetPath());
            if (assetPath.Contains(spriteFolderAssetPath))
            {
                if (SlicedTextures == null || !SlicedTextures.ContainsKey(assetPath)) return;
                var slicedTexture = SlicedTextures[assetPath];
                var fileName = Path.GetFileName(assetPath);
                var importer = assetImporter as TextureImporter;
                if (importer == null) return;

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePackingTag =
                    $"XuidUnity_{Path.GetFileName(Path.GetDirectoryName(assetPath))}";
                importer.spritePixelsPerUnit = 100.0f;
                importer.spritePivot = new Vector2(0.5f, 0.5f);
                importer.mipmapEnabled = false;
                importer.isReadable = false;
                importer.spriteBorder = slicedTexture.Boarder.ToVector4();
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                SlicedTextures.Remove(assetPath);
                if (SlicedTextures.Count == 0) SlicedTextures = null;
            }
        }

        public override int GetPostprocessOrder()
        {
            return 990;
        }
    }
}