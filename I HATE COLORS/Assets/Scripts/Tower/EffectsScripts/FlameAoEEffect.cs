using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAoEEffect : AoEEffect {

    private readonly float LIFESPAN = 5.0f;

    private Sprite sprite;

    private GameTimer gTimer;

    public FlameAoEEffect()
    {
        radius = 3;
        gTimer = new GameTimer(LIFESPAN);
        gTimer.SetPaused(false);
    }

    public override void Update()
    {
        gTimer.Update();

        if (gTimer.IsDone())
        {
            Destroy(gameObject);
        }
    }

    public override void ApplyAoEEffect()
    {
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, LayerMask.NameToLayer("Enemies"));

        foreach (Collider2D c in targets)
        {
			print("tag is " + c.gameObject.tag);
			if(c.gameObject.tag == "Enemy") {
				GameObject target = c.gameObject;
				target.GetComponent<Enemy>().TakeDamage(damage, ColorType.BLACK);
			}
			
        }
    }

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