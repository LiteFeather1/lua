using UnityEngine;

namespace LTF.Timers
{
    [System.Serializable]
    public class TimerFixed : Timer
    {
        [SerializeField] private float _time;

        public TimerFixed(float time,
                          bool canTick = true,
                          TimeType timeType = TimeType.DeltaTime,
                          float elapsedTime = 0f,
                          float deltaMultiplier = 1f)
        : base(time, canTick, timeType, elapsedTime, deltaMultiplier) { }

        public TimerFixed() : this(0f) { }

        public override float TimeToDo { get => _time; protected set => _time = value; }

    }
}