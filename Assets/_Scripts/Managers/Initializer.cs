using LTFUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private static bool _initialized = false;

    [Header("Season")]
    [SerializeField] private Season[] _seasons;

    [Header("Seasonal Flip Sheets")]
    [SerializeField] private SeasonalFlipSheet[] _seasonalFlipSheets;

    [Header("Seasonal Sprite")]
    [SerializeField] private SeasonalSprite[] _seasonalSprites;
    [SerializeField] private SeasonalSprite[] _seasonalDaySprite;

    [Header("Seasonal String Array")]
    [SerializeField] private SeasonalStringArray[] _seasonalStringArray;

    private void Awake()
    {
        if (_initialized)
            return;

        _initialized = true;

        PlayerPrefsHelper.AddSession();
        PlayerPrefsHelper.Save();

        var season = GetSeason(DateTime.Now, out bool isInPeakDay);
        if (!season.Equals(SeasonNames.NOT_IN_ANY_SEASON))
        {
            SetSeasonals(_seasonalFlipSheets, season);

            SetSeasonals(_seasonalSprites, season);

            SetSeasonals(_seasonalStringArray, season);

            if (isInPeakDay)
                SetSeasonals(_seasonalDaySprite, season);
#if UNITY_EDITOR 
            else
                SetSeasonalDefault(_seasonalDaySprite);

            print($"Is Peak Day = {isInPeakDay}");
#endif
        }
#if UNITY_EDITOR
        else
        {
            SetSeasonalDefault(_seasonalFlipSheets);

            SetSeasonalDefault(_seasonalSprites);
            SetSeasonalDefault(_seasonalDaySprite);

            SetSeasonalDefault(_seasonalStringArray);
        }
        print(season);
#endif
    }

    private string GetSeason(DateTime date, out bool isInPeakDay)
    {
        for (int i = 0; i < _seasons.Length; i++)
        {
            if (_seasons[i].IsDateInBetween(date, out bool isPeak))
            {
                isInPeakDay = isPeak;
                return _seasons[i].SeasonName;
            }
        }
        isInPeakDay = false;
        return SeasonNames.NOT_IN_ANY_SEASON;
    }

    private void SetSeasonals(ISeasonal[] seasonals, string season)
    {
        for (int i = 0; i < seasonals.Length; i++)
            seasonals[i].Set(season);
    }

#if UNITY_EDITOR
    private void SetSeasonalDefault(ISeasonal[] seasonals)
    {
        for (int i = 0; i < seasonals.Length; i++)
            seasonals[i].SetDefault();
    }

    private T[] GetSeasonal<T>() where T : ScriptableObject
    {
        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Seasonals");
        return LTFHelpers_EditorOnly.GetScriptableObjects<T>();
    }

    [ContextMenu("Get Seasonal Flip Sheets")]
    private void GetSeasonalFlipSheet() => _seasonalFlipSheets = GetSeasonal<SeasonalFlipSheet>();

    [ContextMenu("Get Seasonal Sprite")]
    private void GetSeasonalSprite()
    {
        var allSprites = GetSeasonal<SeasonalSprite>();
        int d = 1;
        for(int i = 0; i < allSprites.Length - d; i++)
        {
            if (allSprites[i].name[0] != 'D')
                continue;

            (allSprites[i], allSprites[^d]) = (allSprites[^d++], allSprites[i]);
        }
        var offSet = allSprites.Length - d + 1;
        _seasonalSprites = allSprites[0..offSet];
        _seasonalDaySprite = allSprites[offSet..];
    }

    [ContextMenu("Get Seasonal String Array")]
    private void GetSeasonalStringArray() => _seasonalStringArray = GetSeasonal<SeasonalStringArray>();
#endif

    [Serializable]
    private struct Season
    {
        [field: SerializeField] public string SeasonName { get; private set; }

        [Header("Season")]
        [SerializeField] private int _dayStart;
        [SerializeField] private int _monthStart;
        [SerializeField] private int _dayEnd;
        [SerializeField] private int _monthEnd;
        [SerializeField] private int _yearPlus;

        [Header("Peak Day")]
        [SerializeField] private int _day;
        [SerializeField] private int _month;

        public readonly bool IsDateInBetween(DateTime date, out bool isPeakDay)
        {
            var startDate = new DateTime(date.Year, _monthStart, _dayStart);
            var endDate = new DateTime(date.Year + _yearPlus, _monthEnd, _dayEnd);
            isPeakDay = _day == date.Day && _month == date.Month;
            return date >= startDate && date <= endDate;
        }
    }
}
