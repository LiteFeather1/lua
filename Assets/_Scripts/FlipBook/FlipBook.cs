using UnityEngine;

namespace RetroAnimation
{
    public class FlipBook : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private FlipSheet _flipSheet;
        private float _startTime;
        private int _frame;
        private float _fps = 1;
        [Tooltip("If true the animation will play on Start. Also if false the animation will stop")]
        [SerializeField] private bool _playing;
        [Tooltip("Makes the animation loop the current Flip Sheet")]
        [SerializeField] private bool _looping;

        public SpriteRenderer SR => _sr;
        public FlipSheet FlipSheet => _flipSheet;
        public int Frame => _frame + 1;
        public bool Playing => _playing;
        /// <summary>
        /// Current time on animation, restarts on zero when a new animation is set to be played
        /// </summary>
        public float AnimationTime => Time.time - _startTime;
        public float AnimationTimeToFinish => (1 / _fps) * (_flipSheet == null ? 1 : _flipSheet.FramesLength);
        public float T => AnimationTime / AnimationTimeToFinish;

        public System.Action<FlipBook> OnAnimationFinished { get; set; }

        private void Awake()
        {
            //Starts animation
            if (_playing)
                Play(_flipSheet, _looping, true);
        }

        private void Update()
        {
            if (_playing)
                UpdateFrame();
        }

        /// <summary>
        /// Updates current flip sheet
        /// </summary>
        private void UpdateFrame()
        {
            if (AnimationTime < _flipSheet.FramesLength / _fps || _looping)
            {
                _frame = (int)(_fps * AnimationTime) % _flipSheet.FramesLength;
                _sr.sprite = _flipSheet.Sprites[_frame];
            }
            else
            {
                _playing = false;
                OnAnimationFinished?.Invoke(this);
            }
        }

        public void SetSheet(FlipSheet flipSheet) => _flipSheet = flipSheet;

        /// <summary>
        /// Default FPS, getting the fps from the flip sheet
        /// </summary>
        public void Play(FlipSheet flipSheet, bool looping = false, bool overRide = false)
        {
            Play(flipSheet, flipSheet.Fps, looping, overRide);
        }

        /// <summary>
        /// Starts playing a animation
        /// </summary>
        public void Play(FlipSheet flipSheet, float fps, bool looping = false, bool overRide = false)
        {
            _frame = 0;
            if (_flipSheet != flipSheet || overRide)
            {
                _playing = true;
                _flipSheet = flipSheet;
                _fps = fps;
                _looping = looping;
                _startTime = Time.time;
            }
        }

        public void Play(bool looping = false, bool overRide = true)
        {
            Play(_flipSheet, looping, overRide);
        }

        public void SetFrame(int frame)
        {
            _sr.sprite = _flipSheet.Sprites[_frame = frame];
        }

        public void Stop()
        {
            _playing = false;
        }

        /// <summary>
        /// Stops at a specific frame
        /// </summary>
        public void Stop(int frame)
        {
            Stop();
            _sr.sprite = _flipSheet.Sprites[frame];
        }
    }
}
