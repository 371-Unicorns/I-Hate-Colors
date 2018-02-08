using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Tower
{

    public void Reset()
    {
        range = 5f;
        fireRate = 1f;
        countdownToFire = 1f;
        baseUpgradeCost = 20;
    }

}
