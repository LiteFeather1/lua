using UnityEngine;

public class HealthPlayer : Health
{
    [Header("Player Health")]
    [SerializeField] private int _shield;
    [SerializeField] private CompositeValue _defence = new(10f);

    public delegate void MaxHPIncreased(float maxHp, float t);
    public MaxHPIncreased OnMaxHPIncreased { get; set; }
    public System.Action OnShieldDamaged { get; set; }
    public System.Action<int> OnShieldGained { get; set; }
    public CompositeValue Defence => _defence;

    public override bool TakeDamage(float damage, bool crit, Vector2 pos)
    {
        if (_shield > 0)
        {
            _shield--;
            OnDamage?.Invoke(0f, crit, pos);
            OnShieldDamaged?.Invoke();
            return false;   
        }

        damage *= 100f / (100f + _defence.Value);

        return base.TakeDamage(damage, crit, pos);
    }

    public void IncreaseMaxHP(float amount)
    {
        _maxHealth += amount;
        _health += amount;
        OnMaxHPIncreased?.Invoke(_maxHealth, _health / _maxHealth);
    }

    public void AddShield(int amount)
    {
        _shield += amount;
        OnShieldGained?.Invoke(amount);
    }
}
