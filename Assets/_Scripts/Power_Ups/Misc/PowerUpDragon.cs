using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Misc/Dragon")]
public class PowerUpDragon : PowerUp
{
    protected override string Num => "";

    public override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Dragon.Activate();
    }
}
