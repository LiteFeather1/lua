using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Seasonal<,>), true)]
public class SeasonalWindow : Editor
{
    private static string _textField;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var seasonal = target as ISeasonal;

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Add Christmas"))
            {
                Undo.RegisterCompleteObjectUndo(target, "Undo Add Christmas");
                seasonal.AddChristmas();
            }
            else if (GUILayout.Button("Add Halloween"))
            {
                Undo.RegisterCompleteObjectUndo(target, "Undo Add Halloween");
                seasonal.AddHalloween();
            }
        }

        using (new EditorGUILayout.HorizontalScope()) 
        {
            _textField = GUILayout.TextField(_textField);
            if (GUILayout.Button("Add") && !string.IsNullOrEmpty(_textField))
            {
                Undo.RegisterCompleteObjectUndo(target, "Undo Add");
                seasonal.Add(_textField);
            }
        }
    }
}
