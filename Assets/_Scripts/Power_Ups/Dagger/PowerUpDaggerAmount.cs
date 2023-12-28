using System;
using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dagger/Amount")]
    public class PowerUpDaggerAmount : PowerUpFlat
    {
        [Header("Power Up Dagger Amount")]
        [SerializeField, TextArea] private string _unlockText;
        private static bool _unlocked = false;

        public override string Effect => _unlocked ? base.Effect : _unlockText;

        public override void Reset()
        {
            base.Reset();
            _unlocked = false;
        }

        protected override Func<int, int> ModifyValue(GameManager gm)
        {
            _unlocked = true;
            return gm.Witch.DaggerAmount;
        }
    }
}
