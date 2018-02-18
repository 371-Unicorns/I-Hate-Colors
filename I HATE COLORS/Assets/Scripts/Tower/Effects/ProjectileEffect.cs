using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for all projectile tower effects.
/// </summary>
public abstract class ProjectileEffect : Effect, ICollidable
{
    /// <summary>
    /// Speed of this projectile.
    /// </summary>
    protected float speed;

    /// <summary>
    /// Damage this projectile inflicts.
    /// </summary>
    protected float damage;

    /// <summary>
    /// GameObject containing impact FX of this projectile.
    /// </summary>
    [SerializeField]
    protected GameObject projectileImpact;

    /// <summary>
    /// Setup this projectile and activate it.
    /// </summary>
    /// <param name="speed">Speed of this projectile.</param>
    /// <param name="damage">Damage this projectile inflicts.</param>
    /// <param name="target">Target of this effect.</param>
    public void Setup(float speed, float damage, Enemy target)
    {
        this.speed = speed;
        this.damage = damage;
        this.target = target;
        this.active = true;
    }

    /// <summary>
    /// If active, destroy this projectile if target does not exist anymore. If it still exists, move this projectile towards target.
    /// </summary>
    private void Update()
    {
        if (active)
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    /// <summary>
    /// Collision with another collider. Could be a enemy or a scenery element.
    /// Expected behaviour is to check whether other is equal to target, activate this projectiles effect. 
    /// Call ProcessCollision(...) everytime, since this projectile could for example collide with a scenery object.
    /// </summary>
    /// <param name="other"></param>
    protected abstract void OnTriggerEnter2D(Collider2D other);

    /// <summary>
    /// A collision with this projectile ocurs. Better call this from within OnTriggerEnter2D(...)!
    /// Expected behaviour is to play the impact FX and then destroy it and this projectile.
    /// </summary>
    public abstract void ProcessCollision();
}
