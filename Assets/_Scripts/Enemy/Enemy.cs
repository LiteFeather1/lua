using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDeactivatable
{
    [SerializeField] protected EnemyData _data;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Health _health;
    [SerializeField] private HitBox _hitBox;

    public EnemyData Data => _data;

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
        _health.ResetHealth(_data.HealthRange.Evaluate(t));
        _hitBox.SetDamage(_data.DamageRange.Evaluate(t));
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
