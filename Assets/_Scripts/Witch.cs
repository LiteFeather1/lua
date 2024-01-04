using RetroAnimation;
using System;
using System.Collections;
using UnityEngine;
using LTF;
using LTF.RefValue;
using LTF.Utils;
using LTF.CustomWaits;
using LTF.CompositeValue;
using Lua.Managers;
using Lua.Damage;
using Lua.Damage.DamageEffects;
using Lua.Weapons;
using Random = UnityEngine.Random;

namespace Lua
{
    public class Witch : MonoBehaviour
    {
        [Header("Currency")]
        [SerializeField, ReadOnly] private int _currency;
        private int _totalCurrencyGained;

        [Header("Health")]
        [SerializeField] private HealthPlayer _health;
        [SerializeField] private AudioClip _hurtSound;
        [SerializeField] private FlipSheet _deathAnimation; 

        [Header("Moviment")]
        [SerializeField] private float _maxSpeed = 20f;
        [SerializeField] private CompositeValue _acceleration = new(2f);
        private float _initialAcceleration;
        [SerializeField] private Vector2 _decelerationRange = new(.95f, .75f);
        [SerializeField] private float _accelerationMaxForRange = 50f;
        private Vector2 _inputDirection;

        [Header("Main Gun")]
        [SerializeField] private WitchGun _mainGun;
        [SerializeField] private CompositeValue _damage = new(5f); 
        [SerializeField] private CompositeValue _critChance = new(.01f);
        [SerializeField] private CompositeValue _critMultiplier = new(1.5f);
        [SerializeField] private CompositeValue _knockback = new(1f);
        [SerializeField] private CompositeValue _shootTime = new(1f);
        private float _elapsedShootTime = 0f;
        private readonly RefValue<float> r_shootDeltaMult = new(1f);

        [Header("Random Bullet")]
        [SerializeField] private Gun _randomGun;
        [SerializeField] private int _randomBulletAmount = 0;
        [SerializeField] private CompositeValue _randomBulletShootTime = new(2f);
        [SerializeField] private float _randomBulletTimeOffSetPercent = .5f;
        [SerializeField] private Vector2 _randomBulletStatMult = new(.5f, 1.5f);
        [SerializeField] private CustomRandomWaitForSeconds _yieldBetweenRandomBursts = new(new(.125f, .5f));
        private float _randomBulletTimeOffset = 0f;
        private float _elapsedRandomShootTime = 0f;
        private readonly RefValue<float> r_randomDeltaMult = new(1f);

        [Header("Shooting Moon Gun")]
        [SerializeField] private Gun _moonGun;
        [SerializeField] private CompositeValue _moonShootTime = new(3f);
        [SerializeField] private CustomWaitForSeconds _yieldBetweenMoonBursts = new(.33f);
        [SerializeField] private int _moonAmount;
        [SerializeField] private float _moonBulletSpeed = 2.5f;
        private float _moonElapsedTime = 0f;
        private readonly RefValue<float> r_moonDeltaMult = new(1f);

        [Header("Orbital Gun")]
        [SerializeField] private OrbitalGun _orbitalGun;
        [SerializeField] private CompositeValue _orbitalShootTime = new(2f);
        private float _elapsedOrbitalShootTime = 0f;
        private readonly RefValue<float> r_orbitalDeltaMult = new(1f);

        [Header("Dagger Gun")]
        [SerializeField] private Gun _daggerGun;
        [SerializeField] private CompositeValue _daggerShootTime = new(3f);
        [SerializeField] private int _daggerAmount;
        private float _elapsedDaggerShootTime;
        private readonly RefValue<float> r_daggerDeltaMult = new(1f);

        [Header("Life Steal")]
        [SerializeField] private CompositeValue _chanceToLifeSteal = new(.01f);
        [SerializeField] private CompositeValue _lifeStealPercent = new(.1f);

        [Header("Burn Effect")]
        [SerializeField] private EffectCreatorFire _effectCreatorFire;
        [SerializeField] private AudioClip ac_burnEffectApplied;

        [Header("Aura")]
        [SerializeField] private Aura _aura;

        [Header("Thorn")]
        [SerializeField] private CompositeValue _thornRange = new(.64f);
        [SerializeField] private CompositeValue _thorBaseDamage;
        [SerializeField] private CompositeValue _thornDefenceDamageMultiplier = new(.5f);

        [Header("Lightning")]
        [SerializeField] private AudioClip ac_lightningApplied;
        [SerializeField] private CompositeValue _lightningChance = new(0f);
        [SerializeField] private CompositeValue _lightningBaseDamage = new(5f);
        [SerializeField] private CompositeValue _lightningRange = new(.64f);
        [SerializeField] private int _lightningMinChain = 1;

        [Header("Blink on Damage")]
        [SerializeField] private float _invulnerabilityDuration = .5f;
        private WaitForSeconds _waitInvulnerability;
        [SerializeField] private int _blinkAmount;
        [SerializeField] private float _durationBetweenBlinks;
        [SerializeField] private Color _damagedColour = Color.red;

        [Header("Dodge")]
        [SerializeField] private AudioClip ac_dodged;
        [SerializeField] private ParticleSystem _dodgeParticle;
        [SerializeField] private float _shineTime = .5f;
        [SerializeField] private SpriteRenderer _shineSR;
        [SerializeField] private Vector2 _topPos = new(.125f, .125f);
        [SerializeField] private Vector2 _botPos = new(-.125f, -.125f);
        private IEnumerator _dodgeCo;

        [Header("Components")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private FlipBook _flipBook;
        [SerializeField] private SpriteMask _spriteMask;
        [SerializeField] private Collider2D _hurtBox;
        [SerializeField] private BoxCollider2D _boundsCollider;

        public Action<int> OnCurrencyModified { get; set; }

        public Action<float> OnHPModified { get; set; }
        public Action OnDamaged { get; set; }
        public Action OnInvulnerabilityEnded { get; set; }

        public Action OnMainShoot { get; set; }

        public Action<IDamageable> OnLightningEffectApplied { get; set; }

        public int Currency => _currency;
        public int TotalCurrencyGained => _totalCurrencyGained;
        public HealthPlayer Health => _health;

        public CompositeValue Acceleration => _acceleration;

        public WitchGun Gun => _mainGun;
        public CompositeValue Damage => _damage;
        public CompositeValue CritChance => _critChance;
        public CompositeValue CritMultiplier => _critMultiplier;
        public CompositeValue Knockback => _knockback;
        public CompositeValue ShootTime => _shootTime;  

        public CompositeValue RandomBulletShootTime => _randomBulletShootTime;
        public int AddRandomBullet(int amount) => _randomBulletAmount += amount;

        public int AddShootingMoonBullet(int amount) => _moonAmount += amount;

        public OrbitalGun OrbitalGun => _orbitalGun;
        public CompositeValue OrbitalShootTime => _orbitalShootTime;

        public CompositeValue DaggerShootTime => _daggerShootTime;
        public int DaggerAmount(int amount) => _daggerAmount += amount; 

        public CompositeValue ChanceToLifeSteal => _chanceToLifeSteal;
        public CompositeValue LifeStealPercent => _lifeStealPercent;

        public EffectCreatorFire EffectCreatorFire => _effectCreatorFire;

        public Aura Aura => _aura;

        public CompositeValue ThornRange => _thornRange;
        public CompositeValue ThornBaseDamage => _thorBaseDamage;
        public CompositeValue ThornDefenceDamageMultiplier => _thornDefenceDamageMultiplier;
        public float ThornTotalDamage() => _thorBaseDamage + (_health.Defence * _thornDefenceDamageMultiplier) + (_health.Shield * 5f);

        public CompositeValue LightningChance => _lightningChance;
        public CompositeValue LightningBaseDamage => _lightningBaseDamage;
        public CompositeValue LightningRange => _lightningRange;
        public int LightningMinChain => _lightningMinChain;
        public int ChangeLightningMinChain(int amount) => _lightningMinChain += amount;
        public float LightningTotalDamage() => _lightningBaseDamage + (_damage * 0.1f);

        public Color ShineColour => _shineSR.color;

        public FlipBook FlipBook => _flipBook;

        private void Awake()
        {
            _initialAcceleration = _acceleration;
            _waitInvulnerability = new(_invulnerabilityDuration);

            _rb.drag = _decelerationRange.x;

            _dodgeCo = DodgeShine();
        }

        private void OnEnable()
        {
            _mainGun.OnDamageAppplied += DamageAppliedGun;

            _randomGun.OnDamageAppplied += DamageAppliedGun;

            _moonGun.OnDamageAppplied += DamageAppliedGun;

            _orbitalGun.OnDamageAppplied += DamageAppliedGun;

            _daggerGun.OnDamageAppplied += DamageAppliedGun;

            _health.OnDamaged += Damaged;
            _health.OnDeath += OnDeath;
            _health.OnHeal += HPModified;
            _health.OnDodge += Dodged;

            _damage.OnValueModified += _aura.SetDamage;
            _critChance.OnValueModified += _aura.SetCrit;
            _critMultiplier.OnValueModified += _aura.SetCritMultiplier;
        }

        private void Start() => ModifyCurrency(4);

        private void Update()
        {
            var inputPlayer = InputManager.Inputs.Player;    
            if (inputPlayer.Moviment.IsPressed())   
                _inputDirection = inputPlayer.Moviment.ReadValue<Vector2>().normalized;
            else if (inputPlayer.Right_Click.IsPressed())
            {
                var direction = GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition) - transform.localPosition;
                if (Math.Abs(direction.x) < .01f)
                    direction.x = 0f;

                if (Math.Abs(direction.y) < .01f)
                    direction.y = 0f;

                _inputDirection = direction;
                _inputDirection.Normalize();
            }
            else
                _inputDirection = Vector2.zero;

            float delta = Time.deltaTime;

            // Main Shot
            _elapsedShootTime += delta * r_shootDeltaMult;
            if (_elapsedShootTime > _shootTime)
            {
                _elapsedShootTime = 0f;
                r_shootDeltaMult.Value = 0f;
                StartCoroutine(ShootRoutine(_mainGun.ShootRoutine(damage: _damage,
                                                                  critChance: _critChance,
                                                                  critMultiplier: _critMultiplier,
                                                                  knockback: _knockback),
                                            r_shootDeltaMult));
                OnMainShoot?.Invoke();
            }

            // Random Shot
            _elapsedRandomShootTime += delta * r_randomDeltaMult;
            if (_elapsedRandomShootTime > _randomBulletShootTime + _randomBulletTimeOffset)
            {
                _elapsedRandomShootTime = 0f;
                if (_randomBulletAmount > 0)
                {
                    r_randomDeltaMult.Value = 0f;
                    var offset = _randomBulletShootTime * _randomBulletTimeOffSetPercent;
                    _randomBulletTimeOffset = Random.Range(-offset, offset);
                    StartCoroutine(ShootRoutine(RandomBulletShootRoutine(), r_randomDeltaMult));
                }
            }

            // Moon Shot
            _moonElapsedTime += delta * r_moonDeltaMult;
            if (_moonElapsedTime > _moonShootTime)
            {
                _moonElapsedTime = 0f;
                if (_moonAmount > 0)
                {
                    r_moonDeltaMult.Value = 0f;
                    StartCoroutine(ShootRoutine(MoonGunShootRoutine(), r_moonDeltaMult));
                }
            }

            // Orbital Shot
            _elapsedOrbitalShootTime += delta * r_orbitalDeltaMult;
            if (_elapsedOrbitalShootTime > _orbitalShootTime)
            {
                _elapsedOrbitalShootTime = 0f;
                r_orbitalDeltaMult.Value = 0f;
                StartCoroutine(ShootRoutine(_orbitalGun.ShootRoutine(damage: _damage,
                                                                     critChance: _critChance,
                                                                     critMultiplier: _critMultiplier,
                                                                     knockback: _knockback,
                                                                     speed: _mainGun.BulletSpeed,
                                                                     duration: _mainGun.BulletDuration),
                                            r_orbitalDeltaMult));
            }

            // Dagger Shot
            _elapsedDaggerShootTime += delta * r_daggerDeltaMult;
            if (_elapsedDaggerShootTime > _daggerShootTime)
            {
                _elapsedDaggerShootTime = 0f;
                if (_daggerAmount > 0)
                {
                    r_daggerDeltaMult.Value = 0f;
                    StartCoroutine(ShootRoutine(_daggerGun.ShootRoutine(damage: _damage * .5f,
                                                                        critChance: _critChance * .5f,
                                                                        critMultiplier: _critMultiplier * .5f,
                                                                        knockback: 0.1f,
                                                                        size: 1f,
                                                                        speed: _mainGun.BulletSpeed * 2f,
                                                                        pierce: int.MaxValue,
                                                                        bounce: 0,
                                                                        duration: _mainGun.BulletDuration,
                                                                        angle: 0f,
                                                                        burstAmount: Mathf.Max((int)(_mainGun.BurstAmount * .5f), 1),
                                                                        bulletAmount: _daggerAmount,
                                                                        yieldBetweenBurst: _mainGun.YieldBetweenBurts),
                                                r_daggerDeltaMult));
                }
            }

            _spriteMask.sprite = _flipBook.SR.sprite;
        }

        private void FixedUpdate()
        {
            Move(_inputDirection);
        }

        private void OnDisable()
        {
            _mainGun.OnDamageAppplied -= DamageAppliedGun;

            _randomGun.OnDamageAppplied -= DamageAppliedGun;

            _moonGun.OnDamageAppplied -= DamageAppliedGun;

            _orbitalGun.OnDamageAppplied -= DamageAppliedGun;

            _daggerGun.OnDamageAppplied -= DamageAppliedGun;

            _health.OnDamaged -= Damaged;
            _health.OnDeath -= OnDeath;
            _health.OnHeal -= HPModified;
            _health.OnDodge -= Dodged;

            _damage.OnValueModified -= _aura.SetDamage;
            _critChance.OnValueModified -= _aura.SetCrit;
            _critMultiplier.OnValueModified -= _aura.SetCritMultiplier;
        }

        public void Move(Vector2 direction)
        {
            var velocity = _rb.velocity;
            velocity.x += direction.x * _acceleration;
            velocity.y += direction.y * _acceleration;

            if (Mathf.Sign(direction.x) == Mathf.Sign(_rb.velocity.x) && Mathf.Abs(_rb.velocity.x) > _maxSpeed)
                velocity.x = 0f;

            if (Mathf.Sign(direction.y) == Mathf.Sign(_rb.velocity.y) && Mathf.Abs(_rb.velocity.y) > _maxSpeed)
                velocity.y = 0f;

            _rb.AddForce(velocity, ForceMode2D.Force);
        }

        public void ModifyCurrency(int amount)
        {
            _currency += amount;
            OnCurrencyModified?.Invoke(_currency);
            if (amount > 0) 
                _totalCurrencyGained += amount;
        }

        public void EvaluateDrag()
        {
            float t = (_acceleration.Value - _initialAcceleration) / _accelerationMaxForRange;
            if (t > 1f)
                t = 1f;
            _rb.drag = _decelerationRange.Evaluate(t);
        }

        public void TryLifeSteal(float randomValue, float damage, bool crit)
        {
            if (crit)
                randomValue *= .5f;

            if (randomValue < _chanceToLifeSteal)
            {
                _health.Heal(damage * _lifeStealPercent);
            }
        }

        private void DamageAppliedGun(IDamageable damageable, float damage, bool crit)
        {
            float randomValue = Random.value;

            TryLifeSteal(randomValue, damage, crit);

            if (randomValue < _effectCreatorFire.Chance
                && damageable.CanAddDamageEffect((int)IDamageEffect.DamageEffectID.FIRE_ID))
            {
                damageable.AddDamageEffect(_effectCreatorFire.Get(damage));
                AudioManager.Instance.PlayOneShot(ac_burnEffectApplied);
            }

            if (randomValue < _lightningChance)
            {
                AudioManager.Instance.PlayOneShot(ac_lightningApplied);
                OnLightningEffectApplied?.Invoke(damageable);
            }
        }

        private IEnumerator ShootRoutine(IEnumerator routine, RefValue<float> deltaMult)
        {
            yield return routine;
            deltaMult.Value = 1f;
        }

        private IEnumerator RandomBulletShootRoutine()
        {
            var bursts = _randomBulletAmount / Random.Range(1, _randomBulletAmount);
            var perBurst = _randomBulletAmount / bursts;
            var remainder = _randomBulletAmount % bursts;

            for (int i = 0; i < bursts; i++)
            {
                _mainGun.PlayShotSound();
                for (int j = 0; j < perBurst; j++)
                    ShootRandomBullet();

               yield return _yieldBetweenRandomBursts;
            }

            if (remainder > 0)
            {
                _mainGun.PlayShotSound();
                for (int i = 0; i < remainder; i++)
                    ShootRandomBullet();
            }
            yield return _yieldBetweenMoonBursts;

            void ShootRandomBullet()
            {
                _randomGun.ShootBullet(damage: _damage * _randomBulletStatMult.Random(),
                                       critChance: _critChance * _randomBulletStatMult.Random(),
                                       critMultiplier: _critMultiplier * _randomBulletStatMult.Random(),
                                       knockback: _knockback * _randomBulletStatMult.Random(),
                                       size: _mainGun.Size * _randomBulletStatMult.Random(),
                                       speed: _mainGun.BulletSpeed * _randomBulletStatMult.Random(),
                                       pierce: Mathf.RoundToInt(_mainGun.PierceAmount * _randomBulletStatMult.Random()),
                                       bounce: Mathf.RoundToInt(_mainGun.BounceAmount * _randomBulletStatMult.Random()),
                                       duration: _mainGun.BulletDuration * _randomBulletStatMult.Random(),
                                       angle: 0f);
            }
        }

        private IEnumerator MoonGunShootRoutine()
        {
            int bursts = Mathf.RoundToInt(Mathf.Sqrt(_moonAmount));
            var shotsPerBurst = _moonAmount / bursts;
            var remainder = _moonAmount % bursts;

            yield return _moonGun.ShootRoutine(damage: _damage,
                                               critChance: _critChance,
                                               critMultiplier: _critMultiplier,
                                               knockback: _knockback,
                                               size: 1f,
                                               speed: _moonBulletSpeed,
                                               pierce: 1,
                                               bounce: 1,
                                               duration: 3f,
                                               angle: 0f,
                                               burstAmount: bursts,
                                               bulletAmount: shotsPerBurst,
                                               yieldBetweenBurst: _yieldBetweenMoonBursts);

            if (remainder > 0)
                _moonGun.ShootBulletBurst(damage: _damage,
                                          critChance: _critChance,
                                          critMultiplier: _critMultiplier,
                                          knockback: _knockback,
                                          size: 1f,
                                          speed: _moonBulletSpeed,
                                          pierce: 1,
                                          bounce: 1,
                                          duration: 3f,
                                          angle: 0f,
                                          remainder);

            yield return _yieldBetweenMoonBursts;
        }

        private void HPModified()
        {
            OnHPModified?.Invoke(_health.Hp / _health.MaxHP);
        }

        private IEnumerator Invulnerability()
        {
            yield return _waitInvulnerability;
            _hurtBox.enabled = true;
            OnInvulnerabilityEnded?.Invoke();
        }

        private IEnumerator Blink()
        {
            _flipBook.SR.color = _damagedColour;
            var clearColour = _damagedColour;
            clearColour.a = 0f;
            float eTime = 0f;

            for (int i = 0; i < _blinkAmount; i++)
            {
                while (eTime < _durationBetweenBlinks)
                {
                    float t = eTime / _durationBetweenBlinks;
                    _flipBook.SR.color = Color.Lerp(_damagedColour, clearColour, t);
                    eTime += Time.deltaTime;
                    yield return null;
                }

                while (eTime > 0f)
                {
                    float t = eTime / _durationBetweenBlinks;
                    _flipBook.SR.color = Color.Lerp(_damagedColour, clearColour, t);
                    eTime -= Time.deltaTime;
                    yield return null;
                }
            }

            _flipBook.SR.color = Color.white;
        }

        private void Damaged(float damage, float knockbakc, bool crit, Vector2 pos)
        {
            StartCoroutine(Invulnerability());
            AudioManager.Instance.PlayOneShot(_hurtSound, Random.Range(.4f, .6f));
            _hurtBox.enabled = false;
            OnDamaged?.Invoke();
            HPModified();

            if (_health.Hp > 0f)
                StartCoroutine(Blink());
        }

        private IEnumerator DodgeShine()
        {
            float eTime = 0f;
            var transform = _shineSR.transform;
            while (eTime < _shineTime) 
            {
                eTime += Time.deltaTime;
                transform.localPosition = Vector2.Lerp(_topPos, _botPos, eTime / _shineTime);
                yield return null;
            }

            transform.localPosition = _topPos;
        }

        private void Dodged()
        {
            AudioManager.Instance.PlayOneShot(ac_dodged);
            StartCoroutine(Invulnerability());
            StopCoroutine(_dodgeCo);
            _dodgeCo = DodgeShine();
            StartCoroutine(_dodgeCo);
            _dodgeParticle.Play();
        }

        private void OnDeath()
        {
            var pos = new Vector3(-6f, 0f);
            transform.localPosition += pos;
            _flipBook.SR.transform.position -= pos;

            enabled = false;

            _acceleration = new(_acceleration.Value * .5f);
            EvaluateDrag();

            _boundsCollider.offset = new(-.08f, -.04f);
            _boundsCollider.size = new(.16f, .04f);

            _hurtBox.enabled = false;
            _aura.gameObject.SetActive(false);
            _flipBook.Play(_deathAnimation);
        }
    }
}
