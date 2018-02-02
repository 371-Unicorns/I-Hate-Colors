using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridGraphManager : Singleton<GridGraphManager>
{

    private GridGraph gridGraph;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GridGraphManager() { }

    public void AdjustGridGraph(int width, int heigth, float tileSize)
    {
        if (gridGraph == null)
        {
            AstarData data = AstarPath.active.data;
            gridGraph = data.gridGraph;
        }

        // Adjust in order to increase resolution of gridGraph
        gridGraph.SetDimensions(width * 4, heigth * 4, tileSize / 4.0f);

        ScanGridGraph();
    }

    public void ScanGridGraph()
    {
        gridGraph.Scan();
    }
}
