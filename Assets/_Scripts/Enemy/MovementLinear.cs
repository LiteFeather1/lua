using UnityEngine;

public class MovementLinear : Movement
{
    [SerializeField] private Vector2 _direction;

    public void SetDirection(Vector2 direction) => _direction = direction;

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _direction);
    }
}
