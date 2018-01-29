using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite towerHoverSprite;

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;

        }
    }

    public Sprite TowerHoverSprite
    {
        get
        {
            return towerHoverSprite;
        }
    }

}
