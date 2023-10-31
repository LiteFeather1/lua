using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private int _shakeCount = 4;
    [SerializeField] private Vector2 _shakeAmount = new(-.12f, .12f);
    [SerializeField] private float _delay = .0675f;
    private IEnumerator _shake;

    [ContextMenu("Shake")]
    public void Shake()
    {
        if (_shake != null)
            return;
        _shake = Shake_CO();
        StartCoroutine(_shake);
    }

    private IEnumerator Shake_CO()
    {
        for (int i = 0; i < _shakeCount; i++)
        {
            transform.localPosition = new(_shakeAmount.Random(), _shakeAmount.Random());
            yield return new WaitForSeconds(_delay);
        }
        transform.localPosition = Vector3.zero;
        _shake = null;
    }
}
