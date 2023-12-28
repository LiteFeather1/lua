using UnityEngine;
using Lua.Managers;

namespace Lua.Misc
{
    public class PlaySoundOnClickButton : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        public void Play()
        {
            AudioManager.Instance.PlayOneShot(_clip);
        }
    }
}
