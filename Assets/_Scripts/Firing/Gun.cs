using LTFUtils;
using RetroAnimation;
using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ObjectPool<Bullet> _bulletPool;
    [SerializeField] private ObjectPool<ParticleStoppedCallBack> _particlePool;
    [SerializeField] private ObjectPool<FlipBook> _bulletDamage;
    [SerializeField] private AudioClip _bulletShotSound;

    public Action<IDamageable, float> OnDamageAppplied { get; set; }

    public Action OnFinishedShooting { get; set; }

    protected virtual void Awake()
    {
        _bulletPool.ObjectCreated += BulletCreated;
        _bulletPool.InitPool();

        _particlePool.ObjectCreated += ParticleCreated;
        _particlePool.InitPool();

        _bulletDamage.ObjectCreated += DamageExplosionCreated;
        _bulletDamage.InitPool();
    }

    protected virtual void OnDestroy()
    {
        foreach (var bullet in _bulletPool.Objects)
        {
            bullet.ReturnToPool -= ReturnBulletToPool;
            bullet.Hitbox.OnDamageAppplied -= DamageAppplied;
        }
        _bulletPool.ObjectCreated -= BulletCreated;

        foreach (var particle in _particlePool.Objects)
        {
            particle.OnReturn -= ReturnParticleToPool;
        }
        _particlePool.ObjectCreated -= ParticleCreated; ;

        foreach (var explosion in _bulletDamage.Objects)
        {
            explosion.OnAnimationFinished -= ReturnExplosionToPool;
        }
        _bulletDamage.ObjectCreated -= DamageExplosionCreated;
    }

    public void StartShootRoutine(float damage,
                                  float critChance,
                                  float critMultiplier,
                                  float knockback,
                                  float size,
                                  float speed,
                                  int pierce,
                                  int bounce,
                                  float duration,
                                  float angle,
                                  float randomAngle,
                                  float separationPerBullet,
                                  int burstAmount,
                                  int bulletAmount,
                                  float waitBetweenBursts)
    {
        StartCoroutine(Shot_CO(damage: damage,
                               critChance: critChance,
                               critMultiplier: critMultiplier,
                               knockback: knockback,
                               size: size,
                               speed: speed,
                               pierce: pierce,
                               bounce: bounce,
                               duration: duration,
                               angle: angle,
                               randomAngle: randomAngle,
                               separationPerBullet: separationPerBullet,
                               burstAmount: burstAmount,
                               bulletAmount: bulletAmount,
                               waitBetweenBursts: waitBetweenBursts));
    }

    public void ShootBullet(float damage,
                            float critChance,
                            float critMultiplier,
                            float knockback,
                            float size,
                            float speed,
                            int pierce,
                            int bounce,
                            float duration,
                            float angle)
    {
        var bullet = _bulletPool.GetObject();
        bullet.transform.SetPositionAndRotation(_firePoint.position, Quaternion.Euler(0f, 0f, angle));
        bullet.transform.localScale = new(size, size, size);
        bullet.AttachDisable(_particlePool.GetObject());
        bullet.gameObject.SetActive(true);
        bullet.Hitbox.SetStats(damage, critChance, critMultiplier, knockback);
        bullet.Projectile.SetStats(pierce, bounce, duration);
        ShootProjectileMethod(bullet, speed);
    }

    protected virtual void ShootProjectileMethod(Bullet bullet, float speed)
    {
        bullet.Projectile.Shoot(speed, bullet.transform.right);
    }

    protected virtual void ReturnBulletToPool(Bullet bullet)
    {
        _bulletPool.ReturnObject(bullet);
    }

    private float GetAngle(int bulletIndex, int bulletAmount, float separationPerBullet)
    {
        float totalAngle = (bulletAmount - 1) * separationPerBullet * .5f;
        float center = LTFHelpers_Math.AngleBetweenTwoPoints(transform.position, transform.position - _firePoint.right);
        float minAngle = center - totalAngle;
        return minAngle + (bulletIndex * separationPerBullet);
    }

    private IEnumerator Shot_CO(float damage,
                                float critChance,
                                float critMultiplier,
                                float knockback,
                                float size,
                                float speed,
                                int pierce,
                                int bounce,
                                float duration,
                                float angle,
                                float randomAngle,
                                float separationPerBullet,
                                int burstAmount,
                                int bulletAmount,
                                float waitBetweenBursts)
    {
        WaitForSeconds yieldBetweenBurst = burstAmount > 1 ? new(waitBetweenBursts) : null;
        for (int i = 0; i < burstAmount; i++)
        {
            AudioManager.Instance.PlayOneShot(_bulletShotSound);
            for (int j = 0; j < bulletAmount; j++)
            {
                ShootBullet(damage: damage,
                            critChance: critChance,
                            critMultiplier: critMultiplier,
                            knockback: knockback,
                            size: size,
                            speed: speed,
                            pierce: pierce,
                            bounce: bounce,
                            duration: duration,
                            angle: GetAngle(j, bulletAmount, separationPerBullet) + angle + UnityEngine.Random.Range(-randomAngle, randomAngle));
            }
            yield return yieldBetweenBurst;
        }

        OnFinishedShooting?.Invoke(); 
    }

    private void BulletCreated(Bullet bullet)
    {
        bullet.ReturnToPool += ReturnBulletToPool;
        bullet.Hitbox.OnDamageAppplied += DamageAppplied;
    }

    private void DamageAppplied(IDamageable damageable, float damage, Vector2 pos)
    {
        var bulletExplosion = _bulletDamage.GetObject();
        bulletExplosion.transform.localPosition = pos;
        bulletExplosion.Play();
        bulletExplosion.gameObject.SetActive(true);

        OnDamageAppplied?.Invoke(damageable, damage);
    }

    private void ParticleCreated(ParticleStoppedCallBack particle)
    {
        particle.gameObject.SetActive(true);
        particle.OnReturn += ReturnParticleToPool;
    }

    private void ReturnParticleToPool(ParticleStoppedCallBack particle)
    {
        _particlePool.ReturnObject(particle);
    }
        
    private void DamageExplosionCreated(FlipBook explosion)
    {
        explosion.OnAnimationFinished += ReturnExplosionToPool;
    }

    private void ReturnExplosionToPool(FlipBook explosion)
    {
        explosion.gameObject.SetActive(false);
        _bulletDamage.ReturnObject(explosion);
    }
}
