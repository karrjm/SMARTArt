/*===============================================================================
Copyright (c) 2025 PTC Inc. and/or Its Subsidiary Companies. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
    [CustomEditor(typeof(DefaultValidationEventHandler))]
    [CanEditMultipleObjects]
    public class DefaultValidationEventHandlerEditor : DefaultObserverEventHandlerEditor
    {
        SerializedProperty mOnValidationInfoUpdate;

        protected override void OnEnable()
        {
            base.OnEnable();
            mOnValidationInfoUpdate = serializedObject.FindProperty("OnValidationInfoUpdate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("Event(s) when Validation Info update is received:");
            EditorGUILayout.PropertyField(mOnValidationInfoUpdate);

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}