using UnityEngine;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Lightining/Lightning Damage")]
    public class PowerUpLightningDamage : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.Witch.LightningBaseDamage;
        }
    }
}
