using UnityEngine;

namespace RetroAnimation
{
    [CreateAssetMenu(fileName = "New Flip Sheet", menuName = "Flip Sheet")]
    public class FlipSheet : ScriptableObject, LTF.ISetable<FlipSheet.FlipSheetData>
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _fps = 6;

        public int FramesLength => _sprites.Length;
        public Sprite[] Sprites => _sprites;
        public float Fps => _fps;

        public void Set(FlipSheetData value)
        {
            _sprites = value.Sprites;
            _fps = value.Fps;
        }

        [System.Serializable]
        public class FlipSheetData
        {
            [field: SerializeField] public Sprite[] Sprites { get; private set; }
            [field: SerializeField] public int Fps { get; private set; }
        }
    }
}