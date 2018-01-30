using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridGraphManager : Singleton<GridGraphManager>
{

    private GridGraph gridGraph;

    private GridGraphManager() { }

    public void AdjustGridGraph(int width, int heigth, float tileSize, Vector3 worldStart)
    {
        if (gridGraph == null)
        {
            AstarData data = AstarPath.active.data;
            gridGraph = data.gridGraph;
        }

        // Adjust in order to increase resolution of gridGraph
        gridGraph.SetDimensions(width * 4, heigth * 4, tileSize / 4.0f);

        // Adjust gridGraph center to go into bottom left corner
        float centerX = -((-worldStart.x) - (((float)width / 2.0f) * tileSize));
        float centerY = -((-worldStart.y) - (((float)heigth / 2.0f) * tileSize));
        gridGraph.center = new Vector3(centerX, centerY, 0);

        ScanGridGraph();
    }

    public void ScanGridGraph()
    {
        gridGraph.Scan();
    }
}
