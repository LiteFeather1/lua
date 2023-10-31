using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private Vector2 _randomPitchRange = new(.95f, 1.15f);

    [Header("Clips")]
    [SerializeField] private AudioClip _candyPickup;

    public AudioSource MusicSource => _musicSource;
    public AudioSource SFXSource => _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayOneShot(AudioClip clip, float pitch)
    {
        _sfxSource.PlayOneShot(clip, pitch);
    }

    public void PlayOneShot(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip, _randomPitchRange.Random());
    }

    public void PlayCandyPickUp()
    {
        PlayOneShot(_candyPickup);
    }
}
