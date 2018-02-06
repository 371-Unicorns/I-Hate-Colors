using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    public float speed = 10f;
    public GameObject impactFX;

    private Enemy target;

    public void Seek(Enemy _target)
    {
        target = _target;
    }

    void HitTarget()
    {
        GameObject fx = (GameObject)Instantiate(impactFX, transform.position, transform.rotation);
        Destroy(fx, 2f);

        target.TakeDamage(damage);

        Destroy(gameObject);
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

        if (dir.magnitude <= distFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distFrame, Space.World);
    }
}
