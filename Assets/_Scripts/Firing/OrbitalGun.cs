﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTF.ObjectPool;
using LTF.CustomWaits;
using LTF.CompositeValue;
using RetroAnimation;

namespace Lua.Weapons
{
    public class OrbitalGun : Gun
    {
        [Header("Orbital Gun")]
        [SerializeField] private Transform _clockWiseRotation;
        [SerializeField] private Transform _antiClockWiseRotation;
        [SerializeField] private CompositeValue _rotationSpeed;
        [SerializeField] private float _maxBulletSpeed = .9f;
        [SerializeField] private float _maxBulletDuration = 6f;
        [SerializeField] private int _orbitalAmount = 0;
        [SerializeField] private CustomWaitForSeconds _yieldBetweenBursts = new(.75f);
        [SerializeField] private ObjectPool<FlipBook> _disappearPool;

        private readonly List<Bullet> _activeBullets = new();

        public CompositeValue RotationSpeed => _rotationSpeed;
        public int AddOrbitalAmount(int amount) => _orbitalAmount += amount;

        protected override void Awake()
        {
            base.Awake();
            _disappearPool.ObjectCreated += DisappearCreated;
            _disappearPool.InitPool();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            var rot = _rotationSpeed * delta;
            _antiClockWiseRotation.transform.Rotate(0f, 0f, -rot, Space.Self);
            _clockWiseRotation.transform.Rotate(0f, 0f, rot, Space.Self);
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                var activeBullet = _activeBullets[i];
                var activeProjectile = activeBullet.Projectile;
                var speedDelta = activeProjectile.Speed * delta;
                activeBullet.transform.localPosition += new Vector3(activeProjectile.Direction.x * speedDelta,
                                                                    activeProjectile.Direction.y * speedDelta);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _disappearPool.ObjectCreated -= DisappearCreated;
            foreach (var disappear in _disappearPool.Objects)
                disappear.OnAnimationFinished -= ReturnDisappearToPool;
        }

        public IEnumerator ShootRoutine(float damage, float critChance, float critMultiplier, float knockback, float speed, float duration)
        {
            return ShootRoutine(damage: damage * .5f,
                                critChance: critChance,
                                critMultiplier: critMultiplier,
                                knockback: knockback * .33f,
                                size: 1f,
                                speed: Mathf.Clamp(speed * .5f, .4f, _maxBulletSpeed),
                                pierce: 1,
                                bounce: 0,
                                duration: Mathf.Clamp(duration * 2f, 1.5f, _maxBulletDuration),
                                angle: 0f,
                                burstAmount: _orbitalAmount,
                                bulletAmount: 1,
                                yieldBetweenBurst: _yieldBetweenBursts);
        }

        protected override void ShootProjectileMethod(Bullet bullet, float speed, Vector2 direction)
        {
            bullet.Projectile.SetSpeedAndDirection(speed, Random.insideUnitCircle.normalized * .5f);
            bullet.transform.SetParent(Random.value > 0.5f ? _antiClockWiseRotation : _clockWiseRotation);
            _activeBullets.Add(bullet);
        }

        protected override void ReturnBulletToPool(Bullet bullet)
        {
            _activeBullets.Remove(bullet);

            var disappear = _disappearPool.GetObject();
            disappear.transform.SetParent(bullet.transform.parent);
            disappear.transform.localPosition = bullet.transform.localPosition;
            disappear.Play();
            disappear.gameObject.SetActive(true);

            base.ReturnBulletToPool(bullet);
        }

        private void DisappearCreated(FlipBook disappear)
        {
            disappear.OnAnimationFinished += ReturnDisappearToPool;
        }

        private void ReturnDisappearToPool(FlipBook disappear)
        {
            disappear.gameObject.SetActive(false);
            _disappearPool.ReturnObject(disappear);
        }
    }
}
