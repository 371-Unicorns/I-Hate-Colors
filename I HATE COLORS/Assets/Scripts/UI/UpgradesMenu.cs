using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu for upgrades.
/// 
/// Author: Cole Twitchell
/// </summary>
public class UpgradesMenu : MonoBehaviour
{
    public static Button[] upgradeButtons;

    public int upgradeHealthCost = 100;
    public int activateDrabOMaticCost = 1000;

    /// <summary>
    /// Instantiates upgrade buttons and sets upgrades menu panel to not be visible
    /// </summary>
    private void Start()
    {
        upgradeButtons = GameObject.Find("UpgradesMenu").GetComponentsInChildren<Button>();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles the effect of clicking on any upgrade buttons
    /// </summary>
    private void Update()
    {
        if (!SumPause.Status)
        {
            foreach (Button button in upgradeButtons)
            {
                switch (button.gameObject.name)
                {
                    case ("UpgradeCastleHealthButton"):
                        button.interactable = upgradeHealthCost <= GameManager.money ? true : false;
                        break;
                    case ("ActivateDrabOMaticButton"):
                        button.interactable = activateDrabOMaticCost <= GameManager.money ? true : false;
                        break;
                    default:
                        Debug.LogError("A upgrade button, which does not have a money check, was pressed.");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// If eligible, adds 100 to the castle health, takes appropriate amount of money
    /// </summary>
    public void UpgradeCastleHealth()
    {
        if (upgradeHealthCost <= GameManager.money)
        {
            GameManager.AddMoney(-upgradeHealthCost);
            CastleManager.AddCastleHealth();
        }
    }

    /// <summary>
    /// Game over if Drab-o-Matic is activated
    /// </summary>
    public void ActivateDrabOMatic()
    {
        GameManager.gameOver = true;
    }
}
