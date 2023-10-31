using UnityEngine;

public class CurrencyBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private float _timeToMaxSpeed = .5f;
    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private float _yOffset = 1f;
    [SerializeField] private AnimationCurve _yOffsetCurve;
    private int _yDirection;
    private float _elapsedTime;
    private Witch _witch;

    public System.Action<CurrencyBehaviour> ReturnToPool { get; set; }

    private void Update()
    {
        float delta = Time.deltaTime;
        var t = _elapsedTime / _timeToMaxSpeed;
        if (t > 1f)
            t = 1f;
        float time = _speed * delta * _speedCurve.Evaluate(t);
        Vector2 to = _witch.transform.position;
        to.y += _yOffset * _yOffsetCurve.Evaluate(t) * _yDirection;
        transform.position = Vector2.MoveTowards(transform.position, to, time);
        _elapsedTime += delta;

        if (Vector2.Distance(transform.position, _witch.transform.position) < 0.01f)
        {
            gameObject.SetActive(false);
            _yDirection = Random.value > 0.5f ? 1 : -1;
            _elapsedTime = 0f;
            _witch.ModifyCurrency(1);
            ReturnToPool?.Invoke(this);
            AudioManager.Instance.PlayCandyPickUp();
        }
    }

    public void Init(Witch witch)
    {
        _yDirection = Random.value > 0.5f ? 1 : -1;
        _witch = witch;
    }
}
