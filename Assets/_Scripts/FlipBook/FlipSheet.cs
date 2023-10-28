using UnityEngine;

namespace RetroAnimation
{
    [CreateAssetMenu(fileName = "New Flip Sheet", menuName = "Flip Sheet")]
    public class FlipSheet : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _fps = 6;

        public int FramesLength => _sprites.Length;
        public Sprite[] Sprites => _sprites;
        public float Fps => _fps;
    }
}