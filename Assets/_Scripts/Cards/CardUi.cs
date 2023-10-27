using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image i_card;

    public Action OnCardHovered { get; set; }
    public Action OnCardUnHovered { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCardHovered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCardUnHovered?.Invoke();
    }
}
