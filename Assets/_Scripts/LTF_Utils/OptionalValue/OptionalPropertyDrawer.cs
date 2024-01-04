#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.Editor
{
    [CustomPropertyDrawer(typeof(OptionalValue<>))]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        private const float TOGGLE_PAD = 24f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            using var p = property.FindPropertyRelative("_value");
            return EditorGUI.GetPropertyHeight(p, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var valueProperty = property.FindPropertyRelative("_value");
            using var enabledProperty = property.FindPropertyRelative("_enabled");

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.width -= TOGGLE_PAD;
                EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
                EditorGUI.PropertyField(position, valueProperty, label, true);
                EditorGUI.EndDisabledGroup();

                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                position.x += TOGGLE_PAD + position.width;
                position.width = position.height = EditorGUI.GetPropertyHeight(enabledProperty);
                position.x -= position.width;
                EditorGUI.PropertyField(position, enabledProperty, GUIContent.none);
                EditorGUI.indentLevel = indent;
            }
        }
    }
}
#endif