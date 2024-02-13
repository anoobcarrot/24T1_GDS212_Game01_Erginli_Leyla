using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Book))]
public class BookEditor : Editor
{
    SerializedProperty bookPagesText;

    private void OnEnable()
    {
        bookPagesText = serializedObject.FindProperty("bookPagesText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display other serialized properties as usual
        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Book Pages Text");

        // Display the array elements with multiline text fields
        for (int i = 0; i < bookPagesText.arraySize; i++)
        {
            SerializedProperty element = bookPagesText.GetArrayElementAtIndex(i);
            element.stringValue = EditorGUILayout.TextArea(element.stringValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}


