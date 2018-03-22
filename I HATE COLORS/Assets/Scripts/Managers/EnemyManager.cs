using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// This class manages the creation, storage of, and deletion of all enemies in the game.
/// 
/// Author: Cole Twitchell
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static Dictionary<string, Enemy> enemyDictionary;
    public static List<Enemy> activeEnemies = new List<Enemy>();
    public static List<Enemy> deadEnemies = new List<Enemy>();

    /// <summary>
    /// Tiles to be used by enemies as target tiles.
    /// </summary>
    public static Tile[] TargetTiles { get; private set; }

    /// <summary>
    /// Initializes ActiveEnemies, DeadEnemies, EnemyDictionary, and TargetTiles
    /// </summary>
    public void Start()
    {
        activeEnemies = new List<Enemy>();
        deadEnemies = new List<Enemy>();
        
        enemyDictionary = XmlImporter.GetEnemiesFromXml();
        TargetTiles = new Tile[GameManager.Height];
    }

    /// <summary>
    /// Returns list of active enemies
    /// </summary>
    /// <returns>List of active enemies</returns>
    public static List<Enemy> GetEnemies()
    {
        return activeEnemies;
    }

    /// <summary>
    /// Instantiates given enemy at 'obj's location
    /// </summary>
    /// <param name="obj">Enemy</param>
    public static void SpawnEnemy(Enemy obj)
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Height));
        Tile spawnTileScript = LevelManager.TileDict[spawnPoint];

        Enemy enemy = Instantiate(obj, spawnTileScript.transform.position, Quaternion.identity);
        enemy.Initialize(obj);
        enemy.transform.SetParent(GameObject.Find("Enemies").transform);

        Tile randomTargetTile = EnemyManager.GetRandomTargetTile();
        enemy.GetComponent<AIDestinationSetter>().target = randomTargetTile.transform;
        enemy.gameObject.SetActive(true);

        activeEnemies.Add(enemy);
    }

    /// <summary>
    /// Destroys all dead enemies
    /// </summary>
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

    /// <summary>
    /// Adds all active enemies to the dead enemies list
    /// </summary>
    /// <param name="obj">Enemy</param>
    public static void RemoveEnemy(Enemy obj)
    {
        obj.GetComponent<Collider2D>().enabled = false;

        activeEnemies.Remove(obj);
        deadEnemies.Add(obj);
        obj.gameObject.GetComponent<Animator>().SetBool("isDead", true);
    }

    /// <summary>
    /// Counts number of active enemies left in the field
    /// </summary>
    /// <returns>Number of active enemies</returns>
    public static int EnemiesRemaining()
    {
        return activeEnemies.Count;
    }

    /// <summary>
    /// Find tiles to be used by enemies as target tiles.
    /// Currently those are the most right column of the grid.
    /// 
    /// Author: David Askari
    /// </summary>
    public static void FindTargetTiles()
    {
        for (int i = 0; i < GameManager.Height; i++)
        {
            TargetTiles[i] = LevelManager.TileDict[new Point(GameManager.Width - 1, i)];
        }
    }

    /// <summary>
    /// Gets a random target tile.
    /// 
    /// Author: David Askari
    /// </summary>
    /// <returns>Random target tile.</returns>
    public static Tile GetRandomTargetTile()
    {
        int targetTileLength = EnemyManager.TargetTiles.Length;
        return EnemyManager.TargetTiles[Random.Range(0, targetTileLength)];
    }

}
