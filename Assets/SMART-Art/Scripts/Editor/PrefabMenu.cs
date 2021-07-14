using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public static class PrefabMenu
    {
        [MenuItem("GameObject/Create SMART-Art/POI Button")]
        private static void InstantiatePointOfInterestButton()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/POI Button.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("GameObject/Create SMART-Art/Video Panel")]
        private static void InstantiateVideoPanel()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/Video Panel.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("GameObject/Create SMART-Art/Card Stack")]
        private static void InstantiateCardStack()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/Card Stack.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}