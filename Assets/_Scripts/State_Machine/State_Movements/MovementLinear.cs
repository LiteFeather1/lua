using UnityEngine;

public class MovementLinear : MovementState
{
    [SerializeField] private Vector2 _direction;

    public void SetDirection(Vector2 direction) => _direction = direction;

    public override void Do()
    {
        _core.transform.Translate(_speed * Time.deltaTime * _direction);
    }
}
