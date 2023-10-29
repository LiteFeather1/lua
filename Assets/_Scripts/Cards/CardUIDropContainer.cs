using UnityEngine.EventSystems;

public abstract class CardUIDropContainer : CardUi, IDropHandler
{
    public System.Action OnCardUsed { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out CardUIPowerUp card))
        {
            UseCard(card);
            OnCardUsed?.Invoke();
        }
    }

    protected abstract void UseCard(CardUIPowerUp card);
}
