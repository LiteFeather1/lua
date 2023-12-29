using RetroAnimation;
using UnityEngine;
using Lua.Damage;
using Lua.Damage.DamageEffects;
using Lua.Parentables;

namespace Lua.StateMachine.Enemies
{
    public abstract class Enemy : StateMachineCore.StateMachine, StateMachineCore.IDeactivatable
    {
        [SerializeField] protected EnemyData _data;

        [Header("Base States")]
        [SerializeField] private MovementKnockback _knockbackState;

        [Header("Sprite")]
        [SerializeField] private LTF.ValueGeneric.ValueSprite _knockbackSprite;

        [Header("Components")]
        [SerializeField] private FlipBook _flipBook;
        [SerializeField] private Health _health;
        [SerializeField] private HitBox _hitBox;

        private Parentable _fireParticle;

        public EnemyData Data => _data;

        public Health Health => _health;
        public HitBox HitBox => _hitBox;

        public System.Action<Enemy> ReturnToPool { get; set; }
        public System.Action<Enemy> OnDied { get; set; }

        public System.Func<Parentable> OnFireEffectApplied { get; set; }

        protected void OnEnable()
        {
            _health.OnDamaged += Damaged;
            _health.OnDeath += Died;

            _health.OnDamageEffectApplied += EffectApplied;
            _health.OnDamageEffectRemoved += EffectRemoved;

            _knockbackState.OnStateComplete += KnockBackComplete;
        }

        protected void OnDisable()
        {
            _health.OnDamaged -= Damaged;
            _health.OnDeath -= Died;

            _health.OnDamageEffectApplied -= EffectApplied;
            _health.OnDamageEffectRemoved -= EffectRemoved;

            _knockbackState.OnStateComplete -= KnockBackComplete;
        }

        public virtual void Init(Transform transform) { }

        public virtual void Spawn(float t, float tClamped) 
        {
            _flipBook.SR.sortingOrder = Random.Range(100, 500);
            _health.SetNewStats(_data.Health(t), _data.Defence(t));
            _hitBox.SetDamage(_data.Damage(t));
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            ReturnToPool?.Invoke(this);
        }

        protected virtual void KnockBackComplete()
        {
            _flipBook.Play(true);
            transform.localScale = new(1f, 1f, 1f);
        }

        private void Damaged(float damage, float knockback, bool crit, Vector2 pos)
        {
            if (knockback == 0f)
                return;

            _flipBook.Stop();
            _flipBook.SR.sprite = _knockbackSprite.Value;

            var knockbackDirection = (Position - pos).normalized;
            transform.localScale = new(-Mathf.Sign(knockbackDirection.x), 1f, 1f);
            _knockbackState.SetUp(knockbackDirection, crit ? knockback * 1.5f : knockback);
            Set(_knockbackState);
        }

        private void Died()
        {
            _fireParticle?.UnParent();

            gameObject.SetActive(false);
            ReturnToPool?.Invoke(this);
            OnDied?.Invoke(this);
        }

        private void EffectApplied(int effectID)
        {
            if (effectID == (int)IDamageEffect.DamageEffectID.FIRE_ID)
            {
                _fireParticle = OnFireEffectApplied?.Invoke();
                _fireParticle.Parent(_health.transform);
            }
        }

        private void EffectRemoved(int effectID)
        {
            if (effectID == (int)IDamageEffect.DamageEffectID.FIRE_ID)
            {
                _fireParticle.UnParent();
            }
        }
    }
}
