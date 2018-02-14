using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower {
    
    public GameObject laserPrefab;

    void Shoot()
    {
        GameObject goLaser = (GameObject)Instantiate(laserPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Laser laser = goLaser.GetComponent<Laser>();

        if (laser != null)
        {
            laser.Seek(target);
        }
    }
}
