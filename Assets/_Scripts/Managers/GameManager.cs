using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

    [Header("Settings")]
    [SerializeField] private float _slowDownScale = .25f;
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

    [Header("Managers")]
    [SerializeField] private Camera _camera;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private CardManager _cardManager;

    [Header("Recycle Effects")]
    [SerializeField] private CompositeValue _damageEnemiesOnRecycle;
    [SerializeField] private CompositeValue _healOnRecycle;
    [SerializeField] private CompositeValue _addCurrencyOnRecycle;

    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

    public Witch Witch => _witch;
    public Camera Camera => _camera;
    public CardManager CardManager => _cardManager;

    public CompositeValue DamageEnemiesOnRecycle => _damageEnemiesOnRecycle;
    public CompositeValue HealOnRecycle => _healOnRecycle;
    public CompositeValue AddCurrencyOnRecycle => _addCurrencyOnRecycle;

    private void Awake()
    {
        Instance = this;
        Inputs = new();
        Inputs.Enable();

        _cardManager.OnCardHovered += SlowDown;
        _cardManager.OnCardUnHovered += UnSlowDown;

        _cardManager.Recycler.OnCardUsed += CardRecycled;

        _uiManager.BindToWitch(_witch);
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
        _cardManager.OnCardHovered -= SlowDown;
        _cardManager.OnCardUnHovered -= UnSlowDown;

        _cardManager.Recycler.OnCardUsed += CardRecycled;

        _uiManager.UnBindToWitch(_witch);
    }

    public float T()
    {
        return _playTime / _timeForMaxDifficult;
    }

    public float TClamped()
    {
        float t = _playTime / _timeForMaxDifficult;
        if (t > 1f)
            t = 1f;
        return t;
    }

    private void SlowDown()
    {
        Time.timeScale = _slowDownScale;
    }

    private void UnSlowDown()
    {
        Time.timeScale = 1f;
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
            enemies[i].Health.TakeDamage(_damageEnemiesOnRecycle.Value);
        }
    }
}
