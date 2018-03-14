using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for all tower effects.
/// </summary>
public abstract class Effect : MonoBehaviour
{
    /// <summary>
    /// Amount of damage the effect deals.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    [SerializeField, HideInInspector]
    protected float damage;

    /// <summary>
    /// Color of the effect's damage.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    [SerializeField, HideInInspector]
    public ColorType color;

    /// <summary>
    /// How far the effect can travel/grow before it despawns.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    [SerializeField, HideInInspector]
    protected float range;

    /// <summary>
    /// Target of this effect.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    protected Enemy target;

    /// <summary>
    /// Initializes all values for this effect.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    /// <param name="damage">Amount of this effect does to an enemy.</param>
    /// <param name="range">How far the effect can reach.</param>
    /// <param name="color">Color enemy this effect does more damage to.</param>
    public void Initialize(float damage, float range, ColorType color)
    {
        this.damage = damage;
        this.range = range;
        this.color = color;
    }

    /// <summary>
    /// Destroys the effect if the enemy it was targeting is null or dead.
    /// 
    /// Authors: Cole Twitchell, Amy Lewis
    /// </summary>
    public virtual void Update()
    {
        if (target == null || target.isDead())
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Sets the enemy the effect is targeting.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    /// <summary>
    /// Creates the specific effect fired its tower.
    /// 
    /// Authors: Cole Twitchell
    /// </summary>
    /// <param name="prefab">Prefab of sprite for this effect.</param>
    /// <param name="position">Where the effect is drawn.</param>
    /// <param name="target">Enemy the effect is targeting.</param>
    public abstract Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target);
}
