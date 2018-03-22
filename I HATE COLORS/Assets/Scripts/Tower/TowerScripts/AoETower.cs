using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of Area of Effect towers.
/// 
/// Author: Cole Twitchell
/// Edited by others as we've added more to AoE Towers
/// </summary>
public class AoETower : Tower
{
    /// <summary>
    /// Fire rate of this tower.
    /// </summary>
    [SerializeField, HideInInspector]
    private float attackRate;
    [SerializeField, HideInInspector]
    private float attackRateScale;

    /// <summary>
    /// Damage of this towers AoE effect.
    /// </summary>
    [SerializeField, HideInInspector]
    private float aoEDamage;
    [SerializeField, HideInInspector]
    private float aoEDamageScale;

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
    /// <param name="color">Color of this towers effect.</param>
    /// <param name="description">Short description of this tower.</param>
    /// <param name="attackRate">Fire rate of this tower.</param>
    /// <param name="aoEDamage">Damage of this towers AoE effect.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, float upgradeCostsScale, int maxLevel, float range, ColorType color, string description, float attackRate, float aoEDamage)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, color, description);
        this.attackRate = attackRate;
        this.attackRateScale = attackRate;
        this.aoEDamage = aoEDamage;
        this.aoEDamageScale = aoEDamage;

        effectPrefab.GetComponent<AoEEffect>().Initialize(aoEDamage, range, color);

        attackTimer = new GameTimer(attackRate);
        attackTimer.SkipTimer();
        attackTimer.SetPaused(false);
    }

    /// <summary>
    /// Updates the AoE effect so that it only affects enemies for the time it is set.
    /// </summary>
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

    /// <summary>
    /// Applies the AoE effect to the enemeies.
    /// </summary>
    public override void Attack()
    {
        if (aoeEffect != null)
        {
            aoeEffect.ApplyAoEEffect();
        }
    }

    /// <summary>
    /// If the player upgrades the tower, the towers abilities are scaled up.
    /// </summary>
    public override void Upgrade()
    {
        if (level < maxLevel)
        {
            base.Upgrade();

            // Get aoEDamage with (0.34x)^2+1, where x is the current tower level. Then scale back with aoEDamageScale.
            aoEDamage = (Mathf.Pow(0.34f * (float)level, 2.0f) + 1.0f) * aoEDamageScale;
            effectPrefab.GetComponent<AoEEffect>().Initialize(aoEDamage, range, color);

            // Get attackRate with ln(8-x)-0.9, where x is the current tower level. Then scale back with attackRateScale.
            attackRate = (Mathf.Log(8 - level) - 0.9f) * attackRateScale;
            attackTimer = new GameTimer(attackRate);
            attackTimer.SkipTimer();
            attackTimer.SetPaused(false);
        }
    }
}
