using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDeactivatable
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Color Colour { get; private set; }

    [Header("Ranges")]
    [SerializeField] protected Vector2 _speedRange;
    [SerializeField] protected Vector2 _healthRange;
    [SerializeField] protected Vector2 _damageRange;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Health _health;
    [SerializeField] private HitBox _hitBox;

    public Health Health => _health;
    public HitBox HitBox => _hitBox;

    public System.Action<Enemy> ReturnToPool { get; set; }
    public System.Action<Enemy> OnDied { get; set; }

    protected void OnEnable()
    {
        _health.OnDeath += Died;
    }

    protected void OnDisable()
    {
        _health.OnDeath -= Died;
    }

    public virtual void Init(Witch witch) { }

    public virtual void Spawn(float t, float tClamped) 
    {
        _sr.sortingOrder = Random.Range(100, 500);
        _health.ResetHealth(_healthRange.Evaluate(t));
        _hitBox.SetDamage(_damageRange.Evaluate(t));
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        ReturnToPool?.Invoke(this);
    }

    protected void Died()
    {
        gameObject.SetActive(false);
        ReturnToPool?.Invoke(this);
        OnDied?.Invoke(this);
    }
}
