using UnityEngine;

public class Deactivator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDeactivatable deactivatable))
        {
            deactivatable.Deactivate();
        }
    }
}
