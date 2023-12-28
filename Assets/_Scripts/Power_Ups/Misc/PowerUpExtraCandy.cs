using UnityEngine;
using LTF.CompositeValue;
using Lua.Managers;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Misc/Extra Candy")]
    public class PowerUpExtraCandy : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(GameManager gm)
        {
            return gm.SpawnManager.ChanceToExtraCandy;
        }
    }
}
