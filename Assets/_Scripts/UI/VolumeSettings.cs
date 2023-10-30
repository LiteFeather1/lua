using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    //private const string MASTER = "Master";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXsVolume);
    }

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC, .5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX, .5f);
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;

        SetMusicVolume(musicVolume);
        SetSFXsVolume(sfxVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MUSIC, _musicSlider.value);
        PlayerPrefs.SetFloat(SFX, _sfxSlider.value);
    }

    private void SetMusicVolume(float volume)
    {
        SetVolume(volume, MUSIC);
    }

    private void SetSFXsVolume(float volume)
    {
        SetVolume(volume, SFX);
    }

    private void SetVolume(float volume, string channel)
    {
        _masterMixer.SetFloat(channel, Mathf.Log10(volume) * 20f);
    }
}
