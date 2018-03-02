using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet effect for the bullet projectile tower.
/// Behaves like a norml bullet and only inflict damage to its target.
/// </summary>
public class BulletProjectileEffect : ProjectileEffect
{

    public override void Update()
    {
        base.Update();

        float step = speed * Time.deltaTime;
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
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
        BulletProjectileEffect newEffect = Instantiate(prefab, position, Quaternion.identity).GetComponent<BulletProjectileEffect>();
        newEffect.SetTarget(target);

        return newEffect;
    }
}
