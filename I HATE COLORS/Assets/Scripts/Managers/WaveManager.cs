using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static bool waveRunning = false;

    public static string waveXmlFile;
    private static Wave currentWave;
    public static Wave CurrentWave { get { return currentWave; } }
    private static Queue<Wave> waves;

    // Use this for initialization
    public void Start()
    {
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
        if (waves.Count == 0)
        {
            GameManager.gameOver = true;
        }
    }

    public static void SetNextWave()
    {
        currentWave = waves.Dequeue();
        waveRunning = false;
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
