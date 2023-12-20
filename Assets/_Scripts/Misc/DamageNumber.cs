using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private const float GRAVITY = -9.8f;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _time = .5f;
    private float _elapsedTime;    
    private Vector3 _velocity;
    private float _stopDecelX;

    public System.Action<DamageNumber> Return { get; set; }

    private void Update()
    {
        var colour = _text.color;
        colour.a = 1f - (_elapsedTime / _time);
        _text.color = colour;

        var delta = Time.deltaTime;
        _velocity.y += GRAVITY * delta * 16f;
        if (_velocity.x > _stopDecelX)
            _velocity.x *= .99f;
        transform.localPosition += _velocity * delta;

        _elapsedTime += delta;
        if (_elapsedTime < _time)
            return;

        _elapsedTime = 0f;
        gameObject.SetActive(false);
        Return?.Invoke(this);
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
        _stopDecelX = velocity.x * .1f;
    }

    public void SetText(string text, Color colour)
    {
        _text.text = text;
        _text.color = colour;
    }
}
