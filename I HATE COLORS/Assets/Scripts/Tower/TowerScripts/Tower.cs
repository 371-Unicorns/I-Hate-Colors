using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for towers.
/// 
/// Authors: Cole Twitchell, Steven Johnson
/// </summary>
public abstract class Tower : MonoBehaviour
{
    /// <summary>
    /// Name of the tower. Is not 'name', because this would hide 'Object.name'.
    /// </summary>
    [SerializeField, HideInInspector]
    protected string towerName;
    public string Name { get { return towerName; } }

    /// <summary>
    /// Base costs to build tower.
    /// </summary>
    [SerializeField, HideInInspector]
    protected int baseCosts;
    public int BaseCosts { get { return baseCosts; } }

    /// <summary>
    /// Upgrade costs to advance tower to next level.
    /// </summary>
    [SerializeField, HideInInspector]
    protected int upgradeCosts;
    public int UpgradeCosts { get { return upgradeCosts; } }

    /// <summary>
    /// Scale of upgrade costs after each upgrade.
    /// </summary>
    [SerializeField, HideInInspector]
    protected double upgradeCostsScale;

    /// <summary>
    /// Current level of tower.
    /// </summary>
    [SerializeField, HideInInspector]
    protected int level;
    public int Level { get { return level; } }

    /// <summary>
    /// Max level tower can reach.
    /// </summary>
    [SerializeField, HideInInspector]
    protected int maxLevel;
    public int MaxLevel { get { return maxLevel; } }

    /// <summary>
    /// Range tower can attack within.
    /// </summary>
    [SerializeField, HideInInspector]
    protected float range;
    public float Range { get { return range; } }

    /// <summary>
    /// Color of this towers effect.
    /// </summary>
    [SerializeField, HideInInspector]
    protected ColorType color;

    /// <summary>
    /// Short description of this tower.
    /// </summary>
    protected string description;
    public string Description { get { return description; } }

    /// <summary>
    /// Tower's current target.
    /// </summary>
    [SerializeField, HideInInspector]
    protected Enemy target;

    /// <summary>
    /// Effect prefab attached to each tower.
    /// </summary>
    public GameObject effectPrefab;

    /// <summary>
    /// AudioSource to be played if tower upgrades.
    /// </summary>
    [SerializeField, HideInInspector]
    protected AudioSource upgradeAudioSource;

    /// <summary>
    /// Tile this tower stands on.
    /// </summary>
    [SerializeField, HideInInspector]
    protected Tile tile;
    public Tile Tile { get { return tile; } }

    /// <summary>
    /// Setup this tower.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    /// <param name="color">Color of this towers effect.</param>
    /// <param name="description">Short description of this tower.</param>

    /// <summary>
    /// Initialize tower
    /// </summary>
    protected void Initialize(string name, int baseCosts, int upgradeCosts, float upgradeCostsScale, int maxLevel, float range, ColorType color, string description)
    {
        this.towerName = name;
        this.baseCosts = baseCosts;
        this.upgradeCosts = upgradeCosts;
        this.upgradeCostsScale = upgradeCostsScale;
        this.maxLevel = maxLevel;
        this.range = range;
        this.color = color;
        this.description = description;

        this.level = 1;
        this.target = null;
        this.upgradeAudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Finds a new target if the tower does not have a target, the target goes out of range or is dead.
    /// </summary>
    public virtual void Update()
    {
        if (target == null || target.isDead() || (target.transform.position - transform.position).magnitude > range)
        {
            FindClosestTarget();
        }
    }

    /// <summary>
    /// Upgrade tower. 
    /// It does not check whether the player has enough money. Do this BEFORE calling this method.
    /// 
    /// Author: David Askari, Steven Johnson
    /// </summary>
    public virtual void Upgrade()
    {
        if (level < maxLevel)
        {
            level += 1;
            upgradeCosts *= (int)(level * upgradeCostsScale);
            upgradeAudioSource.Play();
        }
    }

    /// <summary>
    /// Attack target with tower's specific attack.
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// When a tower is clicked, set the currently selected tower and update the TowerInformation panel.
    /// Author: Steven Johnson
    /// </summary>
    private void OnMouseUpAsButton()
    {
        if (!SumPause.Status)
        {
            GameManager.SelectTower(this);
            TowerInformation.ShowPlacedTower(this);
        }
    }

    /// <summary>
    /// Place tower on passed tile, if it is possible (enough money & don't block path).
    /// Author: Steven Johnson
    /// </summary>
    /// <param name="parentTile">Parent tile for this tower.</param>
    /// <returns>Created tower.</returns>
    public Tower PlaceTower(Tile parentTile)
    {
        if (baseCosts > GameManager.money)
        {
            GameManager.DisplayErrorText("Can't place tower. Not enough funds.");
            return null;
        }
        GameManager.AddMoney(-baseCosts);
        
        // Place tower.
        GameObject tower = Instantiate(GameManager.SelectedTower.gameObject, parentTile.transform.position, Quaternion.identity, parentTile.transform);

        if(GameManager.CheckForFirstPlacement()) {
            StartCoroutine(GameManager.DisplayRewardsPanel());
        }
        
        // Check if path is blocked.
        if (!GridGraphManager.IsGraphNotBlocked(tower))
        {
            GameManager.DisplayErrorText("Can't place tower here. Path is entirely blocked.");
            GameManager.AddMoney(baseCosts);
            Destroy(tower);
            return null;
        }

        // Set sprite sorting order.
        tower.GetComponent<SpriteRenderer>().sortingOrder = -parentTile.GridPoint.y;

        // Allow multi tower placement by pressing LeftShift.
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Hover.Deactivate();
            GameManager.SelectTower(tower.GetComponent<Tower>());
            TowerInformation.Reset();
        }

        tower.GetComponent<Tower>().tile = parentTile;
        return tower.GetComponent<Tower>();
    }

    /// <summary>
    /// Finds the closest enemy to the tower and sets it as the target.
    /// 
    /// Author: Cole Twitchell
    /// </summary>
    protected void FindClosestTarget()
    {
        List<Enemy> enemies = EnemyManager.GetEnemies();

        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToEnemy < shortestDistance && distToEnemy < range)
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

}
