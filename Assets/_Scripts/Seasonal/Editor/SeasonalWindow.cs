#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
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

        if (target is SeasonalStringArray t)
        {
            if (GUILayout.Button("Load Messages"))
            {
                var dict = new Dictionary<string, string[]>();

                var christmas = GetMessage($"_{SeasonNames.CHRISTMAS}");
                if (christmas != null)
                    dict.Add(SeasonNames.CHRISTMAS, christmas);

                var halloween = GetMessage($"_{SeasonNames.HALLOWEEN}");
                if (halloween != null)
                    dict.Add(SeasonNames.HALLOWEEN, halloween);

                t.LoadMessages(dict);
            }

            static string[] GetMessage(string loc)
            {
                return (Resources.Load($"Messages{loc}") as TextAsset)?.text
                        .Split('\n')
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .ToArray();
            }
        }
    }
}
#endif
