using UnityEngine;

public class Witch : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private CompositeValue _acceleration;
    [SerializeField] private Vector2 _decelerationRange = new(.95f, .75f);
    [SerializeField] private float _accelerationMaxForRange = 50f;
    private float _deceleration;
    private Vector2 _inputDirection;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    private void Awake()
    {
        _deceleration = _decelerationRange.x;
    }

    private void Update()
    {
        _inputDirection = GameManager.Inputs.Player.Moviment.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        float x = _inputDirection.x * _acceleration.Value * Time.deltaTime;
        float y = _inputDirection.y * _acceleration.Value * Time.deltaTime;
        var velocity = _rb.velocity;
        velocity.x += x;
        velocity.y += y;
        velocity *= _deceleration;
        if (Mathf.Abs(velocity.x) > _maxSpeed)
            velocity.x = _maxSpeed * _inputDirection.x;
        if (Mathf.Abs(velocity.y) > _maxSpeed)
            velocity.y = _maxSpeed * _inputDirection.y;
        _rb.velocity = velocity;
    }

    public void AddAccelerationMofifier(CompositeValueModifier mod)
    {
        _acceleration.AddModifier(mod);
        float t = _acceleration.Value / _accelerationMaxForRange;
        _deceleration = Mathf.Lerp(_decelerationRange.x, _decelerationRange.y, t);
    }
}
