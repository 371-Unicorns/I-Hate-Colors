using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Amount of tiles on the x-Axis.
    /// </summary>
    private static int width = 22;
    public static int Width { get { return width; } }

    /// <summary>
    /// Amount of tiles on the y-Axis.
    /// </summary>
    private static int height = 14;
    public static int Height { get { return height; } }

    /// <summary>
    /// Currently selected tower by player. Could either be a ready to place tower or an already placed tower.
    /// </summary>
    public static Tower SelectedTower { get; private set; }
    public static SpriteRenderer rangeIndicatorRenderer;

    [HideInInspector]
    public static bool gameOver;
    private static GameObject gameOverObject;
    private static Text healthText, moneyText, waveText, countdownTimerText;

    public static GameObject canvas;

    /// <summary>
    /// Where the coins should fly to.
    /// </summary>
    public static GameObject bloodFlyTarget;

    /// <summary>
    /// Button to skip the timer in the info panel.
    /// </summary>
    private static Button skipTimeButton;
    public static Button SkipTimeButton { get { return skipTimeButton; } }

    private static GameTimer waveTimer;
    public static GameTimer WaveTimer { get { return waveTimer; } }


    public static int currentWave;
    private static int totalWaves;

    /// <summary>
    /// Post Processing Profile.
    /// </summary>
    public PostProcessingProfile ppProfile;

    public static int money = 150;

    /// <summary>
    /// AudioSource to be played, once a blood reaches the bank.
    /// </summary>
    private AudioClip unicornBeginWaveSound;
    private AudioSource audioSource;

    /// <summary>
    /// Content of the towerScrollView. Add available TowerBtn to this.
    /// </summary>
    private static Transform towerScrollViewContent;

    private static Dictionary<string, Tower> towerDictionary;

    /// <summary>
    /// Boolean variables related to the reward panel
    /// </summary>
    public static GameObject rewardsPanel;
    public static Text rewardsPanelText;
    public static bool didUpgradeFirstTower = false;
    public static bool notYetReceivedFirstTowerUpgradeReward = true;
    public static bool didUpgradeBulletTower = false;
    public static bool didUpgradeShotgunTower = false;
    public static bool didUpgradeBlackHoleTower = false;
    public static bool didUpgradeLaserTower = false;
    public static bool didUpgradeFlameTower = false;
    public static bool notYetReceivedTowerUpgradeReward = true;
    public static bool didPlaceBulletTower = false;
    public static bool didPlaceShotgunTower = false;
    public static bool didPlaceBlackHoleTower = false;
    public static bool didPlaceLaserTower = false;
    public static bool didPlaceFlameTower = false;
    public static bool notYetReceivedTowerPlacementReward = true;
    public static bool rewardsPanelFirstEnable = true;

    /// <summary>
    /// All created TowerBtn.
    /// </summary>
    private static List<TowerBtn> towerBtns;

    public void Start()
    {
        gameOver = false;
        rewardsPanel = GameObject.Find("RewardsPanel");
        rewardsPanelText = rewardsPanel.gameObject.GetComponentInChildren<Text>();
        rewardsPanel.SetActive(false);
        didUpgradeFirstTower = false;
        notYetReceivedFirstTowerUpgradeReward = true;
        didUpgradeBulletTower = false;
        didUpgradeShotgunTower = false;
        didUpgradeBlackHoleTower = false;
        didUpgradeLaserTower = false;
        didUpgradeFlameTower = false;
        notYetReceivedTowerUpgradeReward = true;
        didPlaceBulletTower = false;
        didPlaceShotgunTower = false;
        didPlaceBlackHoleTower = false;
        didPlaceLaserTower = false;
        didPlaceFlameTower = false;
        notYetReceivedTowerPlacementReward = true;

        money = 150;

        canvas = GameObject.Find("Canvas");
        rangeIndicatorRenderer = GameObject.Find("RangeIndicator").gameObject.GetComponent<SpriteRenderer>();
        gameOverObject = canvas.transform.Find("GameOver").gameObject;
        gameOverObject.SetActive(false);

        SelectedTower = null;

        waveTimer = new GameTimer();
        waveTimer.SetTimer(30);
        currentWave = 1;
        totalWaves = WaveManager.GetWaves().Count;

        Transform infoPanel = canvas.transform.Find("InfoPanel");
        bloodFlyTarget = infoPanel.Find("BloodPanel").gameObject;
        healthText = infoPanel.Find("HealthPanel").GetComponentInChildren<Text>();
        moneyText = infoPanel.Find("BloodPanel").GetComponentInChildren<Text>();
        waveText = infoPanel.Find("WavePanel").GetComponentInChildren<Text>();
        countdownTimerText = infoPanel.Find("TimePanel").GetComponentInChildren<Text>();
        waveText.text = currentWave.ToString();
        skipTimeButton = infoPanel.Find("SkipTimeButton").GetComponent<Button>();

        towerScrollViewContent = canvas.transform.Find("TowerScrollView").GetComponentInChildren<GridLayoutGroup>().transform;
        towerBtns = new List<TowerBtn>();

        towerDictionary = XmlImporter.GetTowersFromXml();
        LoadTowerButtons();
        StartCoroutine(BlinkText());

        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        unicornBeginWaveSound = (AudioClip)Resources.Load("Audio/unicornBeginWaveSound");
    }

    public void Update()
    {
        SetTimerText();
        WaveManager.Update();

        var saturation = ppProfile.colorGrading.settings;

        if (currentWave == 1)
        {
            saturation.basic.saturation = 1;
        }

        if (gameOver)
        {
            waveTimer.SetPaused(true);

            if (CastleManager.CastleHealth > 0)
                saturation.basic.saturation = 0;


            SumPause.Status = true;

            GameObject.Find("OptionsMenu").SetActive(false);
            GameObject.Find("OptionsButton").GetComponent<Button>().interactable = false;

            Text gameOverText = gameOverObject.transform.Find("GameOverText").GetComponent<Text>();
            if (CastleManager.CastleHealth > 0)
            {
                SumPause.Status = false;
                SceneManager.LoadScene("victory_cutscene");
            }
            else
            {
                gameOverText.text = "GAME OVER";
                gameOverObject.SetActive(true);
            }
        }
        else
        {
            if (!waveTimer.IsPaused() && waveTimer.IsDone())
            {
                waveTimer.SetPaused(true);
                WaveManager.BeginWave();
                audioSource.PlayOneShot(unicornBeginWaveSound);
            }

            if (WaveManager.WaveFinished() && EnemyManager.EnemiesRemaining() <= 0)
            {
                saturation.basic.saturation = 1.0f - ((float)(currentWave - 1) / (float)totalWaves);
                currentWave++;
                waveText.text = currentWave.ToString();
                waveTimer.Reset();
                waveTimer.SetPaused(false);
                WaveManager.SetNextWave();
                skipTimeButton.interactable = true;
            }

            if (Hover.IsActive())
            {
                GameManager.rangeIndicatorRenderer.transform.position = Hover.GetPosition();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                GameManager.AddMoney(100);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                SkipTimer();
            }
            else if (Input.GetMouseButtonUp(1) && Hover.IsActive())
            {
                GameManager.rangeIndicatorRenderer.enabled = false;
                Hover.Deactivate();
                GameManager.ResetTower();
                TowerInformation.Reset();
            }
        }
        ppProfile.colorGrading.settings = saturation;
        healthText.text = CastleManager.CastleHealth.ToString();
        moneyText.text = money.ToString();
    }

    /// <summary>
    /// Set currently selected tower and activate hovering effect for this tower.
    /// </summary>
    /// <param name="towerBtn">TowerBtn to select.</param>
    public static void SelectTowerAndHover(TowerBtn towerBtn)
    {
        Hover.Activate(towerBtn.TowerPrefab.GetComponent<Tower>().Range, towerBtn.TowerHoverSprite);
        GameManager.SelectedTower = towerBtn.TowerPrefab.GetComponent<Tower>();
        GameManager.rangeIndicatorRenderer.transform.localScale = new Vector3(SelectedTower.Range * .66f, SelectedTower.Range * .66f, 1);
        GameManager.rangeIndicatorRenderer.transform.position = SelectedTower.transform.position;
        GameManager.rangeIndicatorRenderer.enabled = true;
    }

    /// <summary>
    /// Set currently selected tower.
    /// </summary>
    /// <param name="tower">Tower to select.</param>
    public static void SelectTower(Tower tower)
    {
        GameManager.SelectedTower = tower;

        GameManager.rangeIndicatorRenderer.transform.position = SelectedTower.transform.position;
        GameManager.rangeIndicatorRenderer.transform.localScale = new Vector3(SelectedTower.Range * .66f, SelectedTower.Range * .66f, 1);
        GameManager.rangeIndicatorRenderer.enabled = true;
    }

    /// <summary>
    /// Singals player that no tower is currently selected and no tower can be placed.
    /// </summary>
    public static void ResetTower()
    {
        GameManager.SelectedTower = null;
        GameManager.rangeIndicatorRenderer.enabled = false;
    }

    public static bool CheckForFirstUpgrade()
    {
        switch (SelectedTower.tag)
        {
            case "BulletTower":
                if (!GameManager.didUpgradeBulletTower)
                    GameManager.didUpgradeBulletTower = true;
                break;
            case "ShotgunTower":
                if (!GameManager.didUpgradeShotgunTower)
                    GameManager.didUpgradeShotgunTower = true;
                break;
            case "BlackHoleTower":
                if (!GameManager.didUpgradeBlackHoleTower)
                    GameManager.didUpgradeBlackHoleTower = true;
                break;
            case "LaserTower":
                if (!GameManager.didUpgradeLaserTower)
                    GameManager.didUpgradeLaserTower = true;
                break;
            case "FlameTower":
                if (!GameManager.didUpgradeFlameTower)
                    GameManager.didUpgradeFlameTower = true;
                break;
        }
        if (GameManager.didUpgradeBulletTower
            && GameManager.didUpgradeShotgunTower
            && GameManager.didUpgradeBlackHoleTower
            && GameManager.didUpgradeLaserTower
            && GameManager.didUpgradeFlameTower)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckForFirstPlacement()
    {
        if (SelectedTower.CompareTag("BulletTower") && !GameManager.didPlaceBulletTower)
        {
            GameManager.didPlaceBulletTower = true;
        }
        else if (SelectedTower.CompareTag("BlackHoleTower") && !GameManager.didPlaceBlackHoleTower)
        {
            GameManager.didPlaceBlackHoleTower = true;
        }
        else if (SelectedTower.CompareTag("FlameTower") && !GameManager.didPlaceFlameTower)
        {
            GameManager.didPlaceFlameTower = true;
        }
        else if (SelectedTower.CompareTag("LaserTower") && !GameManager.didPlaceLaserTower)
        {
            GameManager.didPlaceLaserTower = true;
        }
        else if (SelectedTower.CompareTag("ShotgunTower") && !GameManager.didPlaceShotgunTower)
        {
            GameManager.didPlaceShotgunTower = true;
        }

        if (GameManager.didPlaceBulletTower && GameManager.didPlaceShotgunTower && GameManager.didPlaceBlackHoleTower && GameManager.didPlaceLaserTower && GameManager.didPlaceFlameTower)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void AddMoney(int m)
    {
        GameManager.money += m;
        foreach (TowerBtn towerBtn in towerBtns)
        {
            towerBtn.CheckEnoughMoney();
        }
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

    public static IEnumerator BlinkText()
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

    public static IEnumerator DisplayRewardsPanel()
    {

        if (didUpgradeFirstTower && notYetReceivedFirstTowerUpgradeReward)
        {
            rewardsPanelText.text = "You Upgraded your first tower! Nice!";
            rewardsPanel.SetActive(true);
            GameManager.notYetReceivedFirstTowerUpgradeReward = false;
            AddMoney(20);

        }
        else if (didPlaceBulletTower && didPlaceShotgunTower && didPlaceBlackHoleTower && didPlaceLaserTower && didPlaceFlameTower && notYetReceivedTowerPlacementReward)
        {
            rewardsPanelText.text = "You placed one of every tower type! Nice!";
            rewardsPanel.SetActive(true);
            GameManager.notYetReceivedTowerPlacementReward = false;
            AddMoney(50);
        }
        else if (didUpgradeBulletTower && didUpgradeShotgunTower && didUpgradeBlackHoleTower && didUpgradeLaserTower && didUpgradeFlameTower && notYetReceivedTowerUpgradeReward)
        {
            rewardsPanelText.text = "You upgraded one of every tower type! Nice!";
            rewardsPanel.SetActive(true);
            GameManager.notYetReceivedTowerUpgradeReward = false;
            CastleManager.AddCastleHealth();
        }

        yield return new WaitForSeconds(6);
        rewardsPanel.SetActive(false);
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
            if (tower == null)
            {
                Debug.Log(string.Format("Tried to load and instantiate Tower, but an error occured."));
                return;
            }
            towerButton.Initalize(tower.gameObject);
            towerBtns.Add(towerButton);
        }
    }

    /// <summary>
    /// Skip the currently running wave timer.
    /// 
    /// Author: David Askari
    /// </summary>
    public void SkipTimer()
    {
        waveTimer.SkipTimer();
        skipTimeButton.interactable = false;
    }

}
