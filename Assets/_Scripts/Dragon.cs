using RetroAnimation;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] private Witch _witch;
    [SerializeField] private Gun _gun;
    [SerializeField] private FlipBook _flipBook;
    private float _forceMultiplier = 0.25f;

    [Header("Growth Stages")]
    [SerializeField] private FlipSheet[] _growthStages;
    private int _currentGrowth = -1;

    [Header("Movement")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _xOffset = -.06f;
    [SerializeField] private float _yMinOffset = .06f;
    [SerializeField] private float _yOffset = 0.12f;
    [SerializeField] private float _ySinSpeed = 1f;
    [SerializeField] private float _ySinAmplitude = 0.04f;
    [SerializeField] private float _xCosSpeed = 1f;
    [SerializeField] private float _xCosAmplitude = 0.04f;
    private Vector2 _velocity;

    private void Update()
    {
        var time = Time.time;
        Vector2 to = _witch.transform.localPosition;
        var xCos = Mathf.Cos(time * _xCosSpeed) * _xCosAmplitude;
        to.x += _xOffset + xCos;
        var camSize = GameManager.Instance.Camera.orthographicSize;
        var yT = Mathf.InverseLerp(-camSize, camSize, to.y);
        var y = Mathf.Lerp(_yOffset, -_yOffset, yT);
        if (Mathf.Abs(y) < _yMinOffset)
            y = _yMinOffset * Mathf.Sign(y);
        var ySin = Mathf.Sin(time * _ySinSpeed) * _ySinAmplitude;
        to.y += y + ySin;
        transform.localPosition = Vector2.SmoothDamp(transform.localPosition, to, ref _velocity, _speed * Time.deltaTime);  
    }

    private void OnDisable()
    {
        if (_witch == null)
            return;

        _witch.OnMainShoot -= Shoot;
    }

    public void Activate(Witch witch)
    {
        _witch = witch;
        witch.OnMainShoot += Shoot;
    }

    public void Grow(float forceMultiplier)
    {
        _currentGrowth++;
        _flipBook.Play(_growthStages[_currentGrowth], true);
        _forceMultiplier = forceMultiplier;
    }

    private void Shoot()
    {
        _gun.ShootRoutine(damage: _witch.Damage.Value * _forceMultiplier,
                          critChance: _witch.CritChance.Value * _forceMultiplier,
                          critMultiplier: Mathf.Min(_witch.CritMultiplier.Value * _forceMultiplier, 1f),
                          knockback: _witch.Knockback.Value * _forceMultiplier,
                          size: Mathf.Max(_witch.Gun.Size.Value * _forceMultiplier, .5f),
                          speed: _witch.Gun.BulletSpeed.Value * 1.25f,
                          pierce: (int)(_witch.Gun.PierceAmount * _forceMultiplier),
                          bounce: (int)(_witch.Gun.BounceAmount * _forceMultiplier),
                          duration: _witch.Gun.BulletDuration.Value,
                          angle: 0f,
                          waitBetweenBursts: _witch.Gun.TimeToCompleteShooting,
                          bulletAmount: Mathf.CeilToInt(_witch.Gun.BulletAmount * _forceMultiplier),
                          burstAmount: Mathf.CeilToInt(_witch.Gun.BurstAmount * _forceMultiplier),
                          separationPerBullet: _witch.Gun.SeparationPerBullet);
    }

}
