using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, Upgradeable
{
    [Header("Attributes")]
    public float range = 0f;
    public float fireRate = 0f;

    [Header("Unity Tags")]
    public string enemyTag = "Enemy";

    public GameObject bulletPrefab;
    public GameObject canvas;
    public AudioSource upgradeSound;

    public Enemy target;
    public float countdownToFire = 0f;
    public int level = 1;
    public int upgradeCost = 20;
    public double upgradeCostScale = 1.25;
    public int baseCost = 20;

    void Start()
    {
        upgradeSound = GetComponent<AudioSource>();
        target = null;
        canvas = GameObject.Find("Canvas");
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

    public void LevelUp()
    {
        fireRate = fireRate + 5;
        range = range + 1;
        level = level + 1;
    }

    public void Upgrade()
    {
        if (level < 5)
        {
            LevelUp();
            upgradeCost = upgradeCost * ((int)(level * upgradeCostScale));
            upgradeSound.Play();
        }
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
        List<Enemy> enemies = EnemyManager.GetEnemies();

        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemies)
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
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    /// <summary>
    /// When a tower is clicked, set the currently selected tower and update the TowerInformation panel.
    /// </summary>
    private void OnMouseUpAsButton()
    {
        GameManager.Instance.SelectTower(this);
        TowerInformation.Instance.ShowPlacedTower(this);
    }

    /// <summary>
    /// Place tower on passed tile.
    /// </summary>
    /// <param name="parentTile">Parent tile for this tower.</param>
    /// <returns>Created tower GameObject.</returns>
    public static GameObject PlaceTower(Tile parentTile)
    {
        int cost = GameManager.Instance.SelectedTower.baseCost;

        if (cost > GameManager.money)
        {
            // TODO: Display warning message with this.
            print("Can't place tower. Not enough funds.");
            return null;
        }
        GameManager.AddMoney(-cost);

        // Place tower
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.gameObject, parentTile.transform.position, Quaternion.identity);

        // Update A* and check if path is blocked.
        GridGraphManager.Instance.ScanGridGraph();
        if (GridGraphManager.IsGraphBlocked())
        {
            // TODO: Display warning message with this.
            print("Can't place tower here. Path is entirely blocked.");
            Destroy(tower);
            return null;
        }

        //  Set sprite sorting order and parent.
        tower.transform.SetParent(parentTile.transform);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -parentTile.GridPoint.y;


        GameManager.Instance.SelectTower(tower.GetComponent<Tower>());
        TowerInformation.Instance.ShowPlacedTower(GameManager.Instance.SelectedTower);

        // Allow multi tower placement by pressing LeftShift.
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Hover.Instance.Deactivate();
        }
        return tower;
    }
}
