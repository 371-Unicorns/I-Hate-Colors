using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shotgun effect for the shotgun projectile tower.
/// Behaves like a shotgun where many low damage bullets are fired and only inflict damage to their target.
/// </summary>
public class ShotgunProjectileEffect : ProjectileEffect
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
}
