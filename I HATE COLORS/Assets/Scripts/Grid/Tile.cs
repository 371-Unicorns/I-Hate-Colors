﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  Represents one tile of the grid.
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// All possible sprite this tile can have.
    /// </summary>
    [SerializeField]
    private Sprite[] possibleSprites;

    /// <summary>
    /// Weight of all possible sprites. possibleSpritesWeight[0] defines the weight of possibleSprites[0]. Sum of all weights can be greater than 1.
    /// </summary>
    [SerializeField]
    private float[] possibleSpritesWeight;

    /// <summary>
    /// Position of the tile on the grid. Grid starts at (0, 0).
    /// </summary>
    public Point GridPoint { get; private set; }

    /// <summary>
    /// Vector3 positon of this tiles center.
    /// </summary>
    public Vector3 Position { get { return new Vector3(GridPoint.x, GridPoint.y, 0); } }

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
        this.GetComponent<SpriteRenderer>().sprite = Helper.WeightedRandom(possibleSprites, possibleSpritesWeight); this.transform.position = gridPosition;
        this.transform.SetParent(LevelManager.Map.transform);
        LevelManager.TileDict.Add(gridPoint, this);
    }

    /// <summary>
    /// Check whether a tower is currently selected and the tile is empty, then either place a tower or reset TowerInformation panel based on Hover. 
    /// Authors: David Askari, Steven Johnson
    /// </summary>
    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.SelectedTower != null && this.placedTower == null)
        {
            if (Hover.IsActive())
            {
                Tower possiblyPlacedTower = GameManager.SelectedTower.PlaceTower(this);
                this.placedTower = possiblyPlacedTower == null ? null : possiblyPlacedTower;
            }
            else
            {
                TowerInformation.Reset();
            }
        }
    }
}
