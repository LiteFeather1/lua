using UnityEngine;

public class EnemyChase : Enemy
{
    [SerializeField] private MovementChase _movement;

    public override void Init(Witch witch)
    {
        base.Init(witch);
        _movement.SetTarget(witch.transform);
    }

    public override void Spawn(float t)
    {
        base.Spawn(t);
        _movement.SetSpeed(Mathf.Lerp(_speedRange.x, _speedRange.y, t));
    }
}
