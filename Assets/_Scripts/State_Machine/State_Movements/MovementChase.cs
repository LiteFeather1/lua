using UnityEngine;

public abstract class MovementChase : MovementState
{
    [SerializeField] protected Transform _target;
    [SerializeField] protected float _yFollowMultiplier = 1f;
    [SerializeField] protected bool _followX = false;
    [SerializeField] protected float _xDirection = -1f;

    public void SetTarget(Transform target) => _target = target;

    protected void Flip(float direction)
    {
        transform.localScale = new(1f * -direction, 1f, 1f); 
    }
}
