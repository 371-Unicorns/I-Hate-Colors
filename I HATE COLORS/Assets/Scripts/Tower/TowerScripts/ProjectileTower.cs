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

    /// <summary>
    /// Color of this towers projectile.
    /// </summary>
    [SerializeField, HideInInspector]
    private ColorType color;

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
    /// <param name="description">Short description of this tower.</param>
    /// <param name="attackRate">Fire rate of this tower.</param>
    /// <param name="projectileSpeed">Speed of the projectile.</param>
    /// <param name="projectileDamage">Damage of the projectile.</param>
    /// <param name="color">Color of this towers projectile.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, float range, string description, float attackRate, float projectileSpeed, float projectileDamage, ColorType color)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range, description);
        this.attackRate = attackRate;
        this.projectileSpeed = projectileSpeed;
        this.projectileDamage = projectileDamage;
        this.color = color;

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
            projectileDamage += (0.2f * projectileDamage);
            projectileSpeed += (0.1f * projectileSpeed);
            attackRate -= (0.2f * attackRate);
            print(attackRate);
        }
    }

    public override void Attack()
    {
        effectPrefab.GetComponent<ProjectileEffect>().SpawnEffect(effectPrefab, transform.position, target);
    }
}