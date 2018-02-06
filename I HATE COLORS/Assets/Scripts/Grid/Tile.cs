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
    /// Public getter, public setter
    /// </summary>
    public Point GridPoint { get; set; }

    /// <summary>
    /// Placed tower on this tile.
    /// </summary>
    private GameObject placedTower = null;

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
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0) && this.placedTower == null)
            {
                PlaceTower();
            }
        }

        // TODO Improve design
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.SelectedTower == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.onTower = false;
            }
        }
    }

    /// <summary>
    /// Place tower on this tile.
    /// </summary>
    private void PlaceTower()
    {
        // Place tower and set sprite sorting order and parent.
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -this.GridPoint.y;
        tower.transform.SetParent(this.transform);

        //  Set tile to be occupied by a tower
        this.placedTower = tower;

        // Update A*
        GridGraphManager.Instance.ScanGridGraph();

        // Deactive all hover elements
        GameManager.Instance.ResetTower();
        Hover.Instance.Deactivate();
    }

}
