using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manage TowerInformation panel, which displays information about the currently selected tower (either a placed one or a hovering one).
/// </summary>
public class TowerInformation : MonoBehaviour
{
    /// <summary>
    /// Currently selected tower.
    /// </summary>
    private static Tower selectedTower;

    /// <summary>
    /// Background image of game.
    /// 
    /// Drawn by Amy Lewis.
    /// </summary>
    private static Image background;

    /// <summary>
    /// Header located next to every tower in side panel of UI.
    /// </summary>
    private static Transform head;

    /// <summary>
    /// Information panel is readable if player hovers over towers in panel.
    /// </summary>
    private static Transform informationPanel;

    /// <summary>
    /// Name of tower displayed in information panel.
    /// </summary>
    private static Text nameText;

    /// <summary>
    /// Range of tower displayed in information panel.
    /// </summary>
    private static Text rangeText;

    /// <summary>
    /// Panel that isreadable if player hovers over towers in panel.
    /// </summary>
    private static Transform hoverBody;

    /// <summary>
    /// Description of tower displayed in information panel.
    /// </summary>
    private static Text descriptionText;

    /// <summary>
    /// Position of panel must be the same Y as the tower it corresponds with.
    /// </summary>
    private static Vector2 hoverPanelPosition;

    private static Transform placedBody;

    /// <summary>
    /// Which level the tower is.
    /// </summary>
    private static Text levelText;

    /// <summary>
    /// How much it costs to upgrade the tower.
    /// </summary>
    private static TextMeshProUGUI upgradeCostText;

    /// <summary>
    /// Button player can click to upgrade tower.
    /// </summary>
    private static Button upgradeButton;

    /// <summary>
    /// Whether TowerInformation is currently in use.
    /// </summary>
    public static bool isActive;

    /// <summary>
    /// Instantiates the tower information.
    /// Author: Steven Johnson
    /// </summary>
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
        upgradeCostText = placedBody.transform.Find("UpgradeCostText").GetComponent<TextMeshProUGUI>();
        upgradeButton = placedBody.transform.Find("UpgradeButton").GetComponent<Button>();

        Reset();
    }


    /// <summary>
    /// Reset panel to represent that no tower is currently selected.
    /// Author: Steven Johnson
    /// </summary>
    public static void Reset()
    {
        background.enabled = false;
        head.gameObject.SetActive(false);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(false);
        isActive = false;

        GameManager.ResetTower();
    }

    /// <summary>
    /// Fill the panel with informations about the currently selected hovering tower.
    /// Author: Steven Johnson
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
        isActive = true;
    }

    /// <summary>
    /// Fill the panel with informations about the currently selected placed tower.
    /// Author: Steven Johnson
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public static void ShowPlacedTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();

        informationPanel.transform.position = Camera.main.WorldToScreenPoint(tower.Tile.transform.position);
        levelText.text = "Level: " + selectedTower.Level.ToString();
        upgradeCostText.text = tower.Level < tower.MaxLevel ? "Upgrade: " + selectedTower.UpgradeCosts.ToString() + " <sprite=1>" : "Max Level";
        CheckUpgrade();

        background.enabled = true;
        head.gameObject.SetActive(true);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(true);
        isActive = true;
    }

    /// <summary>
    /// Fills the head with information from the XML.
    /// </summary>
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
        upgradeButton.interactable = selectedTower.UpgradeCosts <= GameManager.money && selectedTower.Level < selectedTower.MaxLevel ? true : false;
    }

    /// <summary>
    /// Upgrade the currently selected tower.
    /// 
    /// Edited by Courtney Chu
    /// Author: Steven Johnson
    /// </summary>
    public void UpgradeTower()
    {
        GameManager.AddMoney(-selectedTower.UpgradeCosts);
        if (!GameManager.didUpgradeFirstTower)
        {
            GameManager.didUpgradeFirstTower = true;
            StartCoroutine(GameManager.DisplayRewardsPanel());
        }

        if (GameManager.CheckForFirstUpgrade())
        {
            StartCoroutine(GameManager.DisplayRewardsPanel());
        }

        selectedTower.Upgrade();
        TowerInformation.ShowPlacedTower(selectedTower);
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
