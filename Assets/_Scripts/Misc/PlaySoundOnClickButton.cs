using UnityEngine;

public class PlaySoundOnClickButton : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    public void Play()
    {
        AudioManager.Instance.PlayOneShot(_clip);
    }
}
