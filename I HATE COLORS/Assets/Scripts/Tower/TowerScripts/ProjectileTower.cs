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
    private float fireRate;

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
    /// <param name="fireRate">Fire rate of this tower.</param>
    /// <param name="projectileSpeed">Speed of the projectile.</param>
    /// <param name="projectileSpeed">Damage of the projectile.</param>
    public void Initialize(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, float range, float fireRate, float projectileSpeed, float projectileDamage)
    {
        base.Initialize(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range);
        effectPrefab.GetComponent<ProjectileEffect>().Initialize(projectileSpeed, projectileDamage, range);

        attackTimer = new GameTimer(fireRate);
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
        AudioSource upgradeSound = GetComponent<AudioSource>();
        /* TODO: Make these values NOT hardcoded.
        if (level < maxLevel)
        {
            level += 1;
            range += 1;
            fireRate += 5;
            upgradeCosts *= (int)(level * upgradeCostsScale);
            upgradeSound.Play();
        }
        */
    }

    public override void Attack()
    {
        effectPrefab.GetComponent<ProjectileEffect>().SpawnEffect(effectPrefab, transform.position, target);
    }
}