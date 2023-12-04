using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Dragon/Baby Dragon")]
public class PowerUpDragonBaby : PowerUp
{
    [Header("Power Up Dragon")]
    [SerializeField] private Dragon _dragonPrefab;
    protected override string Num => "";

    protected override void ApplyEffect(GameManager gm)
    {
        Dragon dragon = Instantiate(_dragonPrefab);
        dragon.Activate(gm.Witch);
        dragon.Grow(.25f);
        gm.Dragon = dragon;
        Remove(gm.CardManager);
    }
}
