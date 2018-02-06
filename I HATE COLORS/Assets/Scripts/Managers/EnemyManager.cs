using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Pathfinding;

public class EnemyManager : Singleton<EnemyManager> {
    
    public static List<Enemy> activeEnemies = new List<Enemy>();

    public void Awake()
    {
        
    }

    public static List<Enemy> GetEnemies()
    {
        return activeEnemies;
    }

    public static void SpawnEnemy(Enemy obj)
    {
        obj.transform.SetParent(GameObject.Find("Enemies").transform);

        Tile[] targetTiles = LevelManager.GetTargetTiles();
        Tile randomTargetTileScript = targetTiles[Random.Range(0, targetTiles.Length)];
        obj.GetComponent<AIDestinationSetter>().target = randomTargetTileScript.transform;
        obj.gameObject.SetActive(true);

        activeEnemies.Add(obj);
    }

    public static void RemoveEnemy(Enemy obj)
    {
        activeEnemies.Remove(obj);
        Destroy(obj);
    }

    public static Enemy EnemyFromXml(string id)
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Instance.Height));
        Tile spawnTileScript = LevelManager.Instance.TileDict[spawnPoint];
        print(id);
        GameObject obj = Resources.Load("" + id, typeof(GameObject)) as GameObject;
        GameObject enemy = Instantiate(obj, spawnTileScript.transform.position, Quaternion.identity);
        enemy.SetActive(false);

        // TODO: what is this???
        GameManager.onTower = false; 

        return enemy.GetComponent<Enemy>();
    }
}
