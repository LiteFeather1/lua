using UnityEngine;

namespace Lua.StateMachine.Enemies
{
    // Used if the enemy base is not on the root collider
    public class DeactivatableEnemyDeferrer : MonoBehaviour, IDeactivatable
    {
        [SerializeField] private Enemy _enemy;

        public void Deactivate() => _enemy.Deactivate();
    }
}
