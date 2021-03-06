﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the whole game.
/// 
/// Authors: The entire group
/// </summary>
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

    /// <summary>
    /// All the text that is displayed to the player during game play.
    /// </summary>
    [HideInInspector]
    public static bool gameOver;
    private static GameObject gameOverObject;
    private static Text healthText, moneyText, waveText, countdownTimerText, errorText;
    public Text _errorText;

    public static GameObject canvas;

    /// <summary>
    /// Where the rainbow blood should fly to.
    /// </summary>
    public static GameObject bloodFlyTarget;

    /// <summary>
    /// Button to skip the timer in the info panel.
    /// </summary>
    private static Button skipTimeButton;
    public static Button SkipTimeButton { get { return skipTimeButton; } }

    /// <summary>
    /// Shows how long the player has before the next wave comes.
    /// Flashes 0:00 when the enemies are attacking.
    /// </summary>
    private static GameTimer waveTimer, errorTextTimer;
    public static GameTimer WaveTimer { get { return waveTimer; } }

    /// <summary>
    /// The number of the current wave of attacking enemies.
    /// </summary>
    public static int currentWave;

    /// <summary>
    /// How many total waves are in the game as set in wave_composition.xml.
    /// </summary>
    private static int totalWaves;

    /// <summary>
    /// Post Processing Profile that allows for color fade via decreasing saturation.
    /// Downloaded from the Unity Asset Store from this link:
    /// https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public PostProcessingProfile ppProfile;

    /// <summary>
    /// How much money the player starts with.
    /// </summary>
    public static int money = 150;

    /// <summary>
    /// Content of the towerScrollView. Add available TowerBtn to this.
    /// </summary>
    private static Transform towerScrollViewContent;

    /// <summary>
    /// Contains all of the towers in the game as set in towers.xml.
    /// </summary>
    private static Dictionary<string, Tower> towerDictionary;

    /// <summary>
    /// Variables related to the reward panel.
    /// Authors: Steven Johnson, Courtney Chu
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

    /// <summary>
    /// GameObject for sending a boss in the coming wave.
    /// </summary>
    public static GameObject sendBossButton;

    /// <summary>
    /// Initializes everything needed for game play.
    /// 
    /// Authors: Amy Lewis, Cole Twitchell, David Askari, Steven Johnson, Courtney Chu
    /// </summary>
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
        sendBossButton = canvas.transform.Find("SendBossButton").gameObject;
        sendBossButton.SetActive(false);

        SelectedTower = null;

        errorTextTimer = new GameTimer(5);

        waveTimer = new GameTimer();
        waveTimer.SetTimer(60);
        waveTimer.SetResetTime(30);
        currentWave = 1;
        totalWaves = WaveManager.GetWaves().Count;

        Transform infoPanel = canvas.transform.Find("InfoPanel");
        bloodFlyTarget = infoPanel.Find("BloodPanel").gameObject;
        healthText = infoPanel.Find("HealthPanel").GetComponentInChildren<Text>();
        moneyText = infoPanel.Find("BloodPanel").GetComponentInChildren<Text>();
        waveText = infoPanel.Find("WavePanel").GetComponentInChildren<Text>();
        countdownTimerText = infoPanel.Find("TimePanel").GetComponentInChildren<Text>();
        waveText.text = currentWave.ToString();
        errorText = _errorText;
        skipTimeButton = infoPanel.Find("SkipTimeButton").GetComponent<Button>();

        towerScrollViewContent = canvas.transform.Find("TowerScrollView").GetComponentInChildren<GridLayoutGroup>().transform;
        towerBtns = new List<TowerBtn>();

        towerDictionary = XmlImporter.GetTowersFromXml();
        LoadTowerButtons();
        StartCoroutine(BlinkText());
    }

    /// <summary>
    /// Updates times and other necessary pieces of game play.
    /// 
    /// Authors: Amy Lewis, Cole Twitchell, David Askari, Steven Johnson, Courtney Chu
    /// </summary>
    public void Update()
    {
        SetTimerText();
        WaveManager.Update();

        var saturation = ppProfile.colorGrading.settings;

        if (currentWave == 1)
        {
            saturation.basic.saturation = 1;
        }
        else if (currentWave == 6)
        {
            sendBossButton.SetActive(true);
            PauseGame.sendBossButton = sendBossButton.GetComponent<SendBossButton>();
        }

        if (gameOver)
        {
            waveTimer.SetPaused(true);

            if (CastleManager.CastleHealth > 0)
                saturation.basic.saturation = 0;

            PauseGame.Status = true;

            GameObject.Find("OptionsMenu").SetActive(false);
            GameObject.Find("OptionsButton").GetComponent<Button>().interactable = false;
            sendBossButton.GetComponent<SendBossButton>().DisableButton();

            if (CastleManager.CastleHealth > 0)
            {
                XmlImporter.Cleanup();
                PauseGame.Status = false;
                SceneManager.LoadScene("victory_cutscene");
            }
            else
            {
                gameOverObject.transform.Find("GameOverText").GetComponent<Text>().text = "GAME OVER";
                gameOverObject.SetActive(true);
                enabled = false;
            }
        }
        else
        {
            if (!waveTimer.IsPaused() && waveTimer.IsDone())
            {
                waveTimer.SetPaused(true);
                WaveManager.BeginWave();
                AudioManager.PlayBeginWaveSound();
                sendBossButton.GetComponent<SendBossButton>().DisableButton();
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
                sendBossButton.GetComponent<SendBossButton>().ResetButton();
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

        errorTextTimer.Update();
        if (errorTextTimer.IsDone())
        {
            errorText.gameObject.SetActive(false);
            errorTextTimer.Reset();
            errorTextTimer.SetPaused(true);
        }
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

    /// <summary>
    /// Gives player a reward if all of the types of towers have been upgraded at least once.
    /// Author: Courtney Chu, Steven Johnson
    /// </summary>
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
            case "FlameTower":
                if (!GameManager.didUpgradeFlameTower)
                    GameManager.didUpgradeFlameTower = true;
                break;
            case "LaserTower":
                if (!GameManager.didUpgradeLaserTower)
                    GameManager.didUpgradeLaserTower = true;
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

    /// <summary>
    /// Gives player a reward if all of the types of towers have been placed at least once.
    /// Author: Steven Johnson
    /// </summary>
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

    /// <summary>
    /// Adds m amount of money to the player's available blood.
    /// </summary>
    /// <param name="m">Amount of money added.</param>
    public static void AddMoney(int m)
    {
        GameManager.money += m;
        if (TowerInformation.isActive) { TowerInformation.CheckUpgrade(); }
        foreach (TowerBtn towerBtn in towerBtns)
        {
            towerBtn.CheckEnoughMoney();
        }
    }

    /// <summary>
    /// Sets the text of the wave timer. 
    /// Gives 30 seconds between each wave.
    /// Author: Steven Johnson
    /// </summary>
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

    /// <summary>
    /// Makes the wave timer blink when enemies are attacking.
    /// Author: Steven Johnson
    /// </summary>
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

    /// <summary>
    /// Displays when player earns a reward.
    /// Tells them what they earned and why.
    /// Authors: Steven Johnson, Courtney Chu
    /// </summary>
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

    /// <summary>
    /// Loads all of the buttons into the UI that the player can scroll through to select what tower they want.
    /// When clicked on the user can place that tower (if they have enough blood).
    /// </summary>
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

    /// <summary>
    /// Displays any error text to the player so they can know something went wrong.
    /// 
    /// </summary>
    public static void DisplayErrorText(string text)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = text;
        errorTextTimer.SetPaused(false);
    }
}
