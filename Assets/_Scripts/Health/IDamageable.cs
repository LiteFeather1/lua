using UnityEngine;

public interface IDamageable
{
    public float MaxHP { get; }
    public float HP { get; }

    public bool TakeDamage(float damage, bool crit, Vector2 pos);
    public void Heal(float Heal);
}

