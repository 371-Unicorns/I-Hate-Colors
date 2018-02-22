using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoTEffect : DoTEffect {

    public override void ApplyDoTEffect()
    {
        transform.LookAt(target.transform);
        if (target != null)
        {
            target.TakeDamage(damage);
            SpawnEffect(dotEffectImpact, this.transform.position, target);
        }
    }

    public override void SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        DoTEffect effect = (DoTEffect) Instantiate(prefab.gameObject.GetComponent<Effect>(), target.transform, LevelManager.Instance.DoTEffectParent);
        Destroy(effect, dotEffectImpact.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}
