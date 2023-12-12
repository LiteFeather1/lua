public interface ISeasonal
{
#if UNITY_EDITOR
    public void SetDefault();
#endif
    public void Set(string season);
    public void Add(string name);
    public void AddChristmas();
    public void AddHalloween();
}
