using System.Runtime.InteropServices;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI t_version;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI t_message;
    [SerializeField] private string[] _messages;



    private static IntPtr? s_windowPtr;
    private static string s_windowName = "Lua";
    private static string s_randomMessage = "";

    private void Awake()
    {
        t_version.text = Application.version;
        s_randomMessage = _messages.PickRandom();

#if !UNITY_WEBGL || true
        //Import the following.
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        static extern bool SetWindowText(IntPtr hwnd, string lpString);

        //Get the window handle.
        if (s_windowPtr == null)
             s_windowPtr = FindWindow(null, s_windowName);
        s_windowName = $"Lua - {s_randomMessage}";
        SetWindowText(s_windowPtr.Value, s_windowName);
#endif
    }

    public void ButtonPlay() => SceneManager.LoadScene(1);

    public void ButtonQuit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

#if UNITY_EDITOR
    [ContextMenu("Get Messages")]
    private void GetMessages()
    {
        var messages = (Resources.Load("Messages") as TextAsset).text
            .Split("\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        _messages = messages;
    }
#endif
}
