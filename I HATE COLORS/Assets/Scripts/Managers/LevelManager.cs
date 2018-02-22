using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject grassTile;

    [SerializeField]
    private GameObject map;
    public GameObject Map { get { return map; } }

    /// <summary>
    /// Parent for all projectiles effects to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private Transform projectilesEffectParent;
    public Transform ProjectilesEffectParent { get { return projectilesEffectParent; } }

    /// <summary>
    /// Parent for all DoT effects to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private Transform dotEffectParent;
    public Transform DoTEffectParent { get { return dotEffectParent; } }

    /// <summary>
    /// Parent for all instantiated prefabs (towers & enemies) to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private Transform prefabHolderParent;
    public Transform PrefabHolderParent { get { return prefabHolderParent; } }

    /// <summary>
    /// Dictionary mapping each point on the grid to its correspoing tile.
    /// The grid starts at (0, 0) and goes up (GameManager.Instance.Width - 1, GameManager.Instance.Heigth - 1).
    /// </summary>
    public Dictionary<Point, Tile> TileDict { get; private set; }

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private LevelManager() { }

    /// <summary>
    /// Build the whole level.
    /// </summary>
    void Awake()
    {
        TileDict = new Dictionary<Point, Tile>();
        float tileSize = grassTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GenerateTiles(GameManager.Instance.Width, GameManager.Instance.Height, tileSize);
        EnemyManager.Instance.FindTargetTiles();
        SceneryGenerator.GenerateScenery();

        GridGraphManager.Instance.Setup(GameManager.Instance.Width, GameManager.Instance.Height, tileSize);
        Hover.Instance.Deactivate();
    }

    /// <summary>
    /// Instatiate the right tile for each element of the grid.
    /// Start point of the grid is bottom left.
    /// </summary>
    /// <param name="width">Amount of tiles on the x-Axis.</param>
    /// <param name="height">Amount of tiles on the y-Axis.</param>
    /// <param name="tileSize">Length of one size.</param>
    private void GenerateTiles(int width, int heigth, float tileSize)
    {
        float tileXStart = -((width - 1) / 2.0f * tileSize);
        float tileYStart = -((heigth - 1) / 2.0f * tileSize);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                Tile grassTile = Instantiate(this.grassTile).GetComponent<Tile>();
                grassTile.Setup(new Point(x, y), new Vector3(tileXStart + tileSize * x, tileYStart + tileSize * y, 0));
            }
        }
    }

}
