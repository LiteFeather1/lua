
public interface IDamageable
{
    public float MaxHP { get; }
    public float HP { get; }

    public bool TakeDamage(float damage);
    public void Heal(float Heal);
}

