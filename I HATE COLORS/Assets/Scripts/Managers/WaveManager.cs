using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class WaveManager : Singleton<WaveManager> {

    public string waveXmlFile;
    private static Wave currentWave;
    private static List<Wave> waves;
    
    // Use this for initialization
    void Start () {
        waves = XmlImporter.GetWavesFromXml();
        currentWave = null;
    }

    public static void Update()
    {
        if (currentWave != null)
        {
            currentWave.Update();

            if (currentWave.SpawnReady())
            {
                EnemyManager.SpawnEnemy(currentWave.NextEnemy());
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
