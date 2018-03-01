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
    public SpriteRenderer rangeIndicatorRenderer;

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

    private Dictionary<string, Tower> towerDictionary;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private GameManager() { }

    void Awake()
    {
        gameOver = false;
        gameOverText = canvas.transform.Find("GameOverText").gameObject.GetComponent<Text>();
        gameOverText.gameObject.SetActive(false);

        SelectedTower = null;

        toMenuButton.gameObject.SetActive(false);

        waveTimer = new GameTimer();
        waveTimer.SetTimer(30);
        currentWave = 1;

        Transform infoPanel = canvas.transform.Find("InfoPanel");
        coinFlyTarget = infoPanel.Find("BloodPanel").gameObject;
        healthText = infoPanel.Find("HealthPanel").GetComponentInChildren<Text>();
        moneyText = infoPanel.Find("BloodPanel").GetComponentInChildren<Text>();
        waveText = infoPanel.Find("WavePanel").GetComponentInChildren<Text>();
        countdownTimerText = infoPanel.Find("TimePanel").GetComponentInChildren<Text>();
        waveText.text = currentWave.ToString();

        towerScrollViewContent = canvas.transform.Find("TowerScrollView").GetComponentInChildren<GridLayoutGroup>().transform;

        towerDictionary = XmlImporter.GetTowersFromXml();
        LoadTowerButtons();
        StartCoroutine(BlinkText());
    }

    void Update()
    {
        SetTimerText();
        WaveManager.Update();

        if (!waveTimer.IsPaused() && waveTimer.IsDone())
        {
            waveTimer.SetPaused(true);
            WaveManager.BeginWave();
        }

        if (WaveManager.WaveFinished() && EnemyManager.EnemiesRemaining() <= 0)
        {
            waveText.text = currentWave.ToString();
            waveTimer.Reset();
            waveTimer.SetPaused(false);
            WaveManager.SetNextWave();
        }

        if (Hover.Instance.IsActive())
        {
            this.rangeIndicatorRenderer.transform.position = Hover.Instance.GetPosition();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            waveTimer.SkipTimer();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Hover.Instance.IsActive())
            {
                this.rangeIndicatorRenderer.enabled = false;
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
        Hover.Instance.Activate(towerBtn.TowerPrefab.GetComponent<Tower>().GetRange(), towerBtn.TowerHoverSprite);
        this.SelectedTower = towerBtn.TowerPrefab.GetComponent<Tower>();

        this.rangeIndicatorRenderer.transform.localScale = new Vector3(SelectedTower.GetRange() * .66f, SelectedTower.GetRange() * .66f, 1);
        this.rangeIndicatorRenderer.transform.position = SelectedTower.transform.position;
        this.rangeIndicatorRenderer.enabled = true;
    }

    /// <summary>
    /// Set currently selected tower.
    /// </summary>
    /// <param name="tower">Tower to select.</param>
    public void SelectTower(Tower tower)
    {
        this.SelectedTower = tower;

        this.rangeIndicatorRenderer.transform.position = SelectedTower.transform.position;
        this.rangeIndicatorRenderer.transform.localScale = new Vector3(SelectedTower.GetRange() * .66f, SelectedTower.GetRange() * .66f, 1);
        this.rangeIndicatorRenderer.enabled = true;
    }

    /// <summary>
    /// Singals player that no tower is currently selected and no tower can be placed.
    /// </summary>
    public void ResetTower()
    {
        this.SelectedTower = null;
        this.rangeIndicatorRenderer.enabled = false;
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
            waveTimer.startBlinking = true;

        }
        else
        {
            countdownTimerText.color = Color.white;
            TimeSpan t = TimeSpan.FromSeconds(waveTimer.TimeRemaining());
            countdownTimerText.text = string.Format("{0}:{1:00}", t.Minutes, t.Seconds);
        }
    }

    public IEnumerator BlinkText()
    {

        while (true)
        {
            if (waveTimer.startBlinking)
            {
                countdownTimerText.text = "";
            }
            yield return new WaitForSeconds(1);

            if (waveTimer.startBlinking)
            {
                countdownTimerText.text = "0:00";
            }
            yield return new WaitForSeconds(1);

        }
    }

    private void LoadTowerButtons()
    {
        foreach (Tower tower in towerDictionary.Values)
        {
            TowerBtn towerButton = Instantiate(Resources.Load("Prefabs/UI/TowerButton", typeof(TowerBtn)) as TowerBtn, towerScrollViewContent);
            if (towerButton == null)
            {
                Debug.Log(string.Format("Tried to load and instantiate {0}'s GUI button, but an error occured.", tower.name));
                return;
            }

            towerButton.SetSprites(tower.gameObject);
        }
    }

}
