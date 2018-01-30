using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject grassTile;

    [SerializeField]
    private GameObject wallTile;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject enemies;

    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private GameObject pusheenPrefab;

    private GridGraph gridGraph;

    private TileScript[] targetTiles;

    public Dictionary<Point, TileScript> TileDict { get; private set; }

    private LevelManager() { }

    void Start()
    {
        TileDict = new Dictionary<Point, TileScript>();
        GenerateLevel(GameManager.Instance.Width, GameManager.Instance.Heigth);
        Hover.Instance.Deactivate();
    }

    private void GenerateLevel(int width, int heigth)
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        float tileSize = grassTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GenerateTiles(width, heigth, tileSize, worldStart);
        GridGraphManager.Instance.AdjustGridGraph(width, heigth, tileSize, worldStart);

        targetTiles = new TileScript[GameManager.Instance.Heigth];
        for (int i = 0; i < GameManager.Instance.Heigth; i++)
        {
            TileScript tile = TileDict[new Point(GameManager.Instance.Width - 1, i)];
            targetTiles[i] = tile;
        }

    }

    private void GenerateTiles(int width, int heigth, float tileSize, Vector3 worldStart)
    {
        float tileXStart = worldStart.x;
        float tileYStart = worldStart.y;

        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Last tile in row is wall
                if (x == width - 1)
                {
                    TileScript newWallTileScript = Instantiate(wallTile).GetComponent<TileScript>();
                    newWallTileScript.Setup(new Point(x, y), new Vector3(tileSize * x + tileXStart, tileSize * y + tileYStart, 0), map.transform);
                }
                else
                {
                    TileScript newGrassTileScript = Instantiate(grassTile).GetComponent<TileScript>();
                    newGrassTileScript.Setup(new Point(x, y), new Vector3(tileSize * x + tileXStart, tileSize * y + tileYStart, 0), map.transform);
                }
            }
        }

    }

    public void SpawnPusheen()
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Instance.Heigth));
        TileScript spawnTileScript = LevelManager.Instance.TileDict[spawnPoint];
        GameObject pusheen = Instantiate(pusheenPrefab, spawnTileScript.transform.position, Quaternion.identity);
        pusheen.transform.SetParent(enemies.transform);

        TileScript randomTargetTileScript = targetTiles[Random.Range(0, targetTiles.Length)];
        pusheen.GetComponent<AIDestinationSetter>().target = randomTargetTileScript.transform;

        GameManager.PushEnemy(pusheen);
    }
}
