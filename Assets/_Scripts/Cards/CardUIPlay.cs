using UnityEngine;

public class CardUIPlay : CardUIDropContainer
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private AudioClip _played, _denied;

    protected override void UseCard(CardUIPowerUp card)
    {
        if (_gm.Witch.Currency < card.PowerUp.Cost)
        {
            AudioManager.Instance.PlayOneShot(_denied);
            return;
        }

        _gm.Witch.ModifyCurrency(-card.PowerUp.Cost);
        card.PowerUp.ApplyEffect(_gm);
        card.Used();
        AudioManager.Instance.PlayOneShot(_played);
    }
}
