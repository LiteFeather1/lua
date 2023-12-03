using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IDeactivatable
{
    [SerializeField] private int _pierce;
    [SerializeField] private int _bounce;
    [SerializeField] private float _time;
    private Vector2 _direction;
    private float _elapsedTime;
    private float _speed;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    public Action Deactivated { get; set; }

    public Vector2 Direction => _direction;
    public float Speed => _speed;   

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _time)
            Deactivate();
    }

    public void SetSpeedAndDirection(float speed, Vector2 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    public void Shoot(float speed, Vector2 direction)
    {
        _rb.velocity = direction * speed;
        SetSpeedAndDirection(_speed, direction);
    }

    public void Shoot(float speed, Vector2 direction, int pierce, int bounce, float time)
    {
        Shoot(speed, direction);
        _pierce = pierce;
        _bounce = bounce;
        _time = time;
    }

    public void Deactivate()
    {
        _rb.velocity = Vector2.zero;
        _elapsedTime = 0f;
        Deactivated?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isScreen = collision.CompareTag("Screen");
        if (_pierce > 0 && !isScreen)
            _pierce--;
        else if (_bounce > 0)
        {
            _bounce--;
            var contactPoint = collision.ClosestPoint(transform.position);
            var normal = (Vector2)transform.position - contactPoint;
            var reflect = Vector2.Reflect(_rb.velocity.normalized, normal.normalized);
            Shoot(_speed, reflect);
        }
        else if (!isScreen)
            Deactivate();
    }
}
