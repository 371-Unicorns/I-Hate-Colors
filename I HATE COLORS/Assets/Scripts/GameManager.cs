using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int width = 15;

    public int Width
    {
        get
        {
            return width;
        }
    }

    [SerializeField]
    private int heigth = 7;

    public int Heigth
    {
        get
        {
            return heigth;
        }
    }

    public TowerBtn SelectedTower { get; private set; }

    public void SelectTower(TowerBtn towerBtn)
    {
        this.SelectedTower = towerBtn;
        Hover.Instance.Activate(towerBtn.TowerHoverSprite);
    }

    private GameManager() { }

    public void ResetTower()
    {
        this.SelectedTower = null;
    }
}
