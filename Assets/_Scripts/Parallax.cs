using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _directionVelocity;
    [SerializeField] private Vector2 _deltaSpeed;
    private Vector2 _startPos;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _subject;
    [SerializeField] private SpriteRenderer _sr;

    private float DistanceToSubject => transform.position.z - _subject.position.z;
    private float ClippingPlane => _camera.transform.position.z + (DistanceToSubject > 0f ? _camera.farClipPlane : _camera.nearClipPlane);
    private float ParallaxFactor => Mathf.Abs(DistanceToSubject) / ClippingPlane;

    public void Start()
    {
        _startPos = transform.position;
        var speed = _directionVelocity * ParallaxFactor;
        _sr.material.SetVector("_offset_velocity", speed);
    }

    private void LateUpdate()
    {
        var delta = _deltaSpeed * ((Vector2)_subject.position - _startPos);
        var newPos = _startPos + (delta * ParallaxFactor);
        _sr.material.SetVector("_offset", newPos);
    }
}
