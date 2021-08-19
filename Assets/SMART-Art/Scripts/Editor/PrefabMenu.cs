using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Editor
{
#if UNITY_EDITOR
    public static class PrefabMenu
    {
        #region Helpers

        private static void Create(Object prefab)
        {
            var creation = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (Selection.activeTransform) creation.transform.SetParent(Selection.activeTransform);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Undo.RegisterCreatedObjectUndo(creation, "Undo SMART-Art Prefab");
        }

        #endregion

        #region Prefabs

        [MenuItem("GameObject/SMART-Art/Prefabs/Image Target", false, 0)]
        private static void CreateImageTarget()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/ImageTarget.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Topic Card", false, 0)]
        private static void CreateTopicCard()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/TopicCard.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Point of Interest (POI)", false, 0)]
        private static void CreatePoiButton()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/POI.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Card", false, 0)]
        private static void CreateCard()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Card.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Audio Panel", false, 0)]
        private static void CreateAudioPanel()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/AudioPanel.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Video Panel", false, 0)]
        private static void CreateVideoPanel()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/VideoPanel.prefab");
            Create(prefab);
        }

        #endregion
    }

#endif
}