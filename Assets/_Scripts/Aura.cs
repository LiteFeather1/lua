using UnityEngine;
using CompositeValues;
using Lua.Damage;
using Timers;

namespace Lua.Weapons
{
    public class Aura : MonoBehaviour
    {
        [SerializeField] private CompositeValue _damagePercent = new(.1f);

        [Header("Sizes")]
        [SerializeField] private ValueSpriteArray _auraSprites;
        private int _currentAuraIndex = 0;

        [Header("Components")]
        [SerializeField] private FixedTimer _tickRateTimer;
        [SerializeField] private HitBox _hitbox;
        [SerializeField] private SpriteRenderer sr_Aura;
        [SerializeField] private CircleCollider2D _c;

        public CompositeValue DamagePercent => _damagePercent;

        private void Awake()
        {
            _tickRateTimer.TimeEvent += Tick;
        }

        private void OnDestroy()
        {
            _tickRateTimer.TimeEvent -= Tick;
        }

        private void Tick()
        {
            _c.enabled = false;
            _c.enabled = true;
        }

        public void SetAura(Sprite sprite)
        {
            sr_Aura.sprite = sprite;
            _c.radius = sprite.textureRect.width * 0.005f;
        }

        public bool IncreaseAura(int by)
        {
            _currentAuraIndex += by;
            if (_currentAuraIndex >= _auraSprites.Length)
                _currentAuraIndex = _auraSprites.Length - 1;

            SetAura(_auraSprites[_currentAuraIndex]);

            return _currentAuraIndex == _auraSprites.Length;
        }

        public void SetDamage(float value) => _hitbox.SetDamage(Mathf.Max(value * _damagePercent, 1f));
        public void SetCrit(float value) => _hitbox.SetCritChance(value * .5f);
        public void SetCritMultiplier(float value) => _hitbox.SetCritMultiplier(Mathf.Max(value * .5f , 1.25f));
    }
}
