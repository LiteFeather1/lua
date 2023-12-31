using UnityEngine;

namespace LTF.Timers
{
    [System.Serializable]
    public class TimerRange : Timer
    {
        [SerializeField] private Vector2 _range;
        private float _time;

        public TimerRange(Vector2 range,
                          bool canTick = true,
                          TimeType timeType = TimeType.DeltaTime,
                          float elapsedTime = 0f,
                          float deltaMultiplier = 1f)
            : base((range.x + range.y) * .5f, canTick, timeType, elapsedTime, deltaMultiplier)
        {
            TimeEvent += SetTime;
        }

        public TimerRange() : this(Vector2.zero) { }

        ~TimerRange()
        {
            TimeEvent -= SetTime;
        }

        public override float TimeToDo { get => _time; protected set => _time = value; }
        public Vector2 Range { get => _range; set => _range = value; }

        private void SetTime() => _time = Random.Range(_range.x, _range.y);
    }
}