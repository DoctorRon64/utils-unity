using UnityEditor;
using UnityEngine;

namespace Utility {
    [CustomEditor(typeof(DataAsset), true)]
    public class DataAssetEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            SerializedProperty id = serializedObject.FindProperty("m_id");
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(id, new GUIContent("Guid"));

            if (GUILayout.Button("Generate GUID")) {
                string path = AssetDatabase.GetAssetPath(target);
                string guid = AssetDatabase.AssetPathToGUID(path);
                if (string.IsNullOrEmpty(guid))
                    Debug.LogWarning("Save the asset before generating its GUID.", target);
                else
                    id.stringValue = guid;
            }

            DrawPropertiesExcluding(serializedObject, "m_Script", "m_id");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
