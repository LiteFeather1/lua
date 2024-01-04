#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.ObjectPool.Editor
{
    [CustomPropertyDrawer(typeof(ObjectPool<>))]
    public class WeightedObjectDrawer : PropertyDrawer
    {
        private const float PAD = 32f;
        private const string TOOLTIP = "Second Parameter is the Initial pool size";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            using var p = property.FindPropertyRelative("_object");
            return EditorGUI.GetPropertyHeight(p, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var objectP = property.FindPropertyRelative("_object");
            using var initialSizeP = property.FindPropertyRelative("_initialPoolSize");
            label.tooltip = TOOLTIP;
            
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.width -= PAD;
                EditorGUI.PropertyField(position, objectP, label, true);

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position.x += PAD + position.width;
                position.width = PAD - 4;
                position.height = EditorGUI.GetPropertyHeight(initialSizeP);
                position.x -= position.width;

                EditorGUI.PropertyField(position, initialSizeP, GUIContent.none);

                EditorGUI.indentLevel = indent;
            }
        }
    }
}
#endif

