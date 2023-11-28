using UnityEngine;

public class Aura : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LTFUtils.FixedTimer _tickRateTimer;
    [SerializeField] private HitBox _hitbox;
    [SerializeField] private SpriteRenderer sr_Aura;
    [SerializeField] private CircleCollider2D _c;

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

    public void SetDamage(float value) => _hitbox.SetDamage(value * .33f);
}
