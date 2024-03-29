using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    public abstract class PowerUp : ScriptableObject
    {
        [Header("Power Up")]
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _effect;
        [SerializeField] private int _cost = 4;
        [SerializeField] private Rarity _rarity;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _tierIcon;
        [SerializeField] private PowerUp[] _powerUpsToUnlock;
        private bool _unlockedPowerUps;

        public string PowerUpType => GetType().Name;
        public string Name => _name;
        public virtual string Effect => _effect.Replace("$", Num);
        public int Cost => _cost;
        public Rarity Rarity => _rarity;
        public float Weight => _rarity.Weight;
        public Color RarityColour => _rarity.RarityColour;
        public Sprite Icon => _icon;   
        public Sprite TierIcon => _tierIcon;   
        public PowerUp[] PowerUpsToUnlock => _powerUpsToUnlock;

        protected abstract string Num { get; }

        public virtual void Reset()
        {
            _unlockedPowerUps = false;
        }

        public void PowerUpPlayed(Cards.CardManager cm)
        {
            ApplyEffect(cm);

            if (_unlockedPowerUps || _powerUpsToUnlock.Length == 0)
                return;

            _unlockedPowerUps = true;
            cm.AddWeightedPowerUps(_powerUpsToUnlock);
        }

        protected void Remove(Cards.CardManager cardManager) => cardManager.RemoveCardsOfType(GetType().Name);

        protected abstract void ApplyEffect(Cards.CardManager cm);
    }
}
