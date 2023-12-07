using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CheckPowerUps : MonoBehaviour
{
    [SerializeField] private RarityPrice[] _rarityPrices;
    [SerializeField] private Sprite _noTier;
    [SerializeField] private Sprite[] _tiers;
    [SerializeField] private PowerUpProblem[] _powerUpProblems;

    [ContextMenu("find")]
    private void FindProblems()
    {
        Dictionary<Rarity, Vector2Int> rarityToPrice = new(_rarityPrices.Length);
        foreach (var item in _rarityPrices)
        {
            rarityToPrice.Add(item.Rarity, item.Prices);
        }

        Dictionary<Sprite, string> tierToPlus = new(_tiers.Length);
        for (int i = 0; i < _tiers.Length; i++)
        {
            tierToPlus.Add(_tiers[i], new('+', i + 1));
        }

        var allPowerUps = LTFUtils.LTFHelpers_Misc.GetScriptableObjects<PowerUp>();
        var powerUpProblem = new List<PowerUpProblem>();
        foreach (var powerUp in allPowerUps)
        {
            var problems = new List<string>();

            if (string.IsNullOrEmpty(powerUp.Name))
                problems.Add("Power up has no name");

            if (string.IsNullOrEmpty(powerUp.Effect))
                problems.Add("Power has no description");

            if (powerUp.Cost <= 0)
                problems.Add("Power up has no cost");

            if (powerUp.Rarity != null)
            { 
                if (rarityToPrice.ContainsKey(powerUp.Rarity))
                {
                    if (powerUp.Cost < rarityToPrice[powerUp.Rarity].x)
                        problems.Add("Price might be too low");
                    else if (powerUp.Cost > rarityToPrice[powerUp.Rarity].y)
                        problems.Add("Price might be too");
                }
                else
                    problems.Add("Rarity might be be debug.");
            }
            else
                problems.Add("Rarity is null");

            if (powerUp.Icon == null)
                problems.Add("Power Up has no Icon");

            if (powerUp.TierIcon == null)
                problems.Add("Power has no tier icon");
            else if (powerUp.TierIcon == _noTier)
            {
                if (powerUp.Name.Contains('+'))
                    problems.Add("Power has + when it shouldn't");
            }
            else if (!powerUp.Name.Contains(tierToPlus[powerUp.TierIcon]) && !powerUp.name.Contains("Dragon"))
                problems.Add("Power up has not enough pluses");

            if (problems.Count > 0)
                powerUpProblem.Add(new(powerUp, problems.ToArray()));
        }

        _powerUpProblems = powerUpProblem.ToArray();
        if (_powerUpProblems.Length > 0)
        {
            print($"{_powerUpProblems.Length} Problems found");
            EditorUtility.SetDirty(this);
        }
        else
            print("No Problems Found");

    }

    [System.Serializable]
    private struct RarityPrice
    {
        public Rarity Rarity;
        public Vector2Int Prices;
    }

    [System.Serializable]
    private struct PowerUpProblem
    {
        public PowerUp PowerUP;
        public string[] Problems;

        public PowerUpProblem(PowerUp powerUP, string[] problems)
        {
            PowerUP = powerUP;
            Problems = problems;
        }
    }
}
#endif
