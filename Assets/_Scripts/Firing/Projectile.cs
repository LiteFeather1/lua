using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IDeactivatable
{
    [SerializeField] private int _pierce;
    [SerializeField] private int _bounce;
    [SerializeField] private float _duration;
    private Vector2 _direction;
    private float _elapsedTime;
    private float _speed;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    public Action Deactivated { get; set; }

    public Vector2 Direction => _direction;
    public float Speed => _speed;

    public Rigidbody2D RB => _rb;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _duration)
            Deactivate();
    }

    public void SetSpeedAndDirection(float speed, Vector2 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    public void SetStats(int pierce, int bounce, float duration)
    {
        _pierce = pierce;
        _bounce = bounce;
        _duration = duration;
    }

    public void Shoot(Vector2 direction)
    {
        _direction = direction;
        _rb.velocity = direction * _speed;
    }

    public void Shoot(float speed, Vector2 direction)
    {
        _rb.velocity = direction * speed;
        SetSpeedAndDirection(speed, direction);
    }

    public void Deactivate()
    {
        if (_rb != null)
            _rb.velocity = Vector2.zero;
        _elapsedTime = 0f;
        Deactivated?.Invoke();
        gameObject.SetActive(false);
    }

    protected virtual void Bounce(Collider2D collision, bool _)
    {
        _bounce--;
        var contactPoint = collision.ClosestPoint(transform.position);
        var normal = (Vector2)transform.position - contactPoint;
        var reflect = Vector2.Reflect(_rb.velocity.normalized, normal.normalized);
        Shoot(reflect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var isScreen = collision.CompareTag("Screen");
        if (_pierce > 0 && !isScreen)
            _pierce--;
        else if (_bounce > 0)
            Bounce(collision, isScreen);
        else
            Deactivate();
    }
}
