using UnityEngine;
using System;

namespace LTF.Timers
{
    public abstract class TimerBehaviour<T> : MonoBehaviour, ITimer where T : ITimer
    {
        [SerializeField] private T _timer;

        public Action TimeEvent { get => _timer.TimeEvent; set => _timer.TimeEvent = value; }

        public float TimeToDo => _timer.TimeToDo;
        public float ElapsedTime => _timer.ElapsedTime;
        public bool CanTick => _timer.CanTick;

        public void SetDeltaMultiplier(float multipler) => _timer.SetDeltaMultiplier(multipler);
        public void ChangeTime(float time) => _timer.ChangeTime(time);
        public void SetTime(float time) => _timer.SetTime(time);

        private void Update() => _timer.Tick();

        public void Tick() => _timer.Tick();

        public void Continue()
        {
            enabled = true;
            _timer.Continue();
        }

        public void Stop()
        {
            enabled = false;
            _timer.Stop();
        }

        public void Reset_() => _timer.Reset_();
    }
 }