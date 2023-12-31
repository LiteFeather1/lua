using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTF.CompositeValue
{
    [Serializable]
    public class CompositeValue
    {
        [SerializeField] private float _baseValue;
        [SerializeField] private float _value;
        private readonly List<CompositeValueModifier> _compositeModifiers;

        public Action<float> OnValueModified { get; set; }

        public float BaseValue => _baseValue;
        public float Value => _value;

        public CompositeValue() => _compositeModifiers = new();

        public CompositeValue(float baseValue) : this()
        {
            _baseValue = baseValue;
            _value = baseValue;
        }

        public void SetNewBase(float newBase)
        {
            _baseValue = newBase;
            Recalculate();
        }

        public void Clear()
        {
            _compositeModifiers.Clear();
            _value = _baseValue;
            OnValueModified?.Invoke(_baseValue);
        }

        public void Clear(float newBaseValue)
        {
            _baseValue = newBaseValue;
            Clear();
        }

        public void Recalculate()
        {
            _value = CalculateFinalValue();
            OnValueModified?.Invoke(_value);
        }

        public void AddModifier(CompositeValueModifier modifier)
        {
            _compositeModifiers.Add(modifier);
            _compositeModifiers.Sort(CompareModifierOrder);
            Recalculate();
        }

        public void AddMultipleModifiers(IEnumerable<CompositeValueModifier> modifiers)
        {
            foreach (var modifier in modifiers)
                _compositeModifiers.Add(modifier);

            _compositeModifiers.Sort(CompareModifierOrder);
            Recalculate();
        }

        public bool RemoveModifier(CompositeValueModifier modifier)
        {
            if (!_compositeModifiers.Remove(modifier))
                return false;

            Recalculate();
            return true;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            var didRemove = false;
            for (int i = _compositeModifiers.Count - 1; i >= 0; i--)
            {
                if (_compositeModifiers[i].Source != source)
                    continue;

                didRemove = true;
                _compositeModifiers.RemoveAt(i);
            }

            if (didRemove)
                Recalculate();

            return didRemove;
        }

        public override string ToString()
        {
            return $"Base Value {_baseValue}. Value {_value}";
        }

        private float CalculateFinalValue()
        {
            var finalValue = _baseValue;
            var sumPercentAdd = 0f;
            for (int i = 0; i < _compositeModifiers.Count; i++)
            {
                var imodifier = _compositeModifiers[i];
                switch (imodifier.Type)
                {
                    case CompositeValueModifierType.Flat:
                        finalValue += imodifier.Value;
                        break;
                    case CompositeValueModifierType.PercentAdditive:
                        sumPercentAdd += imodifier.Value;
                        if (i + 1 >= _compositeModifiers.Count
                            || _compositeModifiers[i + 1].Type != CompositeValueModifierType.PercentAdditive)
                        {
                            finalValue *= 1f + sumPercentAdd;
                            sumPercentAdd = 0f;
                        }
                        break;
                    case CompositeValueModifierType.PercentMultiplier:
                        finalValue *= 1f + imodifier.Value;
                        break;
                }
            }

            return finalValue;
        }

        static int CompareModifierOrder(CompositeValueModifier lhs, CompositeValueModifier rhs) => lhs.Order switch
        {
            var order when order < rhs.Order => -1,
            var order when order > rhs.Order => 1,
            _ => 0,
        };

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
