using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public static class PrefabMenu
    {
        [MenuItem("GameObject/SMART-Art/Utilities", false, 0)]
        private static void DoSomething()
        {
            
        }
        
        [MenuItem("GameObject/SMART-Art/Prefabs/POI Button", false, 0)]
        private static void InstantiatePointOfInterestButton()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/POI Button.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Video Panel", false, 0)]
        private static void InstantiateVideoPanel()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/Video Panel.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        [MenuItem("GameObject/SMART-Art/Prefabs/Card Stack", false, 0)]
        private static void InstantiateCardStack()
        {
            var prefab =
                AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/Card Stack.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}