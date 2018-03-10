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
    [SerializeField, HideInInspector]
    private float doTDamageScale;

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
    /// <param name="color">Color of this towers effect.</param>
    /// <param name="description">Short description of this tower.</param>
    /// <param name="doTDamage">Damage of the DoT effect.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, float upgradeCostsScale, int maxLevel, float range, ColorType color, string description, float doTDamage)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, color, description);
        this.doTDamage = doTDamage;
        this.doTDamageScale = doTDamage;

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

        // Get doTDamage with (0.34x)^2+1, where x is the current tower level. Then scale back with doTDamageScale.
        doTDamage = (Mathf.Pow(0.34f * (float)level, 2.0f) + 1.0f) * doTDamageScale;
        effectPrefab.GetComponent<DoTEffect>().Initialize(doTDamage, range, color);
    }
}
