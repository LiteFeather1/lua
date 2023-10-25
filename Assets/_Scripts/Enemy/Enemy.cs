using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [field: SerializeField] public string Name { get; set; }

    [Header("Ranges")]
    [SerializeField] protected Vector2 _speedRange;
    [SerializeField] protected Vector2 _healthRange;
    [SerializeField] protected Vector2 _damageRange;

    [Header("Components")]
    [SerializeField] private Health _health;
    [SerializeField] private HitBox _hitBox;

    public Health Health => _health;
    public HitBox HitBox => _hitBox;

    public System.Action<Enemy> ReturnToPool { get; set; }

    protected void OnEnable()
    {
        _health.OnDeath += Died;
    }

    protected void OnDisable()
    {
        _health.OnDeath -= Died;
    }

    public virtual void Init(Witch witch) { }

    public virtual void Spawn(float t) 
    {
        _health.ResetHealth(_healthRange.Evaluate(t));
        _hitBox.SetDamage(_damageRange.Evaluate(t));
        gameObject.SetActive(true);
    }

    protected void Died()
    {
        gameObject.SetActive(false);
    }
}