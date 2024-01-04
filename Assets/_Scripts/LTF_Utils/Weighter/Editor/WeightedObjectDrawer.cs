#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.Weighter.Editor
{
    [CustomPropertyDrawer(typeof(WeightedObject<>))]
    public class WeightedObjectDrawer : PropertyDrawer
    {
        private const float PAD = 48f;
        private const string TOOLTIP = "Second Parameter is the weigth";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            using var p = property.FindPropertyRelative("_object");
            return EditorGUI.GetPropertyHeight(p, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var objectP = property.FindPropertyRelative("_object");
            using var weightP = property.FindPropertyRelative("_weight");
            label.tooltip = TOOLTIP;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.width -= PAD;
                EditorGUI.PropertyField(position, objectP, label, true);

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.x += PAD + position.width;
                position.width = PAD - 4;
                position.height = EditorGUI.GetPropertyHeight(weightP);
                position.x -= position.width;

                EditorGUI.PropertyField(position, weightP, GUIContent.none);

                EditorGUI.indentLevel = indent;
            }
        }
    }
}
#endif
