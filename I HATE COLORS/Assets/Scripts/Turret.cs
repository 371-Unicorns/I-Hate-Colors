using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [Header("Attributes")]
    public float range = 5f;
    public float fireRate = 1f;

    [Header("Unity Tags")]
    public string enemyTag = "Enemy";

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private Transform target;
    private float countdownToFire = 0f;

    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToEnemy < shortestDistance)
            {
                shortestDistance = distToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject goBullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bullet = goBullet.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

	void Update () {
		if (target == null)
        {
            return;
        }

        //where to rotate turrets

        if (countdownToFire <= 0f)
        {
            Shoot();
            countdownToFire = 1f / fireRate;
        }

        countdownToFire -= Time.deltaTime;
   	}
}
