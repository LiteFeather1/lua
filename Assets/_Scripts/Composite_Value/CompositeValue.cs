using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompositeValue
{
    [SerializeField] private float _baseValue;
    [SerializeField, DrawInEditorMode(false)] private float _value;
    private float _lastValue = float.MinValue;
    // Used to determine if it should recalculate
    private bool _isDirty = true;

    public float BaseValue => _baseValue;

    public float Value
    {
        get
        {
            if (_isDirty || _baseValue != _lastValue)
            {
                _lastValue = _baseValue;
                _value = CalculateFinalValue();
                _isDirty = false;
            }
            return _value;
        }
    }

    private List<CompositeValueModifier> _compositeModifiers;

    public int ModifierCount => _compositeModifiers.Count;

    public CompositeValue()
    {
        _compositeModifiers = new();
    }

    public CompositeValue(float baseValue) : this()
    {
        _baseValue = baseValue;
    }

    private float CalculateFinalValue()
    {
        float finalValue = _baseValue;
        float sumPercentAdd = 0;
        for (int i = 0; i < _compositeModifiers.Count; i++)
        {
            CompositeValueModifier imodifier = _compositeModifiers[i];
            switch (imodifier.Type)
            {
                case CompositeValueModifierType.None:
                    break;

                case CompositeValueModifierType.Flat:
                    finalValue += imodifier.Value;
                    break;

                case CompositeValueModifierType.PercentAdditive:
                    sumPercentAdd += imodifier.Value;
                    bool isPercentAdditiveDone = i + 1 >= _compositeModifiers.Count || 
                        _compositeModifiers[i + 1].Type != CompositeValueModifierType.PercentAdditive;
                    if (isPercentAdditiveDone)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                    break;

                case CompositeValueModifierType.PercentMultiplier:
                    finalValue *= 1 + imodifier.Value;
                    break;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }

    public void ForceRecalculate()
    {
        _lastValue = _baseValue;
        _value = CalculateFinalValue();
        _isDirty = false;
    }

    public void AddModifier(CompositeValueModifier modifier)
    {
        _compositeModifiers.Add(modifier);
        _isDirty = true;
        _compositeModifiers.Sort(CompareModifierOrder);
    }

    public void AddMultipleModifiers(IEnumerable<CompositeValueModifier> modifiers)
    {
        foreach (var modifier in modifiers)
        {
            _compositeModifiers.Add(modifier);
        }
        _isDirty = true;
        _compositeModifiers.Sort(CompareModifierOrder);
    }

    public bool RemoveModifier(CompositeValueModifier modifier)
    {
        if (_compositeModifiers.Remove(modifier))
        {
            _isDirty = true;
            return true;
        }

        return false;
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = _compositeModifiers.Count - 1; i >= 0; i--)
        {
            if (_compositeModifiers[i].Source != source)
                continue;

            _isDirty = true;
            didRemove = true;
            _compositeModifiers.RemoveAt(i);
        }
        return didRemove;
    }

    /// <summary>
    /// This will clear all the modifiers and set the value to the base value
    /// </summary>
    public void Clear()
    {
        _compositeModifiers = new();
        _value = _baseValue;
    }

    /// <summary>
    /// This will clear all the modifiers and set the value to the base value
    /// Use this with responsibility 
    /// </summary>
    public void Clear(float newBaseValue)
    {
        _baseValue = newBaseValue;
        Clear();
    }

    private int CompareModifierOrder(CompositeValueModifier a, CompositeValueModifier b)
    {
        if(a.Order < b.Order)
            return -1;
        else if(a.Order > b.Order)
            return 1;
        else
            return 0;
    }
}
