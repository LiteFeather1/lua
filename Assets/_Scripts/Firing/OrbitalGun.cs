using System.Collections.Generic;
using UnityEngine;

public class OrbitalGun : Gun
{
    [Header("Gun")]
    [SerializeField] private CompositeValue _rotationSpeed;
    private readonly List<Bullet> _activeBullets = new();

    private void Start()
    {
        _bulletPool.PoolParent.transform.SetParent(transform);
        _bulletPool.PoolParent.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        _bulletPool.PoolParent.transform.Rotate(0f, 0f, _rotationSpeed * delta, Space.Self);
        for (int i = _activeBullets.Count - 1; i >= 0; i--)
        {
            var activeBullet = _activeBullets[i];
            var activeProjectile = activeBullet.Projectile;
            var speedDelta = activeProjectile.Speed * delta;
            activeBullet.transform.localPosition += new Vector3(activeProjectile.Direction.x * speedDelta,
                                                                activeProjectile.Direction.y * speedDelta);
        }
    }

    protected override void ShootProjectileMethod(Bullet bullet, float speed)
    {
        bullet.Projectile.SetSpeedAndDirection(speed, Random.insideUnitCircle);
        _activeBullets.Add(bullet);
    }
}
