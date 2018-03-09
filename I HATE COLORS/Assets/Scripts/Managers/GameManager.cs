using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using Pathfinding;

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

    private static GameTimer waveTimer;
    public static int currentWave;
    private static int totalWaves;

    /// <summary>
    /// Post Processing Profile.
    /// </summary>
    public PostProcessingProfile ppProfile;

    public static int money = 1000;

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
    public static bool didPlaceBulletTower = false;
    public static bool didPlaceShotgunTower = false;
    public static bool didPlaceBlackHoleTower = false;
    public static bool didPlaceLaserTower = false;
    public static bool didPlaceFlameTower = false;

    /*
    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    public GameManager() { } */

    public void Start()
    {
        gameOver = false;
        rewardsPanel = GameObject.Find("RewardsPanel");
        rewardsPanelText = rewardsPanel.gameObject.GetComponentInChildren<Text>();
        rewardsPanel.SetActive(false);
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

        towerScrollViewContent = canvas.transform.Find("TowerScrollView").GetComponentInChildren<GridLayoutGroup>().transform;

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
            gameOverText.text = CastleManager.CastleHealth <= 0 ? "GAME OVER" : "CONGRATULATIONS";
            gameOverObject.SetActive(true);
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
                waveTimer.SkipTimer();
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

    public static bool CheckForFirstPlacement() {
        if(SelectedTower.CompareTag("BulletTower") && !GameManager.didPlaceBulletTower) {
            GameManager.didPlaceBulletTower = true;
        }
        else if(SelectedTower.CompareTag("BlackHoleTower") && !GameManager.didPlaceBlackHoleTower) {
            GameManager.didPlaceBlackHoleTower = true;
        }
        else if(SelectedTower.CompareTag("FlameTower") && !GameManager.didPlaceFlameTower) {
            GameManager.didPlaceFlameTower = true;
        }
        else if(SelectedTower.CompareTag("LaserTower") && !GameManager.didPlaceLaserTower) {
            GameManager.didPlaceLaserTower = true;
        }
        else if(SelectedTower.CompareTag("ShotgunTower") && !GameManager.didPlaceShotgunTower) {
            GameManager.didPlaceShotgunTower = true;
        }

        if(GameManager.didPlaceBulletTower && GameManager.didPlaceShotgunTower && GameManager.didPlaceBlackHoleTower && GameManager.didPlaceLaserTower && GameManager.didPlaceFlameTower) {
            return true;
        }
        else {
            return false;
        }
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
        rewardsPanel.SetActive(true);

        if (didUpgradeFirstTower)
        {
            rewardsPanelText.text = "You Upgraded your first tower! Nice!";
            AddMoney(20);
            
        }
        else if(didPlaceBulletTower && didPlaceShotgunTower && didPlaceBlackHoleTower && didPlaceLaserTower && didPlaceFlameTower) 
        {
            rewardsPanelText.text = "You placed one of every tower type! Nice!";
            AddMoney(50);
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
            towerButton.SetSprites(tower.gameObject);
        }
    }

}
