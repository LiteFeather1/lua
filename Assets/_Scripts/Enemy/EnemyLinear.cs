using UnityEngine;

public class EnemyLinear : Enemy
{
    [SerializeField] private MovementLinear _movement;

    public override void Spawn(float t)
    {
        base.Spawn(t);
        _movement.SetSpeed(Mathf.Lerp(_speedRange.x, _speedRange.y, t));
    }
}
