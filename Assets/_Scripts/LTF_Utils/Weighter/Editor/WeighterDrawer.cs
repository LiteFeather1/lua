#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace LTF.Weighter.Editor
{
    [CustomPropertyDrawer(typeof(Weighter<>))]
    public class WeighterDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 1f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var p = property.FindPropertyRelative("_objects");
            EditorGUILayout.PropertyField(p, label);
        }
    }
}
#endif
