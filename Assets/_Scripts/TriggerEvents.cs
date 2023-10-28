using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnterTriggerEvent;
    [SerializeField] private UnityEvent _onExitTriggerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onEnterTriggerEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _onExitTriggerEvent.Invoke();
    }
}
