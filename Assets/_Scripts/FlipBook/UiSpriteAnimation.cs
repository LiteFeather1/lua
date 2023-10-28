using UnityEngine;
using UnityEngine.UI;

namespace RetroAnimation
{
    public class UiSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private FlipSheet _flipSheet;
        private int _frame;

        private void Update()
        {
            _frame = (int)(_flipSheet.Fps * Time.unscaledTime) % _flipSheet.FramesLength;
            _image.sprite = _flipSheet.Sprites[_frame];
        }
    }
}
