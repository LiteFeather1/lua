using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUIPowerUp : CardUi, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image i_powerUp;
    [SerializeField] private TextMeshProUGUI t_cardName; 
    private Transform _originalParent;
    private PowerUp _powerUp;

    public PowerUp PowerUp => _powerUp;

    public Action<CardUIPowerUp> OnPickedUp { get; set; }
    public Action<CardUIPowerUp> OnUsed { get; set; }
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
        i_powerUp.sprite = powerUp.Icon;
        i_card.color = powerUp.RarityColour;
        t_cardName.color = powerUp.RarityColour;
        t_cardName.text = powerUp.Name;
    }

    public void Used()
    {
        OnUsed?.Invoke(this);
    }
}
