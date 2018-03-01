using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyManager : Singleton<EnemyManager>
{
    public static Dictionary<string, Enemy> enemyDictionary;
    public static List<Enemy> activeEnemies = new List<Enemy>();
    public static List<Enemy> deadEnemies = new List<Enemy>();

    /// <summary>
    /// Tiles to be used by enemies as target tiles.
    /// </summary>
    public Tile[] TargetTiles { get; private set; }

    public void Awake()
    {
        enemyDictionary = XmlImporter.GetEnemiesFromXml();
        TargetTiles = new Tile[GameManager.Instance.Height];
    }

    public static List<Enemy> GetEnemies()
    {
        return activeEnemies;
    }

    public static void SpawnEnemy(Enemy obj)
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Instance.Height));
        Tile spawnTileScript = LevelManager.Instance.TileDict[spawnPoint];

        Enemy enemy = Instantiate(obj, spawnTileScript.transform.position, Quaternion.identity);
        enemy.Initialize(obj);
        enemy.transform.SetParent(GameObject.Find("Enemies").transform);

        Tile randomTargetTile = EnemyManager.Instance.GetRandomTargetTile();
        enemy.GetComponent<AIDestinationSetter>().target = randomTargetTile.transform;
        enemy.gameObject.SetActive(true);

        activeEnemies.Add(enemy);
    }

    public static void ClearDeadEnemies()
    {
        foreach (Enemy e in deadEnemies)  {
            if (e != null)
            {
                Destroy(e.gameObject);
            }
        }

        deadEnemies.Clear();
    }

    public static void RemoveEnemy(Enemy obj)
    {
        obj.GetComponent<Collider2D>().enabled = false;

        activeEnemies.Remove(obj);
        deadEnemies.Add(obj);
        obj.gameObject.GetComponent<Animator>().SetBool("isDead", true);
    }

    public static int EnemiesRemaining()
    {
        return activeEnemies.Count;
    }

    /// <summary>
    /// Find tiles to be used by enemies as target tiles.
    /// Currently those are the most right column of the grid.
    /// </summary>
    public void FindTargetTiles()
    {
        for (int i = 0; i < GameManager.Instance.Height; i++)
        {
            TargetTiles[i] = LevelManager.Instance.TileDict[new Point(GameManager.Instance.Width - 1, i)];
        }
    }

    /// <summary>
    /// Gets a random target tile.
    /// </summary>
    /// <returns>Random target tile.</returns>
    public Tile GetRandomTargetTile()
    {
        int targetTileLength = EnemyManager.Instance.TargetTiles.Length;
        return EnemyManager.Instance.TargetTiles[Random.Range(0, targetTileLength)];
    }

}
