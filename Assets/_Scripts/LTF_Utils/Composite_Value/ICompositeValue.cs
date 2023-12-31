using System;
using System.Collections.Generic;

namespace LTF.CompositeValue
{
    public interface ICompositeValue
    {
        public Action<float> OnValueModified { get; set; }

        float BaseValue { get; }
        float Value { get; }

        /// <summary>
        /// Recalculates new Value taking into account all modifiers
        /// </summary>
        void Recalculate();

        /// <summary>
        /// Adds a new modifier and Recalculates Value
        /// </summary>
        /// <param name="modifier"></param>
        void AddModifier(ICompositeValueModifier modifier);
        /// <summary>
        /// Adds a collection of modifiers and Recalculates Value
        /// </summary>
        /// <param name="modifiers"></param>
        void AddMultipleModifiers(IEnumerable<ICompositeValueModifier> modifiers);

        /// <summary>
        /// Removes a modifier and recalculates
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns>Returns if a modifier was removed</returns>
        bool RemoveModifier(ICompositeValueModifier modifier);
        /// <summary>
        /// Removes all modifiers from a object source
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Returns if modifiers were removed</returns>
        bool RemoveAllModifiersFromSource(object source);

        /// <summary>
        /// Sets a new Base Value and Recalculates
        /// </summary>
        /// <param name="newBase"></param>
        void SetNewBase(float newBase);

        /// <summary>
        /// Clear all the modifiers and set the value to the Base value
        /// </summary>
        void Clear();

        /// <summary>
        /// Clear all the modifiers and set the value to the new Base value
        /// Use this with responsibility 
        /// </summary>
        void Clear(float newBaseValue);

        static int CompareModifierOrder(ICompositeValueModifier a, ICompositeValueModifier b) => a.Order switch
        {
            var order when order < b.Order => -1,
            var order when order > b.Order => 1,
            _ => 0,
        };
    }
}
