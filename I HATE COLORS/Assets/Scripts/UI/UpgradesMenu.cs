using UnityEngine;
using UnityEngine.UI;

// TODO doc
/// <summary>
/// Menu for upgrades.
/// </summary>
public class UpgradesMenu : MonoBehaviour
{
    public static Button[] upgradeButtons;

    public int upgradeHealthCost = 100;
    public int activateDrabOMaticCost = 1000;

    private void Start()
    {
        upgradeButtons = GameObject.Find("UpgradesMenu").GetComponentsInChildren<Button>();
        this.gameObject.SetActive(false);
    }

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

    public void UpgradeCastleHealth()
    {
        if (upgradeHealthCost <= GameManager.money)
        {
            GameManager.AddMoney(-upgradeHealthCost);
            CastleManager.AddCastleHealth();
        }
    }

    public void ActivateDrabOMatic()
    {
        GameManager.gameOver = true;
    }
}
