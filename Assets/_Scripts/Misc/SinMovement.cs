using UnityEngine;

public class SinMovement : MonoBehaviour
{
    [SerializeField] private Transform _transformToMove;
    [SerializeField] private float _sinAmplitude = 0.08f;
    [SerializeField] private float _speed = 1;
    private float _offset;

    private void Start() => _offset = Random.value * 100f;

    private void Update()
    {
        var y = Mathf.Sin((Time.time + _offset) * _speed) * _sinAmplitude;
        _transformToMove.localPosition = new(_transformToMove.localPosition.x, y);
    }
}
