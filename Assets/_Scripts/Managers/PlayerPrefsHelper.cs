using UnityEngine;

public static class PlayerPrefsHelper
{
    //private const string MASTER = "Master";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    public static string Music => MUSIC;
    public static string Sfx => SFX;

    public static float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC, .5f);
    public static void SaveMusicVolume(float v) => PlayerPrefs.SetFloat(MUSIC, v); 

    public static float GetSFXVolume() => PlayerPrefs.GetFloat(SFX, .5f);
    public static void SaveSFXVolume(float v) => PlayerPrefs.SetFloat(SFX, v); 
}
