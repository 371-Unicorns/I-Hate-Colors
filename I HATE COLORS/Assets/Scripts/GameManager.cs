using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class GameManager : Singleton<GameManager>
{
    public bool gameOver;
    public Text gameOverText;
    public Text healthText;

    public GameObject[] enemyList;

    private static ArrayList activeEnemies = new ArrayList();

    // Use this for initialization
    void Awake()
    {
        gameOver = false;
        gameOverText.text = "";
        healthText.text = "Health: 100";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            healthText.text = "Health: 0";
            gameOverText.text = "GAME OVER";
        }
    }

    public static ArrayList GetEnemies()
    {
        return activeEnemies;
    }

    public static void PushEnemy(GameObject obj)
    {
        activeEnemies.Add(obj);
    }

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
