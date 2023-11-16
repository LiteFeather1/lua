using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Defence")]
public class PowerUpDefence : PowerUpModifier
{
    protected override CompositeValue ValueToModify(GameManager gm)
    {
        return gm.Witch.Health.Defence;
    }
}
