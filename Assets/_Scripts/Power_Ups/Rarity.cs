using UnityEngine;

namespace Lua.PowerUps
{
    [CreateAssetMenu(fileName = "New Rarity")]
    public class Rarity : ScriptableObject
    {
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public Color RarityColour { get; private set; } = Color.white;
    }
}
