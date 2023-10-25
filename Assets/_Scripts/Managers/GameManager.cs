using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Witch _witch;

    [Header("Settings")]
    [SerializeField] private float _playTime;
    [SerializeField] private float _timeForMaxDifficult = 360f;

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
    }

    public float T()
    {
        float t = _playTime / _timeForMaxDifficult;
        if (t > 1f)
            t = 1f;
        return t;
    }

}
