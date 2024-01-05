#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LTF.Editor.Windows
{
    public class RecentAssetsWindow : EditorWindow
    {
        private const string MENU_ITEM = "Tools/Recent Objects/";
        private const int MAX_ELEMENTS_IN_RECENT_LIST = 16;
        private const string RANDOM_GUID = "{AD8E9BB2-096D-413F-829F-E83E0A82FD65}";
        private const float BUTTON_SIZE = 24f;

        private static readonly GUILayoutOption sr_buttonWidth = GUILayout.Width(BUTTON_SIZE);
        private static readonly GUILayoutOption sr_height = GUILayout.Height(BUTTON_SIZE);
        private static readonly Color sr_red = new(.9f, 0f, .1f, .5f);
        private static readonly Color sr_green = new(.2f, .8f, 0f, .5f);
        private static readonly Color sr_blue = new(0f, .33f, .66f, .5f);

        private static readonly GUIContent sr_clearAllContent = new("CLEAR ALL", "Removes all but pinned");
        private static readonly GUIContent sr_toggleContent = new("O");
        private static readonly GUIContent sr_pinContent = new();
        private static readonly GUIContent sr_removeContent = new("X", "Remove Item");

        private static Data _data;
        private static int s_pinnnedAmount;
        private static Vector2 _scrollPos;

        private static string ListPersistanceKey => $"{Application.productName}_{Application.companyName}_{RANDOM_GUID}";

        [MenuItem(MENU_ITEM + "Window", priority = 0)]
        private static void Init()
        {
            var window = GetWindow<RecentAssetsWindow>();
            window.titleContent = new("Recent Prefabs");
            window.Show();
        }

        [MenuItem(MENU_ITEM + "Toggle Scene Visibility", priority = 1)]
        private static void ToggleSceneVisibility()
        {
            _data.ShowScenes = !_data.ShowScenes;
        }

        [MenuItem(MENU_ITEM + "Clear Editor Prefs",priority = 2)]
        private static void ClearPrefs()
        {
            EditorPrefs.DeleteKey(ListPersistanceKey);
        }

        private void OnEnable()
        {
            EditorSceneManager.sceneClosed += SceneClosed;
            PrefabStage.prefabStageClosing += PrefabStageClosing;

            _data = JsonUtility.FromJson<Data>(EditorPrefs.GetString(ListPersistanceKey, JsonUtility.ToJson(new Data())));

            s_pinnnedAmount = _data.List.Where(s => s.IsPinned).Count();
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneClosed -= SceneClosed;
            PrefabStage.prefabStageClosing -= PrefabStageClosing;

            EditorPrefs.SetString(ListPersistanceKey, JsonUtility.ToJson(_data));
        }

        private void OnGUI()
        {
            string label;
            Color c;
            if (_data.ShowScenes)
            {
                label = "Pinned & Recent Prefabs & Scenes:";
                sr_toggleContent.tooltip = "Hide scenes";
                c = sr_blue;
            }
            else
            {
                label = "Pinned & Recent Prefabs:";
                sr_toggleContent.tooltip = "Show scenes";
                c = Color.white;
            }

            EditorGUILayout.LabelField(label, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            DrawAll(s_pinnnedAmount - 1, 0);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            DrawAll(_data.Count - 1, s_pinnnedAmount);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = c;
                // Toggle Scenes button
                if (GUILayout.Button(sr_toggleContent, sr_buttonWidth, sr_height))
                {
                    Undo.RegisterCompleteObjectUndo(this, "Toggle Show Scenes");
                    _data.ShowScenes = !_data.ShowScenes;
                    Repaint();
                }

                GUI.backgroundColor = sr_red;
                // Remove all but pinned button
                if (GUILayout.Button(sr_clearAllContent, sr_height))
                {
                    Undo.RegisterCompleteObjectUndo(this, "Clear ALL");
                    _data.List.RemoveRange(s_pinnnedAmount, _data.Count - s_pinnnedAmount);
                    Repaint();
                }
            }
            GUI.backgroundColor = Color.white;
        }

        private void DrawAll(int from, int to)
        {
            for (int i = from; i >= to; i--)
            {
                var s = _data[i];
                if (string.IsNullOrEmpty(s.Name)
                    || string.IsNullOrEmpty(s.Path)
                    || string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(s.Path, AssetPathToGUIDOptions.OnlyExistingAssets)))
                {
                    _data.List.RemoveAt(i);
                    continue;
                }

                if (!_data.ShowScenes && !s.IsPrefab)
                    continue;

                var stage = PrefabStageUtility.GetCurrentPrefabStage();
                bool isPresent = false;
                if (stage == null)
                    for (int index = 0; index < SceneManager.sceneCount; index++)
                    {
                        if (SceneManager.GetSceneAt(index).path == s.Path)
                        {
                            isPresent = true;
                            break;
                        }
                    }
                else
                    isPresent = stage.assetPath == s.Path;

                using (new EditorGUILayout.HorizontalScope())
                {
                    var lastIconSize = EditorGUIUtility.GetIconSize();
                    using (new EditorGUI.DisabledGroupScope(isPresent))
                    {
                        EditorGUIUtility.SetIconSize(new(16f, 16f));
                        var choiceButtonStyle = new GUIStyle(GUI.skin.button)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold,
                        };
                        // Open
                        if (GUILayout.Button(new GUIContent(s.Name, AssetDatabase.GetCachedIcon(s.Path)), choiceButtonStyle, sr_height))
                        {
                            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                return;

                            if (s.IsPrefab)
                                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(s.Path));
                            else
                                EditorSceneManager.OpenScene(s.Path);
                        }
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
                        _data[i] = s;
                        if (s.IsPinned)
                            (_data[i], _data[s_pinnnedAmount]) = (_data[s_pinnnedAmount++], _data[i]);
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
                        _data.List.RemoveAt(i);
                        if (s.IsPinned)
                            s_pinnnedAmount--;
                        Repaint();
                    }

                    GUI.backgroundColor = Color.white;

                    EditorGUIUtility.SetIconSize(lastIconSize);
                }
            }
        }

        private void SceneClosed(Scene scene)
        {
            if (Application.isPlaying)
                return;

            AddRecentItem(scene.path, scene.name, false);

            Repaint();
        }

        private void PrefabStageClosing(PrefabStage prefab)
        {
            AddRecentItem(prefab.assetPath, prefab.prefabContentsRoot.name, true);

            Repaint();
        }

        private static void AddRecentItem(string path, string name, bool prefab)
        {
            // Checking we already have that item
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                // Don't have so continue
                if (_data[i].Path != path)
                    continue;
                // We have it and it's pinned so we don't have to do anything
                if (_data[i].IsPinned)
                    return;

                // We have it so we need to move it to the top of the list  
                MoveElementBack(i);
                return;
            }

            _data.List.Add(new(path, name, prefab));

            if (_data.List.Count > MAX_ELEMENTS_IN_RECENT_LIST + s_pinnnedAmount)
                _data.List.RemoveAt(s_pinnnedAmount);
        }

        private static void MoveElementBack(int from)
        {
            var tmp = _data[from];
            // Inplace moving items so we don't have to RemoveAt and then Insert
            for (int j = from; j < _data.Count - 1; j++)
                _data[j] = _data[j + 1];
            _data[^1] = tmp;
        }

        [System.Serializable]
        private struct SavedSceneOrPrefab
        {
            [field: SerializeField] public string Path { get; private set; }
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public bool IsPrefab { get; private set; }
            [field: SerializeField] public bool IsPinned { get; set; }

            public SavedSceneOrPrefab(string path, string name, bool isPrefab)
            {
                Path = path;
                Name = name;
                IsPrefab = isPrefab;
                IsPinned = false;
            }
        }

        [System.Serializable]
        private struct Data
        {
            [field: SerializeField] public List<SavedSceneOrPrefab> List { get; private set; }
            [field: SerializeField] public bool ShowScenes { get; set; }

            public Data(List<SavedSceneOrPrefab> list, bool showScenes)
            {
                List = list;
                ShowScenes = showScenes;
            }

            public readonly SavedSceneOrPrefab this[int i] { get => List[i]; set => List[i] = value; }

            public readonly int Count => List.Count;
        }
    }
}
#endif