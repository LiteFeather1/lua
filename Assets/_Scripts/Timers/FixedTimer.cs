using UnityEngine;

namespace LTFUtils
{
    public class FixedTimer : Timer
    {
        [SerializeField] private float _time;
        public override float TimeToDo { get => _time; protected set => _time = value; }
    }
}