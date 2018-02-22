using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for all tower effects.
/// </summary>
public abstract class Effect : MonoBehaviour
{
    /// <summary>
    /// Amount of damage the effet deals.
    /// </summary>
    [SerializeField, HideInInspector]
    protected float damage;

    /// <summary>
    /// Target of this effect.
    /// </summary>
    protected Enemy target;

    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    public virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetTarget(Enemy e)
    {
        target = e;
    }

    public abstract void SpawnEffect(GameObject prefab, Vector3 position, Enemy target);
}
