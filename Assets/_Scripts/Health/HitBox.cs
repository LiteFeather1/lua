﻿using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _critChance;
    [SerializeField] private float _critMultiplier = 1.5f;
    [SerializeField] private float _knockBack = 1f;
    public System.Action<IDamageable, float, Vector2> OnDamageAppplied { get; set; }

    public void SetDamage(float damage) => _damage = damage;
    public void SetCritChance(float chance) => _critChance = chance;
    public void SetCritMultiplier(float multiplier) => _critMultiplier = multiplier;
    public void SetKnockback(float knockBack) => _knockBack = knockBack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            var preDmgHp = damageable.HP;
            bool crit = Random.value < _critChance;
            Vector2 pos = collision.ClosestPoint(transform.position);
            float damage = crit ? _damage * _critMultiplier : _damage;
            damageable.TakeDamage(damage, _knockBack, crit, pos);
            OnDamageAppplied?.Invoke(damageable, preDmgHp - damageable.HP, pos);
        }
    }
}
