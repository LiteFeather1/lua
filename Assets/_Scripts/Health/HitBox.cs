using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _knockBack;
    public System.Action<float> OnDamageAppplied { get; set; }

    public void SetDamage(float damage) => _damage = damage;

    public void SetKnockback(float knockBack) => _knockBack = knockBack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            var preDmgHp = damageable.HP;
            damageable.TakeDamage(_damage);
            OnDamageAppplied?.Invoke(preDmgHp - damageable.HP);
        }
    }
}
