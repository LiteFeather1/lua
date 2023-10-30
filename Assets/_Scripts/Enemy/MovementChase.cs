using UnityEngine;

public abstract class MovementChase : Movement
{
    [SerializeField] protected Transform _target;
    [SerializeField] protected bool _followX = false;
    [SerializeField] protected float _xDirection = -1f;

    public void SetTarget(Transform target) => _target = target;

    protected void Flip(float direction)
    {
        transform.localScale = Vector3.one * -direction; 
    }
}
