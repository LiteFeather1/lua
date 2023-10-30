﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI t_timeText;

    [Header("Fade")]
    [SerializeField] private float _fadeAlpha = .33f;
    [SerializeField] private float _fadeTime = .5f;
    [SerializeField] private AnimationCurve _fadeCurve;

    [Header("Witch HP")]
    [SerializeField] private Image i_hpFill;
    [SerializeField] private Image i_hpFrame;
    [SerializeField] private float _maxHPSize = 272f;

    [Header("Witch Shield")]
    [SerializeField] private Image i_shield;

    [Header("Witch Portrait")]
    [SerializeField] private Image i_witchPortrait;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private TextMeshProUGUI t_currency;

    public void BindToWitch(Witch witch)
    {
        witch.OnHPModified += HpModified;
        witch.OnDamaged += WitchDamaged;
        witch.OnInvulnerabilityEnded += InvulnerabilityEnded;

        witch.Health.OnMaxHPIncreased += MaxHpIncreased;
        witch.Health.OnShieldDamaged += ShieldRemoved;
        witch.Health.OnShieldGained += ShieldGained;

        witch.OnCurrencyModified += UpdateCurrency;
    }

    public void UnBindToWitch(Witch witch)
    {
        witch.OnHPModified -= HpModified;
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

    public void FadeGroup(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeGroupCO(canvasGroup, _fadeAlpha));
    }

    public void UnFadeGroup(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeGroupCO(canvasGroup, 1f));
    }

    private IEnumerator FadeGroupCO(CanvasGroup canvasGroup, float fadeAlpha)
    {
        float initialAlpha = canvasGroup.alpha;
        float eTime = 0f;
        while (eTime  < _fadeTime)
        {
            float t = _fadeCurve.Evaluate(eTime / _fadeTime);
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, fadeAlpha, t);
            eTime += Time.deltaTime;
            yield return null;
        }
    }

    private void HpModified(float t)
    {
        i_hpFill.fillAmount = t;
    }

    private void WitchDamaged()
    {
        i_witchPortrait.sprite = _damagedSprite;
    }

    private void MaxHpIncreased(float maxHP, float t)
    {
        if (maxHP > _maxHPSize)
            maxHP = _maxHPSize;
        i_hpFill.rectTransform.sizeDelta = new(maxHP, i_hpFill.rectTransform.sizeDelta.y);
        var hpFrameWidth = maxHP + i_hpFrame.sprite.texture.width;
        i_hpFrame.rectTransform.sizeDelta = new(hpFrameWidth, i_hpFrame.rectTransform.sizeDelta.y);
        i_hpFill.fillAmount = t;    
    }

    private void ShieldGained(int amount)
    {
        var shieldWidth = i_shield.sprite.texture.width;
        i_shield.rectTransform.sizeDelta += new Vector2(shieldWidth * amount, 0f);
    }

    private void ShieldRemoved()
    {
        var shieldWidth = i_shield.sprite.texture.width;
        i_shield.rectTransform.sizeDelta -= new Vector2(shieldWidth, 0f);
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
