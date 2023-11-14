using UnityEngine;

public abstract class MovementState : StateMachine.State
{
    [SerializeField] protected float _speed = 1f;

    public void SetSpeed(float speed) => _speed = speed;
}
