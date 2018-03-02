﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : MonoBehaviour
{
    private static GameObject grassTile;
    
    private static GameObject map;
    public static GameObject Map { get { return map; } }

    /// <summary>
    /// Parent for all projectiles effects to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private static Transform projectilesEffectParent;
    public static Transform ProjectilesEffectParent { get { return projectilesEffectParent; } }

    /// <summary>
    /// Parent for all DoT effects to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private static Transform dotEffectParent;
    public static Transform DoTEffectParent { get { return dotEffectParent; } }

    /// <summary>
    /// Parent for all instantiated prefabs (towers & enemies) to keep hierachy clean.
    /// </summary>
    [SerializeField]
    private static Transform prefabHolderParent;
    public static Transform PrefabHolderParent { get { return prefabHolderParent; } }

    /// <summary>
    /// Dictionary mapping each point on the grid to its correspoing tile.
    /// The grid starts at (0, 0) and goes up (GameManager.Instance.Width - 1, GameManager.Instance.Heigth - 1).
    /// </summary>
    public static Dictionary<Point, Tile> TileDict { get; private set; }

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    public LevelManager() { }

    /// <summary>
    /// Build the whole level.
    /// </summary>
    public void Start()
    {
        map = GameObject.Find("Map");
        grassTile = (GameObject)Resources.Load("Prefabs/Tiles/Grass");
        TileDict = new Dictionary<Point, Tile>();
        float tileSize = grassTile.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        GenerateTiles(GameManager.Width, GameManager.Height, tileSize);
        EnemyManager.FindTargetTiles();
        SceneryGenerator.GenerateScenery();

        GridGraphManager.Setup(GameManager.Width, GameManager.Height, tileSize);
        Hover.Deactivate();
    }

    /// <summary>
    /// Instatiate the right tile for each element of the grid.
    /// Start point of the grid is bottom left.
    /// </summary>
    /// <param name="width">Amount of tiles on the x-Axis.</param>
    /// <param name="height">Amount of tiles on the y-Axis.</param>
    /// <param name="tileSize">Length of one size.</param>
    private static void GenerateTiles(int width, int heigth, float tileSize)
    {
        float tileXStart = -((width - 1) / 2.0f * tileSize);
        float tileYStart = -((heigth - 1) / 2.0f * tileSize);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                Tile grassTile = Instantiate(LevelManager.grassTile).GetComponent<Tile>();
                grassTile.Setup(new Point(x, y), new Vector3(tileXStart + tileSize * x, tileYStart + tileSize * y, 0));
            }
        }
    }

}
