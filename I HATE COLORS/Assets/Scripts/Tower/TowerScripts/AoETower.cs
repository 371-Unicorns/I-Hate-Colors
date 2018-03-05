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
        effectPrefab.GetComponent<AoEEffect>().Initialize(damage, range, ColorType.BLACK);

        shotTimer = new GameTimer(fireRate);
        shotTimer.SkipTimer();
        shotTimer.SetPaused(false);
    }

    public override void Update()
    {
        base.Update();

        shotTimer.Update();

        if (target != null)
        {
            if (shotTimer.IsDone())
            {
                aoeEffect = effectPrefab.GetComponent<AoEEffect>().SpawnEffect(effectPrefab, transform.position, target) as AoEEffect;
                shotTimer.Reset();
            }

            Attack();
        }
    }

    public override void Attack()
    {
        if (aoeEffect != null)
        {
            aoeEffect.ApplyAoEEffect();
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}
