using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Amount of tiles on the x-Axis.
    /// </summary>
    [SerializeField]
    private int width = 22;
    public int Width { get { return width; } }

    /// <summary>
    /// Amount of tiles on the y-Axis.
    /// </summary>
    [SerializeField]
    private int height = 14;
    public int Height { get { return height; } }

    /// <summary>
    /// Amount of tiles on the y-Axis.
    /// </summary>
    [SerializeField]
    private Button toMenuButton;

    /// <summary>
    /// Currently selected tower by player to place.
    /// </summary>
    public TowerBtn SelectedTower { get; private set; }

    public bool gameOver;
    private Text gameOverText, healthText, moneyText, waveText, countdownTimerText;

    public static GameObject curTower;
    public static bool onTower = false;

    public GameObject canvas;
    public static GameObject upgradePanel;

    private GameTimer waveTimer;
    public int currentWave;

    public static int money = 100;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GameManager() { }

    void Awake()
    {
        gameOver = false;
        gameOverText = canvas.transform.Find("GameOverText").gameObject.GetComponent<Text>();
        gameOverText.gameObject.SetActive(false);
        countdownTimerText = canvas.transform.Find("CountdownTimerText").gameObject.GetComponent<Text>();

        toMenuButton.gameObject.SetActive(false);

        waveTimer = new GameTimer();
        waveTimer.SetTimer(30);
        currentWave = 1;

        Transform infoPanel = canvas.transform.Find("InfoPanel");
        healthText = infoPanel.Find("HealthPanel").GetComponentInChildren<Text>();
        moneyText = infoPanel.Find("MoneyPanel").GetComponentInChildren<Text>();
        waveText = infoPanel.Find("WavePanel").GetComponentInChildren<Text>();
        waveText.text = currentWave.ToString();

        upgradePanel = canvas.transform.Find("UpgradePanel").gameObject;
    }

    void Update()
    {
        SetTimerText();
        WaveManager.Update();

        if (!waveTimer.IsPaused() && waveTimer.IsDone())
        {
            waveTimer.SetPaused(true);
            WaveManager.BeginWave(currentWave++);
        }

        if (WaveManager.WaveFinished() && EnemyManager.EnemiesRemaining() <= 0)
        {
            waveTimer.Reset();
            waveTimer.SetPaused(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            waveTimer.SkipTimer();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Hover.Instance.IsActive())
            {
                Hover.Instance.Deactivate();
            }
        }

        if (gameOver)
        {
            gameOverText.text = "GAME OVER";
            gameOverText.gameObject.SetActive(true);
            toMenuButton.gameObject.SetActive(true);
        }
        else
        {
            healthText.text = CastleManager.Instance.CastleHealth.ToString();
            moneyText.text = money.ToString();
            if (onTower == false)
            {
                upgradePanel.gameObject.SetActive(false);
            }
            else if (upgradePanel.activeSelf == true)
            {
                if (money >= (curTower.GetComponent(typeof(Tower)) as Tower).upgradeCost)
                {
                    Button upgradeButton = upgradePanel.GetComponentInChildren(typeof(Button)) as Button;
                    upgradeButton.interactable = true;
                }
                if ((money < (curTower.GetComponent(typeof(Tower)) as Tower).upgradeCost) ||
                    (curTower.GetComponent(typeof(Tower)) as Tower).level >= 5)
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
                if (upgradeTower.level == 5)
                {
                    t.text = "Fully Upgraded!";
                }
                else
                {
                    t.text = "Upgrade Cost: " + upgradeTower.upgradeCost;
                }

            }
        }

    }

    public static void DisableUpgradeButton()
    {
        Button upgradeButton = upgradePanel.GetComponentInChildren(typeof(Button)) as Button;
        upgradeButton.interactable = false;
    }

    public void UpgradeTower()
    {
        Tower upgradeTower = curTower.GetComponent(typeof(Tower)) as Tower;
        if (money >= upgradeTower.upgradeCost)
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

    public static void AddMoney(int m)
    {
        GameManager.money += m;
    }

    private void SetTimerText()
    {
        waveTimer.Update();

        if (waveTimer.IsDone())
        {
            countdownTimerText.text = "Defend!";
        }
        else
        {
            TimeSpan t = TimeSpan.FromSeconds(waveTimer.TimeRemaining());
            countdownTimerText.text = string.Format("{0}:{1:00}", t.Minutes, t.Seconds);
        }
    }
}
