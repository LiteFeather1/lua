using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Misc/Extra Candy")]
    public class PowerUpExtraCandy : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.SpawnManager.ChanceToExtraCandy;
        }
    }
}
