using UnityEngine;

public class MovementChase : Movement
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool _followX = false;
    [SerializeField] private float _xDirection = -1f;

    public void SetTarget(Transform target) => _target = target;

    private void Update()
    {
        Vector2 target = _target.position;
        if (!_followX)
            target.x = transform.position.x + _xDirection;
        transform.position = Vector2.MoveTowards(transform.position,
                                                 target,
                                                 _speed * Time.deltaTime);
    }
}
