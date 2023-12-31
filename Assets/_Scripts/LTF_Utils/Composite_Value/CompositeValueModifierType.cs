namespace LTF.CompositeValue
{
    public enum CompositeValueModifierType
    {
        /// <summary>
        /// This means it will add a flat value to base
        /// </summary>
        Flat = 100,
        /// <summary>
        /// This means it will multiply the base value
        /// But only after adding all the PercentAditives together.
        /// </summary>
        PercentAdditive = 200,
        /// <summary>
        /// This means it will multiply the base 
        /// Also this stacks multiplicative with other multipliers
        /// </summary>
        PercentMultiplier = 300,
    }
}
