using UnityEngine;

public class HealthPlayer : Health
{
    [Header("Player Health")]
    [SerializeField] private int _shield;
    [SerializeField] private CompositeValue _defence = new(10f);

    public CompositeValue Defence => _defence;

    public override bool TakeDamage(float damage)
    {
        if (_shield > 0)
        {
            _shield--;
            OnDamage?.Invoke();
            return false;   
        }

        damage *= 100f / (100f + _defence.Value);

        return base.TakeDamage(damage);
    }

    public void IncreaseMaxHP(float amount)
    {
        _maxHealth += amount;
        _health += amount;
    }

    public void AddShield(int amount) => _shield += amount;
}
