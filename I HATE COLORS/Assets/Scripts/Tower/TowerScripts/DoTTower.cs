using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTTower : Tower
{

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
        DoTEffect dotEffect = (DoTEffect)Instantiate(effectPrefab.gameObject.GetComponent<Effect>(), transform.position, Quaternion.identity, LevelManager.Instance.DoTEffectParent);
        dotEffect.SetTarget(target);
        dotEffect.ApplyDoTEffect();
    }

    public override void Upgrade()
    {
        AudioSource upgradeSound = GetComponent<AudioSource>();
        upgradeSound.Play();
    }
}
