using UnityEngine;

[CreateAssetMenu(menuName = "Seasonal/String Array")]
public class SeasonalStringArray : Seasonal<string[], ValueStringArray> 
{
#if UNITY_EDITOR
    public void LoadMessages(System.Collections.Generic.IDictionary<string, string[]> dict)
    {
        UnityEditor.EditorUtility.SetDirty(this);
        _dictionary = new(dict);
    }
#endif
}
