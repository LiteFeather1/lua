using LTFUtils;
using UnityEngine;
using UnityEngine.UI;

public class CardUIDropContainerSeek : CardUIDropContainer
{
    [SerializeField] private Image i_powerUpIcon;
    [SerializeField] private Image i_powerUpTier;
    public WeightedObject<PowerUp> WeightedPowerUpSeeking { get; private set; }

    public System.Action<WeightedObject<PowerUp>, WeightedObject<PowerUp>> OnPowerUpDropped { get; set; }

    protected override void UseCard(CardUIPowerUp card)
    {
        card.ReturnToPile();
        var newWeight = new WeightedObject<PowerUp>(card.PowerUp, Mathf.Min(card.PowerUp.Weight * 3f, 0.5f));
        OnPowerUpDropped?.Invoke(WeightedPowerUpSeeking, newWeight);
        WeightedPowerUpSeeking = newWeight;
        i_powerUpIcon.sprite = card.PowerUp.Icon;
        i_powerUpTier.sprite = card.PowerUp.TierIcon;
        i_card.color = card.PowerUp.RarityColour;
    }
}
