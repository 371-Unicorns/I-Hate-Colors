using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Manager for GridGraph of A* Pathfinding.
/// The GridGraph is used to let A* know, which nodes are walkable and which are not.
/// </summary>
public class GridGraphManager : MonoBehaviour
{
    /// <summary>
    /// GridGraph
    /// </summary>
    private static GridGraph gridGraph;

    /// <summary>
    /// Random target tile to check the graph for blocking placements against.
    /// </summary>
    private static Tile randomTargetTile;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    public GridGraphManager() { }

    /// <summary>
    /// Setup GridGraph to match tiles grid.
    /// </summary>
    /// <param name="width">Amount of tiles on the x-Axis.</param>
    /// <param name="height">Amount of tiles on the y-Axis.</param>
    /// <param name="tileSize">Length of one tile.</param>
    public static void Setup(int width, int height, float tileSize)
    {
        //if (gridGraph == null)
        gridGraph = AstarPath.active.data.gridGraph;

        // Adjust in order to increase resolution of GridGraph
        gridGraph.SetDimensions(width * 4, height * 4, tileSize / 4.0f);

        randomTargetTile = EnemyManager.GetRandomTargetTile();

        gridGraph.Scan();
    }

    /// <summary>
    /// Check whether the passed Tower will block the path between spawn and target tiles for the enemies.
    /// Uses Pathfinding.GraphUpdateUtilities.UpdateGraphsNoBlock(...) to do so, which first updates the graph and then checks if all nodes are still reachable from each other.
    /// If the path is blocked, the effect by the new tower on the graph is reverted.
    /// https://arongranberg.com/astar/docs/graphupdates.html#blocking for more information.
    /// </summary>
    /// <param name="newTower">New tower, player wants to place.</param>
    /// <returns>True if path would be blocked by the new tower, false otherwise.</returns>
    public static bool IsGraphNotBlocked(GameObject newTower)
    {
        var guo = new GraphUpdateObject(newTower.GetComponent<BoxCollider2D>().bounds);
        var spawnNode = AstarPath.active.GetNearest(LevelManager.TileDict[new Point(0, 0)].transform.position).node;
        var targetNode = AstarPath.active.GetNearest(randomTargetTile.transform.position).node;

        return GraphUpdateUtilities.UpdateGraphsNoBlock(guo, spawnNode, targetNode, false);
    }
}
