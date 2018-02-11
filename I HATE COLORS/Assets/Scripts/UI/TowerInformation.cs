using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInformation : Singleton<TowerInformation>
{
    // Header
    private Text nameText;

    // Stats panel
    private Text currentLevel;
    private Text upgradeCost;

    // Button panel
    private Button deleteButton;
    private Button upgradeButton;

    private void Start()
    {
        nameText = this.transform.Find("Header").Find("NameImage").gameObject.GetComponentInChildren<Text>();

        Transform body = this.transform.Find("Body");

        Transform statsPanel = body.Find("StatsPanel");
        currentLevel = statsPanel.Find("CurrentLevelText").GetComponent<Text>();
        upgradeCost = statsPanel.Find("UpgradeCostText").GetComponent<Text>();

        Transform buttonsPanel = body.Find("ButtonsPanel");
        deleteButton = buttonsPanel.Find("DeleteButton").GetComponent<Button>();
        upgradeButton = buttonsPanel.Find("UpgradeButton").GetComponent<Button>();

        Reset();
    }

    public void ShowPlacedTower(Tower tower)
    {
        currentLevel.text = "Current level: " + tower.level.ToString();
        upgradeCost.text = "Upgrade cost: " + tower.upgradeCost.ToString();

        deleteButton.interactable = true;
        upgradeButton.interactable = true;
    }

    // TODO better method name
    public void ShowHoverinTower(Tower tower)
    {
        currentLevel.text = "Current level: " + tower.level.ToString();
        upgradeCost.text = "Base cost: " + tower.baseCost.ToString();

        deleteButton.interactable = false;
        upgradeButton.interactable = false;

    }

    public void Reset()
    {
        currentLevel.text = "";
        upgradeCost.text = "";

        deleteButton.interactable = false;
        upgradeButton.interactable = false;
    }

    public void CheckUpgrade()
    {
        Tower selectedTower = GameManager.Instance.newSelectedTower;
        int upgradeCost = selectedTower.upgradeCost;

        if (upgradeCost <= GameManager.money)
        {
            selectedTower.Upgrade();
            this.ShowPlacedTower(selectedTower);
            GameManager.AddMoney(-upgradeCost);
        }
    }

}
