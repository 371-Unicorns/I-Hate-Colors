using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoTEffect : DoTEffect
{

    private Sprite sprite;
    private Vector3 towerPosition;

    public override void ApplyDoTEffect()
    {
        if (target != null)
        {
            transform.position = (target.transform.position + towerPosition) / 2.0f;

            Vector3 targetDir = target.transform.position - towerPosition;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.localScale = new Vector3((target.transform.position - towerPosition).magnitude / sprite.bounds.size.x, 1, 1);

            target.TakeDamage(damage * Time.deltaTime, color);
        }
    }

    public override void Update()
    {
        base.Update();

        if (target == null || (target.transform.position - towerPosition).magnitude > range)
        {
            Destroy(gameObject);
        }
    }

    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        GameObject newEffect = Instantiate(prefab, position, Quaternion.identity);
        LaserDoTEffect effect = newEffect.GetComponent<LaserDoTEffect>();
        effect.SetTarget(target);
        effect.SetTowerPosition(position);
        effect.SetSprite(newEffect.GetComponent<SpriteRenderer>().sprite);

        return effect;
    }

    private void SetSprite(Sprite s) { sprite = s; }

    private void SetTowerPosition(Vector3 position)
    {
        towerPosition = position;
    }
}
