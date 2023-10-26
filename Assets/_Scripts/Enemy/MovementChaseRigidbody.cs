using UnityEngine;

public class MovementChaseRigidbody : MovementChase
{
    [SerializeField] private Rigidbody2D _rb;

    private void FixedUpdate()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        if (!_followX)
            direction.x = _xDirection;

        _rb.velocity = _speed * direction;
    }
}
