using System.Collections;
using UnityEngine;

namespace LTF.CustomWaits
{
    [System.Serializable]
    public class CustomWaitForSeconds : IEnumerator
    {
        [SerializeField] private float _waitTime;
        private float _elapsedTime = 0f;

        public CustomWaitForSeconds() { }

        public CustomWaitForSeconds(float waitTime) => _waitTime = waitTime;

        public object Current => null;

        public bool MoveNext()
        {
            _elapsedTime += Time.deltaTime;
            bool flag = _elapsedTime < _waitTime;
            if (!flag)
                Reset();
            return flag;
        }

        public void Reset() => _elapsedTime = 0f;

        public void SetWaitTime(float waitTime) => _waitTime = waitTime;
    }
}
