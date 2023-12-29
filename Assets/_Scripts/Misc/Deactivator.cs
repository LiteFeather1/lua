using UnityEngine;

namespace Lua.Misc
{
    public class Deactivator : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out StateMachineCore.IDeactivatable deactivatable))
                deactivatable.Deactivate();
        }
    }
}
