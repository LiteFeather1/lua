using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
#if !UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
#endif

public class SplashScreenManager : MonoBehaviour
{
    const string COLOURIZED = "COLOURIZED";
    const string RAINBOW = "RAINBOW";

    [SerializeField] private TextMeshProUGUI t_version;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI t_message;
    [SerializeField] private string[] _messages;
    [SerializeField] private ValueStringArray _seasonalMessages;
    [SerializeField] private ValueColourArray _rainbowColors;
    private static readonly Dictionary<string, RefValue<byte>> sr_lastMessages = new();
    private static Func<string>[] s_specialMessages;
    private static readonly System.Text.StringBuilder sr_stringBuilder = new();

    public static string RandomMessage { get; private set; }

#if !UNITY_WEBGL && !UNITY_EDITOR
    private bool _hasHTMLTags = false;
    private static IntPtr? s_windowPtr;
#endif

    private void Awake()
    {
        t_version.text = Application.version;

        s_specialMessages ??= new Func<string>[]
        {
            // Seed
            () =>
            {
                var tick = (int)DateTime.Now.Ticks;
                Random.InitState(tick);
                return $"Seed: {tick}";
            },
            // Rainbow order
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                for (int i = 0; i < RAINBOW.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(_rainbowColors.Value[i])}>")
                    .Append(RAINBOW[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Rainbow order
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                for (int i = 0; i < RAINBOW.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom())}>")
                    .Append(RAINBOW[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Rainbow random unique
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                var colours = Enumerable.Range(0, 7).ToArray();
                colours.KnuthShuffle();
                sr_stringBuilder.Clear();
                for (int i = 0; i < RAINBOW.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(_rainbowColors[colours[i]])}>")
                    .Append(RAINBOW[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Colourized random rainbow
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                for (int i = 0; i < COLOURIZED.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom())}>")
                    .Append(COLOURIZED[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Colourized order rainbow
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                var r = Random.Range(0, _rainbowColors.Length);
                for (int i = 0; i < COLOURIZED.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(_rainbowColors[(i + r) % _rainbowColors.Length])}>")
                    .Append(COLOURIZED[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Colourized random HSV
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                for (int i = 0; i < COLOURIZED.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color=#{ColorUtility.ToHtmlStringRGB(Random.ColorHSV())}>")
                    .Append(COLOURIZED[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Colourized random HEX
            () =>
            {
#if !UNITY_WEBGL && !UNITY_EDITOR
                _hasHTMLTags = true;
#endif
                sr_stringBuilder.Clear();
                for (int i = 0; i < COLOURIZED.Length; i++)
                {
                    sr_stringBuilder
                    .Append($"<color={string.Format("#{0:X6}", Random.Range(0, 0x1000000))}>")
                    .Append(COLOURIZED[i])
                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            },
            // Random char
        };

        float randomValue = Random.value;
        if (randomValue <= 0.01f)
            RandomMessage = s_specialMessages.PickRandom().Invoke();
        else if (randomValue > (_seasonalMessages.Length == 1 ? 0f : .5f))
            PickRandom(_messages);
        else
            PickRandom(_seasonalMessages);

        sr_lastMessages.Add(RandomMessage, new(0));
        foreach (var key in sr_lastMessages.Keys.ToArray())
            if (sr_lastMessages[key].Value++ == 4)
                sr_lastMessages.Remove(key);

        RandomMessage = s_specialMessages[3].Invoke();
        t_message.text = RandomMessage;

#if !UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        static extern bool SetWindowText(IntPtr hwnd, string lpString);

        //Get the window handle.
        if (s_windowPtr == null)
             s_windowPtr = FindWindow(null, Application.productName);

        if (_hasHTMLTags)
            RandomMessage = Regex.Replace(RandomMessage, "<.*?>", string.Empty);

        SetWindowText(s_windowPtr.Value, $"Lua - {RandomMessage}");
#endif

        static void PickRandom(string[] messages)
        {
            do
                RandomMessage = messages.PickRandom();
            while (sr_lastMessages.ContainsKey(RandomMessage));
        }
    }

    private int a = 0;
    [ContextMenu("Test")]
    private void Test()
    {
        RandomMessage = s_specialMessages[a++ % s_specialMessages.Length].Invoke();
        t_message.text = RandomMessage;
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
        _messages = (Resources.Load("Messages") as TextAsset).text
            .Split("\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        EditorUtility.SetDirty(this);
    }
#endif
}
