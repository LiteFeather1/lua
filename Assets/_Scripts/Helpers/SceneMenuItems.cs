#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;

namespace LTFUtils
{
    internal static class SceneMenuItems
    {
        [MenuItem("Scene Go/Splash Screen", priority = 1)]
        private static void LoadSplash()
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene("Assets/Scenes/Splash_Screen.unity");
        }

        [MenuItem("Scene Go/Main")]
        private static void LoadMain()
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        }
    }
}
#endif
