using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WaveManager : Singleton<WaveManager> {

    private static readonly float SPAWN_DELAY = 0.1f;

    public string waveXmlFile;
    private static Wave currentWave;
    private static List<Wave> waves;

    private static GameTimer waveSpawnTimer;
    
    // Use this for initialization
    void Start () {
        waves = new List<Wave>();
        currentWave = null;
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/" + waveXmlFile);

        waveSpawnTimer = new GameTimer();
        waveSpawnTimer.SetTimer(SPAWN_DELAY);

        XmlNode root = doc.SelectSingleNode("waves");
        foreach (XmlNode node in root.SelectNodes("wave")) {
            waves.Add(Wave.WaveFromXml(node));
        }
    }
	
	public static void Update () {
		if (currentWave != null && !waveSpawnTimer.IsPaused())
        {
            waveSpawnTimer.Update();

            if (waveSpawnTimer.IsDone() && currentWave.EnemiesRemaining() > 0)
            {
                EnemyManager.SpawnEnemy(currentWave.NextEnemy());

                waveSpawnTimer.Reset();
            }
        }
	}

    public static void BeginWave(int wave)
    {
        if (wave > waves.Count)
        {
            print(string.Format("No wave at index {0}!! Add more waves!!", wave));
        } else
        {
            currentWave = waves[wave - 1];
            print("Enemies remaining: " + currentWave.EnemiesRemaining());
        }
    }

    public static bool WaveFinished()
    {
        return currentWave != null && currentWave.EnemiesRemaining() == 0;
    }

    public static List<Wave> GetWaves()
    {
        return waves;
    }
}
