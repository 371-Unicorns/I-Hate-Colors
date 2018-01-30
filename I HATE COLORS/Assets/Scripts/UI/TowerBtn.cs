using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent tower on tower buttons.
/// </summary>
public class TowerBtn : MonoBehaviour
{
    /// <summary>
    /// Prefab of tower.
    /// </summary>
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab { get { return towerPrefab; } }

    /// <summary>
    /// Sprite of tower used for hover effect.
    /// </summary>
    [SerializeField]
    private Sprite towerHoverSprite;
    public Sprite TowerHoverSprite { get { return towerHoverSprite; } }

}
