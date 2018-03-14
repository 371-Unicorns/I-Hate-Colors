using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Black Hole Tower sucks in all enemies within its range.
/// It holds them for LIFESPAN (5 seconds) and then releases them.
/// Does no actual damage to enemies.
/// </summary>
public class BlackHoleAoEEffect : AoEEffect
{
    /// <summary>
    /// How many seconds the Black Hole is activated and holding enemies.
    /// Also how long between each black hole effect generated its tower.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private readonly float LIFESPAN = 5.0f;

    /// <summary>
    /// Black hole effect sprite drawn by Amy Lewis.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private Sprite sprite;

    /// <summary>
    /// Timer that controls how long the black hole is active.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private GameTimer gTimer;

    /// <summary>
    /// Constructor for the effect.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public BlackHoleAoEEffect()
    {
        radius = 5;
        gTimer = new GameTimer(LIFESPAN);
        gTimer.SetPaused(false);
    }

    /// <summary>
    /// Update function that resets the timer. 
    /// Makes sure the black hole is active LIFESPAN seconds and then disactivated LIFESPAN seconds.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public override void Update()
    {
        gTimer.Update();

        if (gTimer.IsDone())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies the effect in the game.
    /// Sucks in any enemies in range and draws them to the center of the effect. 
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public override void ApplyAoEEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, LayerMask.NameToLayer("IgnoreAoE"));

        foreach (Collider2D c in targets)
        {
            GameObject target = c.gameObject;
            target.transform.position = Vector2.Lerp(target.transform.position, transform.position, 1.5f * Time.deltaTime);
        }
    }

    /// <summary>
    /// Applies the effect in the game.
    /// Sucks in any enemies in range and draws them to the center of the effect. 
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="damage">Amount of this effect does to an enemy.</param>
    /// <param name="range">How far the effect can reach.</param>
    /// <param name="color">Color enemy this effect does more damage to.</param>
    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        GameObject newEffect = Instantiate(prefab, position, Quaternion.identity);
        BlackHoleAoEEffect effect = newEffect.GetComponent<BlackHoleAoEEffect>();
        effect.SetTarget(target);
        effect.SetSprite(newEffect.GetComponent<SpriteRenderer>().sprite);
        transform.localScale = new Vector3(radius * 0.66f, radius * 0.66f, 0);
        radius = 5;

        return effect;
    }

    /// <summary>
    /// Sets the black hole sprite to be the sprite for this effect.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    /// <param name="s">Sprite for this effect.</param>
    private void SetSprite(Sprite s) { sprite = s; }
}
