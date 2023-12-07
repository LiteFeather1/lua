using UnityEditor.SceneManagement;
using UnityEditor;

#if UNITY_EDITOR
internal static class SceneMenuItems
{
    [MenuItem("Scene Go/Splash Screen")]
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
#endif
