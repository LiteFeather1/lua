using UnityEngine;

public class EnemyChase : Enemy
{
    [Header("States")]
    [SerializeField] private MovementChase _movement;

    private void Start() => Set(_movement);

    public override void Init(Witch witch)
    {
        base.Init(witch);
        _movement.SetTarget(witch.transform);
    }

    public override void Spawn(float t, float tClamped)
    {
        base.Spawn(t, tClamped);
        _movement.SetSpeed(_data.Speed(tClamped));
    }

    protected override void KnockBackComplete()
    {
        Set(_movement);
        base.KnockBackComplete();
    }
}
