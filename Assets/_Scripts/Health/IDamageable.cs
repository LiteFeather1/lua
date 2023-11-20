using UnityEngine;

public interface IDamageable
{
    public float MaxHP { get; }
    public float HP { get; }
    public Vector2 Pos { get; } 

    public bool TakeDamage(float damage, float knockback, bool crit, Vector2 pos = default);
    public void Heal(float Heal);
    public bool TryAddDamageEffect(IDamageEffect effect);
}
