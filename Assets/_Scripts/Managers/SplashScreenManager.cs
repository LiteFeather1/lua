using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    [SerializeField] private List<string> _messages;
    private static readonly Dictionary<string, RefValue<byte>> sr_lastMessages = new();

    private static IntPtr? s_windowPtr;

    public static string S_RandomMessage { get; private set; }

    private void Awake()
    {
        t_version.text = Application.version;
        do
            S_RandomMessage = _messages.PickRandom();
        while (sr_lastMessages.ContainsKey(S_RandomMessage));

        sr_lastMessages.Add(S_RandomMessage, new(0));
        foreach (var key in sr_lastMessages.Keys.ToArray())
        {
            if (sr_lastMessages[key].Value++ == 4)
                sr_lastMessages.Remove(key);
        }

#if !UNITY_WEBGL || true
        //Import the following.
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        static extern bool SetWindowText(IntPtr hwnd, string lpString);

        //Get the window handle.
        if (s_windowPtr == null)
             s_windowPtr = FindWindow(null, Application.productName);
        SetWindowText(s_windowPtr.Value, $"Lua - {S_RandomMessage}");
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
            .Where(x => !string.IsNullOrWhiteSpace(x));

        _messages = new(messages);
        EditorUtility.SetDirty(this);
    }
#endif
}
