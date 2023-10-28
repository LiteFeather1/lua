using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI t_timeText;

    [Header("Witch HP")]
    [SerializeField] private Image i_hpFill;
    [SerializeField] private float _maxHPSize = 272f;

    [Header("Witch Portrait")]
    [SerializeField] private Image i_witchPortrait;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private TextMeshProUGUI t_currence;

    public void BindToWitch(Witch witch)
    {
        witch.OnDamaged += WitchDamaged;
        witch.OnInvulnerabilityEnded += InvulnerabilityEnded;
        witch.Health.OnMaxHPIncreased += MaxHpIncreased;
    }

    public void UnBindToWitch(Witch witch)
    {
        witch.OnDamaged -= WitchDamaged;
        witch.OnInvulnerabilityEnded -= InvulnerabilityEnded;
        witch.Health.OnMaxHPIncreased -= MaxHpIncreased;
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
        t_currence.text = amount.ToString();
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

    private void InvulnerabilityEnded()
    {
        i_witchPortrait.sprite = _defaultSprite;
    }
}
