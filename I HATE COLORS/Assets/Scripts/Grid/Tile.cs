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
    /*
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.SelectedTower != null)
        {
            if (Input.GetMouseButtonDown(0) && this.placedTower == null)
            {
                PlaceTower();
            }
        }
    }
    */

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.newSelectedTower != null && this.placedTower == null)
        {
            if (Hover.Instance.IsActive())
            {
                PlaceTower();
            }
            else
            {
                TowerInformation.Instance.Reset();
            }
        }
    }

    /// <summary>
    /// Place tower on this tile.
    /// </summary>
    // TODO: Move this code to tower (Tower.PlaceTower())
    private void PlaceTower()
    {
        int cost = GameManager.Instance.SelectedTower.baseCost;

        if (cost > GameManager.money)
        {
            // TODO: Display warning message with this.
            print("Can't place tower. Not enough funds.");
        }

        // Place tower
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.gameObject, transform.position, Quaternion.identity);

        // Update A* and check if path is blocked.
        GridGraphManager.Instance.ScanGridGraph();
        if (GridGraphManager.IsGraphBlocked())
        {
            // TODO: Display warning message with this.
            print("Can't place tower here. Path is entirely blocked.");
            Destroy(tower);
            return;
        }

        //  Set sprite sorting order and parent.
        tower.transform.SetParent(this.transform);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -this.GridPoint.y;

        // Set tile to be occupied by a tower.
        this.placedTower = tower;

        GameManager.Instance.SelectTower(tower.GetComponent<Tower>());
        TowerInformation.Instance.ShowPlacedTower(GameManager.Instance.newSelectedTower);

        GameManager.AddMoney(-cost);

        // Deactive all hover elements
        // TODO check
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            Hover.Instance.Deactivate();
        }
    }

}
