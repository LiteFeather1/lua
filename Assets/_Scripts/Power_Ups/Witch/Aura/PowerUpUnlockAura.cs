using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Aura/Aura Unlock")]
public class PowerUpUnlockAura : PowerUp
{
    [Header("Unlock Witch Aura")]
    [SerializeField] private Sprite _auraSprite;

    protected override string Num => "";

    protected override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Aura.SetAura(_auraSprite);
        gm.Witch.Damage.ForceRecalculate();
        gm.Witch.CritChance.ForceRecalculate();
        gm.Witch.CritMultiplier.ForceRecalculate();
        Remove(gm.CardManager);
    }
}
