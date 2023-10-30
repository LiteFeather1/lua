using System.Collections;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

    [Header("Settings")]
    [SerializeField] private float _slowDownScale = .25f;
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

    [Header("Managers")]
    [SerializeField] private Camera _camera;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private CardManager _cardManager;

    [Header("Recycle Effects")]
    [SerializeField] private CompositeValue _damageEnemiesOnRecycle;
    [SerializeField] private CompositeValue _healOnRecycle;
    [SerializeField] private CompositeValue _addCurrencyOnRecycle;
    private IEnumerator _slowPitch;

    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

    public Witch Witch => _witch;
    public Camera Camera => _camera;
    public AudioManager AudioManager => _audioManager;
    public CardManager CardManager => _cardManager;

    public CompositeValue DamageEnemiesOnRecycle => _damageEnemiesOnRecycle;
    public CompositeValue HealOnRecycle => _healOnRecycle;
    public CompositeValue AddCurrencyOnRecycle => _addCurrencyOnRecycle;

    private void Awake()
    {
        Instance = this;
        Inputs = new();
        Inputs.Enable();

        Inputs.Player.Pause_UnPause.performed += PauseUnpause;
        Inputs.Player.Mute_UnMute.performed += MuteUnMute;

        _cardManager.OnCardHovered += SlowDown;
        _cardManager.OnCardUnHovered += UnSlowDown;

        _cardManager.Recycler.OnCardUsed += CardRecycled;

        _uiManager.BindToWitch(_witch);

        _slowPitch = SetPitch(1f);
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

        _cardManager.OnCardHovered -= SlowDown;
        _cardManager.OnCardUnHovered -= UnSlowDown;

        _cardManager.Recycler.OnCardUsed += CardRecycled;

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
        Time.timeScale = 1f;
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

    private IEnumerator SetPitch(float pitch)
    {
        float eTime = 0f;
        float currentPitch = _audioManager.MusicSource.pitch;
        while (eTime < 0.25f)
        {
            float t = eTime / 0.5f;
            _audioManager.MusicSource.pitch = Mathf.Lerp(currentPitch, pitch, t);
            _audioManager.SFXSource.pitch = Mathf.Lerp(currentPitch, pitch, t);
            eTime += Time.unscaledDeltaTime;
            yield return null;
        }
        _audioManager.MusicSource.pitch = pitch;
        _audioManager.SFXSource.pitch = pitch;
    }

    private void CardRecycled()
    {
        if (_damageEnemiesOnRecycle.Value > 0f)
        {
            DamageEveryEnemy();
        }

        _witch.Health.Heal(_healOnRecycle.Value);
        _witch.ModifyCurrency((int)_addCurrencyOnRecycle.Value);
    }

    private void DamageEveryEnemy()
    {
        var enemies = _spawnManager.ActiveEnemies.ToArray();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Health.TakeDamage(_damageEnemiesOnRecycle.Value, false, enemies[i].transform.position);
        }
    }
}
