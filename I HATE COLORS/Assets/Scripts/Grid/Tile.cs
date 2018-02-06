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
    public bool hitTowerSometime = false;
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
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }
    /*
     * Checks if any colliders overlap the mouse position on click, and if they're tagged tower the UpgradePanel will display, 
     * otherwise nothing happenshitTowerSometime is  boolean that checks to see if in this cast a tower was hit, whereas 
     * onTower checks whether or not the upgradepanel is on a tower currently. If we click and don't find a tower object within
     * our position, we need to remove the upgradepanel, thus why we need both variables
     */ 
    void OnMouseUp()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (c.tag == "Tower")
                {
                    GameObject canvas = GameObject.Find("Canvas");
                    GameObject upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
                    GameManager.curTower = c.gameObject;
                    GameManager.onTower = true;
                    hitTowerSometime = true;
                    Tower hitTower = c.gameObject.GetComponent(typeof(Tower)) as Tower;
                    if (upgradePanel.activeSelf == false)
                    {
                        hitTower.ActivateUpgradePanel();
                    }
                    else
                    {
                        GameManager.UpdateUpgradePanel(hitTower);
                    }

                }

            }
        }
        if (hitTowerSometime == false)
        {
            GameManager.onTower = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            print(GridPoint.x + " " + GridPoint.y);
        }
    }

    /// <summary>
    /// Place tower on this tile.
    /// </summary>
    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -this.GridPoint.y;
        tower.transform.SetParent(this.transform);
        Tower curTower = tower.GetComponent(typeof(Tower)) as Tower;
        GameManager.Instance.ResetTower();
        GridGraphManager.Instance.ScanGridGraph();
        Hover.Instance.Deactivate();
    }

}
