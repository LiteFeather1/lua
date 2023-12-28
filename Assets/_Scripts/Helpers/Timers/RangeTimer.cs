using UnityEngine;

namespace LTF.Timers
{
    public class RangeTimer : Timer
    {
        [SerializeField] private Vector2 _range;
        private float _time;

        public override float TimeToDo { get => _time; protected set => _time = value; }
        public Vector2 Range { get => _range; set => _range = value; }

        private void Start()
        {
            SetTime();
            TimeEvent += SetTime;
        }

        private void SetTime()
        {
            _time = Random.Range(_range.x, _range.y);
        }
    }
}