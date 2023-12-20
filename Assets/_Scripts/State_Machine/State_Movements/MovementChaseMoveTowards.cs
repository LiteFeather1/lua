using UnityEngine;

public class MovementChaseMoveTowards : MovementChase
{
    public override void Do()
    {
        var direction = (Vector2)_target.position - _core.Position;

        if (!_followX)
            direction.x = _xDirection;
        else
            Flip(Mathf.Sign(direction.x));

        //direction.y *= _yFollowMultiplier;

        _core.transform.Translate(Time.deltaTime * _speed * direction.normalized);
    }
}
