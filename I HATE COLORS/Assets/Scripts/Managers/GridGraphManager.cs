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

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GridGraphManager() { }

    /// <summary>
    /// Setup GridGraph to match tiles grid.
    /// </summary>
    /// <param name="length">Amount of tiles on the x-Axis.</param>
    /// <param name="width">Amount of tiles on the y-Axis.</param>
    /// <param name="tileSize">Length of one size.</param>
    public void Setup(int length, int width, float tileSize)
    {
        if (gridGraph == null)
        {
            gridGraph = AstarPath.active.data.gridGraph;
        }

        // Adjust in order to increase resolution of GridGraph
        gridGraph.SetDimensions(length * 4, width * 4, tileSize / 4.0f);

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
}
