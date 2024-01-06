#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LTF.Editor.Windows
{
    public class RecentScriptablesWindow : EditorWindow
    {
        private const string MENU_ITEM = "Tools/Recent Scriptables/";
        private const int MAX_ELEMENTS_IN_RECENT_LIST = 16;
        private const string RANDOM_GUID = "{B19E8C8F-E3FC-4D3E-9733-723A8FF25C50}";
        private const float BUTTON_SIZE = 24f;

        private static readonly GUILayoutOption sr_buttonWidth = GUILayout.Width(BUTTON_SIZE);
        private static readonly GUILayoutOption sr_height = GUILayout.Height(BUTTON_SIZE);
        private static readonly Color sr_red = new(.9f, 0f, .1f, .5f);
        private static readonly Color sr_green = new(.2f, .8f, 0f, .5f);

        private static readonly GUIContent sr_clearAllContent = new("CLEAR ALL", "Removes all but pinned");
        private static readonly GUIContent sr_pinContent = new();
        private static readonly GUIContent sr_removeContent = new("X", "Remove Item");

        private static Data s_data;
        private static int s_pinnnedAmount;
        private static Vector2 s_scrollPos;

        private static string ListPersistanceKey => $"{Application.productName}_{Application.companyName}_{RANDOM_GUID}";

        [MenuItem(MENU_ITEM + "Window", priority = 100)]
        private static void Init()
        {
            GetWindow<RecentScriptablesWindow>("Recent Scriptables");
        }

        [MenuItem(MENU_ITEM + "Clear Editor Prefs", priority = 101)]
        private static void ClearPrefs()
        {
            EditorPrefs.DeleteKey(ListPersistanceKey);
        }

        private void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;

            s_data = JsonUtility.FromJson<Data>(EditorPrefs.GetString(ListPersistanceKey, JsonUtility.ToJson(new Data())));
            for (int i = 0; i < s_data.Count; i++)
            {
                if (s_data[i].IsPinned)
                    s_pinnnedAmount++;

                s_data.List[i].Scriptable = AssetDatabase.LoadAssetAtPath<Object>(s_data[i].Path);
            }
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;

            EditorPrefs.SetString(ListPersistanceKey, JsonUtility.ToJson(s_data));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Pinned & Recent Scriptables: ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });

            s_scrollPos = EditorGUILayout.BeginScrollView(s_scrollPos);

            DrawAll(s_pinnnedAmount - 1, 0);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (s_data.Count > 0)
                DrawAll(s_data.Count - 1, s_pinnnedAmount);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);

            GUI.backgroundColor = sr_red;
            // Remove all but pinned button
            if (GUILayout.Button(sr_clearAllContent, sr_height))
            {
                if (s_data.Count > 0)
                {
                    s_data.List.RemoveRange(s_pinnnedAmount, s_data.Count - s_pinnnedAmount);
                    Repaint();
                }
            }

            GUI.backgroundColor = Color.white;
        }

        private void DrawAll(int from, int to)
        {
            for (int i = from; i >= to; i--)
            {
                var s = s_data[i];
                if (string.IsNullOrEmpty(s.Name)
                    || string.IsNullOrEmpty(s.Path)
                    || string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(s.Path, AssetPathToGUIDOptions.OnlyExistingAssets)))
                {
                    s_data.List.RemoveAt(i);
                    continue;
                }

                bool isPresent = false;
                for (int index = 0; index < SceneManager.sceneCount; index++)
                {
                    if (Selection.activeObject == s_data[i].Scriptable)
                    {
                        isPresent = true;
                        break;
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    var lastIconSize = EditorGUIUtility.GetIconSize();
                    using (new EditorGUI.DisabledGroupScope(isPresent))
                    {
                        EditorGUIUtility.SetIconSize(new(16f, 16f));
                        var choiceButtonStyle = new GUIStyle(GUI.skin.button)
                        {
                            alignment = TextAnchor.MiddleLeft,
                            fontStyle = FontStyle.Bold,
                        };
                        // Open
                        var maxWidth = GUILayout.Width(EditorGUIUtility.currentViewWidth - (BUTTON_SIZE * 3f) - 4f);
                        if (GUILayout.Button(new GUIContent(s.Name, AssetDatabase.GetCachedIcon(s.Path), $"{s.Name} "), choiceButtonStyle, sr_height, maxWidth))
                            Selection.activeObject = s_data[i].Scriptable;
                    }

                    if (s.IsPinned)
                    {
                        GUI.backgroundColor = sr_green;
                        sr_pinContent.text = "!";
                        sr_pinContent.tooltip = "Unpin";
                    }
                    else
                    {
                        GUI.backgroundColor = Color.white;
                        sr_pinContent.text = "i";
                        sr_pinContent.tooltip = "Pin";
                    }

                    var closeButtonStyle = new GUIStyle(GUI.skin.button)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold,
                        padding = new(0, 0, 0, 0),
                    };
                    // Pin Button
                    if (GUILayout.Button(sr_pinContent, closeButtonStyle, sr_buttonWidth, sr_height))
                    {
                        s.IsPinned = !s.IsPinned;
                        if (s.IsPinned)
                        {
                            // Move element forward
                            var tmpD = s_data[i];
                            // Inplace moving items so we don't have to RemoveAt and then Insert
                            for (int j = i; j > s_pinnnedAmount; j--)
                                s_data[j] = s_data[j - 1];
                            s_data[s_pinnnedAmount++] = tmpD;
                        }
                        else
                        {
                            MoveElementBack(i);
                            s_pinnnedAmount--;
                        }

                        Repaint();
                    }

                    GUI.backgroundColor = sr_red;
                    // Remove Button
                    if (GUILayout.Button(sr_removeContent, closeButtonStyle, sr_buttonWidth, sr_height))
                    {
                        s_data.List.RemoveAt(i);
                        if (s.IsPinned)
                            s_pinnnedAmount--;
                        Repaint();
                    }

                    GUI.backgroundColor = Color.white;

                    EditorGUIUtility.SetIconSize(lastIconSize);
                }
            }
        }

        private void SelectionChanged()
        {
            if (Selection.activeObject is not ScriptableObject scriptable)
            {
                Repaint();
                return;
            }

            AddItem(scriptable, AssetDatabase.GetAssetPath(scriptable), scriptable.name);
            Repaint();
        }

        private void AddItem(ScriptableObject scriptable, string path, string name)
        {
            // Checking we already have that item
            for (int i = s_data.Count - 1; i >= 0; i--)
            {
                // Don't have so continue
                if (!s_data[i].Path.Equals(path))
                    continue;
                // We have it and it's pinned so we don't have to do anything
                if (s_data[i].IsPinned)
                    return;

                // We have it so we need to move it to the top of the list  
                MoveElementBack(i);
                return;
            }

            s_data.List.Add(new(scriptable, path, name));

            if (s_data.List.Count > MAX_ELEMENTS_IN_RECENT_LIST + s_pinnnedAmount)
                s_data.List.RemoveAt(s_pinnnedAmount);
        }

        private static void MoveElementBack(int from)
        {
            var tmpD = s_data[from];
            // Inplace moving items so we don't have to RemoveAt and then Insert
            for (int j = from; j < s_data.Count - 1; j++)
                s_data[j] = s_data[j + 1];
            s_data[^1] = tmpD;
        }

        [System.Serializable]
        private class SavedScriptable
        {
            public Object Scriptable { get; set; }
            [field: SerializeField] public string Path { get; private set; }
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public bool IsPinned { get; set; }

            public SavedScriptable(Object scriptable, string path, string name)
            {
                Scriptable = scriptable;
                Path = path;
                Name = name;
                IsPinned = false;
            }
        }

        [System.Serializable]
        private struct Data
        {
            [field: SerializeField] public List<SavedScriptable> List { get; private set; }

            public Data(List<SavedScriptable> list)
            {
                List = list;
            }

            public readonly SavedScriptable this[int i] { get => List[i]; set => List[i] = value; }

            public readonly int Count => List.Count;
        }
    }
}
#endif
