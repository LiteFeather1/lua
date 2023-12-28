using UnityEngine.EventSystems;

namespace Lua.PowerUps.Cards
{
    public abstract class CardUIDropContainer : CardUi, IDropHandler
    {
        public System.Action OnCardUsed { get; set; }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left 
                && eventData.pointerDrag.TryGetComponent(out CardUIPowerUp card))
                DropCard(card);
        }

        public void DropCard(CardUIPowerUp card)
        {
            UseCard(card);
            OnCardUsed?.Invoke();
        }

        protected abstract void UseCard(CardUIPowerUp card);
    }
}
