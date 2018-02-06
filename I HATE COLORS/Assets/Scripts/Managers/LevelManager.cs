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
    private GameObject towerPrefab;

    [SerializeField]
    private GameObject candycanePrefab;

    private GridGraph gridGraph;

    private static Tile[] targetTiles;

    public Dictionary<Point, Tile> TileDict { get; private set; }

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private LevelManager() { }

    void Awake()
    {
        TileDict = new Dictionary<Point, Tile>();
        GenerateLevel(GameManager.Instance.Length, GameManager.Instance.Width);
        Hover.Instance.Deactivate();
    }

    private void GenerateLevel(int length, int width)
    {
        float tileSize = grassTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GenerateTiles(length, width, tileSize);
        GridGraphManager.Instance.Setup(length, width, tileSize);

        targetTiles = new Tile[GameManager.Instance.Width];
        for (int i = 0; i < GameManager.Instance.Width; i++)
        {
            Tile tile = TileDict[new Point(GameManager.Instance.Length - 1, i)];
            targetTiles[i] = tile;
        }

        GenerateScenery();

    }

    private void GenerateScenery()
    {
        int curCandy = 0;

        while (curCandy < 9)
        {
            int posX = Random.Range(-10, 10);
            int posY = Random.Range(-10, 10);
            GameObject candy = Instantiate(candycanePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
            curCandy++;
        }
    }

    public static Tile[] GetTargetTiles()
    {
        return targetTiles;
    }

    private void GenerateTiles(int length, int width, float tileSize)
    {
        // Start point for grid is bottom left
        float tileXStart = -((length - 1) / 2.0f * tileSize);
        float tileYStart = -((width - 1) / 2.0f * tileSize);

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < width; y++)
            {
                // Last column is wall
                if (x == length - 1)
                {
                    Tile newWallTileScript = Instantiate(wallTile).GetComponent<Tile>();
                    newWallTileScript.Setup(new Point(x, y), new Vector3(tileXStart + tileSize * x, tileYStart + tileSize * y, 0));
                }
                else
                {
                    Tile newGrassTileScript = Instantiate(grassTile).GetComponent<Tile>();
                    newGrassTileScript.Setup(new Point(x, y), new Vector3(tileXStart + tileSize * x, tileYStart + tileSize * y, 0));
                }
            }
        }

    }

    public void SpawnUnicorn()
    {
    }
}
