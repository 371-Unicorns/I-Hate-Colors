using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet effect for the bullet projectile tower.
/// Behaves like a norml bullet and only inflict damage to its target.
/// </summary>
public class BulletProjectileEffect : ProjectileEffect
{
    /// <summary>
    /// Allows the bullet to move toward the enemy it is targeting.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public override void Update()
    {
        base.Update();

        float step = speed * Time.deltaTime;
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    /// <summary>
    /// If the bullet collides with its target, the target takes damage.
    /// Otherwise, the bullet just explodes on contact.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="other">The object the bullet collides with. </param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage, color);
        }
        ProcessCollision();
    }

    /// <summary>
    /// If the bullet collides with an object (even if that object is not its target), it explodes.
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
        BulletProjectileEffect newEffect = Instantiate(prefab, position, Quaternion.identity).GetComponent<BulletProjectileEffect>();
        newEffect.SetTarget(target);

        return newEffect;
    }
}
