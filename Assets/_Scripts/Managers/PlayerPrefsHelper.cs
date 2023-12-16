using UnityEngine;

public static class PlayerPrefsHelper
{
    private const string SESSIONS = "Sessions";
    private const string RUNS = "Runs";
    private const string PLAY_TIME = "All Play Time";
    private const string ENEMIES_KILLED = "Enemies Killed";
    private const string CARDS_PLAYED = "Cards Played";
    private const string CARDS_RECYCLED = "Cards Recycled";
    private const string CANDY_EARNED = "Candy Earned";

    //private const string MASTER = "Master";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";

    public static string Music => MUSIC;
    public static string Sfx => SFX;

    public static float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC, .5f);
    public static void SetMusicVolume(float v) => PlayerPrefs.SetFloat(MUSIC, v); 

    public static float GetSFXVolume() => PlayerPrefs.GetFloat(SFX, .5f);
    public static void SetSFXVolume(float v) => PlayerPrefs.SetFloat(SFX, v);

    public static int GetSessions() => PlayerPrefs.GetInt(SESSIONS, 1);
    public static void AddSession() => PlayerPrefs.SetInt(SESSIONS, PlayerPrefs.GetInt(SESSIONS, 0) + 1);

    public static int GetRuns() => PlayerPrefs.GetInt(RUNS, 0);
    public static void AddRun() => PlayerPrefs.SetInt(RUNS, PlayerPrefs.GetInt(RUNS, 0) + 1);

    public static float GetPlayTime() => PlayerPrefs.GetFloat(PLAY_TIME, 0f);
    public static void AddPlayTime(float timeToAdd)
    {
        PlayerPrefs.SetFloat(PLAY_TIME, PlayerPrefs.GetFloat(PLAY_TIME, 0f) + timeToAdd);
    }

    public static int GetEnemiesKilled() => PlayerPrefs.GetInt(ENEMIES_KILLED, 0);
    public static void AddEnemiesKilled(int amount)
    {
        PlayerPrefs.SetInt(ENEMIES_KILLED, PlayerPrefs.GetInt(ENEMIES_KILLED, 0) + amount);
    }

    public static int GetCardsPlayed() => PlayerPrefs.GetInt(CARDS_PLAYED, 0);
    public static void AddCardsPlayed(int amount)
    {
        PlayerPrefs.SetInt(CARDS_PLAYED, PlayerPrefs.GetInt(CARDS_PLAYED, 0) + amount);
    }

    public static int GetCardsRecycled() => PlayerPrefs.GetInt(CARDS_RECYCLED, 0);
    public static void AddCardsRecycled(int amount)
    {
        PlayerPrefs.SetInt(CARDS_RECYCLED, PlayerPrefs.GetInt(CARDS_RECYCLED, 0) + amount);
    }

    public static int GetCandyEarned() => PlayerPrefs.GetInt(CANDY_EARNED, 0);
    public static void AddCandyEarned(int amount)
    {
        PlayerPrefs.SetInt(CANDY_EARNED, PlayerPrefs.GetInt(CANDY_EARNED, 0) + amount);
    }

    public static void Save() => PlayerPrefs.Save();
}
