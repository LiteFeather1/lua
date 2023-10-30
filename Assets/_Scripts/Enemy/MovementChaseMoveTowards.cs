using UnityEngine;

public class MovementChaseMoveTowards : MovementChase
{
    private void Update()
    {
        Vector2 target = _target.position;
        if (!_followX)
            target.x = transform.position.x + _xDirection;
        else
            Flip(Mathf.Sign(target.x));
        transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    }
}
