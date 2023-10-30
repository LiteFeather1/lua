using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private HitBox _hitBox;
    private DisableCallBack _disableCallBack;

    public Projectile Projectile => _projectile;
    public HitBox Hitbox => _hitBox;

    public Action<Bullet> ReturnToPool { get; set; }

    private void Awake()
    {
        _projectile.Deactivated += ProjectileDeactivated;
    }

    private void OnDestroy()
    {
        _projectile.Deactivated -= ProjectileDeactivated;
    }

    public void AttachDisable(DisableCallBack disable)
    {
        _disableCallBack = disable;
        disable.transform.SetParent(transform);
        disable.transform.localPosition = Vector3.zero;
    }

    private void ProjectileDeactivated()
    {
        _disableCallBack.transform.SetParent(null);
        ReturnToPool?.Invoke(this);
    }
}
