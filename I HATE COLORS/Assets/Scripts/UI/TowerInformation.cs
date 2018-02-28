using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO doc

/// <summary>
/// Manage TowerInformation panel, which displays information about the currently selected tower (either a placed one or a hovering one).
/// </summary>
public class TowerInformation : Singleton<TowerInformation>
{
    private Tower selectedTower;

    private Image background;
    private Transform head;
    private Text nameText;
    private Text rangeText;

    private Transform hoverBody;
    private Text descriptionText;

    private Transform placedBody;
    private Text levelText;
    private Text upgradeCostText;
    private Button sellButton;
    private Button upgradeButton;

    void Start()
    {
        background = GetComponent<Image>();

        head = transform.Find("Head").transform;
        nameText = head.transform.Find("NameText").GetComponent<Text>();
        rangeText = head.transform.Find("RangeText").GetComponent<Text>();

        hoverBody = transform.Find("HoverBody").transform;
        descriptionText = hoverBody.transform.Find("Description").GetComponent<Text>();

        placedBody = transform.Find("PlacedBody").transform;
        levelText = placedBody.transform.Find("LevelText").GetComponent<Text>();
        upgradeCostText = placedBody.transform.Find("UpgradeCostText").GetComponent<Text>();
        sellButton = placedBody.transform.Find("SellButton").GetComponent<Button>();
        upgradeButton = placedBody.transform.Find("UpgradeButton").GetComponent<Button>();

        Reset();
    }


    /// <summary>
    /// Reset panel to represent that no tower is currently selected.
    /// </summary>
    public void Reset()
    {
        background.enabled = false;
        head.gameObject.SetActive(false);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(false);

        GameManager.Instance.ResetTower();
    }

    /// <summary>
    /// Fill the panel with informations about the currently selected placed tower.
    /// </summary>
    /// <param name="tower">Tower to show.</param>
    public void ShowHoveringTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();

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
    public void ShowPlacedTower(Tower tower)
    {
        selectedTower = tower;
        FillHead();

        levelText.text = "Level: " + selectedTower.Level.ToString();
        upgradeCostText.text = "Upgrade: " + selectedTower.UpgradeCosts.ToString();
        CheckUpgrade();

        background.enabled = true;
        head.gameObject.SetActive(true);
        hoverBody.gameObject.SetActive(false);
        placedBody.gameObject.SetActive(true);
    }

    private void FillHead()
    {
        nameText.text = selectedTower.Name;
        rangeText.text = "Range: " + selectedTower.Range.ToString();
    }

    /// <summary>
    /// Check whether it's possible to upgrade the currently selected tower and if it is, do so.
    /// </summary>
    public void UpgradeTower()
    {
        selectedTower.Upgrade();
        this.ShowPlacedTower(selectedTower);
        GameManager.AddMoney(-selectedTower.UpgradeCosts);
    }

    /// <summary>
    /// Check whether it's possible to upgrade the currently selected tower and if it is, do so.
    /// </summary>
    public void CheckUpgrade()
    {
        upgradeButton.interactable = selectedTower.UpgradeCosts <= GameManager.money ? true : false;
    }

    /// <summary>
    /// Deletes selected tower and returns a subset of money spent on tower
    /// </summary>
    public void SellTower()
    {
        int returnedMoney = selectedTower.BaseCosts / 2;
        GameManager.AddMoney(returnedMoney);
        Destroy(selectedTower.gameObject);
        Reset();
    }
}
