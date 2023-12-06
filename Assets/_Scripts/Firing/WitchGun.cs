using UnityEngine;

public class WitchGun : Gun
{
    [Header("Witch Gun")]
    [SerializeField] private CompositeValue _size = new(1f);
    [SerializeField] private CompositeValue _bulletSpeed = new(1f);
    [SerializeField] private CompositeValue _bulletDuration = new(1f);
    [SerializeField] private int _pierceAmount;
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _randomAngle = 0f;
    [SerializeField] private float _separationPerBullet = 12.5f;
    [SerializeField] private int _burstAmount = 1;
    [SerializeField] private int _bulletAmount = 1;
    [SerializeField] private Vector2 _timeBetweenBurstsRange = new(.5f, .1f);

    public CompositeValue Size => _size;
    public CompositeValue BulletSpeed => _bulletSpeed;
    public CompositeValue BulletDuration => _bulletDuration;

    public int PierceAmount => _pierceAmount;
    public int AddPierce(int amount) => _pierceAmount += amount;
    public int BurstAmount => _burstAmount;
    public int BounceAmount => _bounceAmount;
    public int AddBounce(int amount) => _bounceAmount += amount;
    public int AddBurst(int amount) => _burstAmount += amount;
    public int BulletAmount => _bulletAmount;
    public int AddBulletAmount(int amount) => _bulletAmount += amount;

    public float RandomAngle => _randomAngle;
    public float SeparationPerBullet => _separationPerBullet;
    public float WaitBetweenBursts => _timeBetweenBurstsRange.EvaluateClamped((_burstAmount - 1) / 9f);

    public void StartShootRoutine(float damage, float critChance, float critMultiplier, float knockback)
    {
        StartShootRoutine(damage: damage,
                          critChance: critChance,
                          critMultiplier: critMultiplier,
                          knockback: knockback,
                          size: _size.Value,
                          speed: _bulletSpeed.Value,
                          pierce: _pierceAmount,
                          bounce: _bounceAmount,
                          duration: _bulletDuration.Value,
                          angle: 0f,
                          randomAngle: _randomAngle,
                          separationPerBullet: _separationPerBullet,
                          burstAmount: _burstAmount,
                          bulletAmount: _bulletAmount,
                          waitBetweenBursts: WaitBetweenBursts);
    }

    public void ShootBullet(float damage, float critChance, float critMultiplier, float knockback, float angle)
    {
        ShootBullet(damage: damage,
                    critChance: critChance,
                    critMultiplier: critMultiplier,
                    knockback: knockback,
                    size: _size.Value,
                    speed: _bulletSpeed.Value,
                    pierce: _pierceAmount,
                    bounce: _bounceAmount,
                    duration: _bulletDuration.Value,
                    angle: angle);
    }
}
