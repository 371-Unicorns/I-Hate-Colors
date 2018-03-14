using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All buttons that are used in the upgrades menu.
/// </summary>
public class UpgradesMenuButton : MonoBehaviour
{
    /// <summary>
    /// Upgrades menu.
    /// </summary>
    private GameObject upgradesMenu;

    /// <summary>
    /// When the game is started this menu is populated with buttons.
    /// </summary>
    private void Start()
    {
        upgradesMenu = GameObject.Find("UpgradesMenu");
    }

    /// <summary>
    /// Used when a button in the menu is toggled.
    /// </summary>
    public void ToggleUpgradesMenu()
    {
        upgradesMenu.SetActive(!upgradesMenu.activeInHierarchy);
    }
}
