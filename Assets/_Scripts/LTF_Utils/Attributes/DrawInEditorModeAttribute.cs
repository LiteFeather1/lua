using UnityEditor;
using UnityEngine;

namespace LTF
{
    public class DrawInEditorModeAttribute : PropertyAttribute
    {
        public bool InEditor { get; }

        public DrawInEditorModeAttribute(bool inEditor = true) => InEditor = inEditor;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DrawInEditorModeAttribute))]
    public class DrawInPlayModeDrawer : PropertyDrawer
    {
        private float _propertyDrawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _propertyDrawer;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if ((attribute as DrawInEditorModeAttribute).InEditor != Application.isPlaying)
            {
                _propertyDrawer = base.GetPropertyHeight(property, label);
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
                _propertyDrawer = 0f;
        }
    }
#endif
}
