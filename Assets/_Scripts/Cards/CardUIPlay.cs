using UnityEngine;

public class CardUIPlay : CardUIDropContainer
{
    [SerializeField] private GameManager _gm;

    protected override void UseCard(CardUIPowerUp card)
    {
        if (_gm.Witch.Currency < card.PowerUp.Cost)
            return;
        _gm.Witch.ModifyCurrency(-card.PowerUp.Cost);
        card.PowerUp.ApplyEffect(_gm);
        card.Used();
    }
}
