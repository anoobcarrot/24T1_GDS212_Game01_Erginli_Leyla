using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonitorInteraction))]
public class MonitorInteractionEditor : Editor
{
    SerializedProperty websiteTextArray;

    private void OnEnable()
    {
        websiteTextArray = serializedObject.FindProperty("websiteTextArray");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display other serialized properties as usual
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Website Text Array");

        // Display the array elements with multiline text fields
        for (int i = 0; i < websiteTextArray.arraySize; i++)
        {
            SerializedProperty element = websiteTextArray.GetArrayElementAtIndex(i);
            element.stringValue = EditorGUILayout.TextArea(element.stringValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

