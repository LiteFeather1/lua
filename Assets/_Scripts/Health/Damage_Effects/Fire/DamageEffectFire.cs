public class DamageEffectFire : DamageEffect
{
    private readonly float _damage;
    private readonly float _tickRate;
    private float _lastTickTime;

    public override int ID => (int)IDamageEffect.DamageEffectID.FIRE_ID;

    public DamageEffectFire(float duration, float damage, float tickRate) : base(duration)
    {
        _damage = damage;
        _tickRate =  tickRate;
    }

    public override bool Tick(IDamageable damageable, float delta)
    {
        if (_elapsedTime - _lastTickTime > _tickRate)
        {
            _lastTickTime += _tickRate;
            damageable.TakeDamage(_damage, 0f, false, damageable.Pos);
        }

        return base.Tick(damageable, delta);
    }
}
