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
            Undo.RegisterCreatedObjectUndo(creation, "Create Screenspace Takeaway");
        }

        #endregion

        #region Prefabs

        [MenuItem("GameObject/SMART-Art/Prefabs/AR Camera", false, 0)]
        private static void CreateCamera()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/ARCamera.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/POI Button", false, 0)]
        private static void CreatePoiButton()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/POI Button.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Video Panel", false, 0)]
        private static void CreateVideoPanel()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Video Panel.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Card Stack", false, 0)]
        private static void CreateCardStack()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Card Stack.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Card Stack", false, 0)]
        private static void CreateScreenspaceTakeaway()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Screenspace Takeaway.prefab");
            Create(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Label Canvas", false, 0)]
        private static void CreateLabelCanvas()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Label Canvas.prefab");
            Create(prefab);
        }
        
        [MenuItem("GameObject/SMART-Art/Prefabs/Image Target", false, 0)]
        private static void CreateImageTarget()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/ImageTarget.prefab");
            Create(prefab);
        }

        #endregion
    }

#endif
}