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
    private int fireRate;

    /// <summary>
    /// Speed of the projectile.
    /// </summary>
    [SerializeField, HideInInspector]
    private float projetileSpeed;

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
    /// Projectile of this tower.
    /// </summary>
    public ProjectileEffect projectileEffect;

    /// <summary>
    /// Setup this tower and activte attackTimer.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    /// <param name="fireRate">Fire rate of this tower.</param>
    /// <param name="projetileSpeed">Speed of the projectile.</param>
    /// <param name="projetileDamage">Damage of the projectile.</param>
    public void Setup(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, int range, int fireRate, float projetileSpeed, float projetileDamage)
    {
        Setup(name, baseCosts, upgradeCosts, upgradeCostsScale, maxLevel, range);
        this.fireRate = fireRate;
        this.projetileSpeed = projetileSpeed;
        this.projectileDamage = projetileDamage;

        attackTimer = new GameTimer();
        attackTimer.SetTimer(this.fireRate);
        attackTimer.SkipTimer();
        attackTimer.SetPaused(false);
    }

    /// <summary>
    /// Update attackTimer and attack if there's a target and it's time to attack.
    /// </summary>
    private void Update()
    {
        attackTimer.Update();
        FindClosestTarget();
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            return;
        }


        if (attackTimer.IsDone())
        {
            Attack();
            attackTimer.Reset();
        }
    }

    public override void Upgrade()
    {
        if (level < maxLevel)
        {
            level += 1;
            range += 1;
            fireRate += 5;
            upgradeCosts *= (int)(level * upgradeCostsScale);
        }
    }

    public override void Attack()
    {
        ProjectileEffect projectileEffect = Instantiate(this.projectileEffect, transform.position, Quaternion.identity, LevelManager.Instance.ProjectilesEffectParent);
        projectileEffect.Setup(projetileSpeed, projectileDamage, target);
    }
}
