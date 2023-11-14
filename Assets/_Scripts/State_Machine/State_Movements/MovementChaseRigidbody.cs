using UnityEngine;

public class MovementChaseRigidbody : MovementChase
{
    [SerializeField] private Rigidbody2D _rb;

    public override void FixedDo()
    {
        Vector2 direction = ((Vector2)_target.position - _core.Position).normalized;
        if (!_followX)
            direction.x = _xDirection;
        else
            Flip(Mathf.Sign(direction.x));
        _rb.velocity = _speed * direction;
    }
}
