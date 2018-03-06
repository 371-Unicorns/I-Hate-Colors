using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu for upgrades.
/// </summary>
public class UpgradesMenu : MonoBehaviour
{

    public int upgradeHealthCost = 100;

    public void UpgradeCastleHealth()
    {
        if (upgradeHealthCost <= GameManager.money)
        {
            GameManager.AddMoney(-upgradeHealthCost);
            CastleManager.AddCastleHealth();
        }
    }
}
