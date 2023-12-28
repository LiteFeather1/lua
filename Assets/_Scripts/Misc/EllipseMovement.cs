using UnityEngine;

namespace Lua.Misc
{
    public class EllipseMovement : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _width = 1f;
        [SerializeField] private float _height = 1f;
        [SerializeField] private float _direction = -1f;
        [SerializeField] private float _startTime = 0f;
        [SerializeField] private float _timeToComplete = 900f;
        private float _elapsedTime;
        [SerializeField, Range(-1f, 1f)] private float _t;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _transform.localPosition = Ellipse(_t);
            if (gameObject == UnityEditor.Selection.activeGameObject)
                print($"Time : {_timeToComplete * ((_t + 1f) * .5f)}");
        }
#endif

        private void Start()
        {
            _elapsedTime = _startTime;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
             _t = (_elapsedTime / (_timeToComplete * .5f)) - 1f;
            _transform.localPosition = Ellipse(_t);

            if (_t > 1f)
                _elapsedTime = 0f;
        }

        private Vector2 Ellipse(float t)
        {
            float y = _height * _height * (1 - t * t / _width * _width);
            return new(_width * t * _direction, y);
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.color = Color.yellow;
        //    Vector2 pos = transform.position;
        //    for (int i = 1; i < 150; i++)
        //    {
        //        Gizmos.DrawWireSphere(pos + Ellipse(-1f + (float)i / 75f), .04f);
        //    }
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawWireSphere(pos + Ellipse(_t), .04f);
        //}
    }
}
