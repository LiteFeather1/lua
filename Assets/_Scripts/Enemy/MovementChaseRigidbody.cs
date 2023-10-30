using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementChaseRigidbody : MovementChase
{
    [SerializeField] private Rigidbody2D _rb;

    private void FixedUpdate()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        if (!_followX)
            direction.x = _xDirection;
        else
            Flip(Mathf.Sign(direction.x));
        _rb.velocity = _speed * direction;
    }
}
