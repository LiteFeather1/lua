using LTFUtils;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Weighter<ObjectPool<Enemy>> _weightedPoolOfEnemies;
    private Dictionary<string, ObjectPool<Enemy>> _enemyToPool;
    [SerializeField] private Vector2Int _maxEnemiesPerBurstRange;
    [SerializeField] private Vector2Int _minEnemiesPerBurstRange;
    [SerializeField] private Vector2Int _enemiesPerBurstOffset;
    [SerializeField] private Vector2 _maxSpawnTimeRange;
    [SerializeField] private Vector2 _minSpawnTimeRange;
    [SerializeField] private Vector2 _spawnTimeOffset;
    private float _spawnTime;
    private float _elapsedTime;
    [SerializeField] private Transform _spawn1;
    [SerializeField] private Transform _spawn2;

    private void Start()
    {
        _enemyToPool = new(_weightedPoolOfEnemies.Count);
        for (int i = 0; i < _weightedPoolOfEnemies.Count; i++)
        {
            var pool = _weightedPoolOfEnemies.Objects[i].Object;
            pool.InitPool();
            foreach (var enemy in pool.Objects)
            {
                enemy.Init(GameManager.Instance.Witch);
                enemy.ReturnToPool += ReturnToPool;
            }
            _enemyToPool.Add(pool.Object.Name, pool);
            pool.ObjectCreated += ObjectCreated;
        }

        var offSet = Random.Range(-_spawnTimeOffset.x, _spawnTimeOffset.x);
        var spawnTime = Random.Range(_minSpawnTimeRange.x, _maxSpawnTimeRange.x);
        _spawnTime = spawnTime + offSet;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _spawnTime)
        {
            _elapsedTime = 0f;
            var tClamped = GameManager.Instance.TClamped();
            var spawnTime = Random.Range(_minSpawnTimeRange.Evaluate(tClamped), _maxSpawnTimeRange.Evaluate(tClamped));
            var offSetTime = _spawnTimeOffset.Evaluate(tClamped);
            _spawnTime = spawnTime + Random.Range(-offSetTime, offSetTime);

            var t = GameManager.Instance.T();
            var minBurstAmount = _minEnemiesPerBurstRange.Evaluate(t);
            var maxBurstAmount = _maxEnemiesPerBurstRange.Evaluate(t);
            var offSetBurstT = _enemiesPerBurstOffset.Evaluate(t);
            var spawnAmount = Random.Range(minBurstAmount, maxBurstAmount) + Random.Range(0, offSetBurstT);
            for (int i = 0; i < spawnAmount; i++)
            {
                var enemy = _weightedPoolOfEnemies.GetWeightedObject().Object.GetObject();
                var randX = Random.Range(_spawn1.localPosition.x, _spawn2.localPosition.x);
                var randY = Random.Range(_spawn1.localPosition.y, _spawn2.localPosition.y);
                enemy.transform.localPosition = new(randX, randY);
                enemy.Spawn(tClamped);
            }
        }
    }

    private void ObjectCreated(Enemy enemy)
    {
        enemy.Init(GameManager.Instance.Witch);
        enemy.ReturnToPool += ReturnToPool;
    }

    private void ReturnToPool(Enemy enemy)
    {
        _enemyToPool[enemy.Name].ReturnObject(enemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_spawn1.localPosition, _spawn2.localPosition);
    }
}
