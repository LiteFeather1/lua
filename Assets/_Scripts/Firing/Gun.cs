﻿using LTFUtils;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ObjectPool<Bullet> _bulletPool;
    [SerializeField] private ObjectPool<DisableCallBack> _particlePool;

    [Header("Stats")]
    [SerializeField] private CompositeValue _bulletSpeed = new(1f);
    [SerializeField] private CompositeValue _size = new(1f);
    [SerializeField] private CompositeValue _bulletDuration = new(1f);
    [SerializeField] private int _bulletAmount = 1;
    [SerializeField] private float _separationPerBullet = 12.5f;
    [SerializeField] private int _burstAmount = 1;
    [SerializeField] private float _timeToCompleteShooting = .25f;
    [SerializeField] private int _bounceAmount;
    [SerializeField] private int _pierceAmount;

    public CompositeValue BulletSpeed => _bulletSpeed;
    public CompositeValue Size => _size;
    public CompositeValue BulletDuration => _bulletDuration;

    public void AddBulletAmount(int amount) => _bulletAmount += amount;
    public void AddBurst(int amount) => _burstAmount += amount;
    public void AddBounce(int amount) => _bounceAmount += amount;
    public void AddPierce(int amount) => _pierceAmount += amount;

    public System.Action<float, Vector2> OnDamageAppplied;

    private void Awake()
    {
        _bulletPool.InitPool();
        foreach (Bullet bullet in _bulletPool.Objects)
        {
            BulletCreated(bullet);
        }
        _bulletPool.ObjectCreated += BulletCreated;

        _particlePool.InitPool();
        foreach (var particle in _particlePool.Objects)
        {
            ParticleCreated(particle);
        }

        _particlePool.ObjectCreated += ParticleCreated;
    }

    private void OnDestroy()
    {
        foreach (var bullet in _bulletPool.Objects)
        {
            bullet.ReturnToPool -= ReturnBulletToPool;
            bullet.Hitbox.OnDamageAppplied -= DamageAppplied;
        }
        _bulletPool.ObjectCreated -= BulletCreated;
    }

    public void ShootRoutine(float damage, float critChance, float knockback)
    {
        StartCoroutine(Shot_CO(damage, critChance, knockback));
    }

    public void ShootBullet(float damage, float critChance, float knockback, float angle)
    {
        var bullet = _bulletPool.GetObject();
        var particle = _particlePool.GetObject();
        particle.gameObject.SetActive(true);
        bullet.transform.SetLocalPositionAndRotation(_firePoint.position,
                                                     Quaternion.Euler(0f, 0f, angle));
        bullet.transform.localScale = Vector3.one * _size.Value;
        bullet.AttachDisable(particle);
        bullet.gameObject.SetActive(true);
        bullet.Hitbox.SetDamage(damage);
        bullet.Hitbox.SetCritChance(critChance);
        bullet.Hitbox.SetKnockback(knockback);
        bullet.Projectile.Shoot(_bulletSpeed.Value, (Vector2)bullet.transform.right,
                                _pierceAmount, _bounceAmount, _bulletDuration.Value);
    }

    private void BulletCreated(Bullet bullet)
    {
        bullet.ReturnToPool += ReturnBulletToPool;
        bullet.Hitbox.OnDamageAppplied += DamageAppplied;
    }

    private void ReturnBulletToPool(Bullet bullet)
    {
        _bulletPool.ReturnObject(bullet);
    }

    private void ParticleCreated(DisableCallBack particle)
    {
        particle.Disabled += ReturnParticleToPool;
    }

    private void ReturnParticleToPool(DisableCallBack particle)
    {
        _particlePool.ReturnObject(particle);
    }

    private void DamageAppplied(float damage, Vector2 pos)
    {
        OnDamageAppplied?.Invoke(damage, pos);
    }

    private IEnumerator Shot_CO(float damage, float critChance, float knockback)
    {
        WaitForSeconds yieldBetweenBurst = new(_timeToCompleteShooting / _burstAmount);
        for (int i = 0; i < _burstAmount; i++)
        {
            for (int j = 0; j < _bulletAmount; j++)
            {
                ShootBullet(damage, critChance, knockback, GetAngle(j));
            }
            yield return yieldBetweenBurst;
        }
    }

    private float GetAngle(int bulletIndex)
    {
        float totalAngle = (_bulletAmount - 1) * _separationPerBullet * .5f;
        float center = LTFHelpers_Math.AngleBetweenTwoPoints(transform.position, transform.position - _firePoint.right);
        float minAngle = center - totalAngle;
        return minAngle + (bulletIndex * _separationPerBullet);
    }
}
