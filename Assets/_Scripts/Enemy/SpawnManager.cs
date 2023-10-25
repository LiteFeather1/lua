using LTFUtils;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Weighter<ObjectPool<Enemy>> _weightedPoolOfEnemies;
    private Dictionary<string, ObjectPool<Enemy>> _enemyToPool;
    [SerializeField] private Vector2Int _enemiesPerBurstRange;
    [SerializeField] private float _spawnTime;
    private float _elapsedTime;
    [SerializeField] private float _spawnX;
    [SerializeField] private float _spawnYRange;

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
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _spawnTime)
        {
            _elapsedTime = 0f;
            var spawnAmount = Random.Range(_enemiesPerBurstRange.x, _enemiesPerBurstRange.y);
            for (int i = 0; i < spawnAmount; i++)
            {
                var randY = Random.Range(-_spawnYRange, _spawnYRange);
                var enemy = _weightedPoolOfEnemies.GetWeightedObject().Object.GetObject();
                enemy.transform.position = transform.position + new Vector3(_spawnX, randY);
                enemy.Spawn(GameManager.Instance.T());
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
        var from = transform.position + new Vector3(_spawnX, _spawnYRange);

        Gizmos.DrawRay(from, 2f * _spawnYRange * Vector3.down);
    }
}
