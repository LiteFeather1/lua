using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using LTF.RefValue;
using LTF.Utils;
using Seasonal;
#if !UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using Random = UnityEngine.Random;

namespace Lua.Managers
{
    public class SplashScreenManager : MonoBehaviour
    {
        private const string COLOURIZED = "COLOURIZED";
        private const string RAINBOW = "RAINBOW";
        private const string CHARACTERS = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^& ";
        private const int MAX_NON_REPEATING = 8;

        [SerializeField] private TextMeshProUGUI t_version;

        [Header("Message")]
        [SerializeField] private TextMeshProUGUI t_message;
        [SerializeField] private string[] _messages;
        [SerializeField] private SeasonalStringArray _seasonalMessages;
        [SerializeField] private LTF.ValueGeneric.ValueColourArray _rainbowColors;
        private readonly Dictionary<string, RefValue<byte>> r_lastMessages = new();
        private static readonly List<string> sr_messages = new();
        private static Func<string>[] s_specialMessages;
        private static readonly System.Text.StringBuilder sr_stringBuilder = new();

        [Header("Message Animation")]
        [SerializeField] private Transform _messageRoot;
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private float _scaleMagnitude;
        [SerializeField] private float _rotSpeed;
        [SerializeField] private float _rotMagnitude;

        public static string RandomMessage { get; private set; } = "Last Message";

#if !UNITY_WEBGL && !UNITY_EDITOR
        private static IntPtr? s_windowPtr;
#endif

        private void Awake()
        {
            t_version.text = Application.version;

            // Initialization
            if (sr_messages.Count == 0)
            {
                sr_messages.AddRange(_messages);

                sr_messages.Add(DateTime.Now.Hour switch
                {
                    int hour when hour >= 4 && hour < 12 => "Good Morning!",
                    int hour when hour >= 12 && hour < 19 => "Good Afternoon!",
                    _ => "Good Evening!",
                });

                sr_messages.Add(Application.version);
                sr_messages.Add(Application.companyName);
                sr_messages.Add(Application.productName);
                sr_messages.Add(Application.genuine ? "It's Genuine" : "Modding? Alone?");

                sr_messages.Add($"Session {PlayerPrefsHelper.GetSessions()}");

                s_specialMessages = new Func<string>[]
                {
                // Seed
                () =>
                {
                    var tick = (int)DateTime.Now.Ticks;
                    Random.InitState(tick);
                    return $"Seed: {tick}";
                },

                 // Rainbow order
                () => ColourizedStringChars(RAINBOW, (i) => ColorUtility.ToHtmlStringRGB(_rainbowColors.Value[i])),
                // Rainbow order random
                () =>
                {
                    var r = Random.Range(0, _rainbowColors.Length);
                    return ColourizedStringChars(RAINBOW, (i) => RandomRainbowColourMod(i, r));
                },
                // Rainbow random
                () => ColourizedStringChars(RAINBOW, _ => ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom())),
                // Rainbow random unique
                () =>
                {
                    var colours = Enumerable.Range(0, 7).ToArray();
                    colours.KnuthShuffle();
                    return ColourizedStringChars(RAINBOW, (i) => ColorUtility.ToHtmlStringRGB(_rainbowColors[colours[i]]));
                },

                // Colourized random rainbow
                () => ColourizedStringChars(COLOURIZED, _ => ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom())),
                // Colourized order rainbow
                () =>
                {
                    var r = Random.Range(0, _rainbowColors.Length);
                    return ColourizedStringChars(COLOURIZED, (i) => RandomRainbowColourMod(i, r));
                },
                // Colourized random HSV
                () => ColourizedStringChars(COLOURIZED, _ => ColorUtility.ToHtmlStringRGB(Random.ColorHSV())),
                // Colourized random HEX
                () => ColourizedStringChars(COLOURIZED, _ => RandomHexColour()),

                // Random char
                () => RandomCharString(),
                () => ColourizedStringChars(RandomCharString(), _ => RandomHexColour()),
                () => ColourizedStringChars(RandomCharString(), _ => ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom())),

                // Scramble
                () =>
                {
                    TryRemoveHTMLTags();

                    var charArray = RandomMessage.ToCharArray();
                    charArray.KnuthShuffle();
                    return new(charArray);
                },
                // Scramble split
                () =>
                {
                    TryRemoveHTMLTags();

                    var splits = RandomMessage.Split(' ');
                    sr_stringBuilder.Clear();
                    for (int i = 0; i < splits.Length; i++)
                    {
                       var charArray = splits[i].ToCharArray();
                       charArray.KnuthShuffle();
                       sr_stringBuilder.Append(charArray).Append(' ');
                    }

                    return sr_stringBuilder.ToString();
                },

                // Random Hexcolour
                () =>
                {
                    var hexColour = RandomHexColour();
                    return sr_stringBuilder.Clear()
                                           .Append("<color=#").Append(hexColour).Append('>')
                                           .Append('#').Append(hexColour)
                                           .Append("</color>")
                                           .ToString();
                },
                // Rainbow Hexcolour
                () =>
                {
                    var hexColour = ColorUtility.ToHtmlStringRGB(_rainbowColors.PickRandom());
                    return sr_stringBuilder.Clear()
                                           .Append("<color=#").Append(hexColour).Append('>')
                                           .Append('#').Append(hexColour)
                                           .Append("</color>")
                                           .ToString();
                },

                // StartUp time
                () => ConvertToHourMinuteSeconds("Since Start up: ", (float)Time.realtimeSinceStartupAsDouble),

                // Sessions
                () => $"Sessions: {PlayerPrefsHelper.GetSessions()}",

                // Player Time
                () => ConvertToHourMinuteSeconds("Play time: ", PlayerPrefsHelper.GetPlayTime()),
                // Runs Played
                () => $"Runs: {PlayerPrefsHelper.GetRuns()}",
                // Enemies Killed
                () => $"Enemies Killed: {PlayerPrefsHelper.GetEnemiesKilled()}",
                // Cards Played
                () => $"Cards Played: {PlayerPrefsHelper.GetCardsPlayed()}",
                // Cards Recycled
                () => $"Cards Recycled: {PlayerPrefsHelper.GetCardsRecycled()}",
                // Candy Eearned
                () => $"Candy Earned: {PlayerPrefsHelper.GetCandyEarned()}",

                // Lua Binary colored
                () =>
                {
                    t_message.fontSizeMin = t_message.fontSizeMax;
                    return sr_stringBuilder.Clear()
                                           .Append("<color=#").Append("ED8511").Append('>').Append("01001100").AppendLine()
                                           .Append("<color=#").Append("E8E6E1").Append('>').Append("01010101").AppendLine()
                                           .Append("<color=#").Append("ED8511").Append('>').Append("01000001").AppendLine()
                                           .Append("</color>")
                                           .ToString();
                },
                // Lua binary meaning
                () =>
                {
                    t_message.fontSizeMin = t_message.fontSizeMax;
                    return sr_stringBuilder.Clear()
                                           .Append("L = ").Append("01001100").AppendLine()
                                           .Append("U = ").Append("01010101").AppendLine()
                                           .Append("A = ").Append("01000001").AppendLine()
                                           .ToString();
                },
                // Lua binary meaning colored
                () =>
                {
                    t_message.fontSizeMin = t_message.fontSizeMax;
                    return sr_stringBuilder.Clear()
                                           .Append("<color=#").Append("ED8511").Append('>').Append("L = ").Append("01001100").AppendLine()
                                           .Append("<color=#").Append("E8E6E1").Append('>').Append("U = ").Append("01010101").AppendLine()
                                           .Append("<color=#").Append("ED8511").Append('>').Append("A = ").Append("01000001").AppendLine()
                                           .Append("</color>")
                                           .ToString();
                },
                };

                var possibilities = sr_messages.Count + _seasonalMessages.Default.Length + s_specialMessages.Length + 2;
                foreach (var value in _seasonalMessages.Dictionary.Values)
                    possibilities += value.Length;

                sr_messages.Add($"{possibilities} Possibilities ");
                sr_messages.Add($"1 in {possibilities}");
            }

            // Load Messages
            var s = PlayerPrefsHelper.GetMessages();
            if (!string.IsNullOrEmpty(s))
            {
                var lastMessages = JsonUtility.FromJson<ArrayWrapper>(s).LastMessages;
                for (var i = 0; i < lastMessages.Length; i++)
                    r_lastMessages.Add(lastMessages[i].Message, new(lastMessages[i].Amount));
            }

            var randomValue = Random.value;
            const float MIN = .05f;
            if (randomValue <= MIN)
                RandomMessage = s_specialMessages.PickRandom()();
            else if (randomValue > (_seasonalMessages.ToSet.Length <= 1 ? MIN : .5f - MIN))
                PickRandomDefaultMessage();
            else
            {
                var i = 0;
                var picked = false;
                do
                {
                    RandomMessage = _seasonalMessages.ToSet.PickRandom();
                    if (!r_lastMessages.ContainsKey(RandomMessage))
                    {
                        picked = true;
                        break;
                    }
                }
                while (++i < _seasonalMessages.ToSet.Length);
                
                if (!picked)
                    PickRandomDefaultMessage();
            }

            r_lastMessages.Add(RandomMessage, new(0));
            foreach (var key in r_lastMessages.Keys.ToArray())
                if (r_lastMessages[key].Value++ == MAX_NON_REPEATING)
                    r_lastMessages.Remove(key);

            t_message.text = RandomMessage;

#if !UNITY_WEBGL && !UNITY_EDITOR
            [DllImport("user32.dll", EntryPoint = "FindWindow")]
            static extern IntPtr FindWindow(string className, string windowName);
            [DllImport("user32.dll", EntryPoint = "SetWindowText")]
            static extern bool SetWindowText(IntPtr hwnd, string lpString);

            //Get the window handle.
            if (s_windowPtr == null)
                 s_windowPtr = FindWindow(null, Application.productName);

            TryRemoveHTMLTags();

            SetWindowText(s_windowPtr.Value, $"Lua - {RandomMessage}");
#endif

            // Save Last Messages
            {
                var toSave = new LastMessage[r_lastMessages.Count];
                var i = 0;
                foreach (var pair in r_lastMessages)
                    toSave[i++] = new(pair.Key, pair.Value);
                PlayerPrefsHelper.SaveMessages(JsonUtility.ToJson(new ArrayWrapper(toSave)));
                PlayerPrefsHelper.Save();
            }

            #region Local Function

            string ColourizedStringChars(string s, Func<int, string> colour)
            {
                sr_stringBuilder.Clear();
                for (int i = 0; i < s.Length; ++i)
                {
                    sr_stringBuilder.Append($"<color=#").Append(colour(i)).Append('>')
                                    .Append(s[i])
                                    .Append("</color>");
                }
                return sr_stringBuilder.ToString();
            }

            string RandomRainbowColourMod(int i, int r)
            {
                return ColorUtility.ToHtmlStringRGB(_rainbowColors[(i + r) % _rainbowColors.Length]);
            }

            static string RandomHexColour() => string.Format("{0:x6}", Random.Range(0, 0x1000000));

            static string RandomCharString()
            {
                var chars = new char[Random.Range(12, 27)];
                for (int i = 0; i < chars.Length; i++)
                    chars[i] = CHARACTERS.PickRandom();
                return new(chars);
            }

            static void TryRemoveHTMLTags()
            {
                if (Regex.IsMatch(RandomMessage, "<.*?>"))
                    RandomMessage = Regex.Replace(RandomMessage, "<.*?>", string.Empty);
            }

            static string ConvertToHourMinuteSeconds(string s, float time)
            {
                var hours = Mathf.FloorToInt(time / 3600f);
                var minutes = Mathf.FloorToInt(time / 60f % 60f);
                var seconds = Mathf.FloorToInt(time % 60f);

                sr_stringBuilder.Clear().Append(s);

                if (hours >= 1)
                    sr_stringBuilder.Append(hours).Append(':');

                return sr_stringBuilder.Append(minutes.ToString("00")).Append(':')
                                       .Append(seconds.ToString("00"))
                                       .ToString();
            }

            void PickRandomDefaultMessage()
            {
                do
                    RandomMessage = sr_messages.PickRandom();
                while (r_lastMessages.ContainsKey(RandomMessage));
            }
        #endregion
        }

        private void Update()
        {
            var time = Time.time;
            var sinScale = 1f + (Mathf.Sin(time * _scaleSpeed) * _scaleMagnitude);
            _messageRoot.localScale = new(sinScale, sinScale, sinScale);

            var sinRot = Mathf.Sin(time * _rotSpeed) * _rotMagnitude;
            _messageRoot.localRotation = Quaternion.Euler(0f, 0f, sinRot);
        }

        public void ButtonPlay() => SceneManager.LoadSceneAsync(1);

        public void ButtonQuit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

#if UNITY_EDITOR
        [ContextMenu("Go thru all Slowly")]
        private void GoThruAllSlowly() => StartCoroutine(GoThruAll_CO());
        private IEnumerator GoThruAll_CO()
        {
            YieldInstruction y = new WaitForSeconds(.5f);
            for (int i = 0; i < sr_messages.Count; i++)
            {
                t_message.text = RandomMessage = sr_messages[i];
                yield return y;
            }

            for (int i = 0; i < s_specialMessages.Length; i++)
            {
                t_message.text = RandomMessage = s_specialMessages[i]();
                yield return y;
            }

            for (int i = 0; i < _seasonalMessages.ToSet.Length; i++)
            {
                t_message.text = RandomMessage = _seasonalMessages.ToSet[i];
                yield return y;
            }
        }

        [ContextMenu("Get Messages")]
        private void GetMessages()
        {
            var fileMessages = (Resources.Load("Messages") as TextAsset).text
                               .Split("\n")
                               .Where(x => !string.IsNullOrWhiteSpace(x))
                               .ToArray();

            var otherMessages = new List<string>();

            var scriptGUID = AssetDatabase.FindAssets($"t:script", new string[] { "Assets/_Scripts" });
            otherMessages.Add($"{scriptGUID.Length} Scripts");

            var lines = 0;
            var characters = 0;
            for (int i = 0;i < scriptGUID.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(scriptGUID[i]);
                lines += System.IO.File.ReadAllLines(path).Length;
                characters += System.IO.File.ReadAllText(path).Length;
            }
            otherMessages.Add($"{lines} Lines");
            otherMessages.Add($"{characters} Characters");

            _messages = new string[fileMessages.Length + otherMessages.Count];
            fileMessages.CopyTo(_messages, 0);
            otherMessages.CopyTo(_messages, fileMessages.Length);

            EditorUtility.SetDirty(this);
        }
#endif

        [Serializable]
        private struct LastMessage
        {
            public string Message;
            public byte Amount;

            public LastMessage(string message, byte amount)
            {
                Message = message;
                Amount = amount;
            }
        }

        [Serializable]
        private struct ArrayWrapper
        {
            public LastMessage[] LastMessages;

            public ArrayWrapper(LastMessage[] lastMessages) => LastMessages = lastMessages;
        }
    }
}
