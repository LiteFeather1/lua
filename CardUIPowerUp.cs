using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}

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
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        _powerUp = powerUp;
    }
}
