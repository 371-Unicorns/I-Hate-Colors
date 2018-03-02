﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlackHoleAoEEffect : AoEEffect {

    private Sprite sprite;

    public BlackHoleAoEEffect()
    {
        radius = 5;
    }

    public override void ApplyAoEEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, LayerMask.NameToLayer("Enemies"));

        foreach (Collider2D c in targets)
        {
            GameObject target = c.gameObject;
            target.transform.position = Vector2.Lerp(target.transform.position, transform.position, 1.5f * Time.deltaTime);
        }
    }

    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        GameObject newEffect = Instantiate(prefab, position, Quaternion.identity);
        BlackHoleAoEEffect effect = newEffect.GetComponent<BlackHoleAoEEffect>();
        effect.SetTarget(target);
        effect.SetSprite(newEffect.GetComponent<SpriteRenderer>().sprite);
        transform.localScale = new Vector3(radius, radius, 0);
        radius = 5;

        return effect;
    }

    private void SetSprite(Sprite s) { sprite = s; }
}
