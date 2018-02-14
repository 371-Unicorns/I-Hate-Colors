using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyManager : Singleton<EnemyManager>
{

    public static Dictionary<string, Enemy> enemyDictionary;
    public static List<Enemy> activeEnemies = new List<Enemy>();

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

        int targetTileLength = EnemyManager.Instance.TargetTiles.Length;
        Tile randomTargetTileScript = EnemyManager.Instance.TargetTiles[Random.Range(0, targetTileLength)];
        enemy.GetComponent<AIDestinationSetter>().target = randomTargetTileScript.transform;
        enemy.gameObject.SetActive(true);

        activeEnemies.Add(enemy);
    }

    public static void RemoveEnemy(Enemy obj)
    {
        activeEnemies.Remove(obj);
        Destroy(obj.gameObject);
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

}
