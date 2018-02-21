using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : Tower
{
    public AoEEffect aoeEffect;

    private GameTimer shotTimer;

    public void Initialize(float fireRate)
    {
        shotTimer = new GameTimer(fireRate);
        shotTimer.SkipTimer();
        shotTimer.SetPaused(false);
    }

    public override void Update()
    {
        base.Update();

        shotTimer.Update();
        if (shotTimer.IsDone() && target != null)
        {
            Attack();
            shotTimer.Reset();
        }
    }

    public override void Attack()
    {
        aoeEffect.ApplyAoEEffect();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
