#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LTF.BiasedRandom.Editor
{
    [CustomPropertyDrawer(typeof(BiasedRandom))]
    public class BiasedRandomDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(property.isExpanded, label))
            {
                EditorGUI.indentLevel++;

                var width = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 54f;
                using var min = property.FindPropertyRelative("_min");
                using var max = property.FindPropertyRelative("_max");
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PropertyField(min);
                    EditorGUILayout.PropertyField(max);
                }
                EditorGUIUtility.labelWidth = width;

                using var bias = property.FindPropertyRelative("_bias");
                EditorGUILayout.Slider(bias, min.floatValue, max.floatValue);
                if (bias.floatValue > max.floatValue)
                    bias.floatValue = max.floatValue;
                else if (bias.floatValue < min.floatValue)
                    bias.floatValue = min.floatValue;

                using var influence = property.FindPropertyRelative("_influence");
                EditorGUILayout.Slider(influence, 0f, 1f);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
#endif
}
