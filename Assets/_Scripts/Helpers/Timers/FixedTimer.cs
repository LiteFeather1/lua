using UnityEngine;

namespace LTF.Timers
{
    public class FixedTimer : Timer
    {
        [SerializeField] private float _time;
        public override float TimeToDo { get => _time; protected set => _time = value; }
    }
}