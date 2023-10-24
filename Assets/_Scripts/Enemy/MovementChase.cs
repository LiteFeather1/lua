using UnityEngine;

public class MovementChase : Movement
{
    [SerializeField] private Transform _target;

    public void SetTarget(Transform target) => _target = target;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                 _target.position,
                                                 _speed * Time.deltaTime);
    }
}
