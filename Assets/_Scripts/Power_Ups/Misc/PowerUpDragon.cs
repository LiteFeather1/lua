using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/Misc/Dragon")]
public class PowerUpDragon : PowerUp
{
    [SerializeField] private Dragon _dragonPrefab;
    protected override string Num => "";

    public override void ApplyEffect(GameManager gm)
    {
        var dragon = Instantiate(_dragonPrefab);
        dragon.Activate(gm.Witch);
    }
}
