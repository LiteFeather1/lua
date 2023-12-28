using System;
using System.Collections;
using System.Collections.Generic;
using LTFUtils;
using RetroAnimation;
using UnityEngine;
using Lua.StateMachine.Enemies;
using Lua.Misc;
using Random = UnityEngine.Random;

namespace Lua.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [NonSerialized] private RefValue<float> _t, _tClamped;

        [Header("Enemy")]
        [SerializeField] private Weighter<ObjectPool<Enemy>> _weightedPoolOfEnemies;
        private readonly Dictionary<string, ObjectPool<Enemy>> _enemyToPool = new();
        private readonly List<Enemy> _activeEnemies = new();
        [SerializeField] private Vector2Int _maxEnemiesPerBurstRange;
        [SerializeField] private Vector2Int _minEnemiesPerBurstRange;
        [SerializeField] private Vector2Int _enemiesPerBurstOffset;
        [SerializeField] private Vector2Int _minEnemiesAtAllTimes = new(1, 5);
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
        [SerializeField] private ObjectPool<Parentable> _fireParticlePool;

        [Header("Thorn Damage")]
        [SerializeField] private ObjectPool<FlipBook> _thornAnimationPool;

        [Header("Lightning Effect")]
        [SerializeField] private ObjectPool<LineRenderer> _lightningEffectPool;
        [SerializeField] private Vector2 _lightningTimeRange = new(.25f, .5f);
        [SerializeField] private Vector2Int _lightningUpdatesRange = new(3, 5);
        [SerializeField] private Vector2 _lightningSegmentLengthRange = new(.04f, .12f);
        [SerializeField] private Vector2 _lightningSegmentOffsetRange = new(0.04f, 0.12f);
        [SerializeField] private Color[] _lightningColours;

        public int EnemiesDied { get; private set; }
        public Action EnemyHurt { get; set; }
        public Action<float> EnemyDamagedInRange { get; set; }

        public CompositeValue ChanceDamageExplosion => _chanceDamageExplosion;
        public CompositeValue ExplosionDamage => _explosionDamage;

        public CompositeValue ChanceToExtraCandy => _chanceToExtraCandy;

        private void Start()
        {
            _spawnTime = 1f;

            for (int i = 0; i < _weightedPoolOfEnemies.Count; i++)
            {
                var pool = _weightedPoolOfEnemies.Objects[i].Object;
                pool.ObjectCreated += EnemyCreated;
                pool.InitPool();
                _enemyToPool.Add(pool.Object.Data.Name, pool);
            }

            _currencyPool.ObjectCreated += CurrencyCreated;
            _currencyPool.InitPool();

            _enemyExplosionPool.ObjectCreated += EnemyExplosionCreated;
            _enemyExplosionPool.InitPool();

            _damageNumPool.ObjectCreated += DamageNumCreated;
            _damageNumPool.InitPool();

            _fireParticlePool.ObjectCreated += FireParticleCreated;
            _fireParticlePool.InitPool();

            _thornAnimationPool.ObjectCreated += ThornAnimationCreated;
            _thornAnimationPool.InitPool();

            _lightningEffectPool.InitPool(true);
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

        public void SetTRef(RefValue<float> t, RefValue<float> tClamped)
        {
            _t = t;
            _tClamped = tClamped;
        }

        public void Tick()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime < _spawnTime)
                return;

            _elapsedTime = 0f;
            var spawnTime = Random.Range(_minSpawnTimeRange.Evaluate(_tClamped), _maxSpawnTimeRange.Evaluate(_tClamped));
            var offSetTime = _spawnTimeOffset.Evaluate(_tClamped);
            _spawnTime = spawnTime + Random.Range(-offSetTime, offSetTime);

            var minBurstAmount = _minEnemiesPerBurstRange.Evaluate(_t);
            var maxBurstAmount = _maxEnemiesPerBurstRange.Evaluate(_t);
            var offSetBurstT = _enemiesPerBurstOffset.Evaluate(_t);
            var spawnAmount = Random.Range(minBurstAmount, maxBurstAmount + 1) + Random.Range(0, offSetBurstT + 1);

            for (int i = 0; i < spawnAmount; i++)
                SpawnEnemy();
        }

        public void SpawnDamageNum(string text, Color color, Vector2 pos)
        {
            var dmgNum = _damageNumPool.GetObject();
            dmgNum.SetText(text, color);
            var x = _xVelocityRange.Random() * (Random.value > .5f ? 1f : -1f);
            dmgNum.SetVelocity(new(x, _yVelocityRange.Random()));
            dmgNum.transform.position = pos;
            dmgNum.gameObject.SetActive(true);
        }

        public void DamageEveryEnemy(float damage)
        {
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
                _activeEnemies[i].Health.TakeDamage(damage, 0f, false, _activeEnemies[i].transform.position);
        }

        public void DamageEveryEnemyInRange(float damage, float knockback, Vector2 pos, float range, Action<Enemy> action)
        {
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = _activeEnemies[i];
                if (Vector2.Distance(enemy.Position, pos) > range)
                    continue;

                const float RANGE = .08f;
                float x = enemy.Position.x + Mathf.Clamp(pos.x - enemy.Position.x, -RANGE, RANGE);
                float y = enemy.Position.y + Mathf.Clamp(pos.y - enemy.Position.y, -RANGE, RANGE);
                var preDmg = enemy.Health.Hp;
                enemy.Health.TakeDamage(damage, knockback, false, new(x, y));
                action(enemy);
                EnemyDamagedInRange?.Invoke(preDmg - enemy.Health.Hp);
            }
        }

        public void SpawnThornAnimation(Enemy enemy)
        {
            var thorn = _thornAnimationPool.GetObject();
            thorn.transform.localPosition = enemy.Position;
            thorn.Play(overRide: true);
            thorn.gameObject.SetActive(true);
        }

        public void LightningDamage(float damage, float range, Vector2 firstPoint, IDamageable firstDamageable, float lightningChance, int minChains)
        {
            var lightingPoints = new List<Vector2>()
            {
                firstPoint,
                firstDamageable.Pos
            };
            var preDmg = firstDamageable.Hp;
            firstDamageable.TakeDamage(damage, 0.05f, false, firstDamageable.Pos);
            EnemyDamagedInRange?.Invoke(preDmg - firstDamageable.Hp);

            var prevPos = firstDamageable.Pos;
            while (Random.value < lightningChance || lightingPoints.Count - 2 < minChains)
            {
                float prevDistance = 0f;
                Enemy enemyToAffect = null;
                for (int i = _activeEnemies.Count - 1; i >= 0; i--)
                {
                    var enemy = _activeEnemies[i];
                    float distance = Vector2.Distance(prevPos, enemy.Position);
                    if (distance < range && distance > prevDistance)
                    {
                        enemyToAffect = enemy;
                        prevDistance = distance;
                        prevPos = enemy.Position;
                        preDmg = enemy.Health.Hp;
                        enemy.Health.TakeDamage(damage, 0.05f, false, enemy.Position);
                        EnemyDamagedInRange?.Invoke(preDmg - enemy.Health.Hp);
                    }
                }

                if (enemyToAffect == null)
                    break;

                lightingPoints.Add(prevPos);
            }

            StartCoroutine(LightiningEffect(lightingPoints));
        }

        private IEnumerator LightiningEffect(IList<Vector2> positions)
        {
            // Get Lightnings
            var lightnings = new LineRenderer[positions.Count - 1];
            var segmentLengths = new float[positions.Count - 1];
            for (int i = 0; i < lightnings.Length; i++)
            {
                var lightning = _lightningEffectPool.GetObject();
                lightning.enabled = true;
                lightning.startColor = _lightningColours.PickRandom();
                lightning.endColor = _lightningColours.PickRandom();
                lightnings[i] = lightning;
                segmentLengths[i] = _lightningSegmentLengthRange.Random();
            }

            var updates = _lightningUpdatesRange.Random();
            var wait = new WaitForSeconds(_lightningTimeRange.Random() /  updates);
            // Draw Lightnings
            for (int i = 0; i < updates; i++)
            {
                for (int j = 0; j < lightnings.Length; j++)
                {
                    var from = positions[j];
                    var to = positions[j + 1];
                    var distance = Vector2.Distance(from, to);
                    var segments = 4;
                    if (distance > segmentLengths[j])
                        segments = Mathf.FloorToInt(distance / segmentLengths[j]) + 2;

                    var lightning = lightnings[j];
                    lightning.positionCount = segments;
                    lightning.SetPosition(0, from);
                    for (int l = 1; l < segments - 1; l++)
                    {
                        var lerp = Vector2.Lerp(from, to, (float)l / segments);
                        lightning.SetPosition(l, new(lerp.x + _lightningSegmentOffsetRange.Random(), lerp.y + _lightningSegmentOffsetRange.Random()));
                    }
                    lightning.SetPosition(segments - 1, to);
                }

                yield return wait;
            }
            // Return Lightnings
            for (int i = 0; i < lightnings.Length; i++)
            {
                _lightningEffectPool.ReturnObject(lightnings[i]);
                lightnings[i].enabled = false;
            }
        }

        private void SpawnEnemy()
        {
            var enemy = _weightedPoolOfEnemies.GetWeightedObject().Object.GetObject();
            var randX = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
            var randY = Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y);
            enemy.transform.localPosition = new(randX, randY);
            enemy.Spawn(_t, _tClamped);
            _activeEnemies.Add(enemy);
        }

        private void EnemyCreated(Enemy enemy)
        {
            enemy.Init(GameManager.Instance.Witch);
            enemy.ReturnToPool += ReturnEnemyToPool;
            enemy.OnDied += EnemyDied;
            enemy.Health.OnDamaged += EnemyDamaged;
            enemy.OnFireEffectApplied += _fireParticlePool.GetObject;
        }

        private void UnSubToEnemy(Enemy enemy)
        {
            enemy.ReturnToPool -= ReturnEnemyToPool;
            enemy.OnDied -= EnemyDied;
            enemy.Health.OnDamaged -= EnemyDamaged;
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
            for (int i = 0; i < _minEnemiesAtAllTimes.Evaluate(_t) - _activeEnemies.Count; i++)
                SpawnEnemy();
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

        private void EnemyDamaged(float damage, float knockback, bool crit, Vector2 pos)
        {
            SpawnDamageNum(damage.ToString($"0.00"), crit ? _critColour : _normalColour, pos);

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

        private void FireParticleCreated(Parentable fireParticle)
        {
            fireParticle.gameObject.SetActive(true);
            fireParticle.OnReturn += ReturnFireParticle;
        }

        private void ReturnFireParticle(Parentable fireParticle)
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
}
