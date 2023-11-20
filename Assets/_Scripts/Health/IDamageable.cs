using UnityEngine;

public interface IDamageable
{
    public float MaxHP { get; }
    public float HP { get; }

    public bool TakeDamage(float damage, float knockback, bool crit, Vector2? pos);
    public void Heal(float Heal);
    public void TryAddDamageEffect(IDamageEffect effect);
}
