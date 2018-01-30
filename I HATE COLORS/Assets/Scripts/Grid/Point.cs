using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct representing a point of the grid.
/// </summary>
public struct Point
{
    public int x;

    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
