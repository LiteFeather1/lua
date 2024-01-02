using System;
using TMPro;
using UnityEngine;

namespace LTF
{
    [ExecuteAlways]
    public class FPSCounter : MonoBehaviour
    {
        private const int DIGITS = 2;

        private float _fps;
        private float _ms;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _updateInterval = .5f;

        [Header("Colours")]
        [SerializeField] private Color _red = new(255f, 85f, 85f, 255f);
        [SerializeField] private Color _yellow = new(229, 215f, 74f, 255f);
        [SerializeField] private Color _green = new(0f, 255f, 83f, 255f);

        private float _elaspedIntervalTime;
        private int _intervalFrameCount;

        private void Update()
        {
            _intervalFrameCount++;
            _elaspedIntervalTime += Time.unscaledDeltaTime;

            if (_elaspedIntervalTime >= _updateInterval)
            {
                _fps = (float)Math.Round(_intervalFrameCount / _elaspedIntervalTime, DIGITS);
                _ms = (float)Math.Round(_elaspedIntervalTime * 1000f / _intervalFrameCount, DIGITS);

                _intervalFrameCount = 0;
                _elaspedIntervalTime = 0f;

                _text.text = $"FPS: {_fps}\n{_ms} ms";
                _text.color = _fps switch
                {
                    < 30 => _red,
                    < 60 => _yellow,
                    _ => _green
                };
            }
        }
    }
}
