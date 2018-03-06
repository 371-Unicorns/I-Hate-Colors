using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu for upgrades.
/// </summary>
public class UpgradesMenu : MonoBehaviour
{

    private int currentHealthLevel = 1;
    public int upgradeHealthCost = 100;

    public void UpgradeCastleHealth()
    {
        if (upgradeHealthCost <= GameManager.money)
        {
            GameManager.AddMoney(-upgradeHealthCost);

            currentHealthLevel++;
        }
    }
}
