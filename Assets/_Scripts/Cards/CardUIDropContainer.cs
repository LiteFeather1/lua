using UnityEngine.EventSystems;

public abstract class CardUIDropContainer : CardUi, IDropHandler
{
    public System.Action CardUsed { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out CardUIPowerUp card))
        {
            print("Hello");
            UseCard(card);
            CardUsed?.Invoke();
        }
    }

    protected abstract void UseCard(CardUIPowerUp card);
}
