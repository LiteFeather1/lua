using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Moon/Amount")]
    public class PowerUpMoonAmount : PowerUpFlat
    {
        [Header("Orbital Bullet Amount")]
        [SerializeField] private string _unlockText = "Unlock Moon Bullet";
        private static bool _unlocked = false;

        public override string Effect => _unlocked ? base.Effect : _unlockText;

        public override void Reset()
        {
            base.Reset();
            _unlocked = false;
        }

        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.AddShootingMoonBullet;
        }
    }
}
