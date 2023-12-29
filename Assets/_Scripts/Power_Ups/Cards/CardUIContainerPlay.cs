using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps.Cards
{
    public class CardUIContainerPlay : CardUIDropContainer
    {
        [SerializeField] private CardManager _cm;
        [SerializeField] private AudioClip _played, _denied;

        public System.Action<PowerUp> OnPowerPlayed { get; set; }

        protected override void UseCard(CardUIPowerUp card)
        {
            if (_cm.GameManager.Witch.Currency < card.PowerUp.Cost)
            {
                AudioManager.Instance.PlayOneShot(_denied);
#if UNITY_EDITOR
                if (card.PowerUp.Rarity.name.Contains("Debug"))
                    _cm.GameManager.Witch.ModifyCurrency(card.Cost);
                else
#endif
                return;
            }

            AudioManager.Instance.PlayOneShot(_played);
            _cm.GameManager.Witch.ModifyCurrency(-card.Cost);
            card.PowerUp.PowerUpPlayed(_cm);
            card.ReturnToPile();
            OnPowerPlayed?.Invoke(card.PowerUp);
        }
    }
}
