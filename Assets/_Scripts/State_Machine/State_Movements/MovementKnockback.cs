using UnityEngine;

public class MovementKnockback : MovementState
{
    private const float MIN_DISTANCE = .01f;
    [SerializeField] private Vector2 _destination;

    public void SetDestination(Vector2 destination) => _destination = destination;

    public override void Do()
    {
        _core.transform.position = Vector2.MoveTowards(_core.Position, _destination, Time.deltaTime * _speed);
        if (Vector2.Distance(_destination, _core.Position) < MIN_DISTANCE)
            CompleteState("Reached Destination");
    }
}
