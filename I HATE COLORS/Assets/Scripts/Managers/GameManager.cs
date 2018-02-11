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

    [SerializeField]
    private Button toMenuButton;

    /// <summary>
    /// Currently selected tower by player. Could either be a ready to place tower or an already placed tower.
    /// </summary>
    public Tower SelectedTower { get; private set; }
    public Tower newSelectedTower;

    public bool gameOver;
    private Text gameOverText, healthText, moneyText, waveText, countdownTimerText;

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
        }
    }

    /// <summary>
    /// Signals player that tower is ready to place by hovering it with the mouse cursor.
    /// </summary>
    /// <param name="towerBtn">TowerBtn to select.</param>
    public void SelectTower(TowerBtn towerBtn)
    {
        this.SelectedTower = towerBtn.TowerPrefab.GetComponent<Tower>();
        Hover.Instance.Activate(towerBtn.TowerHoverSprite);
        newSelectedTower = this.SelectedTower;
    }

    /// <summary>
    /// Signals player that tower is ready to place by hovering it with the mouse cursor.
    /// </summary>
    /// <param name="towerBtn">TowerBtn to select.</param>
    public void SelectTower(Tower tower)
    {
        this.SelectedTower = tower;
        this.newSelectedTower = tower;
    }

    /// <summary>
    /// Singals player that no tower is currently selected and no tower can be placed.
    /// </summary>
    public void ResetTower()
    {
        this.SelectedTower = null;
        this.newSelectedTower = null;
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
