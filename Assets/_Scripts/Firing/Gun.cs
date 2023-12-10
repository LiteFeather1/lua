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

    [Header("Stats")]
    [SerializeField, Range(0f, 1f)] private float _faceRotationAlpha = 1f;
    [SerializeField] private float _separationPerBullet;
    [SerializeField] private Vector2 _randomAngleOffsetRange = new(0f, 0f);
    [SerializeField] private Vector2 _randomSpeedOffsetRange = new(0f, 0f);

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

    public IEnumerator ShootRoutine(float damage,
                                    float critChance,
                                    float critMultiplier,
                                    float knockback,
                                    float size,
                                    float speed,
                                    int pierce,
                                    int bounce,
                                    float duration,
                                    float angle,
                                    int burstAmount,
                                    int bulletAmount,
                                    IEnumerator yieldBetweenBurst)
    {
        for (int i = 0; i < burstAmount; i++)
        {
            ShootBulletBurst(damage, critChance, critMultiplier, knockback, size, speed, pierce, bounce, duration, angle, bulletAmount);
            yield return yieldBetweenBurst;
        }

        OnFinishedShooting?.Invoke();
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
                                  int burstAmount,
                                  int bulletAmount,
                                  IEnumerator yieldBetweenBurst)
    {
        StartCoroutine(ShootRoutine(damage: damage,
                                    critChance: critChance,
                                    critMultiplier: critMultiplier,
                                    knockback: knockback,
                                    size: size,
                                    speed: speed,
                                    pierce: pierce,
                                    bounce: bounce,
                                    duration: duration,
                                    angle: angle,
                                    burstAmount: burstAmount,
                                    bulletAmount: bulletAmount,
                                    yieldBetweenBurst: yieldBetweenBurst));
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
        bullet.transform.SetPositionAndRotation(_firePoint.position, Quaternion.Euler(0f, 0f, angle * _faceRotationAlpha));
        bullet.transform.localScale = new(size, size, 1f);
        bullet.gameObject.SetActive(true);
        bullet.AttachDisable(_particlePool.GetObject());
        bullet.Hitbox.SetStats(damage, critChance, critMultiplier, knockback);
        bullet.Projectile.SetStats(pierce, bounce, duration);
        ShootProjectileMethod(bullet,
                              speed + _randomSpeedOffsetRange.Random(),
                              _firePoint.right.RotateVector(angle - _firePoint.eulerAngles.z));
    }

    public void ShootBulletBurst(float damage,
                                 float critChance,
                                 float critMultiplier,
                                 float knockback,
                                 float size,
                                 float speed,
                                 int pierce,
                                 int bounce,
                                 float duration,
                                 float angle,
                                 int bulletAmount)
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
                        angle: GetAngle(j, bulletAmount) + angle);
        }
    }

    public float GetAngle(int bulletIndex, int bulletAmount)
    {
        float totalAngle = (bulletAmount - 1) * _separationPerBullet * .5f;
        float minAngle = _firePoint.eulerAngles.z - totalAngle;
        return minAngle + (bulletIndex * _separationPerBullet) + _randomAngleOffsetRange.Random();
    }

    public void PlayShotSound() => AudioManager.Instance.PlayOneShot(_bulletShotSound);

    protected virtual void ShootProjectileMethod(Bullet bullet, float speed, Vector2 direction)
    {
        bullet.Projectile.Shoot(speed, direction);
    }

    protected virtual void ReturnBulletToPool(Bullet bullet)
    {
        _bulletPool.ReturnObject(bullet);
    }

    private void BulletCreated(Bullet bullet)
    {
        bullet.ReturnToPool += ReturnBulletToPool;
        bullet.Hitbox.OnDamageAppplied += DamageAppplied;
    }

    private void DamageAppplied(IDamageable damageable, float damage, Vector2 pos)
    {
        var bulletExplosion = _bulletDamage.GetObject();
        bulletExplosion.transform.position = pos;
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
