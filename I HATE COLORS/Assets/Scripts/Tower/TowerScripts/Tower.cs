using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for towers.
/// </summary>
public abstract class Tower : MonoBehaviour
{
    /// <summary>
    /// Name of the tower. Is not 'name', because this would hide 'Object.name'.
    /// </summary>
    [SerializeField, HideInInspector]
    protected string towerName;

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

    /// <summary>
    /// Range tower can attack within.
    /// </summary>
    [SerializeField, HideInInspector]
    protected float range;

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
    /// Setup this tower.
    /// </summary>
    /// <param name="name">Name of the tower.</param>
    /// <param name="baseCosts">Base costs to build tower.</param>
    /// <param name="upgradeCosts">Base costs to build tower.</param>
    /// <param name="upgradeCostsScale">Scale of upgrade costs after each upgrade.</param>
    /// <param name="maxLevel">Max level tower can reach.</param>
    /// <param name="range">Range tower can attack within.</param>
    protected void Initialize(string name, int baseCosts, int upgradeCosts, double upgradeCostsScale, int maxLevel, float range)
    {
        this.towerName = name;
        this.baseCosts = baseCosts;
        this.upgradeCosts = upgradeCosts;
        this.upgradeCostsScale = upgradeCostsScale;
        this.maxLevel = maxLevel;
        this.range = range;

        this.level = 1;
        this.target = null;
    }

    /// <summary>
    /// Upgrade tower. Checking if player has enough money was done before calling this method.
    /// </summary>
    public abstract void Upgrade();

    /// <summary>
    /// Attack target with tower's specific attack.
    /// </summary>
    public abstract void Attack();

    public virtual void Update()
    {
        if (target == null)
        {
            FindClosestTarget();
        }
    }

    public float GetRange() { return range; }

    /// <summary>
    /// When a tower is clicked, set the currently selected tower and update the TowerInformation panel.
    /// </summary>
    private void OnMouseUpAsButton()
    {
        GameManager.Instance.SelectTower(this);
        TowerInformation.Instance.ShowPlacedTower(this);
    }

    /// <summary>
    /// Place tower on passed tile, if it is possible (enough money & don't block path).
    /// </summary>
    /// <param name="parentTile">Parent tile for this tower.</param>
    /// <returns>Created tower.</returns>
    public Tower PlaceTower(Tile parentTile)
    {
        if (baseCosts > GameManager.money)
        {
            // TODO Display warning message with this.
            print("Can't place tower. Not enough funds.");
            return null;
        }
        GameManager.AddMoney(-baseCosts);

        // Place tower.
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.gameObject, parentTile.transform.position, Quaternion.identity, parentTile.transform);

        // Check if path is blocked.
        if (!GridGraphManager.Instance.IsGraphNotBlocked(tower))
        {
            // TODO Display warning message with this.
            print("Can't place tower here. Path is entirely blocked.");
            Destroy(tower);
            return null;
        }

        // Set sprite sorting order.
        tower.GetComponent<SpriteRenderer>().sortingOrder = -parentTile.GridPoint.y;

        // Allow multi tower placement by pressing LeftShift.
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Hover.Instance.Deactivate();
            GameManager.Instance.SelectTower(tower.GetComponent<Tower>());
            TowerInformation.Instance.ShowPlacedTower(GameManager.Instance.SelectedTower);
        }
        return tower.GetComponent<Tower>();
    }

    protected void FindClosestTarget()
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

}
