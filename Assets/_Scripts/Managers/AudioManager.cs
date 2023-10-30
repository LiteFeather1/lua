using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private Vector2 _randomPitchRange = new(.95f, 1.15f);

    public AudioSource MusicSource => _musicSource;
    public AudioSource SFXSource => _sfxSource;

    public void PlayOneShot(AudioClip clip, float pitch)
    {
        _sfxSource.PlayOneShot(clip, pitch);
    }

    public void PlayOneShot(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip, _randomPitchRange.Random());
    }
}
