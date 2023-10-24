using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float _speed = 1f;
    public void SetSpeed(float speed) => _speed = speed;
}
