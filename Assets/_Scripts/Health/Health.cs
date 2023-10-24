using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _maxHealth;
    protected float _health;

    public float MaxHP => _maxHealth;
    public float HP => _health;

    public System.Action Died {  get; set; }

    public void Start()
    {
        _health = _maxHealth;    
    }

    public virtual bool TakeDamage(float damage)
    {
        _health -= damage;
        if (_health < 0f)
        {
            _health = 0f;
            Died?.Invoke();
            return true;
        }

        return false;
    }  
    
    public void Heal(float heal)
    {
        _health += heal;
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
}
