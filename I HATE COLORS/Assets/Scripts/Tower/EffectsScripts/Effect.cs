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
    /// </summary>
    [SerializeField, HideInInspector]
    protected float damage;

    /// <summary>
    /// Color of the effect's damage.
    /// </summary>
    [SerializeField, HideInInspector]
    public ColorType color;

    /// <summary>
    /// How far the effect can travel/grow before it despawns.
    /// </summary>
    [SerializeField, HideInInspector]
    protected float range;

    /// <summary>
    /// Target of this effect.
    /// </summary>
    protected Enemy target;

    public void Initialize(float damage, float range, ColorType color)
    {
        this.damage = damage;
        this.range = range;
        this.color = color;
    }

    public virtual void Update()
    {
        if (target == null || target.isDead())
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    public abstract Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target);
}
