#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.RefValue.Editor
{
    // Mainly used for not indenting in the inspector
    [CustomPropertyDrawer(typeof(RefValue<>))]
    public class RefValuePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");
            return base.GetPropertyHeight(valueProperty, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");

            EditorGUI.BeginProperty(position, label, valueProperty);
            {
                EditorGUI.PropertyField(position, valueProperty, label, true);
            }
            EditorGUI.EndProperty();
        }
    }

}
#endif
