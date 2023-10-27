using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUIPowerUp : CardUi, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image i_card;
    [SerializeField] private Image i_powerUp;
    private Transform _originalParent;
    private PowerUp _powerUp;

    public Action<CardUIPowerUp> OnPickedUp { get; set; }
    public Action<CardUIPowerUp> OnPlayed { get; set; }
    public Action<CardUIPowerUp> OnDropped { get; set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnPickedUp?.Invoke(this);

            _originalParent = transform.parent;
            transform.SetParent(transform.parent.parent);

            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.SetParent(_originalParent);
            _canvasGroup.blocksRaycasts = true;
            OnDropped?.Invoke(this);
        }
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        _powerUp = powerUp;
    }
}

public class DropCardContainer : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out CardUIPowerUp card))
        {
            //card.OnPlayed
        }
    }
}
