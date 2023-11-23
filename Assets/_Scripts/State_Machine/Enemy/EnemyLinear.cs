using UnityEngine;

public class EnemyLinear : Enemy
{
    [Header("States")]
    [SerializeField] private MovementLinear _movement;

    private void Start()
    {
        Set(_movement);
    }

    public override void Spawn(float t, float tClamped)
    {
        base.Spawn(t, tClamped);
        _movement.SetSpeed(_data.SpeedRange.Evaluate(tClamped));
    }

    protected override void KnockBackComplete()
    {
        Set(_movement);
        base.KnockBackComplete();
    }
}
