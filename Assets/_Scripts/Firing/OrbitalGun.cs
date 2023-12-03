using System.Collections.Generic;
using UnityEngine;

public class OrbitalGun : Gun
{
    [Header("Orbital Gun")]
    [SerializeField] private Transform _clockWiseRotation;
    [SerializeField] private Transform _antiClockWiseRotation;
    [SerializeField] private CompositeValue _rotationSpeed;
    [SerializeField] private int _orbitalAmount = 0;
    [SerializeField] private float _waitBetweenBursts = .75f;

    private readonly List<Bullet> _activeBullets = new();

    public CompositeValue RotationSpeed => _rotationSpeed;
    public int AddOrbitalAmount(int amount) => _orbitalAmount += amount;

    private void Update()
    {
        float delta = Time.deltaTime;
        var rot = _rotationSpeed * delta;
        _antiClockWiseRotation.transform.Rotate(0f, 0f, -rot, Space.Self);
        _clockWiseRotation.transform.Rotate(0f, 0f, rot, Space.Self);
        for (int i = _activeBullets.Count - 1; i >= 0; i--)
        {
            var activeBullet = _activeBullets[i];
            var activeProjectile = activeBullet.Projectile;
            var speedDelta = activeProjectile.Speed * delta;
            activeBullet.transform.localPosition += new Vector3(activeProjectile.Direction.x * speedDelta,
                                                                activeProjectile.Direction.y * speedDelta);
        }
    }

    public void StartShootRoutine(float damage, float critChance, float critMultiplier, float knockback, float speed, float duration)
    {
        ShootRoutine(damage: damage,
                     critChance: critChance,
                     critMultiplier: critMultiplier,
                     knockback: knockback,
                     size: 2f,
                     // TODO Limit Speed
                     speed: speed,
                     pierce: 1,
                     bounce: 0,
                     duration: duration,
                     angle: 0f,
                     waitBetweenBursts: _waitBetweenBursts,
                     bulletAmount: 1,
                     burstAmount: _orbitalAmount,
                     separationPerBullet: 0f);
    }

    protected override void ShootProjectileMethod(Bullet bullet, float speed)
    {
        bullet.Projectile.SetSpeedAndDirection(speed, Random.insideUnitCircle.normalized * .5f);
        bullet.transform.SetParent(Random.value > 0.5f ? _antiClockWiseRotation : _clockWiseRotation);
        _activeBullets.Add(bullet);
    }

    protected override void ReturnBulletToPool(Bullet bullet)
    {
        _activeBullets.Remove(bullet);
        base.ReturnBulletToPool(bullet);
    }
}
