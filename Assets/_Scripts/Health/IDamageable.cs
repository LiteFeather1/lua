using UnityEngine;
using CompositeValues;

public interface IDamageable
{
    public float MaxHP { get; }
    public float Hp { get; }
    public CompositeValue Defence { get; }
    public Vector2 Pos { get; } 

    public bool TakeDamage(float damage, float knockback, bool crit, Vector2 pos = default);
    public void Heal(float Heal);
    public bool CanAddDamageEffect(int ID);
    public void AddDamageEffect(IDamageEffect effect);
}
