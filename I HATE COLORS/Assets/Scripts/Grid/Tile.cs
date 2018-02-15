using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  Represents one tile of the grid.
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// Position of the tile on the grid. Grid starts at (0, 0).
    /// </summary>
    public Point GridPoint { get; private set; }

    /// <summary>
    /// Placed tower on this tile.
    /// </summary>
    private Tower placedTower = null;

    /// <summary>
    /// Setup a new tile.
    /// </summary>
    /// <param name="gridPoint">Point of the tile in the grid. Between 0 and GameManager.Instance.Width or .Height.</param>
    /// <param name="gridPosition">Center positon of the tile in world space.</param>
    public void Setup(Point gridPoint, Vector3 gridPosition)
    {
        this.GridPoint = gridPoint;
        this.transform.position = gridPosition;
        this.transform.SetParent(LevelManager.Instance.Map.transform);
        LevelManager.Instance.TileDict.Add(gridPoint, this);
    }

    /// <summary>
    /// Check whether a tower is currently selected and the tile is empty, then either place a tower or reset TowerInformation panel based on Hover. 
    /// </summary>
    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.SelectedTower != null && this.placedTower == null)
        {
            if (Hover.Instance.IsActive())
            {
                Tower possiblyPlacedTower = GameManager.Instance.SelectedTower.PlaceTower(this);
                this.placedTower = possiblyPlacedTower == null ? null : possiblyPlacedTower;
            }
            else
            {
                TowerInformation.Instance.Reset();
            }
        }
    }
}
