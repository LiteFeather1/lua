using LTFUtils;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ObjectPool<Bullet> _bulletPool;

    [Header("Stats")]
    [SerializeField] private CompositeValue _bulletSpeed = new(1f);
    [SerializeField] private CompositeValue _size = new(1f);
    [SerializeField] private int _bulletAmount = 1;
    [SerializeField] private float _separationPerBullet = 12.5f;
    [SerializeField] private int _burstAmount = 1;
    [SerializeField] private float _timeToCompleteShooting = .25f;
    [SerializeField] private int _bounceAmount;
    [SerializeField] private int _pierceAmount;

    public CompositeValue BulletSpeed => _bulletSpeed;
    public CompositeValue Size => _size;

    public void AddBulletAmount(int amount) => _bulletAmount += amount;
    public void AddBurst(int amount) => _burstAmount += amount;
    public void AddBounce(int amount) => _bounceAmount += amount;
    public void AddPierce(int amount) => _pierceAmount += amount;

    public System.Action<float> OnDamageAppplied;

    private void Awake()
    {
        _bulletPool.InitPool();
        foreach (Bullet bullet in _bulletPool.Objects)
        {
            bullet.ReturnToPool += ReturnToPool;
            bullet.Hitbox.OnDamageAppplied += DamageAppplied;
        }
        _bulletPool.ObjectCreated += ObjectCreated;
    }

    private void OnDestroy()
    {
        foreach (var bullet in _bulletPool.Objects)
        {
            bullet.ReturnToPool -= ReturnToPool;
            bullet.Hitbox.OnDamageAppplied -= DamageAppplied;
        }
        _bulletPool.ObjectCreated -= ObjectCreated;
    }

    public void ShootRoutine(float damage, float knockback)
    {
        StartCoroutine(Shot_CO(damage, knockback));
    }

    public void ShootBullet(float damage, float knockback, float angle)
    {
        var bullet = _bulletPool.GetObject();
        bullet.transform.SetLocalPositionAndRotation(_firePoint.position,
                                                     Quaternion.Euler(0f, 0f, angle));
        bullet.transform.localScale = Vector3.one * _size.Value;
        bullet.gameObject.SetActive(true);
        bullet.Hitbox.SetDamage(damage);
        bullet.Hitbox.SetKnockback(knockback);
        bullet.Projectile.Shoot(_bulletSpeed.Value, bullet.transform.right,
                                _pierceAmount, _bounceAmount);
    }

    private void ObjectCreated(Bullet bullet)
    {
        bullet.ReturnToPool += ReturnToPool;
    }

    private void ReturnToPool(Bullet bullet)
    {
        _bulletPool.ReturnObject(bullet);
    }

    public void DamageAppplied(float damage)
    {
        OnDamageAppplied?.Invoke(damage);
    }

    private IEnumerator Shot_CO(float damage, float knockback)
    {
        WaitForSeconds yieldBetweenBurst = new(_timeToCompleteShooting / _burstAmount);
        for (int i = 0; i < _burstAmount; i++)
        {
            for (int j = 0; j < _bulletAmount; j++)
            {
                ShootBullet(damage, knockback, GetAngle(j));
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
