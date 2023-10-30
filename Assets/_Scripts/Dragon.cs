using UnityEngine;

public class Dragon : MonoBehaviour
{
    private const float FORCE_MULTIPLIER = 0.5f;

    [SerializeField] private Gun _gun;
    [SerializeField] private Witch _witch;

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

    private bool _active;

    public void Update()
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

    public void Activate()
    {
        gameObject.SetActive(true);
        _active = true;
    }

    public void Shoot()
    {
        //if (!_active)
        //    return;

        _gun.ShootRoutine(_witch.Damage.Value * FORCE_MULTIPLIER,
                          _witch.CritChance.Value * FORCE_MULTIPLIER,
                          _witch.Knockback.Value * FORCE_MULTIPLIER,
                          _witch.Gun.Size.Value * FORCE_MULTIPLIER,
                          _witch.Gun.BulletSpeed.Value * 1.5f,
                          (int)(_witch.Gun.PierceAmount * FORCE_MULTIPLIER),
                          (int)(_witch.Gun.BounceAmount * FORCE_MULTIPLIER),
                          _witch.Gun.BulletDuration.Value,
                          0f,
                          _witch.Gun.TimeToCompleteShooting,
                          Mathf.CeilToInt(_witch.Gun.BulletAmount * FORCE_MULTIPLIER),
                          Mathf.CeilToInt(_witch.Gun.BurstAmount * FORCE_MULTIPLIER),
                          _witch.Gun.SeparationPerBullet);
    }
}
