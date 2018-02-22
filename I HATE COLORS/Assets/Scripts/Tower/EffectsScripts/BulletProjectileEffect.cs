using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet effect for the bullet projectile tower.
/// Behaves like a norml bullet and only inflict damage to its target.
/// </summary>
public class BulletProjectileEffect : ProjectileEffect
{
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
        BulletProjectileEffect newEffect = Instantiate(prefab, position, Quaternion.identity).GetComponent<BulletProjectileEffect>();
        newEffect.SetTarget(target);
    }
}
