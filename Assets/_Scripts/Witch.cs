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
    [SerializeField] private float _maxSpeed;
    [SerializeField] private CompositeValue _acceleration;
    private float _initialAcceleration;
    [SerializeField] private Vector2 _decelerationRange = new(.95f, .75f);
    [SerializeField] private float _accelerationMaxForRange = 50f;
    private Vector2 _inputDirection;

    [Header("Shoot")]
    [SerializeField] private WitchGun _gun;
    [SerializeField] private CompositeValue _damage;
    [SerializeField] private CompositeValue _critChance = new(.01f);
    [SerializeField] private CompositeValue _critMultiplier = new(1.5f);
    [SerializeField] private CompositeValue _knockback;
    [SerializeField] private CompositeValue _shootTime = new(1f);
    [SerializeField] private CompositeValue _randomBulletShootTime = new(1f);
    [SerializeField] private int _randomBulletAmount;
    private float _elapsedShootTime;
    private float _elapsedRandomShootTime;

    [Header("Life Steal")]
    [SerializeField] private CompositeValue _chanceToLifeSteal = new(.01f);
    [SerializeField] private CompositeValue _lifeStealPercent = new(.1f);

    [Header("Dragon")]
    [SerializeField] private Dragon _dragon;

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

    public int Currency => _currency;
    public int TotalCurrencyGained => _totalCurrencyGained;
    public HealthPlayer Health => _health;

    public WitchGun Gun => _gun;
    public CompositeValue Damage => _damage;
    public CompositeValue CritChance => _critChance;
    public CompositeValue CritMultiplier => _critMultiplier;
    public CompositeValue Knockback => _knockback;
    public CompositeValue ShootTime => _shootTime;
    public CompositeValue RandomBulletShootTime => _randomBulletShootTime;
    public void AddRandomBullet(int amount) => _randomBulletAmount += amount;

    public CompositeValue ChanceToLifeSteal => _chanceToLifeSteal;
    public CompositeValue LifeStealPercent => _lifeStealPercent;

    public Dragon Dragon => _dragon;

    private void Awake()
    {
        _rb.drag = _decelerationRange.x;
        _initialAcceleration = _acceleration.Value;
        _waitInvulnerability = new(_invulnerabilityDuration);
    }

    private void OnEnable()
    {
        _gun.OnDamageAppplied += DamagedApplied;

        _health.OnDamaged += Damaged;
        _health.OnDeath += OnDeath;
        _health.OnHeal += HPModified;
    }

    private void Start()
    {
        ModifyCurrency(4);
    }

    private void Update()
    {
        _inputDirection = GameManager.Inputs.Player.Moviment.ReadValue<Vector2>().normalized;

        float delta = Time.deltaTime;
        _elapsedShootTime += delta;
        if (_elapsedShootTime >= _shootTime.Value)
        {
            _elapsedShootTime = 0f;
            _gun.ShootRoutine(_damage.Value, _critChance.Value , _critMultiplier.Value, _knockback.Value);
            _dragon.Shoot();
        }

        _elapsedRandomShootTime += delta;
        if (_elapsedRandomShootTime >= _randomBulletShootTime.Value)
        {
            _elapsedRandomShootTime = 0f;
            for (int i = 0; i < _randomBulletAmount; i++)
            {
                _gun.ShootBullet(_damage.Value, _critChance.Value, _critMultiplier.Value, _knockback.Value, Random.Range(0f, 360f));
            }
        }
    }

    private void OnDisable()
    {
        _gun.OnDamageAppplied -= DamagedApplied;

        _health.OnDamaged -= Damaged;
        _health.OnDeath -= OnDeath;
        _health.OnHeal -= HPModified;
    }

    private void FixedUpdate()
    {
        var velocity = _rb.velocity;
        velocity.x += _inputDirection.x * _acceleration.Value;
        velocity.y += _inputDirection.y * _acceleration.Value;
        if (Mathf.Sign(_inputDirection.x) == Mathf.Sign(_rb.velocity.x) 
            && Mathf.Abs(_rb.velocity.x) > _maxSpeed)
            velocity.x = 0f;
        if (Mathf.Sign(_inputDirection.y) == Mathf.Sign(_rb.velocity.y) 
            && Mathf.Abs(_rb.velocity.y) > _maxSpeed)
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

    public void AddAccelerationMofifier(CompositeValueModifier mod)
    {
        _acceleration.AddModifier(mod);
        float t = (_acceleration.Value - _initialAcceleration) / _accelerationMaxForRange;
        if (t > 1f)
            t = 1f;
        _rb.drag = _decelerationRange.Evaluate(t);
    }

    private void DamagedApplied(float damage)
    {
        if (Random.value < _chanceToLifeSteal.Value)
        {
            _health.Heal(damage * _lifeStealPercent.Value);
        }
    }

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
