using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Editor
{
#if UNITY_EDITOR
    public static class PrefabMenu
    {
        #region Utilities

        [MenuItem("GameObject/SMART-Art/Utilities", false, 0)]
        private static void DoSomething()
        {
        }

        #endregion

        #region Prefabs

        [MenuItem("GameObject/SMART-Art/Prefabs/POI Button", false, 0)]
        private static void CreatePoiButton()
        {
            // load prefab
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/POI Button.prefab");
            
            // instantiate prefab
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            
            // set prefab as child of selected gameObject in hierarchy
            if (Selection.activeTransform) go.transform.SetParent(Selection.activeTransform);

            // mark scene as dirty
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            // allow undo
            Undo.RegisterCreatedObjectUndo(go, "Create POI Button");
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Video Panel", false, 0)]
        private static void CreateVideoPanel()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Video Panel.prefab");
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (Selection.activeTransform) go.transform.SetParent(Selection.activeTransform);
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            
            Undo.RegisterCreatedObjectUndo(go, "Create Video Panel");
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Card Stack", false, 0)]
        private static void CreateCardStack()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/SMART-Art/Prefabs/Card Stack.prefab");
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (Selection.activeTransform) go.transform.SetParent(Selection.activeTransform);
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            
            Undo.RegisterCreatedObjectUndo(go, "Create Card Stack");
        }

        #endregion
    }
#endif
}