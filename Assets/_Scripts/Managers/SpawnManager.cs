using LTFUtils;
using RetroAnimation;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Weighter<ObjectPool<Enemy>> _weightedPoolOfEnemies;
    private Dictionary<string, ObjectPool<Enemy>> _enemyToPool;
    private readonly List<Enemy> _activeEnemies = new();
    [SerializeField] private Vector2Int _maxEnemiesPerBurstRange;
    [SerializeField] private Vector2Int _minEnemiesPerBurstRange;
    [SerializeField] private Vector2Int _enemiesPerBurstOffset;
    [SerializeField] private Vector2 _maxSpawnTimeRange;
    [SerializeField] private Vector2 _minSpawnTimeRange;
    [SerializeField] private Vector2 _spawnTimeOffset;
    private float _spawnTime;
    private float _elapsedTime;
    [SerializeField] private BoxCollider2D _spawnArea;
    [SerializeField] private AudioClip _enemyDiedSound, _enemyHurtSound;

    [Header("Explosion")]
    [SerializeField] private ObjectPool<FlipBook> _enemyExplosionPool;
    [SerializeField] private FlipSheet _explosionEnemySheet;
    [SerializeField] private FlipSheet _explosionDamageSheet;
    [SerializeField] private CompositeValue _chanceDamageExplosion = new(.01f);
    [SerializeField] private CompositeValue _explosionDamage = new(5f);
    [SerializeField] private float _explosionRadius;

    [Header("Candy")]
    [SerializeField] private ObjectPool<CurrencyBehaviour> _currencyPool;
    [SerializeField] private float _candySpawnOffset = 0.04f;
    [SerializeField] private CompositeValue _chanceToExtraCandy = new(0f);

    [Header("Damage Number")]
    [SerializeField] private ObjectPool<DamageNumber> _damageNumPool;
    [SerializeField] private Transform _worldCanvas;
    [SerializeField] private Vector2 _xVelocityRange;
    [SerializeField] private Vector2 _yVelocityRange;
    [SerializeField] private Color _normalColour, _critColour;

    [Header("Fire Particle")]
    [SerializeField] private ObjectPool<ParticleStoppedCallBack> _fireParticlePool;

    [Header("Thorn Damage")]
    [SerializeField] private ObjectPool<FlipBook> _thornAnimationPool; 

    public int EnemiesDied { get; private set; }
    public Action EnemyHurt { get; set; }

    public CompositeValue ChanceDamageExplosion => _chanceDamageExplosion;
    public CompositeValue ExplosionDamage => _explosionDamage;

    public CompositeValue ChanceToExtraCandy => _chanceToExtraCandy;

    private void Start()
    {
        _spawnTime = 1f;

        _enemyToPool = new(_weightedPoolOfEnemies.Count);
        for (int i = 0; i < _weightedPoolOfEnemies.Count; i++)
        {
            var pool = _weightedPoolOfEnemies.Objects[i].Object;
            pool.InitPool();
            foreach (var enemy in pool.Objects)
            {
                EnemyCreated(enemy);
            }
            pool.ObjectCreated += EnemyCreated;
            _enemyToPool.Add(pool.Object.Data.Name, pool);
        }

        _currencyPool.InitPool();
        foreach (var currency in _currencyPool.Objects)
        {
            CurrencyCreated(currency);
        }
        _currencyPool.ObjectCreated += CurrencyCreated;

        _enemyExplosionPool.InitPool();
        foreach (var explosion in _enemyExplosionPool.Objects)
        {
            EnemyExplosionCreated(explosion);
        }
        _enemyExplosionPool.ObjectCreated += EnemyExplosionCreated;

        _damageNumPool.InitPool();
        foreach (var dmgNum in _damageNumPool.Objects)
        {
            DamageNumCreated(dmgNum);
        }
        _damageNumPool.ObjectCreated += DamageNumCreated;

        _fireParticlePool.InitPool();
        foreach (var fire in _fireParticlePool.Objects)
        {
            FireParticleCreated(fire);
        }
        _fireParticlePool.ObjectCreated += FireParticleCreated;

        _thornAnimationPool.InitPool();
        foreach (var thorn in _thornAnimationPool.Objects)
        {
            ThornAnimationCreated(thorn);
        }
        _thornAnimationPool.ObjectCreated += ThornAnimationCreated;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _weightedPoolOfEnemies.Count; i++)
        {
            var pool = _weightedPoolOfEnemies.Objects[i].Object;
            foreach (var enemy in pool.Objects)
            {
                UnSubToEnemy(enemy);
            }
            pool.ObjectCreated -= EnemyCreated;
        }

        foreach (var currency in _currencyPool.Objects)
        {
            currency.ReturnToPool -= ReturnCurrencyToPool;
        }
        _currencyPool.ObjectCreated -= CurrencyCreated;

        foreach (var explosion in _enemyExplosionPool.Objects)
        {
            explosion.OnAnimationFinished -= ReturnExplosionToPool;
        }
        _enemyExplosionPool.ObjectCreated -= EnemyExplosionCreated;

        foreach (var dmgNum in _damageNumPool.Objects)
        {
            dmgNum.Return -= ReturnDamageNum;
        }
        _damageNumPool.ObjectCreated -= DamageNumCreated;

        foreach (var fire in _fireParticlePool.Objects)
        {
            fire.OnReturn -= FireParticleCreated;
        }
        _fireParticlePool.ObjectCreated -= FireParticleCreated;

        foreach (var thorn in _thornAnimationPool.Objects)
        {
            thorn.OnAnimationFinished -= ThornAnimationCreated;
        }
        _thornAnimationPool.ObjectCreated -= ThornAnimationCreated;
    }

    public void Tick(float t, float tClamped)
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _spawnTime)
        {
            _elapsedTime = 0f;
            var spawnTime = Random.Range(_minSpawnTimeRange.Evaluate(tClamped), _maxSpawnTimeRange.Evaluate(tClamped));
            var offSetTime = _spawnTimeOffset.Evaluate(tClamped);
            _spawnTime = spawnTime + Random.Range(-offSetTime, offSetTime);

            var minBurstAmount = _minEnemiesPerBurstRange.Evaluate(t);
            var maxBurstAmount = _maxEnemiesPerBurstRange.Evaluate(t);
            var offSetBurstT = _enemiesPerBurstOffset.Evaluate(t);
            var spawnAmount = Random.Range(minBurstAmount, maxBurstAmount) + Random.Range(0, offSetBurstT);
            for (int i = 0; i < spawnAmount; i++)
            {
                var enemy = _weightedPoolOfEnemies.GetWeightedObject().Object.GetObject();
                var randX = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
                var randY = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
                enemy.transform.localPosition = new(randX, randY);
                enemy.Spawn(t, tClamped);
                _activeEnemies.Add(enemy);
            }
        }
    }

    public void DamageEveryEnemy(float damage)
    {
        // Coping array so we don't get out of index. Or do double damage to the same thing
        var enemies = _activeEnemies.ToArray();
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].Health.TakeDamage(damage, 0f, false, enemies[i].transform.position);
    }

    public void DamageEveryEnemyInRange(float damage, float knockback, Vector2 pos, float range, Action<Enemy> action)
    {
        var enemies = _activeEnemies.ToArray();
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            if (Vector2.Distance(enemy.Position, pos) > range)
                continue;

            enemy.Health.TakeDamage(damage, knockback, false, pos);
            action(enemy);
        }
    }

    public void SpawnThornAnimation(Enemy enemy)
    {
        var thorn = _thornAnimationPool.GetObject();
        thorn.transform.localPosition = enemy.Position;
        thorn.Play(overRide: true);
        thorn.gameObject.SetActive(true);
    }

    private void EnemyCreated(Enemy enemy)
    {
        enemy.Init(GameManager.Instance.Witch);
        enemy.ReturnToPool += ReturnEnemyToPool;
        enemy.OnDied += EnemyDied;
        enemy.Health.OnDamaged += SpawnDamageNum;
        enemy.OnFireEffectApplied += _fireParticlePool.GetObject;
    }

    private void UnSubToEnemy(Enemy enemy)
    {
        enemy.ReturnToPool -= ReturnEnemyToPool;
        enemy.OnDied -= EnemyDied;
        enemy.Health.OnDamaged -= SpawnDamageNum;
        enemy.OnFireEffectApplied -= _fireParticlePool.GetObject;
    }

    private void ReturnEnemyToPool(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
        _enemyToPool[enemy.Data.Name].ReturnObject(enemy);
    }

    private void EnemyDied(Enemy enemy)
    {
        SpawnCandy(enemy.transform.localPosition);
        SpawnExplosion(enemy.transform.localPosition, enemy.Data.Colour);
        AudioManager.Instance.PlayOneShot(_enemyDiedSound);
        if (Random.value < _chanceToExtraCandy.Value)
            SpawnCandy(enemy.transform.localPosition);
        EnemiesDied++;
    }

    private void EnemyExplosionCreated(FlipBook explosion)
    {
        explosion.OnAnimationFinished += ReturnExplosionToPool;
    }

    private void ReturnExplosionToPool(FlipBook explosion)
    {
        explosion.gameObject.SetActive(false);
        _enemyExplosionPool.ReturnObject(explosion);
    }

    private void SpawnExplosion(Vector2 pos, Color colour)
    {
        var explosion = _enemyExplosionPool.GetObject();
        // Damage Explosion
        if (Random.value < _chanceDamageExplosion)
        {
            explosion.SetSheet(_explosionDamageSheet);
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                var enemy = _activeEnemies[i];
                if (Vector2.Distance(pos, enemy.Position) < _explosionRadius)
                    enemy.Health.TakeDamage(_explosionDamage.Value, 0f, false, pos);
            }
        }
        else
            explosion.SetSheet(_explosionEnemySheet);

        explosion.transform.localPosition = pos;
        explosion.SR.color = colour;
        explosion.Play(overRide: true);
        explosion.gameObject.SetActive(true);
    }

    private void CurrencyCreated(CurrencyBehaviour currency)
    {
        currency.Init(GameManager.Instance.Witch);
        currency.ReturnToPool += ReturnCurrencyToPool;
    }

    private void ReturnCurrencyToPool(CurrencyBehaviour currency)
    {
        _currencyPool.ReturnObject(currency);   
    }

    private void SpawnCandy(Vector2 pos)
    {
        var currency = _currencyPool.GetObject();
        var randX = Random.Range(-_candySpawnOffset, _candySpawnOffset);
        var randY = Random.Range(-_candySpawnOffset, _candySpawnOffset);
        currency.transform.localPosition = pos + new Vector2(randX, randY);
        currency.gameObject.SetActive(true);
    }

    private void SpawnDamageNum(float damage, float knockback, bool crit, Vector2 pos)
    {
        var dmg = _damageNumPool.GetObject();
        dmg.SetText(damage.ToString("0.00"), crit ? _critColour : _normalColour);

        dmg.SetVelocity(new(_xVelocityRange.Random() * Random.value > .5f ? 1f : -1f, _yVelocityRange.Random()));
        dmg.transform.position = pos;
        dmg.gameObject.SetActive(true);

        if (knockback == 0f)
            return;

        AudioManager.Instance.PlayOneShot(_enemyHurtSound);

        EnemyHurt?.Invoke();
    }

    private void DamageNumCreated(DamageNumber num)
    {
        num.transform.SetParent(_worldCanvas);
        num.transform.localScale = Vector3.one;
        num.Return += ReturnDamageNum;
    }

    private void ReturnDamageNum(DamageNumber num)
    {
        _damageNumPool.ReturnObject(num);
    }

    private void FireParticleCreated(ParticleStoppedCallBack fireParticle)
    {
        fireParticle.gameObject.SetActive(true);
        fireParticle.OnReturn += ReturnFireParticle;
    }

    private void ReturnFireParticle(ParticleStoppedCallBack fireParticle)
    {
        _fireParticlePool.ReturnObject(fireParticle);
    }

    private void ThornAnimationCreated(FlipBook flipBook)
    {
        flipBook.OnAnimationFinished += ReturnThornAnimation;
    }

    private void ReturnThornAnimation(FlipBook flipBook)
    {
        flipBook.gameObject.SetActive(false);
        _thornAnimationPool.ReturnObject(flipBook);
    }
}
