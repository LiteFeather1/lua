using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Witch : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField, ReadOnly] private int _currency;
    private int _totalCurrencyGained;

    [Header("Health")]
    [SerializeField] private HealthPlayer _health;
    [SerializeField] private AudioClip _hurtSound;

    [Header("Moviment")]
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private CompositeValue _acceleration = new(2f);
    private float _initialAcceleration;
    [SerializeField] private Vector2 _decelerationRange = new(.95f, .75f);
    [SerializeField] private float _accelerationMaxForRange = 50f;
    private Vector2 _inputDirection;

    // TODO remove time to complete shooting. Add instead time between shoots. Only Start time again after finnish complete burst

    [Header("Main Gun")]
    [SerializeField] private WitchGun _mainGun;
    [SerializeField] private CompositeValue _damage = new(5f); 
    [SerializeField] private CompositeValue _critChance = new(.01f);
    [SerializeField] private CompositeValue _critMultiplier = new(1.5f);
    [SerializeField] private CompositeValue _knockback = new(1f);
    [SerializeField] private CompositeValue _shootTime = new(1f);
    private float _elapsedShootTime = 0f;
    private float _shootDeltaMult = 1f;

    [Header("Random Bullet")]
    [SerializeField] private int _randomBulletAmount = 0;
    [SerializeField] private CompositeValue _randomBulletShootTime = new(1f);
    private float _elapsedRandomShootTime = 0f;

    [Header("Orbital Gun")]
    [SerializeField] private OrbitalGun _orbitalGun;
    [SerializeField] private CompositeValue _orbitalShootTime = new(2f);
    private float _elapsedOrbitalShootTime = 0f;
    private float _orbitalDeltaMult = 1f;

    [Header("Life Steal")]
    [SerializeField] private CompositeValue _chanceToLifeSteal = new(.01f);
    [SerializeField] private CompositeValue _lifeStealPercent = new(.1f);

    [Header("Burn Effect")]
    [SerializeField] private EffectCreatorFire _effectCreatorFire;

    [Header("Aura")]
    [SerializeField] private Aura _aura;

    [Header("Thorn")]
    [SerializeField] private CompositeValue _thornRange = new(.64f);
    [SerializeField] private CompositeValue _thorBaseDamage;
    [SerializeField] private CompositeValue _thornDefenceDamageMultiplier = new(.5f);

    [Header("Lightning")]
    [SerializeField] private CompositeValue _lightningChance = new(0f);
    [SerializeField] private CompositeValue _lightningBaseDamage = new(5f);
    [SerializeField] private CompositeValue _lightningRange = new(.64f);
    [SerializeField] private int _lightningMinChain = 1;

    [Header("Blink on Damage")]
    [SerializeField] private float _invulnerabilityDuration;
    private WaitForSeconds _waitInvulnerability;
    [SerializeField] private int _blinkAmount;
    [SerializeField] private float _durationBetweenBlinks;
    [SerializeField] private Color _damagedColour = Color.red;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Collider2D _hurtBox;

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

    public OrbitalGun OrbitalGun => _orbitalGun;
    public CompositeValue OrbitalShootTime => _orbitalShootTime;

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

    private void Awake()
    {
        _rb.drag = _decelerationRange.x;
        _initialAcceleration = _acceleration.Value;
        _waitInvulnerability = new(_invulnerabilityDuration);
    }

    private void OnEnable()
    {
        _mainGun.OnDamageAppplied += DamageAppliedGun;
        _mainGun.OnFinishedShooting += MainGunFinishedShooting;

        _orbitalGun.OnDamageAppplied += DamageAppliedGun;
        _orbitalGun.OnFinishedShooting += OrbitalFinishedShooting;

        _health.OnDamaged += Damaged;
        _health.OnDeath += OnDeath;
        _health.OnHeal += HPModified;

        _damage.OnValueModified += _aura.SetDamage;
        _critChance.OnValueModified += _aura.SetCrit;
        _critMultiplier.OnValueModified += _aura.SetCritMultiplier;
    }

    private void Start() => ModifyCurrency(4);

    private void Update()
    {
        _inputDirection = GameManager.Inputs.Player.Moviment.ReadValue<Vector2>().normalized;

        float delta = Time.deltaTime;
        _elapsedShootTime += delta * _shootDeltaMult;
        if (_elapsedShootTime >= _shootTime)
        {
            _elapsedShootTime = 0f;
            _shootDeltaMult = 0f;
            _mainGun.StartShootRoutine(_damage, _critChance, _critMultiplier, _knockback);
            OnMainShoot?.Invoke();
        }

        _elapsedRandomShootTime += delta;
        if (_elapsedRandomShootTime >= _randomBulletShootTime)
        {
            _elapsedRandomShootTime = 0f;
            for (int i = 0; i < _randomBulletAmount; i++)
            {
                _mainGun.ShootBullet(_damage, _critChance, _critMultiplier, _knockback, Random.Range(0f, 360f));
            }
        }

        _elapsedOrbitalShootTime += delta * _orbitalDeltaMult;
        if (_elapsedOrbitalShootTime > _orbitalShootTime)
        {
            _elapsedOrbitalShootTime = 0f;
            _orbitalDeltaMult = 0f;
            _orbitalGun.StartShootRoutine(_damage * .33f,
                                          _critChance,
                                          _critMultiplier,
                                          _knockback * .33f,
                                          _mainGun.BulletSpeed * .33f,
                                          _mainGun.BulletDuration * 2f);
        }
    }

    private void FixedUpdate()
    {
        var velocity = _rb.velocity;
        velocity.x += _inputDirection.x * _acceleration.Value;
        velocity.y += _inputDirection.y * _acceleration.Value;

        if (Mathf.Sign(_inputDirection.x) == Mathf.Sign(_rb.velocity.x) && Mathf.Abs(_rb.velocity.x) > _maxSpeed)
            velocity.x = 0f;

        if (Mathf.Sign(_inputDirection.y) == Mathf.Sign(_rb.velocity.y) && Mathf.Abs(_rb.velocity.y) > _maxSpeed)
            velocity.y = 0f;

        _rb.AddForce(velocity, ForceMode2D.Force);
    }

    private void OnDisable()
    {
        _mainGun.OnDamageAppplied -= DamageAppliedGun;
        _mainGun.OnFinishedShooting -= MainGunFinishedShooting;

        _orbitalGun.OnDamageAppplied -= DamageAppliedGun;
        _orbitalGun.OnFinishedShooting -= OrbitalFinishedShooting;

        _health.OnDamaged -= Damaged;
        _health.OnDeath -= OnDeath;
        _health.OnHeal -= HPModified;

        _damage.OnValueModified -= _aura.SetDamage;
        _critChance.OnValueModified -= _aura.SetCrit;
        _critMultiplier.OnValueModified -= _aura.SetCritMultiplier;
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

    public void TryLifeSteal(float randomValue, float damage)
    {
        if (randomValue < _chanceToLifeSteal)
        {
            _health.Heal(damage * _lifeStealPercent);
        }
    }

    private void DamageApplied(float damage)
    {
        float randomValue = Random.value;
        TryLifeSteal(randomValue, damage);
    }

    private void DamageAppliedGun(IDamageable damageable, float damage)
    {
        DamageApplied(damage);
        float randomValue = Random.value;
        if (randomValue < _effectCreatorFire.Chance
            && damageable.CanAddDamageEffect((int)IDamageEffect.DamageEffectID.FIRE_ID))
        {
            damageable.AddDamageEffect(_effectCreatorFire.Get(damage));
        }

        if (randomValue < _lightningChance)
        {
            OnLightningEffectApplied?.Invoke(damageable);

        }
    }

    private void MainGunFinishedShooting() => _shootDeltaMult = 1f;

    private void OrbitalFinishedShooting() => _orbitalDeltaMult = 1f;

    private void HPModified()
    {
        OnHPModified?.Invoke(_health.HP / _health.MaxHP);
    }

    private void Damaged(float damage, float knockbakc, bool crit, Vector2 pos)
    {
        AudioManager.Instance.PlayOneShot(_hurtSound, Random.Range(.4f, .6f));
        _hurtBox.enabled = false;
        OnDamaged?.Invoke();
        HPModified();
        StartCoroutine(Blink());
        StartCoroutine(Invulnerability());
    }

    private IEnumerator Blink()
    {
        _sr.color = _damagedColour;
        var clearColour = _damagedColour;
        clearColour.a = 0f;
        float eTime = 0f;

        for (int i = 0; i < _blinkAmount; i++)
        {
            while (eTime < _durationBetweenBlinks) 
            {
                float t = eTime / _durationBetweenBlinks;
                _sr.color = Color.Lerp(_damagedColour, clearColour, t);
                eTime += Time.deltaTime; 
                yield return null;
            }

            while (eTime > 0f)
            {
                float t = eTime / _durationBetweenBlinks;
                _sr.color = Color.Lerp(_damagedColour, clearColour, t);
                eTime -= Time.deltaTime;
                yield return null;
            }
        }

        _sr.color = Color.white;
    }

    private IEnumerator Invulnerability()
    {
        yield return _waitInvulnerability;
        _hurtBox.enabled = true;
        OnInvulnerabilityEnded?.Invoke();
    }

    private void OnDeath()
    {
        transform.localPosition += new Vector3(-12f, 0f);
        gameObject.SetActive(false);
    }
}
