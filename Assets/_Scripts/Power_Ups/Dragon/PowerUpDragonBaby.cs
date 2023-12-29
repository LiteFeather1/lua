using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(menuName = "Power Up/Dragon/Baby Dragon")]
    public class PowerUpDragonBaby : PowerUp
    {
        [Header("Power Up Dragon")]
        [SerializeField] private Dragon _dragonPrefab;
        protected override string Num => "";

        protected override void ApplyEffect(Cards.CardManager cm)
        {
            Dragon dragon = Instantiate(_dragonPrefab);
            dragon.Activate(cm.GameManager.Witch);
            dragon.Grow(.25f);
            cm.GameManager.Dragon = dragon;
            Remove(cm);
        }
    }
}
