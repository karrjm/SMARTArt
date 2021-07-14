using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public class MenuTest
    {
        [MenuItem("GameObject/Create Other/SMART-Art Prefabs/POI Button")]
        private static void InstantiatePointOfInterestButton()
        {
            Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/POI Button.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        // [MenuItem("GameObject/Create Other/SMART-Art", true)]
        // static bool ValidateCreatePOI()
        // {
        //     
        // }
    }
}