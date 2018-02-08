using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Manager for GridGraph of A* Pathfinding.
/// The GridGraph is used to let A* know, which nodes are walkable and which are not.
/// </summary>
public class GridGraphManager : Singleton<GridGraphManager>
{

    /// <summary>
    /// GridGraph
    /// </summary>
    private GridGraph gridGraph;
    private static GameObject pathChecker;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GridGraphManager() { }

    /// <summary>
    /// Setup GridGraph to match tiles grid.
    /// </summary>
    /// <param name="width">Amount of tiles on the x-Axis.</param>
    /// <param name="height">Amount of tiles on the y-Axis.</param>
    /// <param name="tileSize">Length of one size.</param>
    public void Setup(int width, int height, float tileSize)
    {
        if (gridGraph == null)
        {
            gridGraph = AstarPath.active.data.gridGraph;
        }
        if (pathChecker == null)
        {
            pathChecker = GameObject.Find("PathChecker");
            int targetTileLength = EnemyManager.Instance.TargetTiles.Length;
            Tile randomTargetTileScript = EnemyManager.Instance.TargetTiles[Random.Range(0, targetTileLength)];
            pathChecker.GetComponent<AIDestinationSetter>().target = randomTargetTileScript.transform;
        }

        // Adjust in order to increase resolution of GridGraph
        gridGraph.SetDimensions(width * 4, height * 4, tileSize / 4.0f);

        ScanGridGraph();
    }

    /// <summary>
    /// Update GridGraph for changed nodes.
    /// </summary>
    public void ScanGridGraph()
    {
        if (gridGraph == null)
        {
            gridGraph = AstarPath.active.data.gridGraph;
        }

        gridGraph.Scan();
    }

    public static bool IsGraphBlocked()
    {
        Vector3 targetLoc = pathChecker.GetComponent<AIDestinationSetter>().target.transform.position;
        ABPath p = pathChecker.GetComponent<Seeker>().StartPath(pathChecker.transform.position, targetLoc) as ABPath;
        AstarPath.BlockUntilCalculated(p);

        return Vector3.Distance(p.endPoint, targetLoc) > 0.1;
    }
}
