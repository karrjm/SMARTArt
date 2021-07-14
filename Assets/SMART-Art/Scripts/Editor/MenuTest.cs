using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    public class MenuTest
    {
        [MenuItem("GameObject/Create Other/SMART-Art Prefabs/POI Button")]
        private static void InstantiatePointOfInterestButton()
        {
            GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/SMART-Art/Prefabs/POI Button", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(go);
        }

        // [MenuItem("GameObject/Create Other/SMART-Art", true)]
        // static bool ValidateCreatePOI()
        // {
        //     
        // }
    }
}