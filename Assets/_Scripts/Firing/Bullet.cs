using System;
using UnityEngine;
using LTFUtils.ObjectPool;
using Lua.Damage;
using Lua.Parentables;

namespace Lua.Weapons
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
        [SerializeField] private Projectile _projectile;
        [SerializeField] private HitBox _hitBox;
        private Parentable _disableCallBack;

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

        public void AttachDisable(Parentable disable)
        {
            disable.Parent(transform);
            _disableCallBack = disable;
        }

        private void ProjectileDeactivated()
        {
            _disableCallBack.UnParent();
            ReturnToPool?.Invoke(this);
        }
    }
}
