using System.Collections;
using UnityEngine;
using LTFUtils;

namespace Lua.Managers
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float _delay = .0675f;

        [Header("Small Shake")]
        [SerializeField] private int _shakeCount = 5;
        [SerializeField] private Vector2 _shakeAmount = new(-.01f, .01f);

        [Header("Strong Shake")]
        [SerializeField] private int _shakeCountStrong = 4;
        [SerializeField] private Vector2 _shakeAmountStrong = new(-.04f, .04f);
        private bool _isStrongShaking;

        private IEnumerator _shake;
        private WaitForSeconds _waitDelay;

        private void Awake()
        {
            _waitDelay = new(_delay);
        }

        [ContextMenu("Shake")]
        public void Shake()
        {
            if (_shake != null)
                return;

            _shake = Shake_CO(_shakeCount, _shakeAmount);
            StartCoroutine(_shake);
        }

        [ContextMenu("Strong Shake")]
        public void ShakeStrong()
        {
            if (_isStrongShaking)
                return;

            _isStrongShaking = true;
            if (_shake != null)
                StopCoroutine(_shake);

            _shake = StrongShake();
            StartCoroutine(_shake);
        }

        private IEnumerator Shake_CO(int shakeCount, Vector2 shakeAmount)
        {
            for (int i = 0; i < shakeCount; i++)
            {
                transform.localPosition = new(shakeAmount.Random(), shakeAmount.Random());
                yield return _waitDelay;
            }

            transform.localPosition = Vector3.zero;
            _shake = null;
        }

        private IEnumerator StrongShake()
        {
            yield return Shake_CO(_shakeCountStrong, _shakeAmountStrong);
            _isStrongShaking = false;
        }
    }
}
