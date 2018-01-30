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
    public GameObject Map { get { return map; } }

    [SerializeField]
    private GameObject enemies;

    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private GameObject pusheenPrefab;

    private GridGraph gridGraph;

    private Tile[] targetTiles;

    public Dictionary<Point, Tile> TileDict { get; private set; }

    private LevelManager() { }

    void Start()
    {
        TileDict = new Dictionary<Point, Tile>();
        GenerateLevel(GameManager.Instance.Width, GameManager.Instance.Heigth);
        Hover.Instance.Deactivate();
    }

    private void GenerateLevel(int width, int heigth)
    {
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        float tileSize = grassTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GenerateTiles(width, heigth, tileSize, worldStart);
        GridGraphManager.Instance.AdjustGridGraph(width, heigth, tileSize, worldStart);

        targetTiles = new Tile[GameManager.Instance.Heigth];
        for (int i = 0; i < GameManager.Instance.Heigth; i++)
        {
            Tile tile = TileDict[new Point(GameManager.Instance.Width - 1, i)];
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
                    Tile newWallTileScript = Instantiate(wallTile).GetComponent<Tile>();
                    newWallTileScript.Setup(new Point(x, y), new Vector3(tileSize * x + tileXStart, tileSize * y + tileYStart, 0));
                }
                else
                {
                    Tile newGrassTileScript = Instantiate(grassTile).GetComponent<Tile>();
                    newGrassTileScript.Setup(new Point(x, y), new Vector3(tileSize * x + tileXStart, tileSize * y + tileYStart, 0));
                }
            }
        }

    }

    public void SpawnPusheen()
    {
        Point spawnPoint = new Point(0, Random.Range(0, GameManager.Instance.Heigth));
        Tile spawnTileScript = LevelManager.Instance.TileDict[spawnPoint];
        GameObject pusheen = Instantiate(pusheenPrefab, spawnTileScript.transform.position, Quaternion.identity);
        pusheen.transform.SetParent(enemies.transform);

        Tile randomTargetTileScript = targetTiles[Random.Range(0, targetTiles.Length)];
        pusheen.GetComponent<AIDestinationSetter>().target = randomTargetTileScript.transform;

        GameManager.PushEnemy(pusheen);
    }
}
