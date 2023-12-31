namespace LTF.CompositeValue
{
    public interface ICompositeValueModifier
    {
        public float Value { get; }
        public CompositeValueModifierType Type { get; }
        public int Order { get; }
        public object Source { get; }

        public void SetValue(float value);
        public void SetSource(object source);
    }
}
