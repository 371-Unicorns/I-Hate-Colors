using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : Tower
{
    /// <summary>
    /// Fire rate of this tower.
    /// </summary>
    [SerializeField, HideInInspector]
    private float attackRate;

    /// <summary>
    /// Damage of this towers AoE effect.
    /// </summary>
    [SerializeField, HideInInspector]
    private float aoEDamage;

    /// <summary>
    /// Timer controlling when to attack.
    /// </summary>
    [SerializeField, HideInInspector]
    private GameTimer attackTimer;

    private bool shouldSpawnEffect = true;

    private AoEEffect aoeEffect;

    /// <summary>
    /// Initialize this tower and activte durationTimer.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    /// <param name="description">Short description of this tower.</param>
    /// <param name="attackRate">Fire rate of this tower.</param>
    /// <param name="aoEDamage">Damage of this towers AoE effect.</param>
    public void Initialize(string name, int baseCost, int upgradeCost, float upgradeCostScale, int maxLevel, float range, string description, float attackRate, float aoEDamage)
    {
        base.Initialize(name, baseCost, upgradeCost, upgradeCostScale, maxLevel, range, description);
        this.attackRate = attackRate;
        this.aoEDamage = aoEDamage;

        effectPrefab.GetComponent<AoEEffect>().Initialize(aoEDamage, range, ColorType.BLACK);

        attackTimer = new GameTimer(attackRate);
        attackTimer.SkipTimer();
        attackTimer.SetPaused(false);
    }

    public override void Update()
    {
        base.Update();

        attackTimer.Update();

        if (target != null)
        {
            if (attackTimer.IsDone())
            {
                aoeEffect = effectPrefab.GetComponent<AoEEffect>().SpawnEffect(effectPrefab, transform.position, target) as AoEEffect;
                attackTimer.Reset();
            }

            Attack();
        }
    }

    public override void Attack()
    {
        if (aoeEffect != null)
        {
            aoeEffect.ApplyAoEEffect();
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}
