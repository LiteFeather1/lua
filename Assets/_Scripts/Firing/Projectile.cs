using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IDeactivatable
{
    [SerializeField] private int _pierce;
    [SerializeField] private int _bounce;
    [SerializeField] private float _time;
    private float _elapsedTime;
    private float _speed;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    public Action Deactivated { get; set; }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _time)
        {
            Deactivate();
        }
    }

    public void Shoot(float speed, Vector2 direction)
    {
        _rb.velocity = direction * speed;
        _speed = speed;
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
        if (!collision.CompareTag("Screen"))
        {
            if (_pierce > 0)
                _pierce--;
            else
                Deactivate();
        }
        else if (_bounce > 0)
        {
            _bounce--;
            var contactPoint = collision.ClosestPoint(transform.position);
            var normal = (Vector2)transform.position - contactPoint;
            var reflect = Vector2.Reflect(_rb.velocity.normalized, normal.normalized);
            Shoot(_speed, reflect);
        }
    }
}
