using UnityEngine;

public class EnemyChase : Enemy
{
    [SerializeField] private MovementChase _movement;

    public override void Init(Witch witch)
    {
        base.Init(witch);
        _movement.SetTarget(witch.transform);
    }

    public override void Spawn(float t, float tClamped)
    {
        base.Spawn(t, tClamped);
        _movement.SetSpeed(_speedRange.Evaluate(tClamped));
    }
}
