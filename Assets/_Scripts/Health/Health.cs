using System.Collections.Generic;
using UnityEngine;
using LTF.CompositeValue;
using Lua.Damage.DamageEffects;

namespace Lua.Damage
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float _maxHealth;
        [SerializeField] private CompositeValue _defence = new(0f);
        protected float _health;
        private readonly HashSet<int> _uniqueDamageEffects = new();
        private readonly List<IDamageEffect> _damageEffects = new();


        public delegate void OnDamage(float damage, float knockback, bool crit, Vector2 pos);
        public OnDamage OnDamaged { get; set; }
        public System.Action OnHeal { get; set; }
        public System.Action OnDeath { get; set; }

        public System.Action<int> OnDamageEffectApplied { get; set; }
        public System.Action<int> OnDamageEffectRemoved { get; set; }

        public float MaxHP => _maxHealth;
        public float Hp => _health;
        public CompositeValue Defence => _defence;
        public Vector2 Pos => transform.position;

        public void Start() => _health = _maxHealth;

        private void Update()
        {
            var delta = Time.deltaTime;
            for (int i = _damageEffects.Count - 1; i >= 0; i--)
            {
                var damageEffect = _damageEffects[i];
                if (!damageEffect.Tick(this, delta))
                    continue;

                _damageEffects.RemoveAt(i);
                _uniqueDamageEffects.Remove(damageEffect.ID);
                OnDamageEffectRemoved?.Invoke(damageEffect.ID);
            }
        }

        public virtual bool TakeDamage(float damage, float knockback, bool crit, Vector2 pos)
        {
            // Is alive check
            if (_health < 0f)
                return true;

            damage *= 100f / (100f + _defence.Value);
            _health -= damage;

            OnDamaged?.Invoke(damage, knockback, crit, pos);
            if (_health <= 0f)
            {
                OnDeath?.Invoke();
                _uniqueDamageEffects.Clear();
                _damageEffects.Clear();
                return true;
            }

            return false;
        }

        public void ResetHealth() => _health = _maxHealth;

        public void ResetHealth(float newMax)
        {
            _maxHealth = newMax;
            _health = newMax;
        }

        public void Resets(float newMax, float newDefence)
        {
            _maxHealth = newMax;
            _health = newMax;
            _defence.Clear(newDefence);
        }

        public void SetNewStats(float newMax, float newDefence)
        {
            _maxHealth = newMax;
            _health = newMax;
            _defence.SetNewBase(newDefence);
        }

        public void Heal(float heal)
        {
            _health += heal;
            if (_health > _maxHealth)
                _health = _maxHealth;

            OnHeal?.Invoke();
        }

        public int Heal(int heal)
        {
            Heal((float)heal);
            return (int)_health;
        }

        public bool CanAddDamageEffect(int ID)
        {
            if (_health <= 0f || _uniqueDamageEffects.Contains(ID))
                return false;

            return true;
        }

        public void AddDamageEffect(IDamageEffect damageEffect)
        {
            _uniqueDamageEffects.Add(damageEffect.ID);
            _damageEffects.Add(damageEffect);
            OnDamageEffectApplied?.Invoke(damageEffect.ID);
        }
    }
}
