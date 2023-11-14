using UnityEngine;

public class MovementChaseMoveTowards : MovementChase
{
    public override void Do()
    {
        Vector2 target = _target.position;
        if (!_followX)
            target.x = transform.position.x + _xDirection;
        else
            Flip(Mathf.Sign(target.x));

        _core.transform.position = Vector2.MoveTowards(_core.Position, target, _speed * Time.deltaTime);
    }
}
