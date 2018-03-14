using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shotgun effect for the shotgun projectile tower.
/// Behaves like a shotgun where many low damage bullets are fired and only inflict damage to their target.
/// </summary>
public class ShotgunProjectileEffect : ProjectileEffect
{
    /// <summary>
    /// How many projetiles are fired at once from the tower.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private static readonly int NUM_SHELLS = 3;

    /// <summary>
    /// How spread out the projectiles are when fired.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private static readonly float SHELL_SPREAD = 1.5f;

    /// <summary>
    /// The position of the enemy on instantiation of a new shell.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private Vector3 enemyPos;

    /// <summary>
    /// The position of the projectiles when they are spawn.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private Vector3 spawnPos;

    /// <summary>
    /// The position of the enemy when they are targeted.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public Vector3 GetEnemyPosition()
    {
        return enemyPos;
    }

    /// <summary>
    /// Resets the position of the enemy.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="pos">Sets position of the enemy.</param>
    public void SetEnemyPosition(Vector3 pos)
    {
        enemyPos = pos;
    }

    /// <summary>
    /// Returns the spawn position.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public Vector3 GetSpawnPosition()
    {
        return spawnPos;
    }

    /// <summary>
    /// Resets the spawn position.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="pos">Sets position of the spawn point.</param>
    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPos = pos;
    }

    /// <summary>
    /// Moves the projectiles toward their target.
    /// If they go out of range or collide with their target they are destroyed.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public override void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, this.GetEnemyPosition(), step);
        
        if (Vector3.Distance(transform.position, this.GetSpawnPosition()) >= this.range
            || Vector3.Distance(transform.position, this.GetEnemyPosition()) < 0.2f)
        {
            Destroy(gameObject);
        } 
    }

    /// <summary>
    /// If a projectile collides with its target, the target takes damage.
    /// Otherwise, the projectile just explodes on contact.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="other">The object the projectile collides with. </param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage, color);
        }
        ProcessCollision();
    }

    /// <summary>
    /// If a projectile collides with an object (even if that object is not its target), it explodes.
    /// The explosion is a particle system.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public override void ProcessCollision()
    {
        GameObject fx = (GameObject)Instantiate(projectileImpact, transform.position, transform.rotation, LevelManager.ProjectilesEffectParent);
        Destroy(fx, projectileImpact.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

    /// <summary>
    /// Applies the effect in the game.
    /// Sends a bullet projectile after the targeted enemy.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="damage">Amount of this effect does to an enemy.</param>
    /// <param name="range">How far the effect can reach.</param>
    /// <param name="color">Color enemy this effect does more damage to.</param>
    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        ShotgunProjectileEffect shell = null;

        for (int i = 0; i < NUM_SHELLS; i++)
        {
            shell = Instantiate(prefab, position, Quaternion.identity).GetComponent<ShotgunProjectileEffect>();
            shell.SetSpawnPosition(position);
            shell.SetTarget(target);
            shell.SetEnemyPosition(target.transform.position + new Vector3(Random.Range(-SHELL_SPREAD, SHELL_SPREAD), Random.Range(-SHELL_SPREAD, SHELL_SPREAD), 0));
        }

        return shell;
    }
}
