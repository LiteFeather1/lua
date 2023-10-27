using UnityEngine;

public class CardUIPlay : CardUIDropContainer
{
    [SerializeField] private GameManager _gm;

    protected override void UseCard(CardUIPowerUp card)
    {
        card.PowerUp.ApplyEffect(_gm);
        card.Used();
    }
}
