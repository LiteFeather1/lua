
public interface IDamageEffect
{
    public float Chance { get; }
    public void Tick(IDamageable damageable, float delta);
}

