using RetroAnimation;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Witch")]
    [SerializeField] private Witch _witch;
    [SerializeField] private AudioClip ac_thornDamage;
    private bool _witchDied;
    private bool _saved;

    [Header("Settings")]
    [SerializeField] private AudioClip _music;
    [SerializeField] private float _slowDownScale = .25f;
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

    [Header("Managers")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraShake _shake;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private EndScreenManager _endScreenManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private CardManager _cardManager;
    public Dragon Dragon { get; set; }

    [Header("Volume")]
    [SerializeField] private Volume _volume;
    [SerializeField] private float _aberrationIntensity = 1f;
    [SerializeField] private float _aberrationDuration = .5f;
    private ChromaticAberration _chromaticAberration;

    [Header("Recycle Effects")]
    [SerializeField] private CompositeValue _onRecycleDamageEnemies;
    [SerializeField] private CompositeValue _onRecycleHeal;
    [SerializeField] private CompositeValue _onRecycleAddCurrency;
    [SerializeField] private CompositeValue _onRecycleRefund;

    [Header("On Card Played Effect")]
    [SerializeField] private CompositeValue _onCardPlayedDamageEnemies;
    [SerializeField] private CompositeValue _onCardPlayedHeal;
    [SerializeField] private CompositeValue _onCardPlayedRefund;

    [Header("Others")]
    [SerializeField] private MonoBehaviour[] _toDisableOnPause;

    private static readonly WaitForSecondsRealtime _hitStop = new(.05f);
    private IEnumerator _slowPitch;

    public static GameManager Instance { get; private set; }

    public Witch Witch => _witch;
    public Camera Camera => _camera;
    public SpawnManager SpawnManager => _spawnManager;
    public CardManager CardManager => _cardManager;

    public CompositeValue OnRecycleDamageEnemies => _onRecycleDamageEnemies;
    public CompositeValue OnRecycleHeal => _onRecycleHeal;
    public CompositeValue OnRecycleAddCurrency => _onRecycleAddCurrency;
    public CompositeValue OnRecycleRefund => _onRecycleRefund;

    public CompositeValue OnCardPlayedDamageEnemies => _onCardPlayedDamageEnemies;
    public CompositeValue OnCardPlayedHeal => _onCardPlayedHeal;
    public CompositeValue OnCardPlayedRefund => _onCardPlayedRefund;

    private void Awake()
    {
        Instance = this;

        InputManager.Inputs.Player.Pause_UnPause.performed += PauseUnpause;
        InputManager.Inputs.Player.Mute_UnMute.performed += MuteUnMute;

        _witch.OnDamaged += WitchDamaged;
        _witch.Health.OnDeath += WitchDied;
        _witch.Health.OnDodge += WitchDodged;
        _witch.FlipBook.OnAnimationFinished += WitchDeathAnimationFinished;
        _witch.OnLightningEffectApplied += WitchLightning;

        _cardManager.OnCardHovered += SlowDown;
        _cardManager.OnCardUnHovered += UnSlowDown;
        _cardManager.Recycler.OnCardUsed += CardRecycled;
        _cardManager.PlayArea.OnPowerPlayed += CardPlayed;

        _spawnManager.EnemyHurt += _shake.Shake;
        _spawnManager.EnemyDamagedInRange += EnemyDamagedInRange;

        _uiManager.BindToWitch(_witch);

        _slowPitch = SetPitch(1f);

        _volume.profile.TryGet(out ChromaticAberration aberration);
        _chromaticAberration = aberration;

        Application.quitting += SavePlayerPrefs;
    }

    private void Start()
    {
        AudioManager.Instance.MusicSource.clip = _music;
        AudioManager.Instance.MusicSource.Play();
    }

    private void Update()
    {
        _playTime += Time.deltaTime;

        var t = _playTime / _timeForMaxDifficult;
        var tClamped = t;
        if (tClamped > 1f)
            tClamped = 1f;

        _spawnManager.Tick(t, tClamped);
        _uiManager.UpdateTime(_playTime);
    }

    private void FixedUpdate()
    {
        if (_witchDied)
            _witch.Move(InputManager.Inputs.Player.Moviment.ReadValue<Vector2>().normalized);
    }

    private void OnDestroy()
    {
        InputManager.Inputs.Player.Pause_UnPause.performed -= PauseUnpause;
        InputManager.Inputs.Player.Mute_UnMute.performed -= MuteUnMute;

        _witch.OnDamaged -= WitchDamaged;
        _witch.Health.OnDeath -= WitchDied;
        _witch.Health.OnDodge -= WitchDodged;
        _witch.OnLightningEffectApplied -= WitchLightning;
        _witch.FlipBook.OnAnimationFinished -= WitchDeathAnimationFinished;

        _cardManager.OnCardHovered -= SlowDown;
        _cardManager.OnCardUnHovered -= UnSlowDown;
        _cardManager.Recycler.OnCardUsed -= CardRecycled;
        _cardManager.PlayArea.OnPowerPlayed -= CardPlayed;

        _spawnManager.EnemyHurt -= _shake.Shake;
        _spawnManager.EnemyDamagedInRange -= EnemyDamagedInRange;

        _uiManager.UnBindToWitch(_witch);

        Application.quitting -= SavePlayerPrefs;
    }

    public void PauseUnpause()
    {
        bool wasPaused = Time.timeScale > 0f;
        if (wasPaused)
        {
            Time.timeScale = 0f;

            for (int i = 0; i < _toDisableOnPause.Length; i++)
                _toDisableOnPause[i].enabled = !wasPaused;
        }
        else
        {
            Time.timeScale = 1f;

            for (int i = 0; i < _toDisableOnPause.Length; i++)
                _toDisableOnPause[i].enabled = !wasPaused;
        }

        _uiManager.SetPauseScreen(wasPaused);
    }

    // Button 
    public void LoadSplashScreen()
    {
        SavePlayerPrefs();
        SceneManager.LoadScene(0);
        AudioManager.Instance.MusicSource.Stop();
        Time.timeScale = 1f;
    }

    // Button 
    public void ButtonToggleTips()
    {
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            _uiManager.SetTipsActive();
        }
        else
        {
            Time.timeScale = 1f;
            _uiManager.DeactiveTips();
        }
    }

    private void WitchDamaged()
    {
        _shake.ShakeStrong();
        StartCoroutine(Aberration(_chromaticAberration));
        if (_witch.ThornBaseDamage > 0.01f)
        {
            AudioManager.Instance.PlayOneShot(ac_thornDamage);
            _spawnManager.DamageEveryEnemyInRange(_witch.ThornTotalDamage(),
                                                  _witch.Knockback * .25f,
                                                  _witch.transform.position,
                                                  _witch.ThornRange,
                                                  _spawnManager.SpawnThornAnimation);
        }
    }

    private void WitchDodged()
    {
        _spawnManager.SpawnDamageNum("Dodged!", _witch.ShineColour, _witch.transform.position);
    }

    private void WitchDied()
    {
        InputManager.Inputs.Player.Pause_UnPause.performed -= PauseUnpause;
        _uiManager.GameUi.SetActive(false);
    }

    private void WitchDeathAnimationFinished(FlipBook flipbook)
    {
        _endScreenManager.SetTexts(time: _uiManager.TimeText,
                                   enemies: _spawnManager.EnemiesDied,
                                   cardsReciclyed: _cardManager.Recycler.CardsRecycled,
                                   candy: _witch.TotalCurrencyGained);

        SavePlayerPrefs();

        _endScreenManager.gameObject.SetActive(true);
        _witchDied = true;
        _chromaticAberration.intensity.value = .125f;
    }

    private void SavePlayerPrefs()
    {
        if (_saved)
            return;

        _saved = true;
        PlayerPrefsHelper.AddRun();
        PlayerPrefsHelper.AddPlayTime(_playTime);
        PlayerPrefsHelper.AddEnemiesKilled(_spawnManager.EnemiesDied);
        PlayerPrefsHelper.AddCardsPlayed(_endScreenManager.CardsPlayed);
        PlayerPrefsHelper.AddCardsRecycled(_cardManager.Recycler.CardsRecycled);
        PlayerPrefsHelper.AddCandyEarned(_witch.TotalCurrencyGained);
        PlayerPrefsHelper.Save();
    }

    private void WitchLightning(IDamageable firstDamageable)
    {
        _spawnManager.LightningDamage(damage: _witch.LightningTotalDamage(),
                                      range: _witch.LightningRange,
                                      firstPoint: _witch.transform.localPosition,
                                      firstDamageable: firstDamageable,
                                      lightningChance: _witch.LightningChance,
                                      minChains: _witch.LightningMinChain);
    }

    private void EnemyDamagedInRange(float damage)
    {
        _witch.TryLifeSteal(Random.value, damage, false);
    }

    private void PauseUnpause(InputAction.CallbackContext ctx) => PauseUnpause();

    private void MuteUnMute(InputAction.CallbackContext ctx)
    {
        AudioListener.volume = AudioListener.volume > 0.1f ? 0f : 1f;
    }

    private void SlowDown()
    {
        Time.timeScale = _slowDownScale;
        StopCoroutine(_slowPitch);
        _slowPitch = SetPitch(0.75f);
        StartCoroutine(_slowPitch);
    }

    private void UnSlowDown()
    {
        Time.timeScale = 1f;
        StopCoroutine(_slowPitch);
        _slowPitch = SetPitch(1f);
        StartCoroutine(_slowPitch);
    }

    private void CardRecycled()
    {
        if (_onRecycleDamageEnemies > 0.01f)
            _spawnManager.DamageEveryEnemy(_onRecycleDamageEnemies);

        _witch.Health.Heal(_onRecycleHeal);
        _witch.ModifyCurrency((int)_onRecycleAddCurrency);

        _cardManager.CardRefundDrawer(_onRecycleRefund);
    }

    private void CardPlayed(PowerUp powerUp)
    {
        _endScreenManager.AddCard(powerUp);

        if (_onCardPlayedDamageEnemies > 0.01f)
            _spawnManager.DamageEveryEnemy(_onCardPlayedDamageEnemies);

        _witch.Health.Heal(_onCardPlayedHeal);

        _cardManager.CardRefundDrawer(_onCardPlayedRefund);
    }

    private IEnumerator SetPitch(float pitch)
    {
        float eTime = 0f;
        float currentPitch = AudioManager.Instance.MusicSource.pitch;
        while (eTime < 0.25f)
        {
            float t = eTime / 0.5f;
            AudioManager.Instance.MusicSource.pitch = Mathf.Lerp(currentPitch, pitch, t);
            AudioManager.Instance.SFXSource.pitch = Mathf.Lerp(currentPitch, pitch, t);
            eTime += Time.unscaledDeltaTime;
            yield return null;
        }
        AudioManager.Instance.MusicSource.pitch = pitch;
        AudioManager.Instance.SFXSource.pitch = pitch;
    }

    private IEnumerator Aberration(ChromaticAberration aberration)
    {
        Time.timeScale = 0f;
        yield return _hitStop;
        Time.timeScale =  1f;
        var eTime = 0f;
        while (eTime < _aberrationDuration)
        {
            eTime += Time.unscaledDeltaTime;
            aberration.intensity.value = Mathf.Lerp(_aberrationIntensity, 0f, eTime/ _aberrationDuration);
            yield return null;
        }
        aberration.intensity.value = 0f;
    }
}
