using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public GameObject impactFX;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void HitTarget()
    {
        GameObject fx = (GameObject)Instantiate(impactFX, transform.position, transform.rotation);
        Destroy(fx, 2f);
        GameManager.activeEnemies.Remove(target.transform);
        target.gameObject.SetActive(false);
        Destroy(gameObject);
        GameManager.Instance.money += 20;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distFrame, Space.World);
    }
}
