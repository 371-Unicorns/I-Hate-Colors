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
    private Text gameOverText, healthText, moneyText;

    public static GameObject curTower;
    public static bool onTower = false;
    
    public GameObject canvas;
    public static GameObject upgradePanel;

    private GameTimer timer;
    public int currentWave;

    public static int money = 0;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GameManager() { }

    void Awake()
    {
        gameOver = false;

        timer = new GameTimer();
        timer.SetTimer(30);
        currentWave = 1;

        Transform textObjects = canvas.transform.Find("TextObjects");

        gameOverText = textObjects.Find("GameOverText").gameObject.GetComponent<Text>();
        healthText = textObjects.Find("HealthText").gameObject.GetComponent<Text>();
        moneyText = textObjects.Find("MoneyText").gameObject.GetComponent<Text>();

        healthText.text = "Health: 100";
        moneyText.text = "$ ";
        upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
    }

    void Update()
    {
        WaveManager.Update();

        if (timer.IsDone())
        {
            timer.SetPaused(true);
            WaveManager.BeginWave(++currentWave);
        }

        if (WaveManager.WaveFinished())
        {
            timer.Reset();
            timer.SetPaused(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            print("done");
            timer.SkipTimer();
        }

        if (gameOver)
        {
            gameOverText.text = "GAME OVER";
            gameOverText.gameObject.SetActive(true);
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

    public static void UpdateUpgradePanel(Tower upgradeTower)
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
            UpdateUpgradePanel(upgradeTower);
        }
        

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
