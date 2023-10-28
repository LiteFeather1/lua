using UnityEngine;

public class CurrencyBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _time = .5f;
    private float _elapsedTime;
    private Witch _witch;

    public System.Action<CurrencyBehaviour> ReturnToPool { get; set; }

    private void Update()
    {
        float delta = Time.deltaTime;
        var t = _elapsedTime / _time;
        if (t > 1f)
            t = 1f;
        float time = _speed * delta * _curve.Evaluate(t);
        transform.position = Vector2.MoveTowards(transform.position, _witch.transform.position, time);
        _elapsedTime += delta;

        if (Vector2.Distance(transform.position, _witch.transform.position) < 0.01f)
        {
            gameObject.SetActive(false);
            _elapsedTime = 0f;
            _witch.ModifyCurrency(1);
            ReturnToPool?.Invoke(this);
        }
    }

    public void Init(Witch witch)
    {
        _witch = witch;
    }
}
