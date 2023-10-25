using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

    [Header("Settings")]
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

    [Header("Managers")]
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;

    public static GameManager Instance { get; private set; }
    public static InputMapping Inputs { get; private set; }

    public Witch Witch => _witch;


    private void Awake()
    {
        Instance = this;
        Inputs = new();
        Inputs.Enable();
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
}
