using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

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

    [Header("Volume")]
    [SerializeField] private Volume _volume;
    [SerializeField] private float _aberrationIntensity = 1f;
    [SerializeField] private float _aberrationDuration = .5f;

    [Header("Recycle Effects")]
    [SerializeField] private CompositeValue _onRecycleDamageEnemies;
    [SerializeField] private CompositeValue _onRecycleHeal;
    [SerializeField] private CompositeValue _onRecycleAddCurrency;
    [SerializeField] private CompositeValue _onRecycleRefund;

    [Header("On Card Played Effect")]
    [SerializeField] private CompositeValue _onCardPlayedDamageEnemies;
    [SerializeField] private CompositeValue _onCardPlayedHeal;
    [SerializeField] private CompositeValue _onCardPlayedRefund;

    private IEnumerator _slowPitch;

    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

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
        Inputs = new();
        Inputs.Enable();

        Inputs.Player.Pause_UnPause.performed += PauseUnpause;
        Inputs.Player.Mute_UnMute.performed += MuteUnMute;

        _witch.OnDamaged += WitchDamaged;
        _witch.Health.OnDeath += WitchDied;
        _witch.OnLightningEffectApplied += WitchLightning;

        _cardManager.OnCardHovered += SlowDown;
        _cardManager.OnCardUnHovered += UnSlowDown;
        _cardManager.Recycler.OnCardUsed += CardRecycled;
        _cardManager.PlayArea.OnPowerPlayed += CardPlayed;

        _spawnManager.EnemyHurt += _shake.Shake;
        _spawnManager.EnemyDamagedInRange += EnemyDamagedInRange;

        _uiManager.BindToWitch(_witch);

        _slowPitch = SetPitch(1f);
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

    private void OnDestroy()
    {
        Inputs.Player.Pause_UnPause.performed -= PauseUnpause;
        Inputs.Player.Mute_UnMute.performed -= MuteUnMute;

        _witch.OnDamaged -= WitchDamaged;
        _witch.Health.OnDeath -= WitchDied;
        _witch.OnLightningEffectApplied -= WitchLightning;

        _cardManager.OnCardHovered -= SlowDown;
        _cardManager.OnCardUnHovered -= UnSlowDown;
        _cardManager.Recycler.OnCardUsed -= CardRecycled;
        _cardManager.PlayArea.OnPowerPlayed -= CardPlayed;

        _spawnManager.EnemyHurt -= _shake.Shake;
        _spawnManager.EnemyDamagedInRange -= EnemyDamagedInRange;

        _uiManager.UnBindToWitch(_witch);
    }

    public void PauseUnpause()
    {
        bool paused = Time.timeScale > 0f;
        Time.timeScale = paused ? 0f : 1f;
        _uiManager.SetPauseScreen(paused);
    }

    public void LoadSplashScreen()
    {
        SceneManager.LoadScene(0);
        AudioManager.Instance.MusicSource.Stop();
        Time.timeScale = 1f;
    }

    private void WitchDamaged()
    {
        _shake.ShakeStrong();
        _volume.profile.TryGet(out ChromaticAberration aberration);
        StartCoroutine(Aberration(aberration));
        if (_witch.ThornBaseDamage > 0.01f)
            _spawnManager.DamageEveryEnemyInRange(_witch.ThornTotalDamage(),
                                                  _witch.Knockback * .25f,
                                                  _witch.transform.position,
                                                  _witch.ThornRange,
                                                  _spawnManager.SpawnThornAnimation);
    }

    private void WitchDied()
    {
        Inputs.Player.Pause_UnPause.performed -= PauseUnpause;
        _endScreenManager.gameObject.SetActive(true);
        _uiManager.GameUi.SetActive(false);
        _endScreenManager.SetTexts(_uiManager.TimeText,
                                   _spawnManager.EnemiesDied,
                                   _cardManager.Recycler.CardsRecycled,
                                   _witch.TotalCurrencyGained);
    }

    private void WitchLightning(IDamageable firstDamageable)
    {
        _spawnManager.LightningDamage(_witch.LightningTotalDamage(),
                                      _witch.LightningRange,
                                      _witch.transform.localPosition,
                                      firstDamageable,
                                      _witch.LightningChance,
                                      _witch.LightningMinChain);
    }

    private void EnemyDamagedInRange(float damage)
    {
        _witch.TryLifeSteal(Random.value, damage);
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
