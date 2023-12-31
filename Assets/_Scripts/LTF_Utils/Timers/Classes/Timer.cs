using UnityEngine;
using System;

namespace LTF.Timers
{
    [Serializable]
    public abstract class Timer : ITimer
    {
        [SerializeField] private bool _canTick = true;
        [SerializeField] private TimeType _timeType = TimeType.DeltaTime;
        private float _elapsedTime = 0f;
        private float _deltaMultiplier = 1f;

        protected Timer(float time,
                        bool canTick,
                        TimeType timeType,
                        float elapsedTime,
                        float deltaMultiplier)
        {
            TimeToDo = time;
            _canTick = canTick;
            _timeType = timeType;
            _elapsedTime = elapsedTime;
            _deltaMultiplier = deltaMultiplier;
            TimeEvent = delegate { };
        }

        public Action TimeEvent { get; set; }

        public abstract float TimeToDo { get; protected set; }
        public float ElapsedTime => _elapsedTime;
        public bool CanTick => _canTick;

        public float T => _elapsedTime / TimeToDo;

        public void Tick()
        {
            if (!_canTick)
                return;

            _elapsedTime += _deltaMultiplier * _timeType switch
            {
                TimeType.DeltaTime => Time.deltaTime,
                TimeType.UnscaledDeltaTime => Time.unscaledDeltaTime,
                _ => Time.deltaTime,
            };

            if (_elapsedTime < TimeToDo)
                return;

            Reset_();
            TimeEvent?.Invoke();
        }

        public void SetDeltaMultiplier(float multipler) => _deltaMultiplier = multipler;

        public void ChangeTime(float time) => TimeToDo += time;

        public void SetTime(float time) => TimeToDo = time;

        public void Stop() => _canTick = false;

        public void Continue() => _canTick = true;

        public void Reset_() => _elapsedTime = 0f;
    }
 }