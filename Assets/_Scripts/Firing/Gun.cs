using LTFUtils;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ObjectPool<Bullet> _bulletPool;

    [Header("States")]
    [SerializeField] private CompositeValue _damage;
    [SerializeField] private CompositeValue _knockback;
    [SerializeField] private CompositeValue _bulletSpeed = new(1f);
    [SerializeField] private CompositeValue _size = new(1f);
    [SerializeField] private int _bulletAmount = 1;
    [SerializeField] private float _separationPerBullet = 12.5f;
    [SerializeField] private int _burstAmount = 1;
    private int _rotationSign = 1;
    [SerializeField] private float _timeToCompleteShooting = .25f;
    [SerializeField] private int _bounceAmount;
    [SerializeField] private int _pierceAmount;
    [SerializeField] private int _randomBulletAmount;

    public CompositeValue Damage => _damage;    
    public CompositeValue Knockback => _knockback;
    public CompositeValue BulletSpeed => _bulletSpeed;
    public CompositeValue Size => _size;

    public void AddBulletAmount(int amount) => _bulletAmount += amount;
    public void AddRandomBullet(int amount) => _randomBulletAmount += amount;
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

    public void Shoot()
    {
        StartCoroutine(Shot_CO());
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

    private void ShootBullet(float angle)
    {
        var bullet = _bulletPool.GetObject();
        bullet.transform.SetLocalPositionAndRotation(_firePoint.position,
                                                     Quaternion.Euler(0f, 0f, angle));
        bullet.transform.localScale = Vector3.one * _size.Value;
        bullet.gameObject.SetActive(true);
        bullet.Hitbox.SetDamage(_damage.Value);
        bullet.Hitbox.SetKnockback(_knockback.Value);
        bullet.Projectile.Shoot(_bulletSpeed.Value, bullet.transform.right, 
                                _pierceAmount, _bounceAmount);
    }

    private IEnumerator Shot_CO()
    {
        WaitForSeconds yieldBetweenBurst = new(_timeToCompleteShooting / _burstAmount);
        for (int i = 0; i < _burstAmount; i++)
        {
            _rotationSign *= -1;
            float angleOffset = _separationPerBullet * i * _rotationSign * (_bulletAmount - 1);
            for (int j = 0; j < _bulletAmount; j++)
            {
                ShootBullet(GetAngle(j) + angleOffset);
            }
            yield return yieldBetweenBurst;
        }

        for (int i = 0; i < _randomBulletAmount; i++)
        {
            ShootBullet(Random.Range(0f, 360f));
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
