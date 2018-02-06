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
    public static GameObject upgradePanel;

    public Enemy target;
    public float countdownToFire = 0f;
    public int level = 1;
    public int upgradeCost;
    public int baseUpgradeCost = 20;
    public double upgradeCostScale = 1.25;

    public bool hitTowerSometime = false;

    void Start()
    {
        target = null;
        baseUpgradeCost = 20;
        upgradeCost = baseUpgradeCost;
        canvas = GameObject.Find("Canvas");
        upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
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
            //upgradeCost = upgradeCost * ((int)(level * upgradeCostScale));
            upgradeCost = 20;
        }
    }

    //Activates upgradepanel for the first time
    public void ActivateUpgradePanel()
    {
        upgradePanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30);
        GameManager.UpdateUpgradePanel(this);
        if (this.level == 5 || GameManager.money < this.upgradeCost)
        {
            GameManager.disableUpgradeButton();
        }
        upgradePanel.gameObject.SetActive(true);
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

    public void Reset()
    {
        range = 5f;
        fireRate = 1f;
        countdownToFire = 1f;
        baseUpgradeCost = 20;
    }


    /*
     * Checks if any colliders overlap the mouse position on click, and if they're tagged tower the UpgradePanel will display, 
     * otherwise nothing happenshitTowerSometime is  boolean that checks to see if in this cast a tower was hit, whereas 
     * onTower checks whether or not the upgradepanel is on a tower currently. If we click and don't find a tower object within
     * our position, we need to remove the upgradepanel, thus why we need both variables
     */
    void OnMouseUp()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (c.tag == "Tower")
                {
                    GameObject canvas = GameObject.Find("Canvas");
                    GameObject upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
                    GameManager.curTower = c.gameObject;
                    GameManager.onTower = true;
                    hitTowerSometime = true;
                    Tower hitTower = c.gameObject.GetComponent(typeof(Tower)) as Tower;
                    if (upgradePanel.activeSelf == false)
                    {
                        hitTower.activateUpgradePanel();
                    }
                    else
                    {
                        GameManager.UpdateUpgradePanel(hitTower);
                    }

                }

            }
        }
        if (hitTowerSometime == false)
        {
            GameManager.onTower = false;
        }

        hitTowerSometime = false;
    }

    //Activates upgradepanel for the first time
    public void activateUpgradePanel()
    {
        upgradePanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30);
        GameManager.UpdateUpgradePanel(this);
        if (this.level == 5 || GameManager.money < this.upgradeCost)
        {
            GameManager.disableUpgradeButton();
        }
        upgradePanel.gameObject.SetActive(true);
    }

}
