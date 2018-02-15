﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage TowerInformation panel, which displays information about the currently selected tower (either a placed one or a hovering one).
/// </summary>
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

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private TowerInformation() { }

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

    /// <summary>
    /// Fill the panel with informations about the currently selected placed tower.
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public void ShowPlacedTower(Tower tower)
    {
        currentLevel.text = "Current level: " + tower.level.ToString();
        upgradeCost.text = "Upgrade cost: " + tower.upgradeCost.ToString();

        deleteButton.interactable = true;
        upgradeButton.interactable = true;
    }

    /// <summary>
    /// Fill the panel with informations about the currently hovering tower, ready to place.
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public void ShowHoveringTower(Tower tower)
    {
        currentLevel.text = "Current level: " + tower.level.ToString();
        upgradeCost.text = "Base cost: " + tower.GetCost().ToString();

        deleteButton.interactable = false;
        upgradeButton.interactable = false;
    }

    /// <summary>
    /// Reset panel to represent that no tower is currently selected.
    /// </summary>
    public void Reset()
    {
        currentLevel.text = "";
        upgradeCost.text = "";

        deleteButton.interactable = false;
        upgradeButton.interactable = false;
    }

    /// <summary>
    /// Check whether it's possible to upgrade the currently selected tower and if it is, do so.
    /// </summary>
    public void CheckUpgrade()
    {
        Tower selectedTower = GameManager.Instance.SelectedTower;
        int upgradeCost = selectedTower.upgradeCost;

        if (upgradeCost <= GameManager.money)
        {
            selectedTower.Upgrade();
            this.ShowPlacedTower(selectedTower);
            GameManager.AddMoney(-upgradeCost);
        }
    }
}
