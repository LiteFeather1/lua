using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    private float _health;

    public float MaxHP => _maxHealth;
    public float HP => _health;

    public System.Action Died {  get; set; }

    public void Start()
    {
        _health = _maxHealth;    
    }

    public bool TakeDamage(float damage, IDamageEffect damageEffect)
    {
        _health -= damage;
        if (_health < 0 )
        {
            Died?.Invoke();
            return true;
        }

        if (damageEffect != null)
        {
            if (damageEffect.Chance < Random.value)
            {

            }
        }

        return false;
    }   
}
