#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.CompositeValue.Editor
{
    [CustomPropertyDrawer(typeof(CompositeValue))]
    public class CompositeValueDrawer : PropertyDrawer
    {
        private static readonly GUIContent sr_floatField = new("Starting Value",
            "Not the actually value,\nbut it sets both the Base Value and Value");  

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.PropertyField(property, label, true);
                return;
            }

            var p = new GUIContent(property.displayName, "Composite Value");
            EditorGUILayout.LabelField(p, EditorStyles.boldLabel);

            EditorGUI.indentLevel++;
            var lastValue = property.FindPropertyRelative("_baseValue").floatValue;
            property.FindPropertyRelative("_value").floatValue 
                = property.FindPropertyRelative("_baseValue").floatValue 
                = EditorGUILayout.FloatField(sr_floatField, lastValue);

            EditorGUI.indentLevel--;
        }
    }
}
#endif
