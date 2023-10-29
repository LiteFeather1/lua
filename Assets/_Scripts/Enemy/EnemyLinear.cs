using UnityEngine;

public class EnemyLinear : Enemy
{
    [SerializeField] private MovementLinear _movement;

    public override void Spawn(float t, float tClamped)
    {
        base.Spawn(t, tClamped);
        _movement.SetSpeed(_speedRange.Evaluate(tClamped));
    }
}
