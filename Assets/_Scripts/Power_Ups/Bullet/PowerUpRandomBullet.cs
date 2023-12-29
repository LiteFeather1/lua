using System;
using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Bullet/Random Bullet")]
    public class PowerUpRandomBullet : PowerUpFlat
    {
        [Header("Random Bullet")]
        [SerializeField, TextArea(1, 2)] private string _unlockText = "Periodically Shot bullet in random directions";
        private static bool _picked;

        public override string Effect => _picked ? base.Effect : _unlockText;

        public override void Reset()
        {
            base.Reset();
            _picked = false;
        }

        protected override Func<int, int> ModifyValue(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.AddRandomBullet;
        }
    }
}
