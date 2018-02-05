using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Tower, Upgradeable
{

    public void Reset()
    {
        range = 5f;
        fireRate = 1f;
        countdownToFire = 1f;
    }

    public void LevelUp()
    {
        fireRate = fireRate + 1;
        level = level + 1;
    }

    public void Upgrade()
    {
        if (level <= 5)
        {
            LevelUp();
            level += 1;
            upgradeCost = baseUpgradeCost * ((int)(level * upgradeCostScale));
        }

    }



}
