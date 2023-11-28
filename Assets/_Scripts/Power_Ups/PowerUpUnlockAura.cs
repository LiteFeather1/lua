using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Witch/Aura Unlock")]
public class PowerUpUnlockAura : PowerUp
{
    [Header("Unlock Witch Aura")]
    [SerializeField] private Sprite _auraSprite;

    protected override string Num => "";

    protected override void ApplyEffect(GameManager gm)
    {
        gm.Witch.Aura.SetAura(_auraSprite);
        gm.CardManager.RemoveCardsOfType(PowerUpType);
        gm.Witch.Damage.ForceRecalculate();
    }
}
