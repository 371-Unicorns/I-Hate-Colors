using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager> {

    private static bool waveRunning = false;

    public string waveXmlFile;
    private static Wave currentWave;
    private static Queue<Wave> waves;
    
    // Use this for initialization
    void Start () {
        waves = XmlImporter.GetWavesFromXml();
        WaveManager.SetNextWave();
    }

    public static void Update()
    {
        if (currentWave != null && waveRunning)
        {
            currentWave.Update();

            if (currentWave.SpawnReady())
            {
                EnemyManager.SpawnEnemy(currentWave.NextEnemy());
            }
        }
    }

    public static void SetNextWave()
    {
        if (waves.Count == 0)
        {
            print(string.Format("No more waves, add more!!"));
        }
        else
        {
            currentWave = waves.Dequeue();
            waveRunning = false;
        }
    }

    public static void BeginWave()
    {
        waveRunning = true;

        EnemyManager.ClearDeadEnemies();
    }

    public static bool WaveFinished()
    {
        return currentWave != null && currentWave.EnemiesRemaining() == 0;
    }

    public static Queue<Wave> GetWaves()
    {
        return waves;
    }
}
