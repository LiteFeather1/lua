using UnityEngine;

public class DeactivatableEnemyDeferrer : MonoBehaviour, IDeactivatable
{
    [SerializeField] private Enemy _enemy;

    public void Deactivate() => _enemy.Deactivate();
}
