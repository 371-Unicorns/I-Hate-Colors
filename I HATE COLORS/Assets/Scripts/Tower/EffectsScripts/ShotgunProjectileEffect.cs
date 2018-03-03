using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shotgun effect for the shotgun projectile tower.
/// Behaves like a shotgun where many low damage bullets are fired and only inflict damage to their target.
/// </summary>
public class ShotgunProjectileEffect : ProjectileEffect
{

    private static readonly int NUM_SHELLS = 3;
    private static readonly float SHELL_SPREAD = 1.5f;

    /// <summary>
    /// The position of the enemy on instantiation of a new shell
    /// </summary>
    private Vector3 enemyPos;
    private Vector3 spawnPos;

    public Vector3 GetEnemyPosition()
    {
        return enemyPos;
    }

    public void SetEnemyPosition(Vector3 pos)
    {
        enemyPos = pos;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPos;
    }

    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPos = pos;
    }

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

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage, color);
        }
        ProcessCollision();
    }

    public override void ProcessCollision()
    {
        GameObject fx = (GameObject)Instantiate(projectileImpact, transform.position, transform.rotation, LevelManager.ProjectilesEffectParent);
        Destroy(fx, projectileImpact.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

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
