using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTTower : Tower
{

    private DoTEffect spawnedEffect;

    /// <summary>
    /// Initialize this tower and activte attackTimer.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    /// <param name="effectDamage">Damage of the projectile.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, float range, float effectDamage, ColorType color, string description)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, description);
        effectPrefab.GetComponent<DoTEffect>().Initialize(effectDamage, range, color);
    }

    public override void Update()
    {
        base.Update();

        if (target != null)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if (spawnedEffect == null)
        {
            spawnedEffect = effectPrefab.GetComponent<DoTEffect>().SpawnEffect(effectPrefab, transform.position, target) as DoTEffect;
        }

        spawnedEffect.ApplyDoTEffect();
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}
