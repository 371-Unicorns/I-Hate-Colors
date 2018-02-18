using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO redo as LaserProjectileEffect
public class Laser : MonoBehaviour
{

    private Enemy target;
    private Vector3 ourDir;

    // Use this for initialization
    void Start()
    {
        ourDir = Vector3.left;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        if (ourDir == Vector3.left)
        {
            ourDir = dir;
            transform.LookAt(target.transform);
        }
        if ((dir - ourDir).magnitude > 5)
        {
            Destroy(gameObject);
            return;
        }
    }

    public new void Seek(Enemy _target)
    {
        target = _target;
    }

    public Vector3 GetDirection()
    {
        return ourDir;
    }
}
