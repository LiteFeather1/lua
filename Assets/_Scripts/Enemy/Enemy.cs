using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Vector2 _speedRange;
    [SerializeField] private Health _health;
    [SerializeField] private HitBox _hitBox;

    public Health Health => _health;
    public HitBox HitBox => _hitBox;

    public virtual void Init(Witch witch) { }
    public virtual void Spawn(float t) { }
}
