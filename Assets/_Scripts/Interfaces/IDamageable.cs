
public interface IDamageable
{
    public int MaxHP { get; }
    public int HP { get; }

    public bool TakeDamage(float damage, IDamageEffect damageEffect);
}

