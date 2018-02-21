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
    /// GameObject containing impact FX of this projectile.
    /// </summary>
    [SerializeField]
    protected GameObject projectileImpact;

    public void Initialize(float speed, float damage)
    {
        base.Initialize(damage);

        this.speed = speed;
    }

    /// <summary>
    /// Destroy this projectile if target does not exist anymore. Otherwise, move this projectile towards target.
    /// </summary>
    public override void Update()
    {
        base.Update();

        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
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
