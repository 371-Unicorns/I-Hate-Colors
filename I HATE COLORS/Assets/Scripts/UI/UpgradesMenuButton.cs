using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesMenuButton : MonoBehaviour {
    
    /// <summary>
    /// Upgrades menu
    /// </summary>
    private GameObject upgradesMenu;

    private void Start()
    {
        upgradesMenu = GameObject.Find("UpgradesMenu");
        upgradesMenu.SetActive(false);
    }

    public void ToggleUpgradesMenu()
    {
        upgradesMenu.SetActive(!upgradesMenu.activeInHierarchy);
    }
}
