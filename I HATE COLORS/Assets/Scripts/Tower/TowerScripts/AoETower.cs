using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : Tower
{
    private bool shouldSpawnEffect = true;
   
    private AoEEffect aoeEffect;

    /// <summary>
    /// How long the area is effected.
    /// </summary>
    [SerializeField, HideInInspector]
    private GameTimer shotTimer;

    public void Initialize(string id, int baseCost, float damage, float fireRate, int upgradeCost, float upgradeCostScale, int maxLevel, float range, string description)
    {
        base.Initialize(id, baseCost, upgradeCost, upgradeCostScale, maxLevel, range, description);
        effectPrefab.GetComponent<AoEEffect>().Initialize(damage, range);

        shotTimer = new GameTimer(fireRate);
        shotTimer.SkipTimer();
        shotTimer.SetPaused(false);
    }

    public override void Update()
    {
        base.Update();

        if (aoeEffect == null)
        {
            aoeEffect = effectPrefab.GetComponent<AoEEffect>().SpawnEffect(effectPrefab, transform.position, target) as AoEEffect;
        }

        if (aoeEffect.IsField)
        {
            Attack();
        }
        else
        {
            shotTimer.Update();
            if (shotTimer.IsDone() && target != null)
            {
                Attack();
                shotTimer.Reset();
            }
        }
    }

    public override void Attack()
    {
        if (aoeEffect == null)
        {
            aoeEffect = effectPrefab.GetComponent<AoEEffect>().SpawnEffect(effectPrefab, transform.position, target) as AoEEffect;
        }
        
        aoeEffect.ApplyAoEEffect();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
