public interface IDamageEffect
{
    public int ID { get; }
    public bool Tick(IDamageable health, float delta);
}
