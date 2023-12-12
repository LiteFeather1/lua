using LTFUtils;
using System;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private static bool _initialized = false;

    [SerializeField] private Season[] _seasons;
    [SerializeField] private SeasonalFlipSheet[] _seasonalFlipSheets;
    [SerializeField] private SeasonalSprite[] _seasonalSprites;
    [SerializeField] private SeasonalSprite[] _seasonalDaySprite;

    private void Awake()
    {
        if (_initialized)
            return;

        _initialized = true;

        var season = GetSeason(DateTime.Now, out bool isInPeakDay);
        if (!season.Equals(SeasonNames.NOT_IN_ANY_SEASON))
        {
            SetSeasonals(_seasonalFlipSheets, season);
            SetSeasonals(_seasonalSprites, season);

            if (isInPeakDay)
                SetSeasonals(_seasonalDaySprite, season);
#if UNITY_EDITOR
            else
                SetSeasonalDefault(_seasonalDaySprite);
#endif
        }
#if UNITY_EDITOR
        else
        {
            SetSeasonalDefault(_seasonalFlipSheets);
            SetSeasonalDefault(_seasonalSprites);
            SetSeasonalDefault(_seasonalDaySprite);
        }
        print(season);
        print($"Is Peak Day = {isInPeakDay}");
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

    private void SetSeasonalDefault(ISeasonal[] seasonals)
    {
        for (int i = 0; i < seasonals.Length; i++)
            seasonals[i].SetDefault();
    }

    private T[] GetSeasonal<T>() where T : ScriptableObject
    {
        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Seasonals");
        return LTFHelpers_Misc.GetScriptableObjects<T>();
    }

#if UNITY_EDITOR
    [ContextMenu("Get Seasonal Flip Sheets")]
    private void GetSeasonalFlipSheet()
    {
        _seasonalFlipSheets = GetSeasonal<SeasonalFlipSheet>();
    }

    [ContextMenu("Get Seasonal Flip Sprite")]
    private void GetSeasonalSprite()
    {
        _seasonalSprites = GetSeasonal<SeasonalSprite>();
    }
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
