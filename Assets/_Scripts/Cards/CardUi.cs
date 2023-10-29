using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CardUi : MonoBehaviour, IPointerExitHandler
{
    [Header("Card UI")]
    [SerializeField] protected Image i_card;
        
    public Action OnCardUnHovered { get; set; }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        OnCardUnHovered?.Invoke();
    }
}
