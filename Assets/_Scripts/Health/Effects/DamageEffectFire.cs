public class DamageEffectFire : DamageEffect
{
    private readonly float _damage;
    private readonly float _tickTime;
    private int _ticks;

    public override int ID => 4153;

    public DamageEffectFire(float duration, float damage, int tickAmount) : base(duration)
    {
        _damage = damage;
        _tickTime = duration / tickAmount;
        _ticks = 0;
    }

    public override bool Tick(IDamageable health, float delta)
    {
        if (_elapsedTime > _tickTime * _ticks)
        {
            _ticks++;
            health.TakeDamage(_damage, 0f, false, null);
        }

        return base.Tick(health, delta);
    }
}
