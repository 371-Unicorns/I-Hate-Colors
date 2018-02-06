using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Pathfinding;

public class EnemyManager : Singleton<EnemyManager> {

    public static Dictionary<string, Enemy> enemyDictionary;
    public static List<Enemy> activeEnemies = new List<Enemy>();

    public void Awake()
    {
        enemyDictionary = XmlImporter.GetEnemiesFromXml();
    }

    public static List<Enemy> GetEnemies()
    {
        return activeEnemies;
    }

    public static void SpawnEnemy(Enemy obj)
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Instance.Length));
        Tile spawnTileScript = LevelManager.Instance.TileDict[spawnPoint];

        Enemy enemy = Instantiate(obj, spawnTileScript.transform.position, Quaternion.identity);
        enemy.Initialize(obj);
        enemy.transform.SetParent(GameObject.Find("Enemies").transform);

        Tile[] targetTiles = LevelManager.GetTargetTiles();
        Tile randomTargetTileScript = targetTiles[Random.Range(0, targetTiles.Length)];
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
}
