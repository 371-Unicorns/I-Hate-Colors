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

    public static GameObject curTower;
    public static bool onTower = false;

    public GameObject[] enemyList;
    public GameObject canvas;
    public static GameObject upgradePanel;

    public static ArrayList activeEnemies = new ArrayList();

    public static int money = 0;

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
        canvas = GameObject.Find("Canvas");
        upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
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
            if(onTower == false)
            {
                upgradePanel.gameObject.SetActive(false);
            }
            else if(upgradePanel.activeSelf == true)
            {
                if(money >= (curTower.GetComponent(typeof(Tower)) as Tower).upgradeCost)
                {
                    Button upgradeButton = upgradePanel.GetComponentInChildren(typeof(Button)) as Button;
                    upgradeButton.interactable = true;
                }
                if((money < (curTower.GetComponent(typeof(Tower)) as Tower).upgradeCost) ||
                    (curTower.GetComponent(typeof(Tower)) as Tower).level >= 5 )
                {
                    Button upgradeButton = upgradePanel.GetComponentInChildren(typeof(Button)) as Button;
                    upgradeButton.interactable = false;
                }
            }
        }
    }

    public static void updateUpgradePanel(Tower upgradeTower)
    {
        upgradePanel.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 30);
        Text[] panelText = upgradePanel.GetComponentsInChildren<Text>(true);
        foreach (Text t in panelText)
        {
            if (t.name == "CurLevel")
            {
                t.text = "Current Level: " + upgradeTower.level;
            }
            else if (t.name == "CurCost")
            {
                if(upgradeTower.level == 5)
                {
                    t.text = "Fully Upgraded!";
                }
                t.text = "Upgrade Cost: " + upgradeTower.upgradeCost;
            }
        }

    }

    public static void disableUpgradeButton()
    {
        Button upgradeButton = upgradePanel.GetComponentInChildren(typeof(Button)) as Button;
        upgradeButton.interactable = false;
    }

    public void UpgradeTower()
    {
        Tower upgradeTower = curTower.GetComponent(typeof(Tower)) as Tower;
        if(money >= upgradeTower.upgradeCost)
        {
            money = money - upgradeTower.upgradeCost;
            upgradeTower.Upgrade();
            updateUpgradePanel(upgradeTower);
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
        onTower = false;
    }

    /// <summary>
    /// Singals player that no tower is currently selected and no tower can be placed.
    /// </summary>
    public void ResetTower()
    {
        this.SelectedTower = null;
    }

}
