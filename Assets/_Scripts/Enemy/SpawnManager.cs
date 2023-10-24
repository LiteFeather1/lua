using LTFUtils;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Weighter<ObjectPool<Enemy>> _weightedPoolOfEnemies;
}
