using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _maxHealth;
    protected float _health;

    public float MaxHP => _maxHealth;
    public float HP => _health;

    public System.Action OnDamage { get; set; }
    public System.Action OnDeath { get; set; }

    public void Start()
    {
        _health = _maxHealth;    
    }

    public virtual bool TakeDamage(float damage)
    {
        // Is alive check
        if (_health <= 0f)
            return true;

        _health -= damage;
        OnDamage?.Invoke();
        if (_health <= 0f)
        {
            _health = 0f;
            OnDeath?.Invoke();
            return true;
        }

        return false;
    }  
    
    public void ResetHealth()
    {
        _health = _maxHealth;
    }

    public void ResetHealth(float newMax)
    {
        _maxHealth = newMax;
        _health = newMax;
    }

    public void Heal(float heal)
    {
        _health += heal;
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
}
