using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private HitBox _hitBox;
    private ParticleStoppedCallBack _disableCallBack;

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

    public void AttachDisable(ParticleStoppedCallBack disable)
    {
        disable.transform.SetParent(transform, false);
        disable.transform.localScale = transform.localScale;
        disable.transform.localPosition = Vector3.zero;
        _disableCallBack = disable;
    }

    private void ProjectileDeactivated()
    {
        _disableCallBack.transform.SetParent(null, true);
        ReturnToPool?.Invoke(this);
    }
}
