using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private const float GRAVITY = -9.8f;

    [SerializeField] private TextMeshProUGUI _text;
    private Vector2 _velocity;
    [SerializeField] private float _time = .5f;
    private float _elapsedTime;    

    public System.Action<DamageNumber> Return { get; set; }

    private void Update()
    {
        var colour = _text.color;
        colour.a = 1f - (_elapsedTime / _time);
        _text.color = colour;

        var delta = Time.deltaTime;
        _velocity.y += GRAVITY * delta * 10f;
        _velocity.x *= .98f;
        transform.localPosition += (Vector3)(_velocity * delta);

        _elapsedTime += delta;
        if (_elapsedTime > _time)
        {
            _elapsedTime = 0f;
            gameObject.SetActive(false);
            Return?.Invoke(this);
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
        print(velocity.x);
    }

    public void SetText(string text, Color colour)
    {
        _text.text = text;
        _text.color = colour;
    }
}
