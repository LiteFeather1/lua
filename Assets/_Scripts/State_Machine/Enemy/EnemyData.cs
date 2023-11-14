using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Color _colour;

    [Header("Ranges")]
    [SerializeField] private Vector4 _speedRange;
    [SerializeField] private Vector2 _healthRange;
    [SerializeField] private Vector2 _damageRange;

    public string Name => _name;

    public Color Colour => _colour;

    public Vector4 SpeedRange => _speedRange;
    public Vector2 HealthRange => _healthRange;
    public Vector2 DamageRange => _damageRange;
}
