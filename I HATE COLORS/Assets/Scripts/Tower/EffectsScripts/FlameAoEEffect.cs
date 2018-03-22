using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Flame Tower attacks all enemies within its range.
/// It attacks them for LIFESPAN (5 seconds) and then stops for LIFESPAN.
/// Does the most damage to enemies of all the effects.
/// </summary>
public class FlameAoEEffect : AoEEffect
{

    /// <summary>
    /// How many seconds the flames are activated and attacking enemies.
    /// Also how long between each flame effect generated from its tower.
    /// </summary>
    private readonly float LIFESPAN = 5.0f;

    /// <summary>
    /// Flame effect sprite.
    /// </summary>
    private Sprite sprite;

    /// <summary>
    /// Timer that controls how long the flames are active.
    /// </summary>
    private GameTimer gTimer;

    /// <summary>
    /// Flame prefab.
    /// </summary>
    [SerializeField]
    private ParticleSystem explosionPrefab;

    /// <summary>
    /// Constructor for the effect. 
    /// Author: Steven Johnson
    /// </summary>
    public FlameAoEEffect()
    {
        radius = 3;
        gTimer = new GameTimer(LIFESPAN);
        gTimer.SetPaused(false);
    }

    /// <summary>
    /// Update function that resets the timer. 
    /// Makes sure the flame tower is active LIFESPAN seconds and then disactivated LIFESPAN seconds.
    /// Author: Steven Johnson
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
    /// Sends flames in a circle damaging all targets in the range of this effect. 
    /// Author: Steven Johnson
    /// </summary>
    public override void ApplyAoEEffect()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, LayerMask.NameToLayer("Enemies"));

        foreach (Collider2D c in targets)
        {
            if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Walrusu" || c.gameObject.tag == "Boss")
            {
                GameObject target = c.gameObject;
                target.GetComponent<Enemy>().TakeDamage(damage, ColorType.BLACK);
                if(EnemyManager.deadEnemies.Contains(target.GetComponent<Enemy>()))
                {
                    ParticleSystem explosionParticle = Instantiate(explosionPrefab, target.transform.position, transform.rotation);
                    explosionParticle.transform.SetParent(CastleWall.explosionParent);
                    Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
                }
            }

        }
    }

    /// <summary>
    /// Applies the effect in the game.
    /// Sends flames in a circle damaging all targets in the range of this effect. 
    /// Author: Steven Johnson
    /// </summary>
    /// <param name="damage">Amount of this effect does to an enemy.</param>
    /// <param name="range">How far the effect can reach.</param>
    /// <param name="color">Color enemy this effect does more damage to.</param>
    public override Effect SpawnEffect(GameObject prefab, Vector3 position, Enemy target)
    {
        GameObject newEffect = Instantiate(prefab, position, Quaternion.identity);
        FlameAoEEffect effect = newEffect.GetComponent<FlameAoEEffect>();
        effect.SetTarget(target);
        effect.SetSprite(newEffect.GetComponent<SpriteRenderer>().sprite);
        transform.localScale = new Vector3(radius * 0.66f, radius * 0.66f, 0);
        radius = 5;

        return effect;
    }

    private void SetSprite(Sprite s) { sprite = s; }
}