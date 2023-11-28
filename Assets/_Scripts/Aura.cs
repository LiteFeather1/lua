using UnityEngine;

public class Aura : MonoBehaviour
{
    [Header("Sizes")]
    [SerializeField] private ValueSpriteArray _auraSprites;
    private int _currentAuraIndex = 0;

    [Header("Components")]
    [SerializeField] private LTFUtils.FixedTimer _tickRateTimer;
    [SerializeField] private HitBox _hitbox;
    [SerializeField] private SpriteRenderer sr_Aura;
    [SerializeField] private CircleCollider2D _c;

    // TODO: LifeSteal from here
    // TODO: Crit ???

    private void OnEnable()
    {
        _tickRateTimer.TimeEvent += Tick;
    }

    private void OnDisable()
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
        if (_currentAuraIndex > _auraSprites.Length - 1)
            _currentAuraIndex = _auraSprites.Length;

        SetAura(_auraSprites[_currentAuraIndex]);

        return _currentAuraIndex == _auraSprites.Length;
    }

    public void SetDamage(float value) => _hitbox.SetDamage(value * .33f);
}
