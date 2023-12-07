using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Color _colour;

    [Header("Ranges")]
    [SerializeField] private Vector4 _speedRange;
    [SerializeField] private Vector2 _healthRange;
    [SerializeField] private Vector2 _defenceRange = new(0f, 50f);
    [SerializeField] private Vector2 _damageRange;

    public string Name => _name;
    public Color Colour => _colour;

    public float Speed(float t) => _speedRange.Evaluate(t);
    public float Health(float t) => _healthRange.Evaluate(t);
    public float Defence(float t) => _defenceRange.Evaluate(t);
    public float Damage(float t) => _damageRange.Evaluate(t);
}
