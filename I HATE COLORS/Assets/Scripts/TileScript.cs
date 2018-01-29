using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPoint { get; set; }

    public void Setup(Point gridPoint, Vector3 gridPosition, Transform parent)
    {
        this.GridPoint = gridPoint;
        this.transform.position = gridPosition;
        this.transform.SetParent(parent);
        LevelManager.Instance.TileDict.Add(gridPoint, this);
    }

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

    private void PlaceTower()
    {
        GameObject tower = Instantiate(GameManager.Instance.SelectedTower.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = -this.GridPoint.y;
        tower.transform.SetParent(this.transform);
        GameManager.Instance.ResetTower();
        GridGraphManager.Instance.ScanGridGraph();
        Hover.Instance.Deactivate();
    }
}
