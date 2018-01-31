using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// Width of grid
    /// </summary>
    [SerializeField]
    private int width = 15;
    public int Width { get { return width; } }

    /// <summary>
    /// Height of grid
    /// </summary>
    [SerializeField]
    private int height = 7;
    public int Height { get { return height; } }

    /// <summary>
    /// Currently selected tower by player.
    /// Public getter, private setter
    /// </summary>
    public TowerBtn SelectedTower { get; private set; }

    public bool gameOver;
    public Text gameOverText;
    public Text healthText;
    public Text moneyText;

    public GameObject[] enemyList;

    public static ArrayList activeEnemies = new ArrayList();

    public int money = 0;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GameManager() { }

    void Awake()
    {
        gameOver = false;
        gameOverText.text = "";
        healthText.text = "Health: 100";
        moneyText.text = "$ ";
    }

    void Update()
    {
        if (gameOver)
        {
            healthText.text = "Health: 0";
            gameOverText.text = "GAME OVER";
        }
        else
        {
            healthText.text = "Health: " + CastleController.Instance.CastleHealth.ToString();
            moneyText.text = "$ " + money.ToString();
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

    /// <summary>
    /// Signals player that tower is ready to place by hovering it with the mouse cursor.
    /// </summary>
    /// <param name="towerBtn">TowerBtn to select.</param>
    public void SelectTower(TowerBtn towerBtn)
    {
        this.SelectedTower = towerBtn;
        Hover.Instance.Activate(towerBtn.TowerHoverSprite);
    }

    /// <summary>
    /// Singals player that no tower is currently selected and no tower can be placed.
    /// </summary>
    public void ResetTower()
    {
        this.SelectedTower = null;
    }

}
