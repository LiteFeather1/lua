using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
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
        float musicVolume = PlayerPrefsHelper.GetMusicVolume();
        float sfxVolume = PlayerPrefsHelper.GetSFXVolume();
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;

        SetMusicVolume(musicVolume);
        SetSFXsVolume(sfxVolume);
    }

    private void OnDisable()
    {
        PlayerPrefsHelper.SetMusicVolume(_musicSlider.value);
        PlayerPrefsHelper.SetSFXVolume(_sfxSlider.value);
    }

    private void SetMusicVolume(float volume)
    {
        SetVolume(volume, PlayerPrefsHelper.Music);
    }

    private void SetSFXsVolume(float volume)
    {
        SetVolume(volume, PlayerPrefsHelper.Sfx);
    }

    private void SetVolume(float volume, string channel)
    {
        _masterMixer.SetFloat(channel, Mathf.Log10(volume) * 20f);
    }
}
