using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower, which shoots single projectiles targeting one enemy.
/// </summary>
public class ProjectileTower : Tower
{
    /// <summary>
    /// Fire rate of this tower.
    /// </summary>
    [SerializeField, HideInInspector]
    private float attackRate;
    [SerializeField, HideInInspector]
    private float attackRateScale;

    /// <summary>
    /// Speed of the projectile.
    /// </summary>
    [SerializeField, HideInInspector]
    private float projectileSpeed;

    /// <summary>
    /// Damage of the projectile.
    /// </summary>
    [SerializeField, HideInInspector]
    private float projectileDamage;
    [SerializeField, HideInInspector]
    private float projectileDamageScale;

    /// <summary>
    /// Timer controlling when to attack.
    /// </summary>
    [SerializeField, HideInInspector]
    private GameTimer attackTimer;

    /// <summary>
    /// Initialize this tower and activte attackTimer.
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
    /// <param name="projectileSpeed">Speed of the projectile.</param>
    /// <param name="projectileDamage">Damage of the projectile.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, float upgradeCostsScale, int maxLevel, float range, ColorType color, string description, float attackRate, float projectileSpeed, float projectileDamage)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, color, description);
        this.attackRate = attackRate;
        this.attackRateScale = attackRate;
        this.projectileSpeed = projectileSpeed;
        this.projectileDamage = projectileDamage;
        this.projectileDamageScale = projectileDamage;

        effectPrefab.GetComponent<ProjectileEffect>().Initialize(projectileSpeed, projectileDamage, range, color);

        attackTimer = new GameTimer(attackRate);
        attackTimer.SkipTimer();
        attackTimer.SetPaused(false);
    }

    /// <summary>
    /// Update attackTimer and attack if there's a target and it's time to attack.
    /// </summary>
    public override void Update()
    {
        base.Update();

        attackTimer.Update();
        if (target != null && attackTimer.IsDone())
        {
            Attack();
            attackTimer.Reset();
        }
    }

    public override void Upgrade()
    {
        if (level < maxLevel)
        {
            base.Upgrade();

            // Get projectileDamage with (0.34x)^2+1, where x is the current tower level. Then scale back with projectileDamageScale.
            projectileDamage = (Mathf.Pow(0.34f * (float)level, 2.0f) + 1.0f) * projectileDamageScale;
            effectPrefab.GetComponent<ProjectileEffect>().Initialize(projectileSpeed, projectileDamage, range, color);

            // Get attackRate with ln(8-x)-0.9, where x is the current tower level. Then scale back with attackRateScale.
            attackRate = (Mathf.Log(8 - level) - 0.9f) * attackRateScale;
            attackTimer = new GameTimer(attackRate);
            attackTimer.SkipTimer();
            attackTimer.SetPaused(false);
        }
    }

    public override void Attack()
    {
        effectPrefab.GetComponent<ProjectileEffect>().SpawnEffect(effectPrefab, transform.position, target);
    }
}