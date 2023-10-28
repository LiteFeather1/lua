using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI t_timeText;

    [Header("Witch HP")]
    [SerializeField] private Image i_hpFill;
    [SerializeField] private float _maxHPSize = 272f;

    [Header("Witch Shield")]
    [SerializeField] private Image i_shield;
    private float ShieldWidth => i_shield.sprite.texture.width;

    [Header("Witch Portrait")]
    [SerializeField] private Image i_witchPortrait;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private TextMeshProUGUI t_currency;

    public void BindToWitch(Witch witch)
    {
        witch.OnDamaged += WitchDamaged;
        witch.OnInvulnerabilityEnded += InvulnerabilityEnded;

        witch.Health.OnMaxHPIncreased += MaxHpIncreased;
        witch.Health.OnShieldDamaged += ShieldRemoved;
        witch.Health.OnShieldGained += ShieldGained;

        witch.OnCurrencyModified += UpdateCurrency;
    }

    public void UnBindToWitch(Witch witch)
    {
        witch.OnDamaged -= WitchDamaged;
        witch.OnInvulnerabilityEnded -= InvulnerabilityEnded;

        witch.Health.OnMaxHPIncreased -= MaxHpIncreased;
        witch.Health.OnShieldDamaged -= ShieldRemoved;
        witch.Health.OnShieldGained -= ShieldGained;

        witch.OnCurrencyModified -= UpdateCurrency;
    }

    public void UpdateTime(float time)
    {
        var minutes = (int)(time / 60f);
        var seconds = time % 60f;
        var mili = seconds * 100f % 100f;
        t_timeText.text = $"{minutes:00} : {(int)seconds:00} . {mili:000}";
    }

    public void SetCurrence(int amount)
    {
        t_currency.text = amount.ToString();
    }

    private void WitchDamaged(float t)
    {
        i_witchPortrait.sprite = _damagedSprite;
        i_hpFill.fillAmount = t;    
    }

    private void MaxHpIncreased(float maxHP, float t)
    {
        if (maxHP > _maxHPSize)
            maxHP = _maxHPSize;
        i_hpFill.rectTransform.sizeDelta = new(maxHP, i_hpFill.rectTransform.sizeDelta.y);
        i_hpFill.fillAmount = t;    
    }

    private void ShieldGained(int amount)
    {
        i_shield.rectTransform.sizeDelta += new Vector2(ShieldWidth * amount, 0f);
    }

    private void ShieldRemoved()
    {
        i_shield.rectTransform.sizeDelta -= new Vector2(ShieldWidth, 0f);
    }

    private void InvulnerabilityEnded()
    {
        i_witchPortrait.sprite = _defaultSprite;
    }

    private void UpdateCurrency(int amount)
    {
        t_currency.text = amount.ToString();
    }
}
