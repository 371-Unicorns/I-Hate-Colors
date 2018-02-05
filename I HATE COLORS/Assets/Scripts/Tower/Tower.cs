using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [Header("Attributes")]
    public float range = 0f;
    public float fireRate = 0f;

    [Header("Unity Tags")]
    public string enemyTag = "Enemy";

    public GameObject bulletPrefab;

    public Transform target;
    public float countdownToFire = 0f;
    public int level = 1;
    public int upgradeCost;
    public int baseUpgradeCost = 500;
    public double upgradeCostScale = 1.25;


    // Use this for initialization
    void Start ()
    {
        target = null;
        upgradeCost = baseUpgradeCost;
    }

    void Update()
    {
        //where to rotate turrets
        UpdateTarget();


        if (target == null || !target.gameObject.activeInHierarchy)
        {
            return;
        }

        if (countdownToFire <= 0f)
        {
            Shoot();
            countdownToFire = 1f / fireRate;
        }

        countdownToFire -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject goBullet = (GameObject)Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Bullet bullet = goBullet.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void UpdateTarget()
    {
        ArrayList enemies = GameManager.GetEnemies();

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
}
