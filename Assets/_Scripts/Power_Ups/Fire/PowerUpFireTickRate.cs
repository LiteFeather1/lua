using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Fire/Tick Rate")]
    public class PowerUpFireTickRate : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.EffectCreatorFire.TickRate;
        }
    }
}