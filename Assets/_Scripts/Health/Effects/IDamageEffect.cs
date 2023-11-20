public interface IDamageEffect
{
    public enum DamageEffectID
    {
        FIRE_ID = 4153,
    }

    public int ID { get; }
    public bool Tick(IDamageable health, float delta);
}
