using LTFUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private static bool _initialized = false;

    [SerializeField] private Season[] _seasons;
    [SerializeField] private SeasonalFlipSheet[] _seasonalFlipSheets;
    [SerializeField] private SeasonalSprite[] _seasonalSprites;

    private void Awake()
    {
        if (_initialized)
            return;

        _initialized = true;

        var season = GetSeason(DateTime.Now);

        if (season.Equals(SeasonNames.NOT_IN_ANY_SEASON))
            return;
            
        SetSeasonals(_seasonalFlipSheets, season);
        SetSeasonals(_seasonalSprites, season);
    }

    private string GetSeason(DateTime date)
    {
        for (int i = 0; i < _seasons.Length; i++)
        {
            if (_seasons[i].IsDateInBetween(date))
                return _seasons[i].SeasonName;
        }
        return SeasonNames.NOT_IN_ANY_SEASON;
    }

    private void SetSeasonals(IEnumerable<ISeasonal> seasonals, string season)
    {
        foreach (var seasonal in seasonals)
            seasonal.Set(season);
    }

    private T[] GetSeasonal<T>() where T : ScriptableObject
    {
        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Seasonals");
        return LTFHelpers_Misc.GetScriptableObjects<T>();
    }

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

    [Serializable]
    private class Season
    {
        [field: SerializeField] public string SeasonName { get; private set; }
        [SerializeField] private int _dayStart;
        [SerializeField] private int _monthStart;
        [SerializeField] private int _dayEnd;
        [SerializeField] private int _monthEnd;
        [SerializeField] private int _yearPlus;

        public bool IsDateInBetween(DateTime date)
        {
            var startDate = new DateTime(date.Year, _monthStart, _dayStart);
            var endDate = new DateTime(date.Year + _yearPlus, _monthEnd, _dayEnd);
            return  date >= startDate && date <= endDate;
        }
    }
}
