using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shotgun effect for the shotgun projectile tower.
/// Behaves like a shotgun where many low damage bullets are fired and only inflict damage to their target.
/// </summary>
public class ShotgunProjectileEffect : ProjectileEffect
{

    private static readonly int numShells = 3;
    private Vector3 enemyPos;

    public Vector3 GetEnemyPosition()
    {
        return enemyPos;
    }

    public void SetEnemyPosition(Vector3 pos)
    {
        enemyPos = pos;
    }

    public override void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, step);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage);
        }
        ProcessCollision();
    }

    public override void ProcessCollision()
    {
        GameObject fx = (GameObject)Instantiate(projectileImpact, transform.position, transform.rotation, LevelManager.Instance.ProjectilesEffectParent);
        Destroy(fx, projectileImpact.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

    public override void SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        for (int i = 0; i < numShells; i++)
        {
            ShotgunProjectileEffect shell = Instantiate(prefab, position, Quaternion.identity).GetComponent<ShotgunProjectileEffect>();
            shell.SetTarget(target);
            shell.SetEnemyPosition(target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 1));
        }
    }
}
