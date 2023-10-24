using UnityEngine;

public class Witch : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private CompositeValue _acceleration;
    private float _initialAcceleration;
    [SerializeField] private Vector2 _decelerationRange = new(.95f, .75f);
    [SerializeField] private float _accelerationMaxForRange = 50f;
    private Vector2 _inputDirection;

    [Header("Shoot")]
    [SerializeField] private Gun _gun;
    [SerializeField] private float _shootTime = 1f;
    private float _elaspedShootTime;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    private void Awake()
    {
        _rb.drag = _decelerationRange.x;
        _initialAcceleration = _acceleration.Value;
    }

    private void Update()
    {
        _inputDirection = GameManager.Inputs.Player.Moviment.ReadValue<Vector2>().normalized;

        _elaspedShootTime += Time.deltaTime;

        if (_elaspedShootTime >= _shootTime)
        {
            _elaspedShootTime = 0f;
            _gun.Shoot();
        }
    }

    private void FixedUpdate()
    {
        float x = _inputDirection.x * _acceleration.Value * Time.deltaTime;
        float y = _inputDirection.y * _acceleration.Value * Time.deltaTime;
        var velocity = _rb.velocity;
        velocity.y += y;
        velocity.x += x;
        if (Mathf.Sign(_inputDirection.x) == Mathf.Sign(_rb.velocity.x) 
            && Mathf.Abs(_rb.velocity.x) > _maxSpeed)
            velocity.x = 0f;
        if (Mathf.Sign(_inputDirection.y) == Mathf.Sign(_rb.velocity.y) 
            && Mathf.Abs(_rb.velocity.y) > _maxSpeed)
            velocity.y = 0f;

        _rb.AddForce(velocity, ForceMode2D.Force);
    }

    public void AddAccelerationMofifier(CompositeValueModifier mod)
    {
        _acceleration.AddModifier(mod);
        float t = (_acceleration.Value - _initialAcceleration) / _accelerationMaxForRange;
        _rb.drag = Mathf.Lerp(_decelerationRange.x, _decelerationRange.y, t);
    }
}
