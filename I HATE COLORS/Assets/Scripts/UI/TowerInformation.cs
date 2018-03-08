using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage TowerInformation panel, which displays information about the currently selected tower (either a placed one or a hovering one).
/// </summary>
public class TowerInformation : MonoBehaviour
{
    private static Tower selectedTower;

    private static Image background;
    private static Transform head;
    private static Transform informationPanel;
    private static Text nameText;
    private static Text rangeText;

    private static Transform hoverBody;
    private static Text descriptionText;
    private static Vector2 hoverPanelPosition;

    private static Transform placedBody;
    private static Text levelText;
    private static Text upgradeCostText;
    private static Button upgradeButton;

    public void Start()
    {
        background = GetComponent<Image>();

        hoverPanelPosition = this.gameObject.transform.position;
        informationPanel = this.gameObject.transform;
        head = transform.Find("Head").transform;
        nameText = head.transform.Find("NameText").GetComponent<Text>();
        rangeText = head.transform.Find("RangeText").GetComponent<Text>();

        hoverBody = transform.Find("HoverBody").transform;
        descriptionText = hoverBody.transform.Find("Description").GetComponent<Text>();

        placedBody = transform.Find("PlacedBody").transform;
        levelText = placedBody.transform.Find("LevelText").GetComponent<Text>();
        upgradeCostText = placedBody.transform.Find("UpgradeCostText").GetComponent<Text>();
        upgradeButton = placedBody.transform.Find("UpgradeButton").GetComponent<Button>();

        Reset();
    }


    /// <summary>
    /// Reset panel to represent that no tower is currently selected.
    /// </summary>
    public static void Reset()
    {
        background.enabled = false;
        head.gameObject.SetActive(false);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(false);

        GameManager.ResetTower();
    }

    /// <summary>
    /// Fill the panel with informations about the currently selected hovering tower.
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public static void ShowHoveringTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();
        informationPanel.transform.position = hoverPanelPosition;
        descriptionText.text = selectedTower.Description;
        background.enabled = true;
        head.gameObject.SetActive(true);
        hoverBody.gameObject.SetActive(true);
        placedBody.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fill the panel with informations about the currently selected placed tower.
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public static void ShowPlacedTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();

        informationPanel.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        levelText.text = "Level: " + selectedTower.Level.ToString();
        upgradeCostText.text = "Upgrade: " + selectedTower.UpgradeCosts.ToString();
        CheckUpgrade();

        background.enabled = true;
        head.gameObject.SetActive(true);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(true);
    }

    private static void FillHead()
    {
        nameText.text = TowerInformation.selectedTower.Name;
        rangeText.text = "Range: " + TowerInformation.selectedTower.Range.ToString();
    }

    /// <summary>
    /// Check whether it's possible to upgrade the currently selected tower and if it is, enable the upgrade button.
    /// </summary>
    public static void CheckUpgrade()
    {
        upgradeButton.interactable = selectedTower.UpgradeCosts <= GameManager.money ? true : false;
    }

    /// <summary>
    /// Upgrade the currently selected tower.
    /// </summary>
    public void UpgradeTower()
    {
        selectedTower.Upgrade();
        TowerInformation.ShowPlacedTower(selectedTower);
        GameManager.AddMoney(-selectedTower.UpgradeCosts);
        TowerInformation.CheckUpgrade();
    }

    /// <summary>
    /// Deletes selected tower and returns a subset of money spent on tower
    /// </summary>
    public void SellTower()
    {
        int returnedMoney = selectedTower.BaseCosts / 2;
        GameManager.AddMoney(returnedMoney);
        Destroy(selectedTower.gameObject);
        GridGraphManager.ScanGrid();
        Reset();
    }
}
