using UnityEngine;
using LTF.Utils; 

public class MovementKnockback : StateMachineCore.State
{
    private const int TIMES_TO_SHAKE = 7;

    [Header("Knockback State")]
    [SerializeField] private float _duration = .15f;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private Vector2 _direction;
    private float _knockbackForce = 1f;
    private readonly Vector2 _rockyOffsetRange = new(.2f, .25f);
    private Vector2 _offset;
    private int _timesShaked;

    public void SetUp(Vector2 direction, float knockbackForce)
    {
        _direction = direction;
        _knockbackForce = knockbackForce;
        _timesShaked = 0;
    }

    public override void Do()
    {
        if (StateTime > _duration / TIMES_TO_SHAKE * _timesShaked)
        {
            _timesShaked++;
            float sign = _timesShaked % 2 == 0 ? 1f : -1f;
            _offset.Set(_rockyOffsetRange.Random() / 2f, _rockyOffsetRange.Random() * sign);
        }

        var delta = Time.deltaTime * _speed * _knockbackForce;
        var x = (_offset.x + _direction.x) * delta;
        var y = (_offset.y + _direction.y) * delta;
        _core.transform.position += new Vector3(x, y);

        if (StateTime > _duration)
            CompleteState("State Time Ended");
    }
}
