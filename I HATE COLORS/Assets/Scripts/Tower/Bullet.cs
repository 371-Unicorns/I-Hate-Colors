using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Collidable
{
    public float damage;

    public float speed = 10f;
    public GameObject impactFX;

    private Enemy target;

    public float GetDamage()
    {
        return damage;
    }

    public void Seek(Enemy _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collidable other = collision.gameObject.GetComponent<Collidable>();
        other.ProcessCollision(this);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distFrame, Space.World);
    }

    public override void ProcessCollision(Collidable collidable)
    {
        GameObject fx = (GameObject)Instantiate(impactFX, transform.position, transform.rotation);
        Destroy(fx, 2f);

        Destroy(gameObject);
    }
}
