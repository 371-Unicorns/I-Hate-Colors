using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTTower : Tower
{

    public GameObject dotEffectPrefab;

    public override void Update()
    {
        base.Update();

        if (target != null)
        {
            Attack();
        }
    }

    public override void Attack()
    {

    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    private void ApplyDoTEffect()
    {

    }
}
