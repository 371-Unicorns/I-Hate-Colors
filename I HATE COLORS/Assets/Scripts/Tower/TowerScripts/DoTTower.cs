using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTTower : Tower
{
    /// <summary>
    /// Damage of the DoT effect.
    /// </summary>
    [SerializeField, HideInInspector]
    private float doTDamage;

    /// <summary>
    /// Color of this towers effect.
    /// </summary>
    [SerializeField, HideInInspector]
    private ColorType color;

    private DoTEffect spawnedEffect;

    /// <summary>
    /// Initialize this tower.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    /// <param name="description">Short description of this tower.</param>
    /// <param name="doTDamage">Damage of the DoT effect.</param>
    /// <param name="color">Color of this towers effect.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, float range, string description, float doTDamage, ColorType color)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, description);
        this.doTDamage = doTDamage;
        this.color = color;

        effectPrefab.GetComponent<DoTEffect>().Initialize(doTDamage, range, color);
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
