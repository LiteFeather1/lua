using UnityEngine;

public class HealthPlayer : Health
{
    [Header("Player Health")]
    [SerializeField] private int _shield;
    [SerializeField] private CompositeValue _defence = new(10f);
    [SerializeField] private CompositeValue _dodgeChance = new(0f);

    public delegate void MaxHPIncreased(float maxHp, float t);
    public MaxHPIncreased OnMaxHPIncreased { get; set; }
    public System.Action OnShieldDamaged { get; set; }
    public System.Action<int> OnShieldGained { get; set; }

    public System.Action OnDodge { get; set; }

    public int Shield => _shield;
    public CompositeValue Defence => _defence;
    public CompositeValue DodgeChance => _dodgeChance;

    public override bool TakeDamage(float damage, float knockback, bool crit, Vector2 pos)
    {
        if (Random.value < _dodgeChance)
        {
            OnDodge?.Invoke();
            return false;
        }

        if (_shield > 0)
        {
            _shield--;
            OnDamaged?.Invoke(0f, knockback, crit, pos);
            OnShieldDamaged?.Invoke();
            return false;   
        }

        damage *= 100f / (100f + _defence);

        return base.TakeDamage(damage, knockback, crit, pos);
    }

    public void IncreaseMaxHP(float amount)
    {
        _maxHealth += amount;
        _health += amount;
        OnMaxHPIncreased?.Invoke(_maxHealth, _health / _maxHealth);
    }

    public int IncreaseMaxHP(int amount)
    {
        IncreaseMaxHP((float)amount);
        return (int)_maxHealth;
    }

    public int AddShield(int amount)
    {
        OnShieldGained?.Invoke(amount);
        return _shield += amount;
    }
}
