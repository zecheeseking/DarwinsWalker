using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Link))]
public class ListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Length"), new GUIContent("Length"));
        EditorGUI.EndProperty();
    }
}