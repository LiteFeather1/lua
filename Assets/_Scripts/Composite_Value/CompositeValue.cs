using System;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeValues
{
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

        // I want to see this on debug mode on the inspector
#pragma warning disable IDE0044 // Add readonly modifier
        private List<CompositeValueModifier> _compositeModifiers;
#pragma warning restore IDE0044 // Add readonly modifier

        public Action<float> OnValueModified { get; set; }

        public CompositeValue()
        {
            _compositeModifiers = new();
        }

        public CompositeValue(float baseValue) : this()
        {
            _baseValue = baseValue;
            _value = baseValue;
            _isDirty = false;
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
            OnValueModified?.Invoke(_value);
        }

        public void AddModifier(CompositeValueModifier modifier)
        {
            _compositeModifiers.Add(modifier);
            _isDirty = true;
            _compositeModifiers.Sort(CompareModifierOrder);
            OnValueModified?.Invoke(Value);
        }

        public void AddMultipleModifiers(IEnumerable<CompositeValueModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                _compositeModifiers.Add(modifier);
            }
            _isDirty = true;
            _compositeModifiers.Sort(CompareModifierOrder);
            OnValueModified?.Invoke(Value);
        }

        public bool RemoveModifier(CompositeValueModifier modifier)
        {
            if (_compositeModifiers.Remove(modifier))
            {
                _isDirty = true;
                OnValueModified?.Invoke(Value);
            }

            return _isDirty;
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

            if (didRemove)
                OnValueModified?.Invoke(Value);

            return didRemove;
        }

        public void SetNewBase(float newBase)
        {
            _baseValue = newBase;
            _value = newBase;
        }

        /// <summary>
        /// This will clear all the modifiers and set the value to the base value
        /// </summary>
        public void Clear()
        {
            _compositeModifiers.Clear();
            _value = _baseValue;
            OnValueModified?.Invoke(_baseValue);
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
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            else
                return 0;
        }

        public override string ToString()
        {
            return $"Base Value {_baseValue}. Value {Value}";
        }

        #region Operator
        public static implicit operator float(CompositeValue a) => a.Value;

        public static bool operator >(CompositeValue lhs, CompositeValue rhs) => lhs.Value > rhs.Value;
        public static bool operator >(CompositeValue lhs, float rhs) => lhs.Value > rhs;
        public static bool operator >(float lhs, CompositeValue rhs) => lhs > rhs.Value;
    
        public static bool operator <(CompositeValue lhs, CompositeValue rhs) => lhs.Value < rhs.Value;
        public static bool operator <(CompositeValue lhs, float rhs) => lhs.Value < rhs;
        public static bool operator <(float lhs, CompositeValue rhs) => lhs < rhs.Value;

        public static bool operator >=(CompositeValue lhs, CompositeValue rhs) => lhs.Value >= rhs.Value;
        public static bool operator >=(CompositeValue lhs, float rhs) => lhs.Value >= rhs;
        public static bool operator >=(float lhs, CompositeValue rhs) => lhs >= rhs.Value;

        public static bool operator <=(CompositeValue lhs, CompositeValue rhs) => lhs.Value <= rhs.Value;
        public static bool operator <=(CompositeValue lhs, float rhs) => lhs.Value <= rhs;
        public static bool operator <=(float lhs, CompositeValue rhs) => lhs <= rhs.Value;

        #endregion
    }
}
