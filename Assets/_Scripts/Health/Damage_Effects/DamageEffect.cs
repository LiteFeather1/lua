
public abstract class DamageEffect : IDamageEffect
{
    protected float _duration;
    protected float _elapsedTime;

    public abstract int ID { get; }

    public DamageEffect(float duration)
    {
        _duration = duration;
    }

    public virtual bool Tick(IDamageable damageable, float delta)
    {
        _elapsedTime += delta;
        if (_elapsedTime > _duration)
            return true;

        return false;
    }
}
