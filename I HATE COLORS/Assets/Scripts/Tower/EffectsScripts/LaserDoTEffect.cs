using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Laser effect for the laser damage over time tower.
/// Does damage to enemies as long as the laser is targeting them.
/// </summary>
public class LaserDoTEffect : DoTEffect
{
    /// <summary>
    /// Laser effect sprite.
    /// </summary>
    private Sprite sprite;

    /// <summary>
    /// Position of the laser tower the effect is spawn from.
    /// </summary>
    private Vector3 towerPosition;

    /// <summary>
    /// Applies the effect in the game.
    /// Laser does damage to the target over time. 
    /// </summary>
    /// Author: Cole Twitchell
    /// Edited by Courtney Chu
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

    /// <summary>
    /// Update function that creates the laser. 
    /// If the laser fires out of range or kills its target it is destroyed.
    /// </summary>
    /// Edited by Courtney Chu
    public override void Update()
    {
        base.Update();

        if (target == null || (target.transform.position - towerPosition).magnitude > range)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies the effect in the game.
    /// Does damage to enemies as long as the laser is targeting them.
    /// </summary>
    /// Edited by Courtney Chu
    /// <param name="damage">Amount of this effect does to an enemy.</param>
    /// <param name="range">How far the effect can reach.</param>
    /// <param name="color">Color enemy this effect does more damage to.</param>
    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        GameObject newEffect = Instantiate(prefab, position, Quaternion.identity);
        LaserDoTEffect effect = newEffect.GetComponent<LaserDoTEffect>();
        effect.SetTarget(target);
        effect.SetTowerPosition(position);
        effect.SetSprite(newEffect.GetComponent<SpriteRenderer>().sprite);

        return effect;
    }

    /// <summary>
    /// Sets the laser sprite to be the sprite for this effect.
    /// </summary>
    /// <param name="s">Sprite for this effect.</param>
    private void SetSprite(Sprite s) { sprite = s; }

    /// <summary>
    /// Sets the position of the laser tower.
    /// </summary>
    /// <param name="position">Position of the tower.</param>
    private void SetTowerPosition(Vector3 position)
    {
        towerPosition = position;
    }
}
