using UnityEngine;

public class MovementChaseRigidbody : MovementChase
{
    [SerializeField] private Rigidbody2D _rb;

    public override void FixedDo()
    {
        var direction = (Vector2)_target.position - _core.Position;

        if (!_followX)
            direction.x = _xDirection;
        else
            Flip(Mathf.Sign(direction.x));

        direction.y *= _yFollowMultiplier;

        _rb.velocity = _speed * direction.normalized;
    }
}
