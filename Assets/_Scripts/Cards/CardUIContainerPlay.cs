using UnityEngine;

namespace Lua.Cards
{
    public class CardUIContainerPlay : CardUIDropContainer
    {
        [SerializeField] private GameManager _gm;
        [SerializeField] private AudioClip _played, _denied;

        public System.Action<PowerUp> OnPowerPlayed { get; set; }

        protected override void UseCard(CardUIPowerUp card)
        {
            if (_gm.Witch.Currency < card.PowerUp.Cost)
            {
                AudioManager.Instance.PlayOneShot(_denied);
#if UNITY_EDITOR
                if (card.PowerUp.Rarity.name.Contains("Debug"))
                    _gm.Witch.ModifyCurrency(card.Cost);
                else
#endif
                return;
            }

            AudioManager.Instance.PlayOneShot(_played);
            _gm.Witch.ModifyCurrency(-card.Cost);
            card.PowerUp.PowerUpPlayed(_gm);
            card.ReturnToPile();
            OnPowerPlayed?.Invoke(card.PowerUp);
        }
    }
}
