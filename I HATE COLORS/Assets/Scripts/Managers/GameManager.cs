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

    [HideInInspector]
    public bool gameOver;
    private Text gameOverText, healthText, moneyText, waveText, countdownTimerText;

    public GameObject canvas;

    /// <summary>
    /// Where the coins should fly to.
    /// </summary>
    public GameObject coinFlyTarget;

    private GameTimer waveTimer;
    public int currentWave;

    public static int money = 100;

    /// <summary>
    /// Content of the towerScrollView. Add available TowerBtn to this.
    /// </summary>
    private Transform towerScrollViewContent;

    /// <summary>
    /// Whether the game is currently running or not.
    /// </summary>
    [HideInInspector]
    public bool gameRunning = true;

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
        coinFlyTarget = infoPanel.Find("MoneyPanel").gameObject;
        healthText = infoPanel.Find("HealthPanel").GetComponentInChildren<Text>();
        moneyText = infoPanel.Find("MoneyPanel").GetComponentInChildren<Text>();
        waveText = infoPanel.Find("WavePanel").GetComponentInChildren<Text>();
        waveText.text = currentWave.ToString();

        towerScrollViewContent = canvas.transform.Find("TowerScrollView").GetComponentInChildren<GridLayoutGroup>().transform;

        // TEMPORARY CODE --- Showcase of how to add towers with new tower design.
        CreateBulletProjectileTower();
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
            waveText.text = currentWave.ToString();
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
                GameManager.Instance.ResetTower();
                TowerInformation.Instance.Reset();
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
    /// Set currently selected tower and activate hovering effect for this tower.
    /// </summary>
    /// <param name="towerBtn">TowerBtn to select.</param>
    public void SelectTowerAndHover(TowerBtn towerBtn)
    {
        Hover.Instance.Activate(towerBtn.TowerHoverSprite);
        this.SelectedTower = towerBtn.TowerPrefab.GetComponent<Tower>();
    }

    /// <summary>
    /// Set currently selected tower.
    /// </summary>
    /// <param name="tower">Tower to select.</param>
    public void SelectTower(Tower tower)
    {
        this.SelectedTower = tower;
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

    /// <summary>
    /// TEMPORARY CODE
    /// 
    /// Showcase of how to add towers with new tower design.
    /// 
    /// 1. Load tower prefab
    /// 2. Instatiate tower prefab and fill with values.
    /// 3. Load and instatiate corresponding TowerBtn prefab. Set its parent to towerScrollViewContent in order to add it to showed tower buttons.
    /// 4. Set tower button's tower prefab to previously instatited tower.
    /// </summary>
    private void CreateBulletProjectileTower()
    {
        // 1.
        ProjectileTower bulletProjectileTowerPrefab = Resources.Load("Prefabs/Towers/BulletTower", typeof(ProjectileTower)) as ProjectileTower;
        if (bulletProjectileTowerPrefab == null)
        {
            Debug.Log("Tried to load bulletProjectileTowerPrefab, but it does not exist.");
            return;
        }

        // 2.
        ProjectileTower bulletProjectileTower = Instantiate(bulletProjectileTowerPrefab, LevelManager.Instance.PrefabHolderParent);
        bulletProjectileTower.Setup("Bullet Tower", 20, 10, 1.25, 5, 5, 2, 10, 10);


        // 3.
        TowerBtn bulletProjectileTowerBtn = Instantiate(Resources.Load("Prefabs/UI/BulletTowerBtn", typeof(TowerBtn)) as TowerBtn, towerScrollViewContent);
        if (bulletProjectileTowerBtn == null)
        {
            Debug.Log("Tried to load and instantiate bulletProjectileTowerBtn, but an error occured.");
            return;
        }

        // 4.
        bulletProjectileTowerBtn.TowerPrefab = bulletProjectileTower.gameObject;
    }

}
