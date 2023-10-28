using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

    [Header("Settings")]
    [SerializeField] private float _slowDownScale = .25f;
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private CardManager _cardManager;

    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

    public Witch Witch => _witch;
    public CardManager CardManager => _cardManager;

    private void Awake()
    {
        Instance = this;
        Inputs = new();
        Inputs.Enable();
    }

    private void OnEnable()
    {
        _cardManager.OnCardHovered += SlowDown;
        _cardManager.OnCardUnHovered += UnSlowDown;
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

    private void OnDisable()
    {
        _cardManager.OnCardHovered -= SlowDown;
        _cardManager.OnCardUnHovered -= UnSlowDown;
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
}
