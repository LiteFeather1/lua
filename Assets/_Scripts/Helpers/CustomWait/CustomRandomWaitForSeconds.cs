using System.Collections;
using UnityEngine;
using LTF.Utils;

namespace LTF.CustomWaits
{
    [System.Serializable]
    public class CustomRandomWaitForSeconds : IEnumerator
    {
        [SerializeField] private Vector2 _randomWaitTime;
        private float _waitTime;
        private float _elapsedTime = 0f;

        public CustomRandomWaitForSeconds() { }

        public CustomRandomWaitForSeconds(Vector2 randomWaitTime)
        {
            _randomWaitTime = randomWaitTime;
            _waitTime = (randomWaitTime.x + randomWaitTime.y) * .5f;
        }

        public object Current => null;

        public bool MoveNext()
        {
            _elapsedTime += Time.deltaTime;
            bool flag = _elapsedTime < _waitTime;
            if (!flag)
                Reset();
            return flag;
        }

        public void Reset()
        {
            _elapsedTime = 0f;
            _waitTime = _randomWaitTime.Random();
        }
    }
}
