using LTFUtils;
using UnityEngine;
using UnityEngine.UI;

public class CardUIDropContainerSeek : CardUIDropContainer
{
    [SerializeField] private Image i_powerUpIcon;
    public WeightedObject<PowerUp> WeightedPowerUpSeeking { get; private set; }

    public System.Action<WeightedObject<PowerUp>, WeightedObject<PowerUp>> OnPowerUpDropped { get; set; }

    protected override void UseCard(CardUIPowerUp card)
    {
        card.Used();
        var newWeight = new WeightedObject<PowerUp>(card.PowerUp, card.PowerUp.Weight * 4f);
        OnPowerUpDropped?.Invoke(WeightedPowerUpSeeking, newWeight);
        WeightedPowerUpSeeking = newWeight;
        i_powerUpIcon.sprite = card.PowerUp.Icon;
        i_card.color = card.PowerUp.RarityColour;
    }
}
