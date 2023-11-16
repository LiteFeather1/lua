using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Misc/Dragon")]
public class PowerUpDragon : PowerUp
{
    [Header("Power Up Dragon")]
    [SerializeField] private Dragon _dragonPrefab;
    protected override string Num => "";

    protected override void ApplyEffect(GameManager gm)
    {
        Instantiate(_dragonPrefab).Activate(gm.Witch);
        gm.CardManager.RemoveCardsOfType(PowerUpType);
    }
}
