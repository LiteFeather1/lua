using UnityEngine;
using LTF.CompositeValue;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Witch/Speed")]
    public class PowerUpSpeed : PowerUpModifier
    {
        protected override CompositeValue ValueToModify(Cards.CardManager cm)
        {
            return cm.GameManager.Witch.Acceleration;
        }

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            base.ApplyEffect(cm);
            cm.GameManager.Witch.EvaluateDrag();
        }
    }
}
