using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage, null);
    }
}
